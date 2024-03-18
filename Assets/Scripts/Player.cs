using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    AllManager allmng;
    Rigidbody2D rgbd;

    public float moveSpeed;
    public float fallAcceleration;
    public float turnTime;
    public float groundCheckDistance;
    public float wallCheckDistance;
    public Transform moveRightDirector;
    public Transform moveLeftDirector;
    public Transform downDirector;
    public Transform upDirector;
    public int maxEndurance;
    [SerializeField]
    int currentEndurance;
    [SerializeField]
    bool isMoving;
    [SerializeField]
    bool isTurning;
    [SerializeField]
    bool isFalling;
    Collider2D col;
    Transform currentMoveDirector;
    Vector2 nextPosition;
    Vector2 currentFallVelocity;
    InputActionMap currentActionMap;
    public Actions actions;

    void Awake(){
        actions = new Actions();
    }
    void Start()
    {
        allmng = GameObject.Find("AllManager").GetComponent<AllManager>();
        rgbd = GetComponent<Rigidbody2D>();
        currentEndurance = maxEndurance;
        col = GetComponent<Collider2D>();
        currentActionMap = actions.PlayerMove;
        actions.PlayerMove.Up.performed += MoveUp;
        actions.PlayerMove.Right.performed += MoveRight;
        actions.PlayerMove.Down.performed += MoveDown;
        actions.PlayerMove.Left.performed += MoveLeft;
        actions.PlayerMove.Reset.performed += Reset;
        currentActionMap.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving && !isTurning && !allmng.levelEventsActive && !isFalling){
            if((Vector2)transform.position != nextPosition){
                transform.position = Vector2.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
            }
            else{

                RaycastHit2D groundHit = Physics2D.Raycast(transform.position, (downDirector.position - transform.position).normalized, groundCheckDistance, LayerMask.GetMask("Solid"));
                if(groundHit.collider == null){
                    isTurning = true;
                    nextPosition = downDirector.transform.position;
                    StartCoroutine(TurnHole());
                }
                else{
                    TriggerWorldEvent();
                    CheckEndurance();
                    isMoving = false;
                }
            }
        }
        else if(isFalling && !allmng.levelEventsActive){
            if(transform.rotation != Quaternion.Euler(0f, 0f, 0f)){
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            Vector2 fallDirection = (new Vector2(transform.position.x, transform.position.y -1) - (Vector2)transform.position).normalized;
            currentFallVelocity += fallDirection * fallAcceleration * Time.deltaTime;
            rgbd.velocity = currentFallVelocity;
        }
    }
    IEnumerator TurnHole(){
        float startTime = Time.time;
        float elapsedTime = 0f;
        Vector3 currentEulerRotation = transform.rotation.eulerAngles;
        Vector3 targetEulerRotation = currentMoveDirector == moveRightDirector ? new Vector3(0f, 0f, currentEulerRotation.z -90f) : new Vector3(0f, 0f, currentEulerRotation.z +90f);
        while(Time.time - startTime < turnTime){
            float t = elapsedTime / turnTime;
            Vector3 nextEulerRotation = Vector3.Lerp(currentEulerRotation, targetEulerRotation, t);
            transform.rotation = Quaternion.Euler(nextEulerRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(targetEulerRotation);
        isTurning = false;
    }
    IEnumerator TurnWall(){
        float startTime = Time.time;
        float elapsedTime = 0f;
        Vector3 currentEulerRotation = transform.rotation.eulerAngles;
        Vector3 targetEulerRotation = currentMoveDirector != moveRightDirector ? new Vector3(0f, 0f, currentEulerRotation.z -90f) : new Vector3(0f, 0f, currentEulerRotation.z +90f);
        while(Time.time - startTime < turnTime){
            float t = elapsedTime / turnTime;
            Vector3 nextEulerRotation = Vector3.Lerp(currentEulerRotation, targetEulerRotation, t);
            transform.rotation = Quaternion.Euler(nextEulerRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(targetEulerRotation);
        TriggerWorldEvent();
        CheckEndurance();
        isTurning = false;
    }

    private void TriggerWorldEvent(){
        RaycastHit2D levenEventHit = Physics2D.Raycast(transform.position, (downDirector.position - transform.position).normalized, groundCheckDistance, LayerMask.GetMask("LevelEvent"));
            if(levenEventHit.collider != null){
            allmng.levelEventCounter++;
            StartCoroutine(levenEventHit.collider.gameObject.GetComponentInParent<LevelEvent>().DoEvent());
        }
    }
    private void CheckEndurance(){
        if(upDirector.transform.position.y > transform.position.y + 0.25f){
            // Is on floor
            currentEndurance = maxEndurance;
        }
        else{
            currentEndurance--;
        }

        if(currentEndurance <= 0){
            currentEndurance = maxEndurance;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
            isFalling = true;
        }
    }

    void MoveRight(InputAction.CallbackContext context){
        if(!isMoving && !isTurning && !isFalling && !allmng.levelEventsActive){
                    Debug.Log("should move right");
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, (moveRightDirector.transform.position - transform.position).normalized, wallCheckDistance, LayerMask.GetMask("Solid"));
            if(wallHit.collider != null){
                isTurning = true;
                currentMoveDirector = moveRightDirector;
                StartCoroutine(TurnWall());
            }
            else{
                currentMoveDirector = moveRightDirector;
                nextPosition = currentMoveDirector.transform.position;
                isMoving = true;
            }

        }
    }

    void MoveLeft(InputAction.CallbackContext context){

        if(!isMoving && !isTurning && !isFalling && !allmng.levelEventsActive){
            Debug.Log("should move left");
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, (moveLeftDirector.transform.position - transform.position).normalized, wallCheckDistance, LayerMask.GetMask("Solid"));
            if(wallHit.collider != null){
                isTurning = true;
                currentMoveDirector = moveLeftDirector;
                StartCoroutine(TurnWall());
            }
            else{
                currentMoveDirector = moveLeftDirector;
                nextPosition = currentMoveDirector.transform.position;
                isMoving = true;
            }

        }
    }
    void MoveUp(InputAction.CallbackContext context){

    }
    void MoveDown(InputAction.CallbackContext context){

    }
    void Reset(InputAction.CallbackContext context){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.collider.gameObject.layer == LayerMask.NameToLayer("Solid") && isFalling){
            rgbd.velocity = new Vector2(0f, 0f);
            isFalling = false;
            currentFallVelocity = new Vector2(0f, 0f);
        }
        else if(other.collider.gameObject.layer == LayerMask.NameToLayer("Death")){
            Destroy(this.gameObject);
        }
    }
    
}

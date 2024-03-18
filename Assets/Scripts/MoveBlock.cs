using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : LevelEvent
{  
    AllManager allmng;
    Transform currentMoveDirector;
    public float moveTime;
    void Start()
    {
        allmng = GameObject.Find("AllManager").GetComponent<AllManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override IEnumerator DoEvent()
    {
        Vector2 currentPosition = transform.position;
        Vector2 nextPosition = currentMoveDirector.position;
        float currentTime = Time.time;
        float elapsedTime = 0f;
        while(Time.time - currentTime < moveTime){
            float t = elapsedTime / moveTime;
            transform.position = Vector2.Lerp(currentPosition, nextPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = nextPosition;
        // Possibly add fixed force to player at this point, or move in fixed curve idk
        allmng.levelEventCounter--;
    }
    public void SetMoveDirector(Transform moveDirector){
        currentMoveDirector = moveDirector;
    }
}

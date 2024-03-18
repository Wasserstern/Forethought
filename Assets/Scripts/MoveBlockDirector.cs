using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockDirector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Trigger enter running");
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            Debug.Log("should set moveDirector");
            GetComponentInParent<MoveBlock>().SetMoveDirector(transform);
        }
    }

}

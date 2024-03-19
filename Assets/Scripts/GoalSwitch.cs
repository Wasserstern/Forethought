using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSwitch : MonoBehaviour
{
    AllManager allmng;
    public bool isOn;
    private void Start(){
        allmng = GameObject.Find("AllManager").GetComponent<AllManager>();
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Solid")){
            isOn = true;
            allmng.switchTriggered.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Solid")){
            isOn = false;
            allmng.switchTriggered.Invoke();
        }
    }
}

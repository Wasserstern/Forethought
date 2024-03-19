using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    AllManager allmng;
    public List<GoalSwitch> switches;
    bool canBeEntered;
    void Start()
    {
        allmng = GameObject.Find("AllManager").GetComponent<AllManager>();
        switches = new List<GoalSwitch>();
        allmng.switchTriggered.AddListener(CheckSwitches);
        if(switches.Count > 0){
            CheckSwitches();
        }
    }
    void CheckSwitches(){
        for(int i = 0; i< switches.Count; i++){
            if(!switches[i].isOn){
                canBeEntered = false;
                break;
            }
            if(i == switches.Count -1 && switches[i].isOn){
                canBeEntered = true;
            }
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(switches.Count > 0){
            for(int i = 0; i < switches.Count; i++){
                if(switches[i].isOn){
                    
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex +1);
        }
    }
}

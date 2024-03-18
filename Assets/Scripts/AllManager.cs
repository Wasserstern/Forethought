using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int levelEventCounter;
    public bool levelEventsActive;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        levelEventsActive = levelEventCounter > 0;
    }
}

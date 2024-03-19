using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AllManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int levelEventCounter;
    public bool levelEventsActive;

    public UnityEvent switchTriggered;

    // Update is called once per frame
    void Update()
    {
        levelEventsActive = levelEventCounter > 0;
    }
}

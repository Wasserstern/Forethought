using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvent : MonoBehaviour
{
    public virtual IEnumerator DoEvent(){
        return null;
    }
}

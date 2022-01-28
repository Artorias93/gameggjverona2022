using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DieZone : MonoBehaviour
{
    
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 8)
        
            col.gameObject.GetComponent<PlayerCollisionInteractions>().eventDie?.Invoke();

    }
}

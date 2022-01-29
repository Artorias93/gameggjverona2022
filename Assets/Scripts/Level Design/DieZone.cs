using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DieZone : MonoBehaviour
{
    
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 8)
        {
            PlayerCollisionInteractions pci = col.gameObject.GetComponent<PlayerCollisionInteractions>();
            pci.eventDie?.Invoke();
            AudioManager.Play(pci.player.deathSoundEvent);
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndGameTrigger : MonoBehaviour
{
    public UnityEvent endLevel;
    private void OnTriggerEnter2D(Collider2D other)
    {
        endLevel?.Invoke();
    }
}

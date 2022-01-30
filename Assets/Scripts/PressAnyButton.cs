using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressAnyButton : MonoBehaviour
{
    public UnityEvent anyButtonPressed;
    private void Update()
    {
        if (Input.anyKey)
        {
            anyButtonPressed?.Invoke();
        }
    }
}

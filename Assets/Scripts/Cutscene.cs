using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour
{
    public UnityEvent cutsceneEnded;

    public void CutsceneEnded()
    {
        cutsceneEnded?.Invoke();
    }
}

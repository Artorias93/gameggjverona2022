using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIViewScoreSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void OnScoreChanged(float score)
    {
        // Debug.Log($"{name} - on score slider updated");
        slider.value = score;
    }

    public void OnMaxScoreSet(float maxScore)
    {
        slider.maxValue = maxScore;
    }
}

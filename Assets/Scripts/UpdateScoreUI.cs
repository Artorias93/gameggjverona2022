using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScoreUI : MonoBehaviour
{
    [SerializeField] private Text label;
    
    public void UpdateScore(float points)
    {
        label.text = $"Score: {((int)points).ToString()}";
    }
}

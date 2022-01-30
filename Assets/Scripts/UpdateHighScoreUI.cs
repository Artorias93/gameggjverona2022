using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHighScoreUI : MonoBehaviour
{
   [SerializeField] private Text label;
    
    public void UpdateHighScore(int score)
    {
        // Debug.Log($"{name} - setting high score on UI: {score.ToString()}");
        label.text = $"High Score: {(score).ToString()}";
    }
}

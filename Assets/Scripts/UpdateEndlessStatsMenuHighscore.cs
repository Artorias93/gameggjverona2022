using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateEndlessStatsMenuHighscore : MonoBehaviour
{
    [SerializeField] public Text text;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            text.text = PlayerPrefs.GetInt("Highscore").ToString();
        }
        else
        {
            text.text = "0";
        }
    }
}

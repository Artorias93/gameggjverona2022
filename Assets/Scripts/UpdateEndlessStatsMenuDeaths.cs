using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateEndlessStatsMenuDeaths : MonoBehaviour
{
    [SerializeField] public Text text;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Deaths"))
        {
            text.text = PlayerPrefs.GetInt("Deaths").ToString();
        }
        else
        {
            text.text = "0";
        }
    }
}

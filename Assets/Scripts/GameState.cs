using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[System.Serializable] public class SingleFloatEvent : UnityEvent<float> {}
[System.Serializable] public class SingleIntEvent : UnityEvent<int> { }

public class GameState : MonoBehaviour
{
    [SerializeField] public float maxScore;
    
    // events
    public SingleFloatEvent scoreChanged;
    public SingleFloatEvent maxScoreSet;
    public UnityEvent maxScoreReached;
    public SingleIntEvent emitHighscoreSoFar;

    private float currentScore;

    private float reducedFrameRate = 1 / 24;
    private float lastUpdateTime;

    private bool stopTrigger = false;
    
    // Start is called before the first frame update
    void Start()
    {
        currentScore = 0.0f;
        scoreChanged?.Invoke(currentScore);
        
        maxScoreSet?.Invoke(maxScore);

        lastUpdateTime = Time.time;
        StartCoroutine(AdvanceScore());
        
        // signal high score so far
        if (!PlayerPrefs.HasKey("Highscore")) {
            PlayerPrefs.SetInt("Highscore", 0);
        }
        emitHighscoreSoFar?.Invoke(PlayerPrefs.GetInt("Highscore"));
    }

    public void StopAndStoreHighscore()
    {
        stopTrigger = true;
        
        // store high score
        var score = (int)currentScore;
        var highScore = PlayerPrefs.GetInt("Highscore");
        if (score > highScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
            // Debug.Log($"{name} - Saving score: {score.ToString()}");
            PlayerPrefs.Save();
        }
        // Debug.Log($"{name} - Score now: {score.ToString()}. Highscore so far: {highScore.ToString()}");
        
        // store also deaths
        int deaths = 0;
        if (PlayerPrefs.HasKey("Deaths"))
        {
            deaths = PlayerPrefs.GetInt("Deaths");
        }
        deaths += 1;
        PlayerPrefs.SetInt("Deaths", deaths);
        Debug.Log($"{name} - Saving deaths: {deaths}");
    }

    private IEnumerator AdvanceScore()
    {
        while (!stopTrigger)
        {
            var elapsedTime = Time.time - lastUpdateTime;

            currentScore += elapsedTime / 1000; // in seconds

            // Debug.Log($"{name} - score changed event");
            scoreChanged?.Invoke(currentScore);

            if (currentScore >= maxScore)
            {
                maxScoreReached?.Invoke();
            }
            
            yield return new WaitForSeconds(reducedFrameRate);
        }
    }
 
}

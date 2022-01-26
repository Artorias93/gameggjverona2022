using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[System.Serializable] public class SingleFloatEvent : UnityEvent<float> {}

public class GameState : MonoBehaviour
{
    [SerializeField] public float maxScore;
    
    // events
    public SingleFloatEvent scoreChanged;
    public SingleFloatEvent maxScoreSet;
    public UnityEvent maxScoreReached;

    private float currentScore;

    private float reducedFrameRate = 1 / 24;
    private float lastUpdateTime;
    
    // Start is called before the first frame update
    void Start()
    {
        currentScore = 0.0f;
        scoreChanged?.Invoke(currentScore);
        
        maxScoreSet?.Invoke(maxScore);

        lastUpdateTime = Time.time;
        StartCoroutine(AdvanceScore());
    }

    private IEnumerator AdvanceScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(reducedFrameRate);
            var elapsedTime = Time.time - lastUpdateTime;

            currentScore += elapsedTime / 1000; // in seconds

            // Debug.Log($"{name} - score changed event");
            scoreChanged?.Invoke(currentScore);

            if (currentScore >= maxScore)
            {
                maxScoreReached?.Invoke();
            }
        }
    }
 
}

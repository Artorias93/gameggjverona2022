using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SceneField[] levels;
    [SerializeField] private Text gameOverOverlay;

    public UnityEvent pauseWorld;

    public void ReloadLevel0After2Sec()
    {
        ReloadLevel(0, 2.0f);
    }

    void ReloadLevel(int levelIdx, float afterSeconds)
    {
        // Debug.Log($"{name} - reloading level. emitting pause world");
        // stop updates of all objects in the scene
        // excluding UI and this very object
        // and stop spawning coroutines
        pauseWorld?.Invoke();

        // show game over overlay
        gameOverOverlay.gameObject.SetActive(true);
        
        // wait and reload
        StartCoroutine(
            ReloadLevelCoro(
                levelIdx, afterSeconds));
    }

    IEnumerator ReloadLevelCoro(int levelIdx, float afterSeconds)
    {
        yield return new WaitForSeconds(afterSeconds);
        ReloadLevelImmediately(levelIdx);
    }
    
    void ReloadLevelImmediately(int levelIdx)
    {
        SceneManager.LoadScene(levels[levelIdx]);
    }
    
}

using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SceneField[] nonLevelScenes;
    [SerializeField] private SceneField pauseOverlayScene;
    [SerializeField] private SceneField[] levels;
    // [SerializeField] private Text gameOverOverlay;
    [SerializeField] private float loadingDelay;
    [SerializeField] private Image fader;
    [SerializeField] private float fadeDuration;
    
    // public UnityEvent gameOver;

    [SerializeField] private int currentLevel;

    private bool paused = false;
    // private static SceneManager instance;

    private void Awake()
    {
        // // singleton
        // if (instance == null)
        // {
        //     instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    // public void LoadNextLevel()
    // {
    //     LoadLevelAfter(currentLevel++, loadingDelay); 
    // }

    public void LoadScene(SceneField scene)
    {
        StartCoroutine(LoadSceneAfterCoro(scene, loadingDelay));
    }

    public void LoadNonLevelScene(int sceneIdx)
    {
        StartCoroutine(LoadSceneAfterCoro(nonLevelScenes[sceneIdx], loadingDelay));
    }

    public void LoadLevel(int levelIdx)
    {
        StartCoroutine(LoadSceneAfterCoro(levels[levelIdx], loadingDelay));
        currentLevel = levelIdx;
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadSceneAfterCoro(levels[currentLevel++], loadingDelay));
    }
    
    // public void LoadLevel(int levelIdx)
    // {
    //     LoadLevelAfter(levelIdx, loadingDelay);
    // }
    
    // public void ReloadLevel0After2Sec()
    // {
    //     LoadSceneAfter(0, 2.0f);
    // }

    // void LoadLevelAfter(int levelIdx, float afterSeconds)
    // {
    //     // Debug.Log($"{name} - reloading level. emitting pause world");
    //     // stop updates of all objects in the scene
    //     // excluding UI and this very object
    //     // and stop spawning coroutines
    //     // pauseWorld?.Invoke();
    //
    //     // show game over overlay
    //     // gameOverOverlay.gameObject.SetActive(true);
    //     
    //     // gameOver?.Invoke();
    //     
    //     // wait and reload
    //     StartCoroutine(
    //         LoadLevelCoro(
    //             levelIdx, afterSeconds));
    // }

    // IEnumerator LoadLevelCoro(int levelIdx, float afterSeconds)
    // {
    //     yield return new WaitForSeconds(afterSeconds);
    //     LoadLevelImmediately(levelIdx);
    // }
    
    // void LoadSceneAfter(SceneField scene, float afterSeconds)
    // {
    //     StartCoroutine(LoadSceneAfterCoro(scene, afterSeconds));
    // }

    IEnumerator LoadSceneAfterCoro(SceneField scene, float afterSeconds)
    {
        // fade in
        // var fader = Instantiate(faderPrefab, Vector3.zero, quaternion.identity);
        fader.enabled = true;
        for (var t = 0.0f; t < fadeDuration; t += Time.deltaTime / fadeDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null; // wait until the next frame
        }
        
        // load with artificial wait
        yield return new WaitForSeconds(afterSeconds); // artificial wait
        LoadSceneImmediately(scene);
        
        
    }

    IEnumerator FadeOut()
    {
        fader.enabled = true;
        fader.color = new Color(0, 0, 0, 1);
        // fade out
        for (var t = 0.0f; t < fadeDuration; t += Time.deltaTime / fadeDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, 1 - t));
            yield return null; // wait until the next frame
        }

        // fader.gameObject.SetActive(false);
        fader.enabled = false;
    }

    void LoadSceneImmediately(SceneField scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.SceneName);
    }
    
    // void LoadLevelImmediately(int levelIdx)
    // {
    //     UnityEngine.SceneManagement.SceneManager.LoadScene(levels[levelIdx]);
    // }
    
    public void PauseWorld()
    {
        paused = true;
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        paused = false;
        Time.timeScale = 1;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!paused){
                PauseWorld();
                UnityEngine.SceneManagement.SceneManager.LoadScene(pauseOverlayScene, LoadSceneMode.Additive);
            } 
            else
            {
                StartCoroutine(ResumeGame());
            }
        }
    }

    IEnumerator ResumeGame()
    {
        AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(pauseOverlayScene.SceneName);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        UnpauseGame();
    }
}

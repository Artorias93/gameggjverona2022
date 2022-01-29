using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Object = UnityEngine.Object;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SceneField[] nonLevelScenes;
    // [SerializeField] private SceneField pauseOverlayScene;
    [SerializeField] private SceneField[] levels;
    // [SerializeField] private Text gameOverOverlay;
    [SerializeField] private float loadingDelay;
    [SerializeField] private Image fader;
    [SerializeField] private float fadeDuration;
    [SerializeField] private Animator pauseOverlayAnim;
    [SerializeField] private Animator resumeButtonAnim;
    [SerializeField] private Animator restartButtonAnim;
    
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
        UnpauseGame(); // call this in case we transitioned after pausing
        pauseOverlayAnim.SetBool("GameOver", false); // in case we transitioned from gameOver
        StartCoroutine(FadeOut());
    }

    // public void LoadNextLevel()
    // {
    //     LoadLevelAfter(currentLevel++, loadingDelay); 
    // }

    public void LoadScene(SceneField scene)
    {
        pauseOverlayAnim.SetBool("Fade", false);
        StartCoroutine(LoadSceneAfterCoro(scene, loadingDelay));
    }

    public void LoadScene(Scene scene)
    {
        pauseOverlayAnim.SetBool("Fade", false);
        StartCoroutine(LoadSceneAfterCoro(scene, loadingDelay));
    }
    
    public void ReloadScene()
    {
        // Debug.Log("Reloading scene");
        // UnpauseGame();
        pauseOverlayAnim.SetBool("Fade", false);
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    public void LoadNonLevelScene(int sceneIdx)
    {
        pauseOverlayAnim.SetBool("Fade", false);
        StartCoroutine(LoadSceneAfterCoro(nonLevelScenes[sceneIdx], loadingDelay));
    }

    public void LoadLevel(int levelIdx)
    {
        pauseOverlayAnim.SetBool("Fade", false);
        StartCoroutine(LoadSceneAfterCoro(levels[levelIdx], loadingDelay));
        currentLevel = levelIdx;
    }

    public void LoadNextLevel()
    {
        pauseOverlayAnim.SetBool("Fade", false);
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
        for (var t = 0.0f; t < fadeDuration; t += Time.unscaledDeltaTime / fadeDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null; // wait until the next frame
        }
        
        // load with artificial wait
        yield return new WaitForSecondsRealtime(afterSeconds); // artificial wait
        LoadSceneImmediately(scene);
        
        
    }
    
    IEnumerator LoadSceneAfterCoro(Scene scene, float afterSeconds)
    {
        // fade in
        // var fader = Instantiate(faderPrefab, Vector3.zero, quaternion.identity);
        // Debug.Log($"{name} - Fading in");
        fader.enabled = true;
        for (var t = 0.0f; t < fadeDuration; t += Time.unscaledDeltaTime / fadeDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null; // wait until the next frame
        }
        
        // Debug.Log($"{name} - artificial wait");
        // load with artificial wait
        yield return new WaitForSecondsRealtime(afterSeconds); // artificial wait
        // Debug.Log($"{name} - load scene immediately");
        LoadSceneImmediately(scene);
        
        
    }

    IEnumerator FadeOut()
    {
        fader.enabled = true;
        fader.color = new Color(0, 0, 0, 1);
        // fade out
        for (var t = 0.0f; t < fadeDuration; t += Time.unscaledDeltaTime / fadeDuration)
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
    
    void LoadSceneImmediately(Scene scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.name);
    }
    // void LoadLevelImmediately(int levelIdx)
    // {
    //     UnityEngine.SceneManagement.SceneManager.LoadScene(levels[levelIdx]);
    // }
    
    public void PauseWorld()
    {
        paused = true;
        
        // we need to set this here because by setting the time scale to 0 
        // the button's animators won't go back to Normal otherwise
        resumeButtonAnim.SetTrigger("Normal");
        
        Time.timeScale = 0;
        pauseOverlayAnim.SetBool("Fade", true);

        // if (button != null)
        // {
        //     button.animator.se
        // }
        // pauseOverlayAnim.SetTrigger();
    }

    public void UnpauseGame()
    {
        pauseOverlayAnim.SetBool("Fade", false);
        Time.timeScale = 1;
        paused = false;
    }
    
    public void GameOver()
    {
        paused = true;
        
        restartButtonAnim.SetTrigger("Normal");
        
        Time.timeScale = 0;
        
        pauseOverlayAnim.SetBool("GameOver", true);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!paused){
                PauseWorld();
                // UnityEngine.SceneManagement.SceneManager.LoadScene(pauseOverlayScene, LoadSceneMode.Additive);
            } 
            else
            {
                UnpauseGame();
                // StartCoroutine(ResumeGame());
            }
        }
    }

    // IEnumerator ResumeGame()
    // {
    //     AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(pauseOverlayScene.SceneName);
    //     while (!asyncUnload.isDone)
    //     {
    //         yield return null;
    //     }
    //     UnpauseGame();
    // }
}

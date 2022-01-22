using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Spawnable[] aboveObstacles;
    [SerializeField] private Spawnable[] belowObstacles;
    [SerializeField] private Transform spawningPointAbove;
    [SerializeField] private Transform spawningPointBelow;
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpawnInterval;
    [SerializeField] private float worldSpeed;
    [SerializeField] private SceneField[] levels;
    [SerializeField] private Text gameOverOverlay;

    private IEnumerator aboveSpawningCoro;
    private IEnumerator belowSpawningCoro;
    
    // Start is called before the first frame update
    void Start()
    {
        aboveSpawningCoro = StartSpawning(true, aboveObstacles);
        belowSpawningCoro = StartSpawning(false, belowObstacles);
        StartCoroutine(aboveSpawningCoro);
        StartCoroutine(belowSpawningCoro);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartSpawning(bool above, Spawnable[] obstacles)
    {
        var rng = new System.Random();
        while (true)
        {
            // wait random time
            var waitFor = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitFor);
            
            // create new random obstacle that will start running
            var idx = rng.Next(obstacles.Length);
            var objToSpawn = obstacles[idx];
            var obj = Instantiate(
                objToSpawn,
                above ? spawningPointAbove.position : spawningPointBelow.position,
                Quaternion.identity);
            
            // give it constant velocity
            obj.gameObject.AddComponent<ConstantVelocity2D>()
                .setVelocity(new Vector2(-worldSpeed, 0));
            
            // adjust its gravity
            if (!above)
            {
                obj.GetComponent<Rigidbody2D>().gravityScale = -1;
            }
        }
    }

    public void ReloadLevel0After2Sec()
    {
        ReloadLevel(0, 2.0f);
    }

    void ReloadLevel(int levelIdx, float afterSeconds)
    {
        // stop updates of all objects in the scene
        // excluding UI and this very object
        // and stop spawning coroutines
        PauseWorld();

        // show game over overlay
        gameOverOverlay.gameObject.SetActive(true);
        
        // wait and reload
        StartCoroutine(
            ReloadLevelCoro(
                levelIdx, afterSeconds));
    }

    private void PauseWorld()
    {
        // stop all objects
        var allGameObjects = FindObjectsOfType<GameObject>();
        foreach (var go in allGameObjects)
        {
            if (go != this.gameObject && go.layer == 0)
            {
                foreach (var c in go.GetComponents<MonoBehaviour>())
                {
                    c.enabled = false;
                }
            }
        }
        
        // stop spawning coroutines
        StopCoroutine(aboveSpawningCoro);
        StopCoroutine(belowSpawningCoro);
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
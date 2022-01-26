using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableInfo
    {
        public Spawnable spawnable;
        public float waitAtLeast;
    }
    
    public SpawnableInfo[] aboveObstacles;
    public SpawnableInfo[] belowObstacles;
    [SerializeField] private Transform spawningPointAbove;
    [SerializeField] private Transform spawningPointBelow;
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpawnInterval;
    [SerializeField] private float worldSpeed;

    private IEnumerator aboveSpawningCoro;
    private IEnumerator belowSpawningCoro;
    
    private int UILayerIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        
        UILayerIndex = LayerMask.NameToLayer("UI");
        
        aboveSpawningCoro = StartSpawning(true, aboveObstacles);
        belowSpawningCoro = StartSpawning(false, belowObstacles);
        StartCoroutine(aboveSpawningCoro);
        StartCoroutine(belowSpawningCoro);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartSpawning(bool above, SpawnableInfo[] spawnableInfos)
    {
        var rng = new System.Random();
        // wait random initial time to de-sync obstacles
        yield return new WaitForSeconds(Random.Range(0.0f, 1.5f));
        while (true)
        {
            // create new random obstacle that will start running
            var idx = rng.Next(spawnableInfos.Length);
            var objToSpawn = spawnableInfos[idx].spawnable;
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
            
            // wait random time + minimum time required to wait after that obstacle
            var waitFor = Random.Range(minSpawnInterval, maxSpawnInterval) + spawnableInfos[idx].waitAtLeast;
            yield return new WaitForSeconds(waitFor);
        }
    }

    public void OnPauseWorld()
    {
        // Debug.Log($"{name} - on pause world");
        // stop all objects
        var allGameObjects = FindObjectsOfType<GameObject>();
        foreach (var go in allGameObjects)
        {
            // BUG: how do we stop objects that are not using Update, but coroutines, like GameState?
            if (go != this.gameObject && go.layer != UILayerIndex)
            {
                foreach (var c in go.GetComponents<MonoBehaviour>())
                {
                    c.enabled = false;
                }
                foreach (var c in go.GetComponents<Rigidbody2D>())
                {
                    c.velocity = Vector2.zero;
                    Destroy(c);
                }
            }
        }

        // stop spawning coroutines
        StopCoroutine(aboveSpawningCoro);
        StopCoroutine(belowSpawningCoro);
    }
}
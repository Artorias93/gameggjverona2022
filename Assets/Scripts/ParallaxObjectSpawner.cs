using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpriteInfo
    {
        public Sprite sprite;
        public float yOffset;
    }

    [SerializeField] private SpriteInfo[] aboveObjects;
    [SerializeField] private SpriteInfo[] belowObjects;

    float vertExtent;
    float horzExtent;

    // Start is called before the first frame update
    void Start()
    {
        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
        StartCoroutine(Spawn(aboveObjects));
        StartCoroutine(Spawn(belowObjects));
    }

    private IEnumerator Spawn(SpriteInfo[] sprites)
    {
        float cameraBoundMaxX = Camera.main.transform.position.x + horzExtent;

        while(true)
        {
            int spriteIndex = Random.Range(0, sprites.Length);
            GameObject g = Instantiate(new GameObject(sprites[spriteIndex].sprite.name), new Vector3(cameraBoundMaxX + Random.Range(5, 20), sprites[spriteIndex].yOffset), Quaternion.identity);
            SpriteRenderer sp = g.AddComponent<SpriteRenderer>();
            sp.sprite = sprites[spriteIndex].sprite;
            sp.sortingLayerName = "Background";
            g.AddComponent<ParallaxBackground>();

            yield return new WaitForSeconds(Random.Range(3, 10));
        }
    }
}

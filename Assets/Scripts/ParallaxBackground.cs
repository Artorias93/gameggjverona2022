using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 speed = new Vector2(-6, 0);
    [SerializeField] private bool infiniteHorizontal = false;
    [SerializeField] private float minSpawnDistance = 0;
    [SerializeField] private float maxSpawnDistance = 10;


    private float tileCoeff = 9;

    private Transform cameraTransform;
    float horizontalSingleSpriteSize;

    private SpriteRenderer spriteRenderer;

    float vertExtent;
    float horzExtent;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        horizontalSingleSpriteSize = spriteRenderer.bounds.max.x - spriteRenderer.bounds.min.x;
        if(infiniteHorizontal)
            spriteRenderer.size = new Vector2(spriteRenderer.size.x * tileCoeff, spriteRenderer.size.y);

        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }

    void Update()
    {
        transform.position += (Vector3) (speed * Time.deltaTime);
        float cameraBoundMinX = cameraTransform.position.x - horzExtent;
        float cameraBoundMaxX = cameraTransform.position.x + horzExtent;

        if(infiniteHorizontal)
        {
            //move the sprite to simulate infinite background
            if (transform.position.x < cameraBoundMinX)
            {
                transform.position = new Vector3(cameraBoundMinX + horizontalSingleSpriteSize - (cameraBoundMinX - transform.position.x), transform.position.y);
            }
        }
        else
        {
            if(spriteRenderer.bounds.max.x < cameraBoundMinX)
            {
                Destroy(gameObject);
            }
        }
    }
}

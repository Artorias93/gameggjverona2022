using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 speed;
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
        spriteRenderer.size = new Vector2(spriteRenderer.size.x * tileCoeff, spriteRenderer.size.y);

        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }

    void Update()
    {
        transform.position += (Vector3) (speed * Time.deltaTime);
        float cameraBoundMinX = cameraTransform.position.x - horzExtent;

        if (transform.position.x < cameraBoundMinX)
        {
            transform.position = new Vector3(cameraBoundMinX + horizontalSingleSpriteSize - (cameraBoundMinX - transform.position.x), transform.position.y);
        }
    }
}

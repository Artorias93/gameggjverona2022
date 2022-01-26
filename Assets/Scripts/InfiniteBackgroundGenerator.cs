using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InfiniteBackgroundGenerator : MonoBehaviour
{
    [SerializeField] private Sprite image;
    [SerializeField] private float scrollingSpeed;

    private GameObject[] pieces;
    
    // Start is called before the first frame update
    void Start()
    {
        // TODO continue and solve this
        // pieces[0] = Instantiate(image, transform.position, Quaternion.identity);
        pieces[0] = InstantiateSprite(image, transform.position);
        var borderX = pieces[0].GetComponent<SpriteRenderer>().bounds.max.x;
        // pieces[1] = Instantiate(image, transform.position, Quaternion.identity);
        pieces[1] = InstantiateSprite(image, transform.position);
        var halfWidth = pieces[1].GetComponent<SpriteRenderer>().bounds.extents.x;
        pieces[1].transform.position += new Vector3(borderX + halfWidth, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject InstantiateSprite(Sprite sprite, Vector2 position)
    {
        Debug.Log("new sprite");
        GameObject go = new GameObject(sprite.name + Random.Range(0, Int32.MaxValue));
        go.transform.position = position;
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        go.transform.parent = transform;
        return go;
    }
}

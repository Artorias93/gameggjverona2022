using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InfiniteBackgroundGenerator : MonoBehaviour
{
    [SerializeField] private Sprite image;
    [SerializeField] private float scrollingSpeed;
    [SerializeField] private float yOffset;
    // we can't serialize SortingLayers. for now it's just a string
    [SerializeField] private string sortingLayer = "Background";
    [SerializeField] private int orderInLayer = 1;
    
    private GameObject[] pieces;
    private GameObject leftmostPiece;
    private GameObject rightmostPiece;
    private float halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        pieces = new GameObject[3];
        
        // left
        pieces[0] = InstantiateSprite(image, transform.position);
        halfWidth = pieces[0].GetComponent<SpriteRenderer>().bounds.extents.x;
        pieces[0].transform.position += new Vector3(- 2 * halfWidth, yOffset, 0);
        leftmostPiece = pieces[0];
        
        // center
        pieces[1] = InstantiateSprite(image, transform.position);
        pieces[1].transform.position += new Vector3(0, yOffset, 0);
        
        // right
        pieces[2] = InstantiateSprite(image, transform.position);
        var borderX = pieces[1].GetComponent<SpriteRenderer>().bounds.max.x;
        pieces[2].transform.position += new Vector3(borderX + halfWidth, yOffset, 0);
        rightmostPiece = pieces[2];
    }

    // Update is called once per frame
    void Update()
    {
        // if rightmost's left bound is at the center
        if (rightmostPiece.GetComponent<SpriteRenderer>().bounds.min.x < 0)
        {
            // move the leftmost to the right of the rightmost
            var rightmostRightBorderX = rightmostPiece.GetComponent<SpriteRenderer>().bounds.max.x;
            leftmostPiece.transform.position = new Vector3(
                rightmostRightBorderX + halfWidth, leftmostPiece.transform.position.y, 0);
        }
    }

    private void FixedUpdate()
    {
        // scroll pieces
        foreach (var piece in pieces)
        {
            piece.transform.position += Vector3.left * Time.fixedDeltaTime * scrollingSpeed;
        }
    }

    private GameObject InstantiateSprite(Sprite sprite, Vector2 position)
    {
        GameObject go = new GameObject(sprite.name + Random.Range(0, Int32.MaxValue));
        // Debug.Log($"{name} - new sprite {go.name}");
        go.transform.position = position;
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        
        // setup sorting layer
        renderer.sortingLayerName = sortingLayer;
        renderer.sortingOrder = orderInLayer;

        go.transform.parent = transform;
        return go;
    }
}

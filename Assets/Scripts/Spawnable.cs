using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
public class Spawnable : MonoBehaviour
{
    //Commented because Start and Update get called even though they do nothing
    /*private void Awake()
    {
        Assert.IsNotNull(GetComponent<Rigidbody2D>());

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}

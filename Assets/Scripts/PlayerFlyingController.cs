using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyingController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private KeyCode upButton;
    [SerializeField] private KeyCode leftButton;
    [SerializeField] private KeyCode downButton;
    [SerializeField] private KeyCode rightButton;
    [SerializeField] public float movementSpeed;
    
    private int horizontalMovement;
    private int verticalMovement;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(upButton) && !Input.GetKey(downButton))
        {
            verticalMovement = 1;
        } else if (Input.GetKey(downButton) && !Input.GetKey(upButton))
        {
            verticalMovement = -1;
        }
        else
        {
            verticalMovement = 0;
        }

        if (Input.GetKey(rightButton) && !Input.GetKey(leftButton))
        {
            horizontalMovement = 1;
        } else if (Input.GetKey(leftButton) && !Input.GetKey(rightButton))
        {
            horizontalMovement = -1;
        }
        else
        {
            horizontalMovement = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(
            horizontalMovement,
            verticalMovement).normalized * movementSpeed;
    }
}

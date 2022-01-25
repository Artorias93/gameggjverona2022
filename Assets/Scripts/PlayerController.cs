using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D coll;
    [SerializeField] private KeyCode upButton;
    [SerializeField] private KeyCode leftButton;
    [SerializeField] private KeyCode downButton;
    [SerializeField] private KeyCode rightButton;
    [SerializeField] public KeyCode usePickupButton;
    [SerializeField] public KeyCode givePickupButton;
    [SerializeField] public float verticalJumpSpeed;
    [SerializeField] public float sideMovementSpeed;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] public LayerMask obstacleLayerMask;
    
    private float heightOfCenter;
    private int horizontalMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        heightOfCenter = coll.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(upButton)
            && isGrounded())
        {
            rb.velocity = new Vector2(
                rb.velocity.x,
                UpsideDownSign() * verticalJumpSpeed
            );
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
            horizontalMovement * sideMovementSpeed,
            rb.velocity.y);
    }

    private bool isGrounded()
    {
        return Physics2D.Raycast(
            transform.position,
            - UpsideDownSign() * Vector2.up,
            heightOfCenter + 0.1f,
            groundLayerMask | obstacleLayerMask
        );
    }

    // +1 if straight, -1 if upside down
    public int UpsideDownSign()
    {
        return Math.Sign(rb.gravityScale);
    }
}

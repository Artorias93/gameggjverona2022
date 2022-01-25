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
    [SerializeField] public float jumpTime; //maybe calculate it based on verticalJumpSpeed?
    [SerializeField] private float jumpAcceleration;
    [SerializeField] public float sideMovementSpeed;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] public LayerMask obstacleLayerMask;
    
    private float heightOfCenter;
    private int horizontalMovement;
    private Timer jumpTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        heightOfCenter = coll.bounds.extents.y;
        jumpTimer = new Timer(jumpTime); // instantiate here just once, then just start and stop and reuse the same
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(upButton)
            && isGrounded())
        {
            // initial impulse
            rb.velocity = new Vector2(rb.velocity.x, UpsideDownSign() * verticalJumpSpeed);
            jumpTimer.Start();
        }
        else if (Input.GetKeyUp(upButton))
        {
            jumpTimer.Reset();
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
        
        // if we are keeping the jump button down, boost me
        if (jumpTimer.Started() && !jumpTimer.HasExpiredIfSoReset())
        {
            rb.AddForce(Vector2.up * UpsideDownSign() * jumpAcceleration, ForceMode2D.Force);
        }
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

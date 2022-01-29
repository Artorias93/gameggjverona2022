using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D coll;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] public LayerMask obstacleLayerMask;
    
    [Header("Controls")]
    [SerializeField] private KeyCode upButton;
    [SerializeField] private KeyCode leftButton;
    [SerializeField] private KeyCode downButton;
    [SerializeField] private KeyCode rightButton;
    [SerializeField] public KeyCode usePickupButton;
    [SerializeField] public KeyCode givePickupButton;
    
    [Header("Jump")]
    [SerializeField] public float verticalJumpSpeed;
    [SerializeField] public float jumpTime; //maybe calculate it based on verticalJumpSpeed?
    [SerializeField] private float jumpAcceleration;
    [SerializeField] public float sideMovementSpeed;
    [SerializeField] private float distanceBufferingThreshold = 1;
    
    [Header("Smash")]
    [SerializeField] private bool canSmash;
    [SerializeField] private float smashSpeed;

    // private float heightOfCenter;
    private int horizontalMovement;
    private Timer jumpTimer;

    /* jump buffer: if the player jumps just before landing (distanceBufferingThreshold),
     * buffer the jump and make the character jump when isGrounded again
     * so the controls doesn't feel like garbage
     */
    private bool jumpBuffer = false;

    private float coyoteTimeOffset = .2f; //coyote time
    
    // Start is called before the first frame update
    void Start()
    {
        // heightOfCenter = coll.bounds.extents.y;
        jumpTimer = new Timer(jumpTime); // instantiate here just once, then just start and stop and reuse the same
    }

    // Update is called once per frame
    void Update()
    {
        if(jumpBuffer && isGrounded())
        {
            jumpBuffer = false;
            // initial impulse
            rb.velocity = new Vector2(rb.velocity.x, UpsideDownSign() * verticalJumpSpeed);
            jumpTimer.Start();
        }
        else if (Input.GetKeyDown(upButton))
        {
            if(isGrounded())
            {
                // initial impulse
                rb.velocity = new Vector2(rb.velocity.x, UpsideDownSign() * verticalJumpSpeed);
                jumpTimer.Start();
            }
            else if(CanBufferJump())
            {
                jumpBuffer = true;
            }
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

        if (canSmash && Input.GetKeyDown(downButton) && !isGrounded())
        {
            rb.velocity = new Vector2(
                rb.velocity.x, 
                rb.velocity.y - UpsideDownSign() * smashSpeed);
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
            // new Vector2(coll.bounds.min.x - coyoteTimeOffset, transform.position.y),
            new Vector2(coll.bounds.min.x - coyoteTimeOffset, UpsideDownSign() > 0 ? coll.bounds.min.y : coll.bounds.max.y),
            UpsideDownSign() * Vector2.down,
              coyoteTimeOffset + 0.1f,
            groundLayerMask | obstacleLayerMask
        );
    }

    private bool CanBufferJump()
    {
        return Physics2D.Raycast(
            // new Vector2(coll.bounds.min.x, transform.position.y),
            new Vector2(coll.bounds.min.x, UpsideDownSign() > 0 ? coll.bounds.min.y : coll.bounds.max.y),
            UpsideDownSign() * Vector2.down,
            // heightOfCenter + distanceBufferingThreshold,
            distanceBufferingThreshold,
            groundLayerMask | obstacleLayerMask
        );
    }

    // +1 if straight, -1 if upside down
    public int UpsideDownSign()
    {
        return Math.Sign(rb.gravityScale);
    }
}

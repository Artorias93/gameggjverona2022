using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantVelocity2D : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.right * velocity.x) * Time.deltaTime;
    }

    public ConstantVelocity2D setVelocity(Vector2 vel)
    {
        velocity = vel;
        return this;
    }
}

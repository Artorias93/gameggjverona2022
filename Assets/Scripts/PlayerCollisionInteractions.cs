using UnityEngine;
using UnityEngine.Events;

public class PlayerCollisionInteractions : MonoBehaviour
{
    [SerializeField] private Collider2D coll;
    [SerializeField] private PlayerController player;

    [Header("No Collision Near Vertex")] 
    [SerializeField] private Transform noCollisionVertex;
    [SerializeField] private float radiusNoCollision = 0.1f;
    
    public UnityEvent eventDie;

    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     
    //     
    //         Vector3 hit = col.contacts[0].normal;
    //         Debug.Log(hit);
    //         float angle = Vector3.Angle(hit, Vector3.up);
    //
    //         if (Mathf.Approximately(angle, 0))
    //         {
    //             //Down
    //             Debug.Log("Down");
    //         }
    //         if (Mathf.Approximately(angle, 180))
    //         {
    //             //Up
    //             Debug.Log("Up");
    //         }
    //         if (Mathf.Approximately(angle, 90))
    //         {
    //             // Sides
    //             Vector3 cross = Vector3.Cross(Vector3.forward, hit);
    //             if (cross.y > 0)
    //             { // left side of the player
    //                 // Debug.Log("Left");
    //             }
    //             else
    //             { // right side of the player
    //                 // Debug.Log("Right");
    //             eventDie?.Invoke();
    //         }
    //         }
    // }
    
    
//     private void OnCollisionEnter2D(Collision2D other)
//     {
//              
//                 
//         // if it's a frontal collision and with a certain kind of object, die
//         var otherMaybeSpawnable = other.gameObject.GetComponent<Spawnable>();
//         var yOfContact = other.GetContact(0).point.y;
// =======
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Spawnable otherMaybeSpawnable = collision.gameObject.GetComponent<Spawnable>();
        Vector3 normal = collision.GetContact(0).normal;
        
        if (otherMaybeSpawnable != null
            && normal != transform.up * player.UpsideDownSign()
            && !IsCloseToNoCollisionVertex(collision.GetContact(0).point))
        {
            eventDie?.Invoke();
        }
    }

    private bool IsCloseToNoCollisionVertex(Vector2 point)
    {
        return (new Vector3(point.x, point.y, 0) - noCollisionVertex.position).sqrMagnitude <
            radiusNoCollision * radiusNoCollision;
    }
}

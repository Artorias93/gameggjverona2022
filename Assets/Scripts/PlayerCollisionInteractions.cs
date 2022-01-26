using UnityEngine;
using UnityEngine.Events;

public class PlayerCollisionInteractions : MonoBehaviour
{
    [SerializeField] private Collider2D coll;
    [SerializeField] private PlayerController player;

    public UnityEvent eventDie;

// HEAD
    void OnCollisionEnter2D(Collision2D col)
    {
        
        
            Vector3 hit = col.contacts[0].normal;
            Debug.Log(hit);
            float angle = Vector3.Angle(hit, Vector3.up);

            if (Mathf.Approximately(angle, 0))
            {
                //Down
                Debug.Log("Down");
            }
            if (Mathf.Approximately(angle, 180))
            {
                //Up
                Debug.Log("Up");
            }
            if (Mathf.Approximately(angle, 90))
            {
                // Sides
                Vector3 cross = Vector3.Cross(Vector3.forward, hit);
                if (cross.y > 0)
                { // left side of the player
                    Debug.Log("Left");
                }
                else
                { // right side of the player
                    Debug.Log("Right");
                eventDie?.Invoke();
            }
            }
    }
    
    /*
    private void OnCollisionEnter2D(Collision2D other)
    {
             
                
        // if it's a frontal collision and with a certain kind of object, die
        var otherMaybeSpawnable = other.gameObject.GetComponent<Spawnable>();
        var yOfContact = other.GetContact(0).point.y;
=======
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Spawnable otherMaybeSpawnable = collision.gameObject.GetComponent<Spawnable>();
        Vector3 normal = collision.GetContact(0).normal;
        
>>>>>>> 3b9393d519b3e52c3a78745fdd41aa1d4d6ea107
        if (otherMaybeSpawnable != null
            && normal != transform.up * player.UpsideDownSign())
        {
            eventDie?.Invoke();
        }
    }*/
}

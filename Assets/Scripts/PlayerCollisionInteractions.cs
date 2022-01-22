using UnityEngine;
using UnityEngine.Events;

public class PlayerCollisionInteractions : MonoBehaviour
{
    [SerializeField] private Collider2D coll;

    public UnityEvent eventDie;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // if it's a frontal collision and with a certain kind of object, die
        var otherMaybeSpawnable = other.gameObject.GetComponent<Spawnable>();
        var yOfContact = other.GetContact(0).point.y;
        if (otherMaybeSpawnable != null
            && yOfContact >= coll.bounds.min.y
            && yOfContact <= coll.bounds.max.y)
        {
             eventDie?.Invoke();
             
             // stop the other so that it doesn't
             // continue to be affected by physics
             // and moves even if we are stopping
             // the world for the game over
             other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}

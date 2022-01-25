using UnityEngine;
using UnityEngine.Events;

public class PlayerCollisionInteractions : MonoBehaviour
{
    [SerializeField] private Collider2D coll;
    [SerializeField] private PlayerController player;

    public UnityEvent eventDie;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Spawnable otherMaybeSpawnable = collision.gameObject.GetComponent<Spawnable>();
        Vector3 normal = collision.GetContact(0).normal;
        
        if (otherMaybeSpawnable != null
            && normal != transform.up * player.UpsideDownSign())
        {
            eventDie?.Invoke();
        }
    }
}

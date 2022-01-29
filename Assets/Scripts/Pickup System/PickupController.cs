using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public LayerMask characterLayerMask;
    public float areInContactDistance = 1f;

    private Pickup currentPickup = null;
    private GameObject currentPickupObject;
    private PlayerController playerController;
    private PickupController otherCharacterPickupController = null;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        //dunno why the pickup object doesn't follow the parent automatically
        //this could be a problem when animating GivePickup
        //maybe due to direct rb.velocity assignment in playercontroller instead of AddForce?
        //TODO: check this
        if (currentPickupObject)
            currentPickupObject.transform.position = transform.position;

        if (currentPickup != null)
        {
            if (Input.GetKeyDown(playerController.usePickupButton))
            {
                currentPickup.Use();
                currentPickup = null;
                Destroy(currentPickupObject);
                currentPickupObject = null; //maybe redundant
            }
            else if (Input.GetKeyDown(playerController.givePickupButton) && AreInContact())
            {
                GivePickup();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Pickup pickup = collision.GetComponent<Pickup>();
        if (pickup != null && currentPickup == null)
        {
            currentPickup = pickup;
            currentPickupObject = collision.gameObject;

            collision.transform.parent = this.transform;
            collision.transform.position = transform.position;

            AudioManager.Play(playerController.pickupSoundEvent);
        }
    }

    private bool AreInContact()
    {
        //Debug.DrawRay(transform.position, Vector2.down * playerController.UpsideDownSign() * areInContactDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down * playerController.UpsideDownSign(), areInContactDistance, characterLayerMask);

        if (hit && !otherCharacterPickupController) //lazy initialization
            otherCharacterPickupController = hit.collider.GetComponent<PickupController>();

        return hit;
    }

    private void GivePickup()
    {
        if(otherCharacterPickupController)
        {
            otherCharacterPickupController.TakePickup(currentPickup, currentPickupObject);
            currentPickup = null;
            currentPickupObject = null;
        }
    }

    public void TakePickup(Pickup pickup, GameObject pickupObject)
    {
        currentPickup = pickup;
        currentPickupObject = pickupObject;
        pickupObject.transform.position = transform.position;
        pickupObject.transform.parent = this.transform;
    }
}

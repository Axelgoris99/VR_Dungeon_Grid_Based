using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    BoxCollider box;
    public PlayerController player;
    // Start is called before the first frame update
    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("LeftController"))
        {
            if(player.focusLeft != null)
            {
                player.focusLeft.Interact();
                player.interactingLeft = false;
            }
        }
        if (other.CompareTag("RightController"))
        {
            if (player.focusRight != null)
            {
                player.focusRight.Interact();
                player.interactingRight = false;
            }
        }
    }
}

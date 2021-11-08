using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    BoxCollider box;
    public PlayerController player;
    Inventory3D inventaire;
    // Start is called before the first frame update
    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        inventaire = player.inventory3D.GetComponent<Inventory3D>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("LeftController"))
        {
            if(player.focusLeft != null)
            {
                player.focusLeft.Interact();
                player.interactingLeft = false;
                inventaire.UpdateUI();
            }
        }
        if (other.CompareTag("RightController"))
        {
            if (player.focusRight != null)
            {
                player.focusRight.Interact();
                player.interactingRight = false;
                inventaire.UpdateUI();
            }
        }
    }
}

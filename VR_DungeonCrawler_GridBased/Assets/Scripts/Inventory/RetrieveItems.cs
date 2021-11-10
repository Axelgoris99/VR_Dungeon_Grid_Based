using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class RetrieveItems : MonoBehaviour
{
    private Controls playerInput;
    public PlayerController player;
    public Inventory3D inventaire;
    InventorySlot invent;

    GameObject itemToSpawn;
    void Awake()
    {
        playerInput = new Controls();
        invent = GetComponent<InventorySlot>();
    }
    private void OnEnable()
    {
        playerInput.Player.InteractLeft.Enable();
        playerInput.Player.InteractRight.Enable();
    }
    private void OnDisable()
    {
        playerInput.Player.InteractLeft.Disable();
        playerInput.Player.InteractRight.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerInput.Player.InteractLeft.triggered || playerInput.Player.InteractRight.triggered)
        {
            if (invent.Item != null)
            {
                if (other.CompareTag("LeftController"))
                {
                    if (player.focusLeft == null)
                    {
                        SpawnItem(ref player.leftController, ref player.interactingRight);
                        player.focusLeft = itemToSpawn.GetComponent<ItemPickup>();
                    }
                }
                if (other.CompareTag("RightController"))
                {
                    if (player.focusRight == null)
                    {
                        SpawnItem(ref player.rightController, ref player.interactingRight);
                        player.focusRight = itemToSpawn.GetComponent<ItemPickup>();
                    }
                }
            }
        }
    }


    void SpawnItem(ref Transform controllerTransform, ref bool interacting) {
        itemToSpawn = Instantiate(invent.Item.prefabObject, controllerTransform);
        //itemToSpawn.AddComponent<MeshFilter>().mesh = invent.Item.mesh;
        //itemToSpawn.AddComponent<MeshCollider>().convex = true;
        //itemToSpawn.AddComponent<Outline>();
        //itemToSpawn.AddComponent<ItemPickup>().item = invent.Item;
        //itemToSpawn.AddComponent<MeshRenderer>();
        itemToSpawn.name = invent.Item.prefabObject.name;
        invent.RemoveItemFromInventory();
        invent.ClearSlot();
        
        interacting = true;
    }
}

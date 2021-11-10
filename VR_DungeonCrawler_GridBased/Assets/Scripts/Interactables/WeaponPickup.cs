using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable
{
	public Item item;   // Item to put in the inventory if picked up

	// When the player interacts with the item
	public override void Interact()
	{
		base.Interact();

		PickUp();
	}

	public override void OnDefocused()
	{
		Destroy(gameObject);
	}

	public override void InHand(Transform controller)
	{
		transform.position = controller.position;
		transform.parent = controller;
	}
	// Pick up the item
	void PickUp()
	{
		Inventory.instance.Add(item);   // Add to inventory
		Destroy(gameObject);    // Destroy item from scene
	}

	public override void Use()
	{
		item.Use();
		Destroy(gameObject);
	}
}

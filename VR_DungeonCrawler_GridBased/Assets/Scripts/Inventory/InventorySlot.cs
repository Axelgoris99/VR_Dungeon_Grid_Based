using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour {
	public GameObject itemSlot;
	Item item;  // Current item in the slot
	Renderer render;
	float radius;

    private void Awake()
    {
		render = GetComponent<Renderer>();
		radius = render.bounds.size.x;
    }

    // Add item to the slot
    public void AddItem (Item newItem)
	{
		itemSlot.transform.localScale = new Vector3(1f,1f,1f);
		item = newItem;
		itemSlot.GetComponent<MeshFilter>().mesh = item.mesh;
		float diagonal = itemSlot.GetComponent<MeshRenderer>().bounds.size.magnitude;

		if(diagonal >= radius)
        {
			itemSlot.transform.localScale *= (radius/diagonal);
        }


	}

	// Clear the slot
	public void ClearSlot ()
	{
		item = null;
		itemSlot.GetComponent<MeshFilter>().mesh = null;

	}

	// If the remove button is pressed, this function will be called.
	public void RemoveItemFromInventory ()
	{
		Inventory.instance.Remove(item);
	}

	// Use the item
	public void UseItem ()
	{
		if (item != null)
		{
			item.Use();
		}
	}

}

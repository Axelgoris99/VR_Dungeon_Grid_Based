using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory3D : MonoBehaviour
{
    public GameObject backpack;  // The entire UI
    public Transform itemsParent;   // The parent object of all the items

    Inventory inventory;	// Our current inventory
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
    }

	// Check to see if we should open/close the inventory
	void Update()
	{
		if (Input.GetButtonDown("Inventory"))
		{
			backpack.SetActive(!backpack.activeSelf);
			UpdateUI();
		}
	}

	// Update the inventory UI by:
	//		- Adding items
	//		- Clearing empty slots
	// This is called using a delegate on the Inventory.
	public void UpdateUI()
	{
		InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();

		for (int i = 0; i < slots.Length; i++)
		{
			if (i < inventory.items.Count)
			{
				slots[i].AddItem(inventory.items[i]);
			}
			else
			{
				slots[i].ClearSlot();
			}
		}
	}

}

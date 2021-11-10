using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory3D : MonoBehaviour
{
    public GameObject backpack;  // The entire UI
    public Transform itemsParent;   // The parent object of all the items

    Inventory inventory;    // Our current inventory
	//public float tol = 90f;
    // Start is called before the first frame update
    void Start()
	{ 
        inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;
    }

	// Check to see if we should open/close the inventory
	void Update()
	{
		//Vector3 euler = transform.rotation.eulerAngles;
		//if (euler.x < 360-tol && euler.x > 360/2 ) euler.x = 360-tol;
		//else if (euler.x > tol && euler.x < 360/2) euler.x = tol;
		//if (euler.y< 360 - tol && euler.y > 360 / 2) euler.y = 360 - tol;
		//else if (euler.y > tol && euler.y < 360 / 2) euler.y = tol;
		//if (euler.z < 360 - tol && euler.z > 360 / 2) euler.z = 360 - tol;
		//else if (euler.z > tol && euler.z < 360 / 2) euler.z = tol;

		transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);

        
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

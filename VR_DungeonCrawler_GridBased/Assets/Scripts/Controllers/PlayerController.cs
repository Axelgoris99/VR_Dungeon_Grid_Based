using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
/* Controls the player. Here we chose our "focus" and where to move. */

public class PlayerController : MonoBehaviour {

	public delegate void OnFocusChanged(Interactable newFocus);
	public OnFocusChanged onFocusChangedCallback;

	public Interactable focusLeft;	// Our current focus: Item, Enemy etc.
	public Interactable focusRight;	// Our current focus: Item, Enemy etc.

	public LayerMask movementMask;		// The ground
	public LayerMask interactionMask;	// Everything we can interact with

	public Camera cam;              // Reference to our camera

	public Transform leftController;
	public Transform rightController;
	[SerializeField] private Vector3 orientation = new Vector3(0f,-3f,0f);

	private Controls playerInput;
	[SerializeField] float rayCastDist = 1.0f;
	public bool interactingLeft = false;
	public bool interactingRight = false;
	public bool holdingInventoryLeft = false;
	public bool holdingInventoryRight = false;

	public AnimatedGridMovement refGrid;

	public GameObject inventory3D;
	BoxCollider boxInventory;
	private Inventory3D invent;
	Vector3 originalSizeBox; 

	public float heightBackpack = 0.65f;


	private void Awake()
    {
		playerInput = new Controls();
		boxInventory = inventory3D.GetComponent<BoxCollider>();
		invent = inventory3D.GetComponent<Inventory3D>();
		originalSizeBox = new Vector3(boxInventory.size.x, boxInventory.size.y, boxInventory.size.z);
	}
    private void OnEnable()
    {
		rayCastDist = refGrid.GridSize;

		playerInput.Player.InteractLeft.performed += InteractLeft;
		playerInput.Player.InteractLeft.Enable();

		playerInput.Player.InteractRight.performed += InteractRight;
		playerInput.Player.InteractRight.Enable();
	}
    private void OnDisable()
    {
		playerInput.Player.InteractLeft.Disable();
		playerInput.Player.InteractRight.Disable();
    }
    // Get references
    void Start ()
	{
		cam = Camera.main;
	}

    void InteractLeft(InputAction.CallbackContext button)
    {
		Interact(leftController, ref interactingLeft, ref focusLeft, ref holdingInventoryLeft);     
    }

    void InteractRight(InputAction.CallbackContext button)
    {
		Interact(rightController, ref interactingRight, ref focusRight, ref holdingInventoryRight);
	}


	void Interact(Transform controller, ref bool interacting, ref Interactable focus, ref bool holdingInventory)
    {
		if (interacting)
		{
			interacting = false;
			focus.OnDefocused();
			focus = null;
		}
		if (!interacting)
		{
			if (!holdingInventory)
			{
				if (boxInventory.bounds.Intersects(controller.GetComponent<BoxCollider>().bounds))
				{
					invent.backpack.SetActive(true);
					invent.UpdateUI();
					inventory3D.transform.parent = controller;
					inventory3D.transform.position = controller.TransformPoint(Vector3.zero);
					boxInventory.size = originalSizeBox /= 3;
				}
				else
				{
					// Shoot out a ray
					Ray ray = new Ray(controller.position, controller.forward + controller.TransformDirection(orientation));
					RaycastHit hit;
					// If we hit
					if (Physics.Raycast(ray, out hit, rayCastDist, interactionMask))
					{
						SetFocus(ref focus, hit.collider.GetComponent<Interactable>(), controller);
						interacting = true;
					}
				}
			}
			if (holdingInventory)
			{
				inventory3D.transform.parent = null;
				RaycastHit hit;
				if (Physics.Raycast(inventory3D.transform.position, Vector3.down, out hit))
				{
					/*
					 * Set the target location to the location of the hit.
					 */
					Vector3 targetLocation = hit.point;
					/*
					 * Modify the target location so that the object is being perfectly aligned with the ground (if it's flat).
					 */
					targetLocation += new Vector3(0, heightBackpack, 0);
					/*
					 * Move the object to the target location.
					 */
					transform.position = targetLocation;
				}
			}

		}

		if (inventory3D.transform.parent == controller)
		{
			holdingInventory = true;
		}
		else
		{
			holdingInventory = false;
		}
	}


    // Set our focus to a new focus
    void SetFocus (ref Interactable side, Interactable newFocus, Transform controller)
	{
		if (onFocusChangedCallback != null)
			onFocusChangedCallback.Invoke(newFocus);

		// If our focus has changed
		if (side != newFocus && side != null)
		{
			// Let our previous focus know that it's no longer being focused
			side.OnDefocused();
		}

		// Set our focus to what we hit
		// If it's not an interactable, simply set it to null
		side = newFocus;

		if (side != null)
		{
			// Let our focus know that it's being focused
			side.OnFocused(transform, controller);
		}

	}

}

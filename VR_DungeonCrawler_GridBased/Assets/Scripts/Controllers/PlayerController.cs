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
	private InputAction interactLeft;
	private InputAction interactRight;
	[SerializeField] float rayCastDist = 1.0f;
	public bool interactingLeft = false;
	public bool interactingRight = false;

	public AnimatedGridMovement refGrid;

	public GameObject inventory3D;
	BoxCollider boxInventory;
	private void Awake()
    {
		playerInput = new Controls();
		boxInventory = inventory3D.GetComponent<BoxCollider>();
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
		if (interactingLeft)
		{
			interactingLeft = false;
			focusLeft.OnDefocused();
			focusLeft = null;
		}
		if (!interactingLeft)
		{
			if (boxInventory.bounds.Intersects(leftController.GetComponent<BoxCollider>().bounds))
			{
				inventory3D.GetComponent<Inventory3D>().backpack.SetActive(true);
				inventory3D.transform.parent = leftController;
				inventory3D.transform.position = leftController.InverseTransformPoint(Vector3.zero);
			}
			else
			{
				// Shoot out a ray
				Ray ray = new Ray(leftController.position, leftController.forward + leftController.TransformDirection(orientation));
				//================= MOUSE AND KEYBOARD INSTEAD ============ //
				ray = cam.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				// If we hit
				if (Physics.Raycast(ray, out hit, rayCastDist, interactionMask))
				{
					SetFocus(ref focusLeft, hit.collider.GetComponent<Interactable>(), leftController);
					interactingLeft = true;
				}
			}
		}
        

    }

    void InteractRight(InputAction.CallbackContext button)
    {
		if (interactingRight)
		{
			interactingRight = false;
			focusRight.OnDefocused();
			focusRight = null;
		}
		if (!interactingRight)
		{
			if (boxInventory.bounds.Intersects(rightController.GetComponent<BoxCollider>().bounds))
			{
				inventory3D.GetComponent<Inventory3D>().backpack.SetActive(true);
				inventory3D.transform.parent = rightController;
				inventory3D.transform.position = rightController.TransformPoint(Vector3.zero);
			}
			else
			{
				// Shoot out a ray
				Ray ray = new Ray(rightController.position, rightController.forward + rightController.TransformDirection(orientation));
				RaycastHit hit;
				// If we hit
				if (Physics.Raycast(ray, out hit, rayCastDist, interactionMask))
				{
					SetFocus(ref focusRight, hit.collider.GetComponent<Interactable>(), rightController);
					interactingRight = true;
				}
			}
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

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
/* Controls the player. Here we chose our "focus" and where to move. */

public class PlayerController : MonoBehaviour {

	public delegate void OnFocusChanged(Interactable newFocus);
	public OnFocusChanged onFocusChangedCallback;

	public Interactable focus;	// Our current focus: Item, Enemy etc.

	public LayerMask movementMask;		// The ground
	public LayerMask interactionMask;	// Everything we can interact with

	public Camera cam;              // Reference to our camera

	public Transform leftController;
	public Transform rightController;
	private Controls playerInput;
	private InputAction interactLeft;
	private InputAction interactRight;

	private void Awake()
    {
		playerInput = new Controls();
    }
    private void OnEnable()
    {
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

		// Shoot out a ray
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If we hit
        if (Physics.Raycast(ray, out hit, 1f, interactionMask))
        {
            SetFocus(hit.collider.GetComponent<Interactable>());
        }

    }

	void InteractRight(InputAction.CallbackContext button)
	{
		
		// Shoot out a ray
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		// If we hit
		if (Physics.Raycast(ray, out hit, 1f, interactionMask))
		{
			SetFocus(hit.collider.GetComponent<Interactable>());
		}

	}

	// Set our focus to a new focus
	void SetFocus (Interactable newFocus)
	{
		if (onFocusChangedCallback != null)
			onFocusChangedCallback.Invoke(newFocus);

		// If our focus has changed
		if (focus != newFocus && focus != null)
		{
			// Let our previous focus know that it's no longer being focused
			focus.OnDefocused();
		}

		// Set our focus to what we hit
		// If it's not an interactable, simply set it to null
		focus = newFocus;

		if (focus != null)
		{
			// Let our focus know that it's being focused
			focus.OnFocused(transform);
		}

	}

}

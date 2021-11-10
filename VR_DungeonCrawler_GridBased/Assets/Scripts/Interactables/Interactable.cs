using UnityEngine;
using UnityEngine.AI;

/*	
	This component is for all objects that the player can
	interact with such as enemies, items etc. It is meant
	to be used as a base class.
*/

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour {

	public float radius = 3f;
	public Transform interactionTransform;

	protected bool isFocus = false;   // Is this interactable currently being focused?
	public bool IsFocus;
	protected Transform player;       // Reference to the player transform
	protected Transform controller;
	protected bool hasInteracted = false; // Have we already interacted with the object?

	
    private void Awake()
    {
        if(interactionTransform == null)
        {
			interactionTransform = transform;
        }
    }
    void Update ()
	{
		if (isFocus)	// If currently being focused
		{
			float distance = Vector3.Distance(player.position, interactionTransform.position);
			// If we haven't already interacted and the player is close enough
			if (!hasInteracted && distance <= radius)
			{
				// Interact with the object
				hasInteracted = true;
				InHand(controller);
			}
		}
	}

	// Called when the object starts being focused
	public void OnFocused (Transform playerTransform, Transform controllerTransform)
	{
		isFocus = true;
		hasInteracted = false;
		player = playerTransform;
		controller = controllerTransform;
    }

	// Called when the object is no longer focused
	public virtual void OnDefocused ()
	{
		isFocus = false;
		hasInteracted = false;
		player = null;
		controller = null;
		transform.parent = null;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit))
		{
			/*
			 * Set the target location to the location of the hit.
			 */
			Vector3 targetLocation = hit.point;
			/*
			 * Modify the target location so that the object is being perfectly aligned with the ground (if it's flat).
			 */
			
			targetLocation += new Vector3(0, GetComponent<MeshRenderer>().bounds.extents.y, 0);
			/*
			 * Move the object to the target location.
			 */
			transform.position = targetLocation;
		}
	}

	// This method is meant to be overwritten
	public virtual void Interact ()
	{
		
	}

	// This one too
	public virtual void InHand(Transform controller)
    {

    }

	public virtual void Use()
	{
		
	}
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}

}

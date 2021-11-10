using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllersOutline : MonoBehaviour
{
    public LayerMask interactionMask;	// Everything we can interact with
    private GameObject objectHit;
    private Outline component;
    private Vector3 orientation = new Vector3(0f, -3f, 0f);
    public AnimatedGridMovement refGrid;
    [SerializeField] float rayCastDist;

    private void OnEnable()
    {
        rayCastDist = refGrid.GridSize;

    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward + transform.TransformDirection(orientation));
        RaycastHit hit;
        // If we hit
        if (Physics.Raycast(ray, out hit, rayCastDist, interactionMask))
        { 
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("wallMask"))
            {
                if (component == null)
                {
                    objectHit = hit.transform.gameObject;
                    component = objectHit.GetComponent<Outline>();
                    if (component.enabled == false)
                    {
                        component.enabled = true;
                    }
                }
            }
            else
            {
                if (component != null)
                {
                    if (component.enabled == true)
                    {
                        component.enabled = false;
                        component = null;
                    }
                }
            }
        }
        else
        {
            if (component != null)
            {
                if (component.enabled == true)
                {
                    component.enabled = false;
                    component = null;
                }
            }
        }
    }
}

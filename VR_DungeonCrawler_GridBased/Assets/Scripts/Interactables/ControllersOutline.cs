using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllersOutline : MonoBehaviour
{
    public LayerMask interactionMask;	// Everything we can interact with
    private GameObject objectHit;
    private Outline component;
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // If we hit
        if (Physics.Raycast(ray, out hit, 1f, interactionMask))
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamaged : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Touch");
        Debug.Log(collision.gameObject.layer.ToString());
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            Debug.Log("Successful hit");
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalHit : MonoBehaviour
{
    public GameObject Mesh;
    public GameObject prefab;
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
        Debug.Log("test");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            if(collision.relativeVelocity.x > 2*collision.relativeVelocity.y && collision.relativeVelocity.x > 2*collision.relativeVelocity.z)
            {
                GameObject obj = Instantiate(prefab);
                obj.transform.parent = gameObject.transform.parent;
                obj.transform.position = gameObject.transform.position + new Vector3(0f, 0.2f, 0f);
                obj.transform.rotation = gameObject.transform.rotation;
                //obj.GetComponent<CriticalHit>().Mesh = Mesh;
                //obj.GetComponent<CriticalHit>().prefab = prefab;
                Destroy(gameObject);
            }
        }
    }
}

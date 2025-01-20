using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryRigitbody : MonoBehaviour
{
    Rigidbody rigidbody;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        if (rigidbody != null) 
        {
            Destroy(rigidbody);
            GetComponent<DestoryRigitbody>().enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
}

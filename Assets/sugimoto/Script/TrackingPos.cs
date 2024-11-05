using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPos : MonoBehaviour
{
    [SerializeField] GameObject move_obj;
    [SerializeField] Transform target_pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move_obj.transform.position = new Vector3 (target_pos.position.x,move_obj.transform.position.y,target_pos.position.z);
    }
}

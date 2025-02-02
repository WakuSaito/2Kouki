using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public Transform start_pos;
    public Transform taret_pos;
    public GameObject target_obj;

    float time = 0.0f;

    Vector3 befor_pos;

    // Start is called before the first frame update
    void Start()
    {
        befor_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

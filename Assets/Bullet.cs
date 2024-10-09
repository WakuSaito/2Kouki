using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    const float G = 9.80665f;

    [SerializeField] float speed = 2.0f; 
    [SerializeField] float mass = 1.0f; 

    float vec_y = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Œü‚¢‚Ä‚¢‚é•ûŒü‚ÖˆÚ“®
        transform.position += transform.forward * speed * Time.deltaTime;
        //’e‚Ì—Ž‰º‘¬“x
        transform.position += new Vector3(0, -vec_y, 0) * Time.deltaTime;
        vec_y += G * Time.deltaTime * mass;

        Destroy(this.gameObject, 3.0f);
    }
}

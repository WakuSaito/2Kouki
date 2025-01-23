using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    //�d��
    const float GRAVITY = -9.81f;

    [SerializeField] float speed = 12f;
    private Vector3 m_objVelocity;
    bool m_gravity_flag = true;

    // Start is called before the first frame update
    void Start()
    {
        m_objVelocity = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_gravity_flag)
        {
            //��������
            m_objVelocity.y += GRAVITY * Time.deltaTime * speed;
            transform.position = m_objVelocity;
            if(transform.position.y>=0.0f)
            {
                m_gravity_flag = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    Vector3 m_beforPos;
    bool m_hitFlag = false;

    // Update is called once per frame
    void Update()
    {
        if (m_beforPos == transform.position && m_hitFlag)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().isTrigger = true;
        }

        m_beforPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" || collision.gameObject.tag != null)
        {
            m_hitFlag = true;
        }
    }
}

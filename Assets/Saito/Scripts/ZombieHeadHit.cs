using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHeadHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Attack") return;

        transform.root.gameObject.GetComponent<Zombie>().DamageHead();
    }
}

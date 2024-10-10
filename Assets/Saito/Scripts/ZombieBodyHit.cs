using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBodyHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Attack") return;

        transform.root.gameObject.GetComponent<Zombie>().DamageBody();
    }
}

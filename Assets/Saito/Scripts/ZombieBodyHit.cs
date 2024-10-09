using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBodyHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        transform.root.gameObject.GetComponent<Zombie>().DamageBody();
    }
}

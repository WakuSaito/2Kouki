using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBodyHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("body�ƐڐG");
        if (other.tag != "pistol") return;

        transform.root.gameObject.GetComponent<Zombie>().DamageBody();
    }
}

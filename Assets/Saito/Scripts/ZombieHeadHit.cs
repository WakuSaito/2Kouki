using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHeadHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("head�ƐڐG");
        if (other.tag != "pistol") return;

        transform.root.gameObject.GetComponent<Zombie>().DamageHead();
    }
}

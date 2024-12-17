using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
    public Transform handR = null;
    public Transform handL = null;

    public Transform knife_hand_R = null;

    public GameObject target_obj;
    public Transform target_pos;

    [SerializeField] GameObject player;

    private Animator animator;

    public bool onIK = false;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        switch (player.GetComponent<player>().WeaponInventory.select_weapon)
        {
            case WeaponInventory.Sloat_Order.GUN:
                onIK = true;
                handL = player.GetComponent<player>().hand_weapon.GetComponent<SetHandIK>().HandL;
                handR = player.GetComponent<player>().hand_weapon.GetComponent<SetHandIK>().HandR;
                break;
            case WeaponInventory.Sloat_Order.KNIFE:
                onIK = true;
                break;
            default:
                onIK = false;
                break;
        }


        if (!onIK) return;


        if (player.GetComponent<player>().WeaponInventory.select_weapon == WeaponInventory.Sloat_Order.GUN)
        {
            if (handR != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, handR.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, handR.rotation);
            }
            if (handL != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, handL.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, handL.rotation);
            }
        }
        else if (player.GetComponent<player>().WeaponInventory.select_weapon == WeaponInventory.Sloat_Order.KNIFE)
        {
            if (knife_hand_R != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, knife_hand_R.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, knife_hand_R.rotation);
            }
        }
    }

    void OnIK()
    {
        onIK = true;
    }
}

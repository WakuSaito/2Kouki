using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
    public Transform handR = null;
    public Transform handL = null;

    [SerializeField] GameObject player;

    private Animator animator;

    public bool onIK = false;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {

        if (player.GetComponent<Inventory>().hand_weapon == Inventory.WEAPON_ID.PISTOL )
        {
            onIK = true;
        }
        else
        {
            onIK = false;
        }

        if (!onIK) return;


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

    void OnIK()
    {
        onIK = true;
    }
}

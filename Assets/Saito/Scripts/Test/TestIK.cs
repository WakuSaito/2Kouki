using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIK : MonoBehaviour
{

    public Transform handR = null;
    public Transform handL = null;

    private Animator animator;

    public bool onIK = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
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
}

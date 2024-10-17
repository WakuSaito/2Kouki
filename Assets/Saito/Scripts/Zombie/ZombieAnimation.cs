using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimation : ZombieBase
{
    [SerializeField]//アニメーター
    private Animator animator;

    public override void SetUpZombie()
    {
        
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void Walk()
    {
        animator.SetTrigger("Walk");
    }

    public void Idle()
    {
        animator.SetTrigger("Idle");
    }
}

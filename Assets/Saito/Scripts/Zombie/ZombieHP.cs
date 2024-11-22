using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHP : ZombieBase
{
    //�ő�̗�
    [SerializeField] private int maxHP = 10;

    //���݂̗̑�
    private int currentHP;

    //���S�t���O
    private bool isDead = false;

    public override void SetUpZombie()
    {
        currentHP = maxHP;
    }

    /// <summary>
    /// �_���[�W�֐�
    /// </summary>
    public void Damage(int _damage)
    {
        if (_damage <= 0) return;

        Debug.Log("�]���r�Ƀ_���[�W:" + _damage);

        if(currentHP - _damage <= 0)
        {
            currentHP = 0;
            isDead = true;
        }
        else
        {
            currentHP -= _damage;
        }

        Debug.Log("�]���r��HP:" + currentHP);
    }

    //�Q�b�^�[
    public bool IsDead()
    {
        return isDead;
    }
    public int GetCurrentHP()
    {
        return currentHP;
    }
    public int GetMaxHP()
    {
        return maxHP;
    }
}

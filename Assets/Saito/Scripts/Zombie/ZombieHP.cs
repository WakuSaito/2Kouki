using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �]���r�̗̓N���X
/// �̗͂̊Ǘ��A�������s��
/// </summary>
public class ZombieHP : ZombieBase
{
    //�ő�̗�
    [SerializeField] private int m_maxHP = 10;

    //���݂̗̑�
    private int m_currentHP;

    //���S�t���O
    private bool m_isDead = false;

    public override void SetUpZombie()
    {
        m_currentHP = m_maxHP;
    }

    /// <summary>
    /// �_���[�W�֐�
    /// </summary>
    public void Damage(int _damage)
    {
        if (_damage <= 0) return;

        Debug.Log("�]���r�Ƀ_���[�W:" + _damage);

        if(m_currentHP - _damage <= 0)
        {
            m_currentHP = 0;
            m_isDead = true;
        }
        else
        {
            m_currentHP -= _damage;
        }

        Debug.Log("�]���r��HP:" + m_currentHP);
    }

    //�Q�b�^�[
    public bool IsDead()
    {
        return m_isDead;
    }
    public int GetCurrentHP()
    {
        return m_currentHP;
    }
    public int GetMaxHP()
    {
        return m_maxHP;
    }
}

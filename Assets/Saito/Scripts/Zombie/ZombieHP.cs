using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHP : ZombieBase
{
    //最大体力
    [SerializeField] private int m_maxHP = 10;

    //現在の体力
    private int m_currentHP;

    //死亡フラグ
    private bool m_isDead = false;

    public override void SetUpZombie()
    {
        m_currentHP = m_maxHP;
    }

    /// <summary>
    /// ダメージ関数
    /// </summary>
    public void Damage(int _damage)
    {
        if (_damage <= 0) return;

        Debug.Log("ゾンビにダメージ:" + _damage);

        if(m_currentHP - _damage <= 0)
        {
            m_currentHP = 0;
            m_isDead = true;
        }
        else
        {
            m_currentHP -= _damage;
        }

        Debug.Log("ゾンビのHP:" + m_currentHP);
    }

    //ゲッター
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

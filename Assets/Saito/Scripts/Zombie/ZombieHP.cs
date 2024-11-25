using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHP : ZombieBase
{
    //最大体力
    [SerializeField] private int maxHP = 10;

    //現在の体力
    private int currentHP;

    //死亡フラグ
    private bool isDead = false;

    public override void SetUpZombie()
    {
        currentHP = maxHP;
    }

    /// <summary>
    /// ダメージ関数
    /// </summary>
    public void Damage(int _damage)
    {
        if (_damage <= 0) return;

        Debug.Log("ゾンビにダメージ:" + _damage);

        if(currentHP - _damage <= 0)
        {
            currentHP = 0;
            isDead = true;
        }
        else
        {
            currentHP -= _damage;
        }

        Debug.Log("ゾンビのHP:" + currentHP);
    }

    //ゲッター
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttack
{
    /*[SerializeField] インスペクターから設定*/
    /// <summary> ゲージオブジェクト </summary>
    [SerializeField] GameObject m_inventoryObj;

    /*プライベート　private*/
    /// <summary> 今の数値 </summary>
    InventoryWeapon m_inventoryWeapon;

    /// <summary>
    /// 武器インベントリ取得
    /// </summary>
    public void SetAttack()
    {
        m_inventoryWeapon = m_inventoryObj.GetComponent<InventoryWeapon>();
    }

    /// <summary>
    /// ナイフ攻撃
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void AttackKnife(bool _phsh)
    {
        if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.KNIFE) return;

        m_inventoryWeapon.m_weaponSlotObj[(int)SLOT_ORDER.KNIFE].GetComponent<knifeAttackAnimetion>().AttackAnimation(_phsh);
    }

    /// <summary>
    /// 銃のリロード
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void GunReload(bool _phsh)
    {
        if (!_phsh) return;
        if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.GUN) return;

        m_inventoryWeapon.m_weaponSlotObj[(int)SLOT_ORDER.GUN].GetComponent<GunManager>().Reload();
    }

    /// <summary>
    /// 銃の単発攻撃
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void AttackGunSingle(bool _phsh)
    {
        if (!_phsh) return;
        if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.GUN) return;

        m_inventoryWeapon.m_weaponSlotObj[(int)SLOT_ORDER.GUN].GetComponent<GunManager>().PullTriggerDown();
    }

    /// <summary>
    /// 銃の連射攻撃
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void AttackGunRapidFire(bool _phsh)
    {
        if (!_phsh) return;
        if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.GUN) return;

        m_inventoryWeapon.m_weaponSlotObj[(int)SLOT_ORDER.GUN].GetComponent<GunManager>().PullTrigger();
    }
}


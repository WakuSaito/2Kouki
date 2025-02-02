using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttack
{
    /*[SerializeField] �C���X�y�N�^�[����ݒ�*/
    /// <summary> �Q�[�W�I�u�W�F�N�g </summary>
    [SerializeField] GameObject m_inventoryObj;

    /*�v���C�x�[�g�@private*/
    /// <summary> ���̐��l </summary>
    InventoryWeapon m_inventoryWeapon;

    /// <summary>
    /// ����C���x���g���擾
    /// </summary>
    public void SetAttack()
    {
        m_inventoryWeapon = m_inventoryObj.GetComponent<InventoryWeapon>();
    }

    /// <summary>
    /// �i�C�t�U��
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void AttackKnife(bool _phsh)
    {
        if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.KNIFE) return;

        m_inventoryWeapon.m_weaponSlotObj[(int)SLOT_ORDER.KNIFE].GetComponent<knifeAttackAnimetion>().AttackAnimation(_phsh);
    }

    /// <summary>
    /// �e�̃����[�h
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void GunReload(bool _phsh)
    {
        if (!_phsh) return;
        if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.GUN) return;

        m_inventoryWeapon.m_weaponSlotObj[(int)SLOT_ORDER.GUN].GetComponent<GunManager>().Reload();
    }

    /// <summary>
    /// �e�̒P���U��
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void AttackGunSingle(bool _phsh)
    {
        if (!_phsh) return;
        if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.GUN) return;

        m_inventoryWeapon.m_weaponSlotObj[(int)SLOT_ORDER.GUN].GetComponent<GunManager>().PullTriggerDown();
    }

    /// <summary>
    /// �e�̘A�ˍU��
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void AttackGunRapidFire(bool _phsh)
    {
        if (!_phsh) return;
        if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.GUN) return;

        m_inventoryWeapon.m_weaponSlotObj[(int)SLOT_ORDER.GUN].GetComponent<GunManager>().PullTrigger();
    }
}


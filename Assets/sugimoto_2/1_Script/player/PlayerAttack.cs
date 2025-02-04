using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttack
{
    /*[SerializeField] �C���X�y�N�^�[����ݒ�*/
    /// <summary> �C���x���g���I�u�W�F�N�g </summary>
    [SerializeField] GameObject m_playerObj;
    /// <summary> �C���x���g���I�u�W�F�N�g </summary>
    [SerializeField] GameObject m_dogObj;

    /*�v���C�x�[�g�@private*/
    /// <summary> InventoryWeapon�N���X </summary>
    InventoryWeapon m_inventoryWeapon;
    /// <summary> SearchViewArea�N���X </summary>
    SearchViewArea m_searchViewArea;
    /// <summary> PlayerSound�N���X </summary
    PlayerSound m_playerSound;

    /// <summary>
    /// ����C���x���g���擾
    /// �T�[�`�G���A�擾
    /// �v���C���[�T�E���h�擾
    /// </summary>
    public void SetAttack()
    {
        //�R���|�[�l���g�擾
        m_inventoryWeapon = m_playerObj.GetComponent<InventoryWeapon>();
        m_searchViewArea = m_playerObj.GetComponent<SearchViewArea>();
        m_playerSound = m_playerObj.GetComponent<PlayerSound>();
    }

    /// <summary>
    /// �莝���̃I�u�W�F�N�g�𒲂ׂ�
    /// </summary>
    /// <returns>�莝���̃I�u�W�F�N�g�A���̏ꍇ�͌��{��</returns>
    public GameObject HandWeapon()
    {
        if (SelectWeaponSlot() != SLOT_ORDER.DOG)
        {
            return m_inventoryWeapon.m_weaponSlotObj[(int)m_inventoryWeapon.m_selectSlot];
        }
        else
        {
            return m_dogObj;
        }
    }

    public SLOT_ORDER SelectWeaponSlot()
    {
        return m_inventoryWeapon.m_selectSlot;
    }

    /// <summary>
    /// �i�C�t�U��
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void AttackKnife(bool _phsh)
    {
        if (SelectWeaponSlot() != SLOT_ORDER.KNIFE) return;

        HandWeapon().GetComponent<knifeAttackAnimetion>().AttackAnimation(_phsh);
    }

    /// <summary>
    /// �e�̃����[�h
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void GunReload(bool _phsh)
    {
        if (!_phsh) return;
        if (SelectWeaponSlot() != SLOT_ORDER.GUN) return;

        HandWeapon().GetComponent<GunManager>().Reload();
    }

    /// <summary>
    /// �e�̒P���U��
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void AttackGunSingle(bool _phsh)
    {
        if (!_phsh) return;
        if (SelectWeaponSlot() != SLOT_ORDER.GUN) return;

        HandWeapon().GetComponent<GunManager>().PullTriggerDown();
    }

    /// <summary>
    /// �e�̘A�ˍU��
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    public void AttackGunRapidFire(bool _phsh)
    {
        if (!_phsh) return;
        if (SelectWeaponSlot() != SLOT_ORDER.GUN) return;

        HandWeapon().GetComponent<GunManager>().PullTrigger();
    }

    /// <summary>
    /// ���̍U��
    /// </summary>
    /// <param name="phsh">���͂���Ă��邩</param>
    /// <param name="_search">SearchViewArea�N���X</param>
    /// <param name="_se">PlayerSound�N���X</param>
    public void AttackDog(bool phsh)
    {
        //���̃X���b�g�ȊO�̏ꍇ�]���r�̐F�����ɖ߂�
        if (SelectWeaponSlot() != SLOT_ORDER.DOG)
        {
            m_searchViewArea.ResetColor("Zombie");
            return;
        }

        //��ԋ߂��]���r�I�u�W�F�N�g���擾
        GameObject targt_zombie_obj = m_searchViewArea.GetObjUpdate("Zombie", 20f, 0.5f);

        if (!phsh || targt_zombie_obj == null) return;

        m_playerSound.PlayWhistleAttack();//se
        HandWeapon().GetComponent<DogManager>().OrderAttack(targt_zombie_obj.GetComponentInParent<ZombieManager>().gameObject);
    }

    /// <summary>
    /// ���̒T���X�L��
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    /// <param name="_se">PlayerSound�N���X</param>
    public void SearchSkillDog(bool _phsh)
    {
        if (!_phsh) return;
        if (SelectWeaponSlot() != SLOT_ORDER.DOG) return;

        m_playerSound.PlayWhistleDetect();//se
        HandWeapon().GetComponent<DogManager>().OrderDetection();
    }
}


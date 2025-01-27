using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�i�C�t�g�p�t�F�C�Y</para>
/// �i�C�t���g���A�]���r��|���t�F�C�Y
/// �������Ɍ������ԂɂȂ�
/// </summary>
public class PhaseUseKnife : TutorialBase
{
    //����̍��W
    [SerializeField] private GameObject m_targetZombieObj;

    //�C���x���g���̊J�Ď��p
    [SerializeField] private InventoryManager m_inventoryManager;
    //����C���x���g��
    [SerializeField] private InventoryWeapon m_inventoryWeapon;

    //����؂�ւ��𑣂�UI
    [SerializeField] private GameObject m_plzChangeWeaponUI;
    //����؂�ւ��𑣂�UI
    [SerializeField] private GameObject m_plzUseKnifeUI;

    [SerializeField] private DogManager m_dogManager;
    //�J
    [SerializeField] private GameObject m_dogWhistle;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("�i�C�t�Ń]���r��|����");
        m_tutorialManager.CreateMarker(m_targetZombieObj.transform.position);
    }

    public override void UpdatePhase()
    {
        if (m_inventoryWeapon == null) return;

        //�C���x���g�����J���Ă���Ƃ��͎ז��ɂȂ�̂ŏ���
        if (m_inventoryManager.m_inventoryState == INVENTORY.CHEST)
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseKnifeUI.SetActive(false);
        }
        //if �i�C�t�������Ă��Ȃ��Ȃ� �X���b�g�؂�ւ��𑣂�
        else if (m_inventoryWeapon.mSelectSlot != SLOT_ORDER.KNIFE)
        {
            m_plzChangeWeaponUI.SetActive(true);
            m_plzUseKnifeUI.SetActive(false);
        }
        //�����Ă���Ȃ� �U���𑣂�
        else
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseKnifeUI.SetActive(true);

        }

        //�]���r��|������
        //�t�F�[�Y�ڍs
        if (m_targetZombieObj == null)
        {
            //�������ԂɂȂ�
            m_dogManager.OnStopAction(false);
            //�J����������
            m_inventoryWeapon.WeaponGet(m_dogWhistle);

            m_tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();

        m_plzChangeWeaponUI.SetActive(false);
        m_plzUseKnifeUI.SetActive(false);
    }
}

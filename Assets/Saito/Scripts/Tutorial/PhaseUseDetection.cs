using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>�T�m�g�p�t�F�C�Y</para>
/// �T�m�̎g�������o���Ă��炤
/// </summary>
public class PhaseUseDetection : TutorialBase
{
    //����؂�ւ��𑣂�UI
    [SerializeField] private GameObject m_plzChangeWeaponUI;
    //����؂�ւ��𑣂�UI
    [SerializeField] private GameObject m_plzUseDetectionUI;

    //�C���x���g���֘A
    [SerializeField] private InventoryManager m_inventoryManager;
    [SerializeField] private InventoryWeapon m_inventoryWeapon;

    [SerializeField] private DogManager m_dogManager;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("���Ɏw�����o��\n�Ƃ𒲂ׂ悤");
    }

    public override void UpdatePhase()
    {
        //�C���x���g�����J���Ă���Ƃ��͎ז��ɂȂ�̂ŏ���
        if (m_inventoryManager.m_inventoryState == INVENTORY.ITEM ||
            m_inventoryManager.m_inventoryState == INVENTORY.CHEST)
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseDetectionUI.SetActive(false);
        }
        else if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.DOG)
        {
            //�J�������Ă��Ȃ��Ȃ� ��������悤����
            m_plzChangeWeaponUI.SetActive(true);
            m_plzUseDetectionUI.SetActive(false);
        }    
        else
        {
            //�J�������Ă���Ȃ�@�g���悤����
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseDetectionUI.SetActive(true);
        }

        //�T�m���g������
        if (m_dogManager.UsedOrderDetection())
        {
            //���̃t�F�[�Y�ɐi�߂�
            m_tutorialManager.NextPhase();

        }
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();

        m_plzChangeWeaponUI.SetActive(false);
        m_plzUseDetectionUI.SetActive(false);
    }

}

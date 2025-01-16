using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseUseDetection : TutorialBase
{
    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject m_plzChangeWeaponUI;
    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject m_plzUseDetectionUI;

    private Inventory m_inventory;

    [SerializeField]
    private DogManager m_dogManager;

    public override void SetUpPhase()
    {
        m_inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        m_tutorialManager.SetText("���Ɏw�����o��\n�Ƃ𒲂ׂ悤");
    }

    public override void UpdatePhase()
    {
        //�C���x���g�����J���Ă���Ƃ��͎ז��ɂȂ�̂ŏ���
        if (m_inventory.item_inventory_flag == true)
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseDetectionUI.SetActive(false);
        }
        //if �J�������Ă��Ȃ��Ȃ�@��������悤����
        else if (m_inventory.hand_weapon != Inventory.WEAPON_ID.DOG)
        {
            m_plzChangeWeaponUI.SetActive(true);
            m_plzUseDetectionUI.SetActive(false);
        }
        //�����Ă���Ȃ�@�g���悤����
        else
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseDetectionUI.SetActive(true);
        }

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

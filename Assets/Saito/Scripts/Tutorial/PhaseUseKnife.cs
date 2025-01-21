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
    [SerializeField]//����̍��W
    private GameObject m_targetZombieObj;

    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject m_plzChangeWeaponUI;
    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject m_plzUseKnifeUI;

    private Inventory m_inventory;

    private WeaponInventory m_weaponInventory;

    [SerializeField]
    private DogManager m_dogManager;

    [SerializeField]//�J
    private GameObject m_dogWhistle;

    public override void SetUpPhase()
    {
        m_inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        //weaponInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<weaponInventory>();

        m_tutorialManager.SetText("�i�C�t�Ń]���r��|����");
        m_tutorialManager.CreateMarker(m_targetZombieObj.transform.position);
    }

    public override void UpdatePhase()
    {
        if (m_inventory == null) return;

        //�C���x���g�����J���Ă���Ƃ��͎ז��ɂȂ�̂ŏ���
        if (m_inventory.item_inventory_flag == true)
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseKnifeUI.SetActive(false);
        }
        //if �i�C�t�������Ă��Ȃ��Ȃ� �X���b�g�؂�ւ��𑣂�
        else if (m_inventory.hand_weapon != Inventory.WEAPON_ID.KNIFE)
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
            m_inventory.weapon_hand_obj[3] = m_dogWhistle;
            //weaponInventory.weapon[3] = dogWhistle;

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

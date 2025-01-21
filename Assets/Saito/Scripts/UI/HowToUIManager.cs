using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�������UI�Ǘ��N���X</para>
/// ��Ԃɂ���đ�������̕\����ύX����
/// </summary>
public class HowToUIManager : MonoBehaviour
{
    [SerializeField]//�ړ�
    private GameObject m_moveUI;
    [SerializeField]//����؂�ւ�
    private GameObject m_slotChangeUI;
    [SerializeField]//�E�� �E����A�C�e�������鎞�ɒ����ɔz�u���Ă���������
    private GameObject m_pickUpUI;

    [SerializeField]//�U���i�i�C�t�j
    private GameObject m_attackUI;

    [SerializeField]//���C
    private GameObject m_shotUI;
    [SerializeField]//�����[�h
    private GameObject m_reloadUI;

    [SerializeField]//�U���w��
    private GameObject m_attackOrderUI;
    [SerializeField]//�T�m�w��
    private GameObject m_detectOrderUI;

    [SerializeField]//�A�C�e���g�p
    private GameObject m_useItemUI;

    private Inventory m_inventory;
    [SerializeField]//���̎w�����\�����ׂ�p
    private DogManager m_dogManager;

    private bool m_isOpenBag = false;
    private Inventory.WEAPON_ID m_gripWeaponID;

    private void Awake()
    {
        m_inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    //�C���x���g���A���̏�Ԃɂ���ĕ\����ς���
    //�C���x���g���̌`���ς�����̂ŕύX����K�v�����邩��
    private void Update()
    {
        if (m_inventory == null) return;

        //��Ԃ��ς���Ă��Ȃ����
        if (m_isOpenBag == m_inventory.item_inventory_flag &&
            m_gripWeaponID == m_inventory.hand_weapon) return;

        //�o�b�O���J���Ă��邩�擾
        m_isOpenBag = m_inventory.item_inventory_flag;
        m_gripWeaponID = m_inventory.hand_weapon;

        HideUI();//�S�Ĕ�\��

        //�o�b�O���J���Ă���ꍇ
        if (m_isOpenBag)
        {
            m_useItemUI.SetActive(true);
        }
        //���Ă���ꍇ
        else
        {
            m_moveUI.SetActive(true);
            m_slotChangeUI.SetActive(true);
            m_pickUpUI.SetActive(true);

            switch (m_inventory.hand_weapon)
            {
                case Inventory.WEAPON_ID.KNIFE:
                    m_attackUI.SetActive(true);
                    break;
                case Inventory.WEAPON_ID.PISTOL:
                    m_shotUI.SetActive(true);
                    m_reloadUI.SetActive(true);
                    break;
                case Inventory.WEAPON_ID.DOG:
                    m_attackOrderUI.SetActive(true);
                    m_detectOrderUI.SetActive(true);
                    break;
            }
        }


        if (m_dogManager == null) return;

        //�����s���\���ɂ����UI�̓����x�ύX
        if(m_dogManager.CanOrderAttack())
            m_attackOrderUI.GetComponent<CanvasGroup>().alpha = 1f;
        else
            m_attackOrderUI.GetComponent<CanvasGroup>().alpha = 0.5f;

        if (m_dogManager.CanOrderDetection())
            m_detectOrderUI.GetComponent<CanvasGroup>().alpha = 1f;
        else
            m_detectOrderUI.GetComponent<CanvasGroup>().alpha = 0.5f;
    }

    /// <summary>
    /// UI�̈ꊇ��\��
    /// </summary>
    private void HideUI()
    {
        m_moveUI.SetActive(false);
        m_slotChangeUI.SetActive(false);
        m_pickUpUI.SetActive(false);
        m_attackUI.SetActive(false);
        m_shotUI.SetActive(false);
        m_reloadUI.SetActive(false);
        m_attackOrderUI.SetActive(false);
        m_detectOrderUI.SetActive(false);
        m_useItemUI.SetActive(false);
    }
}

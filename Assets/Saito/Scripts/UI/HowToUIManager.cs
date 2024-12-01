using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToUIManager : MonoBehaviour
{
    [SerializeField]//�ړ�
    private GameObject moveUI;
    [SerializeField]//����؂�ւ�
    private GameObject slotChangeUI;
    [SerializeField]//�E��
    private GameObject pickUpUI;

    [SerializeField]//�U���i�i�C�t�j
    private GameObject attackUI;

    [SerializeField]//���C
    private GameObject shotUI;
    [SerializeField]//�����[�h
    private GameObject reloadUI;

    [SerializeField]//�U���w��
    private GameObject attackOrderUI;
    [SerializeField]//�T�m�w��
    private GameObject detectOrderUI;

    [SerializeField]//�A�C�e���g�p
    private GameObject useItemUI;

    private Inventory inventory;

    private bool isOpenBag = false;
    private Inventory.WEAPON_ID gripWeaponID;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void Update()
    {
        if (inventory == null) return;

        //��Ԃ��ς���Ă��Ȃ����
        if (isOpenBag == inventory.item_inventory_flag &&
            gripWeaponID == inventory.hand_weapon) return;

        //�o�b�O���J���Ă��邩�擾
        isOpenBag = inventory.item_inventory_flag;
        gripWeaponID = inventory.hand_weapon;

        HideUI();//�S�Ĕ�\��

        //�o�b�O���J���Ă���ꍇ
        if (isOpenBag)
        {
            useItemUI.SetActive(true);
        }
        //���Ă���ꍇ
        else
        {
            moveUI.SetActive(true);
            slotChangeUI.SetActive(true);
            pickUpUI.SetActive(true);

            switch (inventory.hand_weapon)
            {
                case Inventory.WEAPON_ID.KNIFE:
                    attackUI.SetActive(true);
                    break;
                case Inventory.WEAPON_ID.PISTOL:
                    shotUI.SetActive(true);
                    reloadUI.SetActive(true);
                    break;
                case Inventory.WEAPON_ID.DOG:
                    attackOrderUI.SetActive(true);
                    detectOrderUI.SetActive(true);
                    break;
            }

        }

    }

    //�S�Ĕ�\���ɂ���
    private void HideUI()
    {
        moveUI.SetActive(false);
        slotChangeUI.SetActive(false);
        pickUpUI.SetActive(false);
        attackUI.SetActive(false);
        shotUI.SetActive(false);
        reloadUI.SetActive(false);
        attackOrderUI.SetActive(false);
        detectOrderUI.SetActive(false);
        useItemUI.SetActive(false);
    }
}

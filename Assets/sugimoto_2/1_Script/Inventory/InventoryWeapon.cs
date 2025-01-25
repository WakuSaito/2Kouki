using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SLOT_ORDER
{
    HAND,
    KNIFE,
    GUN,
    DOG,
}

public class InventoryWeapon : MonoBehaviour
{
    /// <summary>�X���b�g�ɂ���e�ȊO�̎�ނ̐�</summary>
    const int OTHER_GUN_TYPE_NUM = 2;

    //�C���x���g���̗v�f
    public InventoryClass m_Inventory;
    public int m_sloatSize = 4;
    public Transform[] m_SlotBoxTrans;
    public Transform[] m_spriteTrans;

    //�I�u�W�F�N�g
    [SerializeField] GameObject m_uiObj; //�C���x���g��UI
    public GameObject[] m_weaponSlotObj;
    [SerializeField] Transform m_frame;
    [SerializeField] GameObject m_weaponParent;                 //�e�̐e�I�u�W�F�N�g
    [SerializeField] GameObject[] m_saveOtherGun = new GameObject[OTHER_GUN_TYPE_NUM];



    public SLOT_ORDER m_selectSlot = SLOT_ORDER.HAND;
    Color mColorAlphaHalf = new Color(1.0f, 1.0f, 1.0f, 0.5f);//������
    Color mColorAlphaFull = new Color(1.0f, 1.0f, 1.0f, 1.0f);//�s����

    // Start is called before the first frame update
    void Start()
    {
        //�C���x���g���̃C���X�g���N�^�쐬
        m_Inventory = new InventoryClass(m_sloatSize, m_SlotBoxTrans);

        //mWeaponSlotObj�ɂ���I�u�W�F�N�g�̏����X���b�g�ɓ����
        ItemSlotSet();
    }

    // Update is called once per frame
    void Update()
    {
        SetUI(m_spriteTrans);
        SetWeapon();
    }

    void ItemSlotSet()
    {
        for (int i = 0; i < m_weaponSlotObj.Length; i++)
        {
            if (m_weaponSlotObj[i] == null) continue;

            ITEM_ID id = m_weaponSlotObj[i].GetComponent<ItemSetting>().iteminfo.id;

            //�A�C�e��ID�ɂ���ď�������X���b�g���قȂ�
            switch (id)
            {
                case ITEM_ID.PISTOL:
                case ITEM_ID.ASSAULT:
                case ITEM_ID.SHOTGUN:
                    m_Inventory.Slots[(int)SLOT_ORDER.GUN].ItemInfo = m_weaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.HAND:
                    m_Inventory.Slots[(int)SLOT_ORDER.HAND].ItemInfo = m_weaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.KNIFE:
                    m_Inventory.Slots[(int)SLOT_ORDER.KNIFE].ItemInfo = m_weaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.DOG_DIRECTION:
                    m_Inventory.Slots[(int)SLOT_ORDER.DOG].ItemInfo = m_weaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
            }
        }
    }

    public GameObject ChangeWeapon()
    {
        //��]�̎擾
        float mouse_wheel = Input.GetAxis("Mouse ScrollWheel");

        //�}�E�X�z�C�[���ɓ�������������ύX
        //if (Mathf.Abs(mouse_wheel) != 0)
        //{
        //    //���݂̕����\��
        //    mWeaponSlotObj[(int)mSelectSlot].SetActive(false);
        //    sprite[(int)mSelectSlot].GetComponent<Image>().color = mColorAlphaHalf;
        //    ////�C���x���g���\��
        //    //display_timer = 0.0f;
        //    //display_flag = true;

        //    //if (weapon[(int)select_weapon].GetComponent<ItemInformation>().id == ITEM_ID.PISTOL)
        //    //{
        //    //    weapon[(int)select_weapon].GetComponent<GunManager>().StopReload();
        //    //}
        //}

        //�}�E�X�z�C�[������
        if (mouse_wheel < 0)
        {
            //���̕���C���x���g����
            m_selectSlot++;
            //����C���x���g���̗̈�𒴂�����ŏ��ɖ߂�
            if ((int)m_selectSlot >= m_sloatSize)
            {
                m_selectSlot = 0;
            }

            //����C���x���g���̒��g�������Ȃ���Β��g�̂��镐���
            while (m_weaponSlotObj[(int)m_selectSlot] == null)
            {
                if (m_weaponSlotObj[(int)m_selectSlot] == null)
                {
                    //���̕���C���x���g����
                    m_selectSlot++;
                    //����C���x���g���̗̈�𒴂�����ŏ��ɖ߂�
                    if ((int)m_selectSlot >= m_sloatSize)
                    {
                        m_selectSlot = 0;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        //���
        if (mouse_wheel > 0)
        {
            //�O�̕���C���x���g��
            m_selectSlot--;
            if (m_selectSlot < 0)
            {
                //����C���x���g���̗̈�𒴂�����Ō�ɂ���
                m_selectSlot = (SLOT_ORDER)m_sloatSize - 1;
            }

            //����C���x���g���̒��g�������Ȃ���Β��g�̂��镐���
            while (m_weaponSlotObj[(int)m_selectSlot] == null)
            {
                if (m_weaponSlotObj[(int)m_selectSlot] == null)
                {
                    //�O�̕���C���x���g��
                    m_selectSlot--;
                    if (m_selectSlot < 0)
                    {
                        //����C���x���g���̗̈�𒴂�����Ō�ɂ���
                        m_selectSlot = (SLOT_ORDER)m_sloatSize - 1;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        //�}�E�X�z�C�[���ɓ�������������ύX
        //if (Mathf.Abs(mouse_wheel) != 0)
        //{
        //    //�����Ă��镐���ύX
        //    mWeaponSlotObj[(int)mSelectSlot].SetActive(true);
        //    sprite[(int)mSelectSlot].GetComponent<Image>().color = mColorAlphaFull;
        //    mFrame.position = slot_box[(int)mSelectSlot].transform.position;
        //}
        return m_weaponSlotObj[(int)m_selectSlot];
    }

    public int CanWeaponGet(GameObject _item)
    {
        ITEM_ID id = _item.GetComponent<ItemSetting>().iteminfo.id;

        //�擾���镐��͏e�ƌ��p�A�C�e���݂̂Ȃ̂�HAND��KNIFE�͏ȗ�
        switch (id)
        {
            case ITEM_ID.PISTOL:
            case ITEM_ID.ASSAULT:
            case ITEM_ID.SHOTGUN:
                    return (int)SLOT_ORDER.GUN;
            case ITEM_ID.DOG_DIRECTION:
                    return (int)SLOT_ORDER.DOG;
        }

        return -1;
    }

    public void WeaponGet(GameObject _item)
    {
        ITEM_ID id = _item.GetComponent<ItemSetting>().iteminfo.id;

        //�擾���镐��͏e�ƌ��p�A�C�e���݂̂Ȃ̂�HAND��KNIFE�͏ȗ�
        switch (id)
        {
            case ITEM_ID.PISTOL:
            case ITEM_ID.ASSAULT:
            case ITEM_ID.SHOTGUN:
                if (m_weaponSlotObj[(int)SLOT_ORDER.GUN] == null)
                {
                    m_Inventory.Slots[(int)SLOT_ORDER.GUN].ItemInfo = _item.GetComponent<ItemSetting>().iteminfo;
                    m_weaponSlotObj[(int)SLOT_ORDER.GUN] = _item.GetComponent<ItemSetting>().iteminfo.weaponitem_info.weapon_obj;
                    //�I��ł��镐�킪HAND�̏ꍇ�E���������I��ł��镐��ɕύX
                    if (m_selectSlot == SLOT_ORDER.HAND) m_selectSlot = SLOT_ORDER.GUN;
                }
                break;
            case ITEM_ID.DOG_DIRECTION:
                if (m_weaponSlotObj[(int)SLOT_ORDER.DOG] == null)
                {
                    m_Inventory.Slots[(int)SLOT_ORDER.DOG].ItemInfo = _item.GetComponent<ItemSetting>().iteminfo;
                    m_weaponSlotObj[(int)SLOT_ORDER.DOG] = _item.GetComponent<ItemSetting>().iteminfo.weaponitem_info.weapon_obj;
                    //�I��ł��镐�킪HAND�̏ꍇ�E���������I��ł��镐��ɕύX
                    if (m_selectSlot == SLOT_ORDER.HAND) m_selectSlot = SLOT_ORDER.DOG;
                }
                break;
        }

        //����擾�̍ۂ̐ݒ�
        ParentChildren(m_weaponParent.gameObject, _item);
        _item.GetComponent<GunManager>().GetItemSetting();

    }

    public void GunObjChenge(ItemInformation _inventoryitem_item)
    {
        //���̕�����\��
        m_weaponSlotObj[(int)SLOT_ORDER.GUN].SetActive(false);
        //����ւ�������ɕύX
        m_weaponSlotObj[(int)SLOT_ORDER.GUN] = _inventoryitem_item.weaponitem_info.weapon_obj;
    }


    /// <summary>
    /// UI���Z�b�g����
    /// �C���x���g���̃X���b�g��UI������
    /// �C���x���g���N���X�ɂ���̂ƈႢ�AText�Ȃ��o�[�W����
    /// </summary>
    /// <param name="_sprite">�X�v���C�g������g�����X�t�H�[��</param>
    public void SetUI(Transform[] _sprite)
    {
        //�X�v���C�g
        for (int slot = 0; slot < m_sloatSize; slot++)
        {
            if (m_Inventory.Slots[slot].ItemInfo == null)
            {
                //�A�C�e����񂪂Ȃ��ꍇ

                _sprite[slot].gameObject.SetActive(false);          //��\��
                _sprite[slot].GetComponent<Image>().sprite = null;  //�X�v���C�g������
            }
            else
            {
                //�A�C�e����񂪂���ꍇ

                _sprite[slot].gameObject.SetActive(true);                                             //�\��
                _sprite[slot].GetComponent<Image>().sprite = m_Inventory.Slots[slot].ItemInfo.sprite;   //�X���b�g�ɂ���A�C�e����񂩂�X�v���C�g����
            }
        }
    }

    public void SetWeapon()
    {
        for (int slot = 0; slot < m_sloatSize; slot++)
        {
            //�A�C�e����񂪂Ȃ�
            if (m_weaponSlotObj[slot] == null) continue;

            //�I�����Ă���X���b�g�̃I�u�W�F�N�g�\��
            if (slot == (int)m_selectSlot)
            {
                m_weaponSlotObj[(int)m_selectSlot].SetActive(true);
                m_spriteTrans[(int)m_selectSlot].GetComponent<Image>().color = mColorAlphaFull;
                m_frame.position = m_spriteTrans[(int)m_selectSlot].position;
            }
            else
            {
                //��\��

                m_weaponSlotObj[slot].SetActive(false);
                m_spriteTrans[slot].GetComponent<Image>().color = mColorAlphaHalf;
            }
        }
    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        //�e�q�֌W�ɐݒ�

        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

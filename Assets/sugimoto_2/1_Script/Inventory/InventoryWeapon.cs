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
    const int OTHER_GUN_TYPE_NUM = 2;

    //�C���x���g���}�l�[�W���[
    InventoryManager mInventoryManager;

    //�C���x���g���̗v�f
    public InventoryClass Inventory;
    public int slot_size = 4;
    public Transform[] sprite;
    public Transform[] slot_box;

    //�I�u�W�F�N�g
    [SerializeField] GameObject mInventoryManagerObj;
    [SerializeField] GameObject mWeapon_inventory_UI_obj; //�C���x���g��UI
    public GameObject[] mWeaponSlotObj;
    [SerializeField] Transform mFrame;
    [SerializeField] GameObject mWeaponParent;                 //�e�̐e�I�u�W�F�N�g
    [SerializeField] GameObject[] m_saveOtherGun = new GameObject[OTHER_GUN_TYPE_NUM];



    public SLOT_ORDER mSelectSlot = SLOT_ORDER.HAND;
    Color mColorAlphaHalf = new Color(1.0f, 1.0f, 1.0f, 0.5f);//������
    Color mColorAlphaFull = new Color(1.0f, 1.0f, 1.0f, 1.0f);//�s����

    // Start is called before the first frame update
    void Start()
    {
        //�C���x���g���̃C���X�g���N�^�쐬
        Inventory = new InventoryClass(slot_size, slot_box);
        //�C���x���g���}�l�[�W���[�擾
        mInventoryManager = mInventoryManagerObj.GetComponent<InventoryManager>();

        //mWeaponSlotObj�ɂ���I�u�W�F�N�g�̏����X���b�g�ɓ����
        ItemSlotSet();
    }

    // Update is called once per frame
    void Update()
    {
        SetUI(sprite);
        SetWeapon();
    }

    void ItemSlotSet()
    {
        for (int i = 0; i < mWeaponSlotObj.Length; i++)
        {
            if (mWeaponSlotObj[i] == null) continue;

            ITEM_ID id = mWeaponSlotObj[i].GetComponent<ItemSetting>().iteminfo.id;

            //�A�C�e��ID�ɂ���ď�������X���b�g���قȂ�
            switch (id)
            {
                case ITEM_ID.PISTOL:
                case ITEM_ID.ASSAULT:
                case ITEM_ID.SHOTGUN:
                    Inventory.Slots[(int)SLOT_ORDER.GUN].ItemInfo = mWeaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.HAND:
                    Inventory.Slots[(int)SLOT_ORDER.HAND].ItemInfo = mWeaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.KNIFE:
                    Inventory.Slots[(int)SLOT_ORDER.KNIFE].ItemInfo = mWeaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.DOG_DIRECTION:
                    Inventory.Slots[(int)SLOT_ORDER.DOG].ItemInfo = mWeaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
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
            mSelectSlot++;
            //����C���x���g���̗̈�𒴂�����ŏ��ɖ߂�
            if ((int)mSelectSlot >= slot_size)
            {
                mSelectSlot = 0;
            }

            //����C���x���g���̒��g�������Ȃ���Β��g�̂��镐���
            while (mWeaponSlotObj[(int)mSelectSlot] == null)
            {
                if (mWeaponSlotObj[(int)mSelectSlot] == null)
                {
                    //���̕���C���x���g����
                    mSelectSlot++;
                    //����C���x���g���̗̈�𒴂�����ŏ��ɖ߂�
                    if ((int)mSelectSlot >= slot_size)
                    {
                        mSelectSlot = 0;
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
            mSelectSlot--;
            if (mSelectSlot < 0)
            {
                //����C���x���g���̗̈�𒴂�����Ō�ɂ���
                mSelectSlot = (SLOT_ORDER)slot_size - 1;
            }

            //����C���x���g���̒��g�������Ȃ���Β��g�̂��镐���
            while (mWeaponSlotObj[(int)mSelectSlot] == null)
            {
                if (mWeaponSlotObj[(int)mSelectSlot] == null)
                {
                    //�O�̕���C���x���g��
                    mSelectSlot--;
                    if (mSelectSlot < 0)
                    {
                        //����C���x���g���̗̈�𒴂�����Ō�ɂ���
                        mSelectSlot = (SLOT_ORDER)slot_size - 1;
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
        return mWeaponSlotObj[(int)mSelectSlot];
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
                if (mWeaponSlotObj[(int)SLOT_ORDER.GUN] == null)
                {
                    Inventory.Slots[(int)SLOT_ORDER.GUN].ItemInfo = _item.GetComponent<ItemSetting>().iteminfo;
                    mWeaponSlotObj[(int)SLOT_ORDER.GUN] = _item.GetComponent<ItemSetting>().iteminfo.weaponitem_info.weapon_obj;
                    //�I��ł��镐�킪HAND�̏ꍇ�E���������I��ł��镐��ɕύX
                    if (mSelectSlot == SLOT_ORDER.HAND) mSelectSlot = SLOT_ORDER.GUN;

                    //����擾�̍ۂ̐ݒ�
                    ParentChildren(mWeaponParent.gameObject, _item);
                    _item.GetComponent<GunManager>().GetItemSetting();
                }
                break;
            case ITEM_ID.DOG_DIRECTION:
                if (mWeaponSlotObj[(int)SLOT_ORDER.DOG] == null)
                {
                    Inventory.Slots[(int)SLOT_ORDER.DOG].ItemInfo = _item.GetComponent<ItemSetting>().iteminfo;
                    mWeaponSlotObj[(int)SLOT_ORDER.DOG] = _item.GetComponent<ItemSetting>().iteminfo.weaponitem_info.weapon_obj;
                    //�I��ł��镐�킪HAND�̏ꍇ�E���������I��ł��镐��ɕύX
                    if (mSelectSlot == SLOT_ORDER.HAND) mSelectSlot = SLOT_ORDER.DOG;

                    ParentChildren(mWeaponParent.gameObject, _item);
                }
                break;
        }      
    }

    public void GunObjChenge(ItemInformation _inventoryitem_item)
    {
        //���̕�����\��
        mWeaponSlotObj[(int)SLOT_ORDER.GUN].SetActive(false);
        //����ւ�������ɕύX
        mWeaponSlotObj[(int)SLOT_ORDER.GUN] = _inventoryitem_item.weaponitem_info.weapon_obj;
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
        for (int slot = 0; slot < slot_size; slot++)
        {
            if (Inventory.Slots[slot].ItemInfo == null)
            {
                //�A�C�e����񂪂Ȃ��ꍇ

                _sprite[slot].gameObject.SetActive(false);          //��\��
                _sprite[slot].GetComponent<Image>().sprite = null;  //�X�v���C�g������
            }
            else
            {
                //�A�C�e����񂪂���ꍇ

                _sprite[slot].gameObject.SetActive(true);                                             //�\��
                _sprite[slot].GetComponent<Image>().sprite = Inventory.Slots[slot].ItemInfo.sprite;   //�X���b�g�ɂ���A�C�e����񂩂�X�v���C�g����
            }
        }
    }

    public void SetWeapon()
    {
        for (int slot = 0; slot < slot_size; slot++)
        {
            //�A�C�e����񂪂Ȃ�
            if (mWeaponSlotObj[slot] == null) continue;

            //�I�����Ă���X���b�g�̃I�u�W�F�N�g�\��
            if (slot == (int)mSelectSlot)
            {
                mWeaponSlotObj[(int)mSelectSlot].SetActive(true);
                sprite[(int)mSelectSlot].GetComponent<Image>().color = mColorAlphaFull;
                mFrame.position = sprite[(int)mSelectSlot].position;
            }
            else
            {
                //��\��

                mWeaponSlotObj[slot].SetActive(false);
                sprite[slot].GetComponent<Image>().color = mColorAlphaHalf;
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

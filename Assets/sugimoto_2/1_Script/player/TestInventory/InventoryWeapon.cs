using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWeapon : MonoBehaviour
{
    //�C���x���g���}�l�[�W���[
    inventoryManager mInventoryManager;

    //�C���x���g���̗v�f
    public InventoryClass Inventory;
    public int slot_size = 4;
    public Transform[] sprite;
    public Transform[] slot_box;
    public Text[] text;

    //�I�u�W�F�N�g
    [SerializeField] GameObject mInventoryManagerObj;
    [SerializeField] GameObject mWeapon_inventory_UI_obj; //�C���x���g��UI
    [SerializeField] GameObject[] mWeaponSlotObj;
    [SerializeField] Transform mFrame;

    public enum SLOT_ORDER
    {
        HAND,
        KNIFE,
        GUN,
        DOG,
    }

    public SLOT_ORDER mSelectSlot = SLOT_ORDER.HAND;
    Color mColorAlphaHalf = new Color(1.0f, 1.0f, 1.0f, 0.5f);//������
    Color mColorAlphaFull = new Color(1.0f, 1.0f, 1.0f, 1.0f);//�s����

    // Start is called before the first frame update
    void Start()
    {
        //�C���x���g���̃C���X�g���N�^�쐬
        Inventory = new InventoryClass(slot_size, slot_box);
        //�C���x���g���}�l�[�W���[�擾
        mInventoryManager = mInventoryManagerObj.GetComponent<inventoryManager>();

        //mWeaponSlotObj�ɂ���I�u�W�F�N�g�̏����X���b�g�ɓ����
        ItemSlotSet();
    }

    // Update is called once per frame
    void Update()
    {
        SetUI(sprite);
    }

    void ItemSlotSet()
    {
        for (int i = 0; i < mWeaponSlotObj.Length; i++)
        {
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
        if (Mathf.Abs(mouse_wheel) != 0)
        {
            //���݂̕����\��
            mWeaponSlotObj[(int)mSelectSlot].SetActive(false);
            sprite[(int)mSelectSlot].GetComponent<Image>().color = mColorAlphaHalf;
            ////�C���x���g���\��
            //display_timer = 0.0f;
            //display_flag = true;

            //if (weapon[(int)select_weapon].GetComponent<ItemInformation>().id == ITEM_ID.PISTOL)
            //{
            //    weapon[(int)select_weapon].GetComponent<GunManager>().StopReload();
            //}
        }

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
        if (Mathf.Abs(mouse_wheel) != 0)
        {
            //�����Ă��镐���ύX
            mWeaponSlotObj[(int)mSelectSlot].SetActive(true);
            sprite[(int)mSelectSlot].GetComponent<Image>().color = mColorAlphaFull;
            mFrame.position = slot_box[(int)mSelectSlot].transform.position;
        }
        return mWeaponSlotObj[(int)mSelectSlot];
        {
            //����C���x���g���\����\��
            //{
            //    display_timer += Time.deltaTime;

            //    if (display_timer >= 5.0f)
            //    {
            //        display_flag = false;
            //        color = 0.0f;
            //    }

            //    if (!display_flag)
            //    {
            //        color += Time.deltaTime;

            //        for (int i = 0; i < weapon_sprite_obj.Length; i++)
            //        {
            //            //�e�I�u�W�F
            //            weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color.a - color);
            //            //�I�u�W�F�{��
            //            weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, weapon_sprite_obj[i].GetComponent<Image>().color.a - color);
            //            //�g�I�u�W�F
            //            frame_pos.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, frame_pos.GetComponent<Image>().color.a - color);
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < weapon_sprite_obj.Length; i++)
            //        {
            //            //�e�I�u�W�F
            //            weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            //            //�I�u�W�F�{��
            //            if (hand_weapon == (WEAPON_ID)i)
            //            {
            //                weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            //            }
            //            else if (weapon_hand_obj[i] != null)
            //            {
            //                weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            //            }
            //            else
            //            {
            //                weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            //            }

            //            //�g�I�u�W�F
            //            frame_pos.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            //        }
            //    }
            //}

            //�z�u�AUI�ݒ�
            //bullet_text_obj.SetActive(false);

            //if (hand_weapon != befor_weapon)
            //{
            //    switch (hand_weapon)
            //    {
            //        case WEAPON_ID.PISTOL:
            //            //bullet_text_obj.SetActive(true);
            //            //transform�ݒ�
            //            ParentChildren(hand_pos.gameObject, weapon_hand_obj[weapon_cnt]);
            //            weapon_hand_obj[weapon_cnt].transform.localRotation = Quaternion.identity;
            //            weapon_hand_obj[weapon_cnt].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            //            //weapon_hand_obj[weapon_cnt].transform.localPosition = hand_start_pos.localPosition;
            //            Debug.Log(weapon_hand_obj[weapon_cnt].transform);
            //            weapon_hand_obj[weapon_cnt].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //�X�P�[���ύX
            //            break;
            //        case WEAPON_ID.KNIFE:
            //            //weapon_hand_obj[weapon_cnt].SetActive(true);
            //            break;
            //    }
            //}
        }
    }

    public void WeaponGet(GameObject _item)
    {
        if (mWeaponSlotObj[(int)SLOT_ORDER.GUN] == null)
        {

        }
    }

    /// <summary>
    /// UI���Z�b�g����
    /// �C���x���g���̃X���b�g��UI������
    /// �C���x���g���N���X�ɂ���̂ƈႢ�AText�Ȃ��o�[�W����
    /// </summary>
    /// <param name="_sprite">�X�v���C�g������g�����X�t�H�[��</param>
    public void SetUI(Transform[] _sprite)
    {
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  �ł��邱��
 �E�A�C�e�����X���b�g�ɓ����
 �E�A�C�e���ړ�
 �E�X���b�g�̊Ǘ�
 �E�����A�C�e�����m�A�X�^�b�N�������Ȃ���΃X�^�b�N����
 */


[System.Serializable]
public class InventoryTest
{
    public InventorySloat[] Sloats { get; set; } //�X���b�g���

    public Transform[] Sloat_Box { get; set; }  //�X���b�g�̈ʒu

    public InventoryTest(int _sloat_size,Transform[] _sloat_box)//�R���X�g���N�^
    {
        //�T�C�Y���̔z��쐬
        Sloats = new InventorySloat[_sloat_size];
        Sloat_Box = new Transform[_sloat_size];

        //�X���b�g��������������
        for (int sloat = 0; sloat < _sloat_size; sloat++)
        {
            Sloat_Box[sloat] = _sloat_box[sloat];
            Sloats[sloat] = new InventorySloat() { Start_Pos = _sloat_box[sloat].position, Sloat_No = sloat };//Set_Pos = _set_pos[sloat], Text = _text[sloat], 
        }
    }

    //�A�C�e���擾
    public bool AddInventory_PickUP_Item(ItemInformation _iteminfo ,WeaponInventory _weaponInventory)
    {
        for (int sloat = 0; sloat < Sloats.Length; sloat++)
        {
            if (Sloats[sloat].CanAdd_PickUPItem(_iteminfo, _weaponInventory))
            {
                int remaining_num = Sloats[sloat].Add_PickUPItem(_iteminfo);

                //���ׂĒǉ��ł����ꍇ
                if (remaining_num <= 0)
                {
                    return true;
                }
                else
                {
                    //�ǉ��ł��Ȃ����������X�V
                    _iteminfo.get_num = remaining_num;
                }
            }
        }
        return false;
    }

    public bool AddWeaponInventory_PickUP_Item(ItemInformation _iteminfo ,ItemInventory _itemInventory)
    {
        for (int sloat = 0; sloat < Sloats.Length; sloat++)
        {
            //���łɏ������Ă��镐��Ȃ�
            if (Sloats[sloat].ItemInfo.id == _iteminfo.id)
            {
                //����e�ۂɕύX���A�A�C�e���C���x���g���ɓ����
                _iteminfo.id = ITEM_ID.BULLET;
                //_itemInventory.Inventory.AddInventory_PickUP_Item(_iteminfo);
                break;
            }
        }

        return false;
    }

    //�X���b�g�A�C�e������X���b�g�A�C�e����
    public bool AddSloatSloatInventory(int _catch, int _in_sloat)
    {
        if (Sloats[_in_sloat].CanAdd_SloatItem(Sloats[_catch]))
        {
            int remaining_num = Sloats[_in_sloat].Add_SloatItem(Sloats[_catch]);
            //Sloats[_catch].SetSloatItemInfo();

            //���ׂĒǉ��ł����ꍇ
            if (remaining_num <= 0)
            {
                if (Sloats[_catch].IsEmpty())
                {
                    Sloats[_catch].CrearSloat();
                }
                return true;
            }
            else
            {
                //�ǉ��ł��Ȃ����������X�V
                Sloats[_catch].ItemInfo.get_num = remaining_num;
            }
        }

        return false;

    }

    public void Add_WeaponInventory(GameObject[] _sloats)
    {
        for (int sloat = 0; sloat < Sloats.Length; sloat++)
        {
            if (_sloats[sloat] != null)
            {
                Sloats[sloat].ItemInfo = _sloats[sloat].GetComponent<ItemSetting>().iteminfo;
            }
        }
    }

    public void SetUI()
    {
        for (int sloat = 0; sloat < Sloats.Length; sloat++)
        {
            if (Sloats[sloat].ItemInfo != null)
            {
                Sloat_Box[sloat].GetChild(0).gameObject.SetActive(true);
                Sloat_Box[sloat].GetChild(0).GetComponent<Image>().sprite = Sloats[sloat].ItemInfo.sprite;
                Sloat_Box[sloat].GetChild(0).GetChild(0).GetComponent<Text>().text = Sloats[sloat].ItemInfo.get_num + "";
            }
            else
            {
                Sloat_Box[sloat].GetChild(0).gameObject.SetActive(false);
                Sloat_Box[sloat].GetChild(0).GetComponent<Image>().sprite = null;
                Sloat_Box[sloat].GetChild(0).GetChild(0).GetComponent<Text>().text = 0 + "";
            }
        }
    }
    public void SetWeaponUI()
    {
        for (int sloat = 0; sloat < Sloats.Length; sloat++)
        {
            if (Sloats[sloat].ItemInfo != null)
            {
                Sloat_Box[sloat].GetChild(0).gameObject.SetActive(true);
                Sloat_Box[sloat].GetChild(0).GetComponent<Image>().sprite = Sloats[sloat].ItemInfo.sprite;
            }
            else
            {
                Sloat_Box[sloat].GetChild(0).gameObject.SetActive(false);
                Sloat_Box[sloat].GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
    }

    public void ItemSloatChange(int _cach_num ,int in_sloat_num)
    {
        InventorySloat temp = Sloats[in_sloat_num];
        Sloats[in_sloat_num] = Sloats[_cach_num];
        Sloats[_cach_num] = temp;
        Sloats[in_sloat_num].Sloat_No = in_sloat_num;
        Sloats[_cach_num].Sloat_No = _cach_num;
    }

    // �f�o�b�O�p���\�b�h��ǉ�
    public void PrintInventory() 
    {
        for (int i = 0; i < Sloats.Length; i++) 
        { 
            var slot = Sloats[i]; 
            if (slot.ItemInfo != null) 
            {
                Debug.Log($"Slot {i}: {slot.Sloat_No}, Quantity: {slot.ItemInfo.id} , Get_Num ; {slot.ItemInfo.get_num}"); 
            } 
            else 
            { 
                Debug.Log($"Slot {i}: Empty"); 
            } 
        } 
    }
}

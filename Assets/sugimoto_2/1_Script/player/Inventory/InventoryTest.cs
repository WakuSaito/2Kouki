using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  �ł��邱��
 �E�A�C�e���擾
 �E�A�C�e���ړ�
 �E�X���b�g�̊Ǘ�
 �E�����A�C�e�����m�A�X�^�b�N�������Ȃ���΃X�^�b�N����
 */


[System.Serializable]
public class InventoryTest
{
    public InventorySloat[] Sloats { get; set; }

    public InventoryTest(int _sloat_size ,Transform[] _set_pos,Text[] _text)
    {
        //�T�C�Y���̔z��쐬
        Sloats = new InventorySloat[_sloat_size];

        //�X���b�g��������������
        for (int sloat = 0; sloat < _sloat_size; sloat++)
        {
            Sloats[sloat] = new InventorySloat() { Set_Pos = _set_pos[sloat], Text = _text[sloat], Start_Pos = _set_pos[sloat].position, Sloat_No = sloat };
        }
    }
    //�A�C�e���擾
    public bool AddItemInventory(ItemInformation _iteminfo)
    {
        foreach(var sloat in Sloats)
        {
            if (sloat.CanAddItem(_iteminfo))
            {
                int remaining_num = sloat.AddItem(_iteminfo);

                sloat.SetSloatUI();
                sloat.SetSloatItemInfo();
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

    public bool AddSloatSloatInventory(InventorySloat _sloat)
    {
        //�S���ƒ��ׂ���_��
        foreach (var sloat in Sloats)//�~
        {
            if (sloat.CanAddSloatSloat(_sloat))
            {
                int remaining_num = sloat.AddSloatSloat(_sloat);

                sloat.SetSloatUI();
                sloat.SetSloatItemInfo();
                //���ׂĒǉ��ł����ꍇ
                if (remaining_num <= 0)
                {
                    if (_sloat.IsEmpty())
                    {
                        _sloat.CrearSloat();
                    }
                    return true;
                }
                else
                {
                    //�ǉ��ł��Ȃ����������X�V
                    _sloat.ItemInfo.get_num = remaining_num;
                }
            }
        }
    
        return false;

    }
    public bool AddSloatSloatInventory(InventorySloat _catch, InventorySloat _in_sloat)
    {
        if (_in_sloat.CanAddSloatSloat(_catch))
        {
            int remaining_num = _in_sloat.AddSloatSloat(_catch);

            _in_sloat.SetSloatUI();
            _in_sloat.SetSloatItemInfo();
            //���ׂĒǉ��ł����ꍇ
            if (remaining_num <= 0)
            {
                if (_catch.IsEmpty())
                {
                    _catch.CrearSloat();
                }
                return true;
            }
            else
            {
                //�ǉ��ł��Ȃ����������X�V
                _catch.ItemInfo.get_num = remaining_num;
            }
        }
    
        return false;

    }

    public void ItemSloatChange(int _cach_num ,int in_sloat_num)
    {
        InventorySloat temp = Sloats[in_sloat_num];
        Sloats[in_sloat_num] = Sloats[_cach_num];
        Sloats[_cach_num] = temp;
        Sloats[in_sloat_num].Sloat_No = in_sloat_num;
        Sloats[_cach_num].Sloat_No = _cach_num;
        Sloats[in_sloat_num].SetSloatUI();
        //Sloats[_cach_num].SetSloatUI();
    }

    // �f�o�b�O�p���\�b�h��ǉ�
    public void PrintInventory() 
    {
        for (int i = 0; i < Sloats.Length; i++) 
        { 
            var slot = Sloats[i]; 
            if (slot.ItemInfo != null) 
            { 
                Debug.Log($"Slot {i}: {slot.Sloat_No}, Quantity: {slot.Item_Num}"); 
            } 
            else 
            { 
                Debug.Log($"Slot {i}: Empty"); 
            } 
        } 
    }
}

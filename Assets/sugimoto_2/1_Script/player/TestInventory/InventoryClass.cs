using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryClass
{
    public SlotClass[] Slots; //スロット情報

    public InventoryClass(int _size,Transform[] _box)
    {
        //コンストラクタ
        for (int slot = 0; slot < _size; slot++) 
        {
            Slots[slot] = new SlotClass() { SlotBox = _box[slot] };
        }
    }

    public bool OpenClose(GameObject _inventory_obj ,bool _open_flag)
    {
        if (_open_flag)
        {
            Screen.lockCursor = true;
            _inventory_obj.SetActive(true);
            return true;
        }
        else
        {
            Screen.lockCursor = false;
            _inventory_obj.SetActive(false);
            return false;
        }
    }

    public void SameInventorySloatChange(int _cach_num, int in_sloat_num)
    {
        SlotClass temp = Slots[in_sloat_num];
        Slots[in_sloat_num] = Slots[_cach_num];
        Slots[_cach_num] = temp;
        Slots[in_sloat_num].SlotNo = in_sloat_num;
        Slots[_cach_num].SlotNo = _cach_num;
    }

    public void SameInventorySloatChange(ref SlotClass _slot1 ,ref SlotClass _slot2)
    {
        SlotClass temp = _slot1;
        _slot1 = _slot2;
        _slot2 = temp;
    }

    public void SetUI(Transform[] _sprite,Text[] _text)
    {
        for (int slot = 0; slot < Slots.Length; slot++) 
        {
            if (Slots[slot].ItemInfo == null)
            {
                _sprite[slot].gameObject.SetActive(false);
                _sprite[slot].GetComponent<Image>().sprite = null;
                _text[slot].GetComponent<Text>().text = 0 + "";
            }
            else
            {
                _sprite[slot].gameObject.SetActive(true);
                _sprite[slot].GetComponent<Image>().sprite = Slots[slot].ItemInfo.sprite;
                _text[slot].GetComponent<Text>().text = Slots[slot].ItemInfo.get_num + "";
            }
        }
    }
}

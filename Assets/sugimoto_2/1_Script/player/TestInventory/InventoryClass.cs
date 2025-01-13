using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryClass
{
    public SlotClass[] Slots; //スロット情報

    public InventoryClass(int _size)
    {
        //コンストラクタ
        for (int slot = 0; slot < _size; slot++) 
        {
            Slots[slot] = new SlotClass();
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
}

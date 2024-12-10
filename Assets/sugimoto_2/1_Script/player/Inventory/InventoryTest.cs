using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class InventoryTest
{
    public InventorySloat[] Sloats { get; set; }

    public InventoryTest(int _sloat_size ,Transform[] _set_pos,Text[] _text)
    {
        //サイズ分の配列作成
        Sloats = new InventorySloat[_sloat_size];

        //スロット内初期化初期化
        for (int sloat = 0; sloat < _sloat_size; sloat++)
        {
            Sloats[sloat] = new InventorySloat() { Set_Pos = _set_pos[sloat], Text = _text[sloat], Start_Pos = _set_pos[sloat].position };
        }
    }

    public bool AddItemInventory(ItemInformation _iteminfo)
    {
        foreach(var sloat in Sloats)
        {
            if (sloat.CanAddItem(_iteminfo))
            {
                int remaining_num = sloat.AddItem(_iteminfo);

                sloat.SetSloatUI();
                sloat.SetSloatItemInfo();
                //すべて追加できた場合
                if (remaining_num <= 0)
                {
                    return true;
                }
                else
                {
                    //追加できなかった分を更新
                    _iteminfo.get_num = remaining_num;
                }
            }
        }
        return false;
    }



    // デバッグ用メソッドを追加
    public void PrintInventory() 
    {
        for (int i = 0; i < Sloats.Length; i++) 
        { 
            var slot = Sloats[i]; 
            if (slot.ItemInfo != null) 
            { 
                Debug.Log($"Slot {i}: {slot.Item_Num}, Quantity: {slot.Text}"); 
            } 
            else 
            { 
                Debug.Log($"Slot {i}: Empty"); 
            } 
        } 
    }
}

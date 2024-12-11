using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  できること
 ・アイテム取得
 ・アイテム移動
 ・スロットの管理
 ・同じアイテム同士、スタック上限じゃなければスタックする
 */


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
            Sloats[sloat] = new InventorySloat() { Set_Pos = _set_pos[sloat], Text = _text[sloat], Start_Pos = _set_pos[sloat].position, Sloat_No = sloat };
        }
    }
    //アイテム取得
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

    public bool AddSloatSloatInventory(InventorySloat _sloat)
    {
        //全部と調べちゃダメ
        foreach (var sloat in Sloats)//×
        {
            if (sloat.CanAddSloatSloat(_sloat))
            {
                int remaining_num = sloat.AddSloatSloat(_sloat);

                sloat.SetSloatUI();
                sloat.SetSloatItemInfo();
                //すべて追加できた場合
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
                    //追加できなかった分を更新
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
            //すべて追加できた場合
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
                //追加できなかった分を更新
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

    // デバッグ用メソッドを追加
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

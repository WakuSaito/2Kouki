using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  できること
 ・アイテムをスロットに入れる
 ・アイテム移動
 ・スロットの管理
 ・同じアイテム同士、スタック上限じゃなければスタックする
 */


[System.Serializable]
public class InventoryTest
{
    public InventorySloat[] Sloats { get; set; } //スロット情報

    public Transform[] Sloat_Box { get; set; }  //スロットの位置

    public InventoryTest(int _sloat_size,Transform[] _sloat_box)//コンストラクタ
    {
        //サイズ分の配列作成
        Sloats = new InventorySloat[_sloat_size];
        Sloat_Box = new Transform[_sloat_size];

        //スロット内初期化初期化
        for (int sloat = 0; sloat < _sloat_size; sloat++)
        {
            Sloat_Box[sloat] = _sloat_box[sloat];
            Sloats[sloat] = new InventorySloat() { Start_Pos = _sloat_box[sloat].position, Sloat_No = sloat };//Set_Pos = _set_pos[sloat], Text = _text[sloat], 
        }
    }

    //アイテム取得
    public bool AddInventory_PickUP_Item(ItemInformation _iteminfo)
    {
        for (int sloat = 0; sloat < Sloats.Length; sloat++)
        {
            if (Sloats[sloat].CanAdd_PickUPItem(_iteminfo))
            {
                int remaining_num = Sloats[sloat].Add_PickUPItem(_iteminfo);
                Sloats[sloat].SetSloatItemInfo();

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

    //スロットアイテムからスロットアイテムへ
    public bool AddSloatSloatInventory(int _catch, int _in_sloat)
    {
        if (Sloats[_in_sloat].CanAdd_SloatItem(Sloats[_catch]))
        {
            int remaining_num = Sloats[_in_sloat].Add_SloatItem(Sloats[_catch]);
            Sloats[_catch].SetSloatItemInfo();

            //すべて追加できた場合
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
                //追加できなかった分を更新
                Sloats[_catch].ItemInfo.get_num = remaining_num;
            }
        }

        return false;

    }

    public void SetUI()
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
            Sloat_Box[sloat].GetChild(0).GetChild(0).GetComponent<Text>().text = Sloats[sloat].Item_Num + "";
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

    // デバッグ用メソッドを追加
    public void PrintInventory() 
    {
        for (int i = 0; i < Sloats.Length; i++) 
        { 
            var slot = Sloats[i]; 
            if (slot.ItemInfo != null) 
            {
                Debug.Log($"Slot {i}: {slot.Sloat_No}, Quantity: {slot.Item_Num} , Get_Num ; {slot.ItemInfo.get_num}"); 
            } 
            else 
            { 
                Debug.Log($"Slot {i}: Empty"); 
            } 
        } 
    }
}

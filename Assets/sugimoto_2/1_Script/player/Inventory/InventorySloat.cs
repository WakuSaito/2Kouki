using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySloat
{
    public ItemInformation ItemInfo { get; set; } // スロットに格納されるアイテム
    public int Item_Num { get; set; } // スロット内のアイテム数

    //スロットが空か調べる
    public bool CheckSloatEmpty()
    {
        return Item_Num == 0;
    }

    //アイテムを追加できるか調べる
    public bool CanAddItem(ItemInformation _iteminfro)
    {
        //武器の場合の処理を追加


        return ItemInfo == null || (ItemInfo.id == _iteminfro.id && Item_Num < ItemInfo.stack_max);
    }

    //アイテムを追加、スロットに入らなかったアイテム数を返す
    public int AddItem(ItemInformation _iteminfo)
    {

        if(ItemInfo == null)
        {
            ItemInfo = _iteminfo;
            Debug.Log(ItemInfo);
            Item_Num = 0;           //アイテム数を後で数えて入れなおすため
        }

        if(ItemInfo.id != _iteminfo.id)
        {
            return ItemInfo.get_num;
        }

        //スロットの空き容量を調べる
        int stack_space = ItemInfo.stack_max - Item_Num;
        //追加できるアイテム数を調べる
        int add_num = Mathf.Min(_iteminfo.get_num, stack_space);//取得可能がはいるか、空き容量の数しか入らないか
        //スロットのアイテム数を更新
        Item_Num += add_num;

        return ItemInfo.get_num - add_num;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 アイテム情報保存
 アイテムを入れれるか調べる
アイテム情報を入れる（残りを返す）
 */

public class SlotClass
{
    public ItemInformation ItemInfo;
    public Vector2 Slot_pos;
    public int SlotNo;

    public bool Empty()//スロットが空か調べる
    {
        if (ItemInfo == null) return true;
        if (ItemInfo.get_num == 0) return true;

        return false;
    }

    public bool CanAdd_PickUPItem(ItemInformation _item)
    {
        if (ItemInfo == null) return true;
        if (ItemInfo.id == _item.id && ItemInfo.get_num < ItemInfo.stack_max) return true;

        return false;
    }


    public int Add_PickUPItem(ItemInformation _item)
    {
        //アイテム情報がなければ入れる
        if (ItemInfo == null)
        {
            ItemInfo = new ItemInformation(_item);
            ItemInfo.get_num = 0;//アイテム数を後で数えて入れなおすため
        }

        //アイテムが入っていて、IDが違う場合は入れれない
        if (ItemInfo.id != _item.id)
        {
            return ItemInfo.get_num;
        }

        //スロットの空き容量を調べる
        int stack_space = ItemInfo.stack_max - ItemInfo.get_num;
        //追加できるアイテム数を調べる
        int add_num = Mathf.Min(_item.get_num, stack_space);//取得可能数がはいるか、空き容量の数しか入らないか

        //スロットのアイテム数を更新
        ItemInfo.get_num += add_num;
        _item.get_num -= add_num;

        return _item.get_num;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 アイテム情報保存
 アイテムを入れれるか調べる
アイテム情報を入れる（残りを返す）
 */

//[System.Serializable]
public class SlotClass
{
    public ItemInformation ItemInfo { get; set; }
    public int SlotNo { get; set; }
    public Transform SlotBox { get; set; }

    //空か調べる
    public bool CheckEmpty()
    {
        //中身がない
        if (ItemInfo == null) return true;
        //獲得可能数がゼロ
        if (ItemInfo.get_num == 0) return true;

        return false;
    }

    //アイテム情報
    public bool Can_Add_Slot(ItemInformation _item)
    {
        //中身がない
        if (ItemInfo == null) return true;
        //IDが同じ&&アイテム数が上限より少ない
        if (ItemInfo.id == _item.id && ItemInfo.get_num < ItemInfo.stack_max) return true;

        return false;
    }

    /// <summary>
    /// アイテムを入れられるか調べる
    /// </summary>
    /// <param name="_id">調べたいアイテムID</param>
    /// <returns>入れられればTrue</returns>
    public bool CanAddItem(ITEM_ID _id)
    {
        if (ItemInfo.id != _id) return false;
        if (ItemInfo.get_num == ItemInfo.stack_max) return false;

        return true;
    }

    /// <summary>
    /// アイテムを重ねることができるか
    /// 移動先のスロットの中身と掴んでいるスロットの中身(引数)と比べる
    /// 中身が空の場合、異なる種類のアイテム同士の場合は中身の入れ替えのみなのでスタック処理はしない
    /// 中身があり、同じ種類のアイテム&&上限より少なければスタック処理
    /// </summary>
    /// <param name="_slot">掴んでいるスロット</param>
    /// <returns></returns>
    public bool CanAddStackItem(SlotClass _slot)
    {
        //中身がない
        if (ItemInfo == null) return false;
        //IDが同じ&&アイテム数が上限より少ない
        if (ItemInfo.id == _slot.ItemInfo.id && ItemInfo.get_num < ItemInfo.stack_max) return true;

        return false;
    }

    /// <summary>
    /// アイテムをスタック
    /// 掴んでいるスロットの中身を移動先のスロットの中身に、上限まで入れる
    /// 追加できるアイテム数を調べ、両方のスロットのアイテム数を更新
    /// 掴んでいるスロットの中身が空になったら初期化しておく
    /// </summary>
    /// <param name="_slot">掴んでいるスロット</param>
    public void AddStackItem(ref SlotClass _slot)
    {
        //スロットの空き容量を調べる
        int stack_space = ItemInfo.stack_max - ItemInfo.get_num;
        //追加できるアイテム数を調べる
        int add_num = Mathf.Min(_slot.ItemInfo.get_num, stack_space);//取得可能数がはいるか、空き容量の数しか入らないか

        //スロットのアイテム数を更新
        ItemInfo.get_num += add_num;
        _slot.ItemInfo.get_num -= add_num;

        //中身が空になった場合初期化
        if (_slot.CheckEmpty())
        {
            _slot.initializationSlot();
        }
    }

    public void initializationSlot()
    {
        if(CheckEmpty())
        {
            ItemInfo = null;
        }
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

    public void CheckCanAddWeapon(ItemInformation _item)
    {
        if (ItemInfo != null) return;

        ItemInfo = _item;

        return;
    }
}

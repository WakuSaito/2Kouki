using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  できること
 ・アイテム情報管理
 ・スロットアイテムの位置
 */

public class InventorySloat
{
    public ItemInformation ItemInfo { get; set; } // スロットに格納されるアイテム
    //public int Item_Num { get; set; }   // スロット内のアイテム数
    public int Sloat_No { get; set; }   //スロットの番号
    public Vector3 Start_Pos { get; set; }

    //public Transform Set_Pos { get; set; }

    //public Text Text { get; set; }

    //スロットが空か調べる
    public bool IsEmpty()
    {
        return ItemInfo.get_num == 0;
    }
    
    /*プレイヤーが拾ったアイテム*/
    //アイテムを追加できるか調べる
    public bool CanAdd_PickUPItem(ItemInformation _iteminfro)
    {
        //武器の場合の処理を追加
        if (ItemInfo == null) return true;
        if (ItemInfo.id == _iteminfro.id && ItemInfo.get_num < ItemInfo.stack_max) return true;

        return false;
    }

    public bool CanAdd_SloatItem(InventorySloat _sloat)
    {
        if (ItemInfo == null) return false;
        if (Sloat_No == _sloat.Sloat_No) return false;
        if (ItemInfo.id != _sloat.ItemInfo.id) return false;
        if (ItemInfo.get_num == ItemInfo.stack_max) return false;

        return true;
    }

    //アイテムを追加、スロットに入らなかったアイテム数を返す
    //public int Add_PickUPItem(ItemInformation _iteminfo)
    //{
    //    //アイテム情報がなければ入れる
    //    if(ItemInfo == null)
    //    {
    //        ItemInfo = _iteminfo;
    //        Item_Num = 0;           //アイテム数を後で数えて入れなおすため
    //    }

        

    //    //アイテムが入っていて、IDが違う場合は入れれない
    //    if (ItemInfo.id != _iteminfo.id)
    //    {
    //        return ItemInfo.get_num;
    //    }


    //    //スロットの空き容量を調べる
    //    int stack_space = ItemInfo.stack_max - Item_Num;
    //    //追加できるアイテム数を調べる
    //    int add_num = Mathf.Min(_iteminfo.get_num, stack_space);//取得可能数がはいるか、空き容量の数しか入らないか

    //    //スロットのアイテム数を更新
    //    Item_Num += add_num;
    //    _iteminfo.get_num -= add_num;

    //    return _iteminfo.get_num;
    //}

    public int Add_PickUPItem(ItemInformation _iteminfo)
    {
        //アイテム情報がなければ入れる
        if(ItemInfo == null)
        {
            ItemInfo = new ItemInformation(_iteminfo.type, _iteminfo.id, _iteminfo.get_num, _iteminfo.stack_max, _iteminfo.sprite);
            ItemInfo.get_num = 0;           //アイテム数を後で数えて入れなおすため
        }

        

        //アイテムが入っていて、IDが違う場合は入れれない
        if (ItemInfo.id != _iteminfo.id)
        {
            return ItemInfo.get_num;
        }


        //スロットの空き容量を調べる
        int stack_space = ItemInfo.stack_max - ItemInfo.get_num;
        //追加できるアイテム数を調べる
        int add_num = Mathf.Min(_iteminfo.get_num, stack_space);//取得可能数がはいるか、空き容量の数しか入らないか

        //スロットのアイテム数を更新
        ItemInfo.get_num += add_num;
        _iteminfo.get_num -= add_num;

        return _iteminfo.get_num;
    }


    /*スロットアイテムを足し合わせる*/

    public int Add_SloatItem(InventorySloat _sloat)
    {
        //スロットの空き容量を調べる
        int stack_space = ItemInfo.stack_max - ItemInfo.get_num;
        //追加できるアイテム数を調べる
        int add_num = Mathf.Min(_sloat.ItemInfo.get_num, stack_space);//取得可能数がはいるか、空き容量の数しか入らないか

        //スロットのアイテム数を更新
        ItemInfo.get_num += add_num;
        _sloat.ItemInfo.get_num -= add_num;

        return _sloat.ItemInfo.get_num;
    }

    /*設定関係*/

    //public void SetSloatItemInfo()
    //{
    //    if (ItemInfo != null)
    //    {
    //        //獲得可能数は所持している数
    //        Debug.Log(ItemInfo.get_num);
    //        Debug.Log(ItemInfo.get_num);
    //        ItemInfo.get_num = ItemInfo.get_num;
    //    }
    //}

    public void UseItem()
    {
        ItemInfo.get_num--;
        //SetSloatItemInfo();

        //空になったら初期化
        if (IsEmpty())
        {
            ItemInfo = null;
        }
    }

    public void CrearSloat()
    {
        ItemInfo = null;
    }
}

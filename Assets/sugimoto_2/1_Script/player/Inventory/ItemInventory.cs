using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryInformation
{
    public int stack_num = 0;
}

public class ItemInventory : MonoBehaviour
{
    const int SLOAT_MAX = 10;

    //アイテム情報を保存
    ItemInformation[] iteminfo = new ItemInformation[SLOAT_MAX];
    //アイテムの所持情報を取得
    ItemInventoryInformation[] iteminventoryinfo = new ItemInventoryInformation[SLOAT_MAX];

    ItemInformation ItemGet(ItemInformation _item ,int _sloat_num)
    {
        //スロットに入れれなければそのまま返す
        if (_sloat_num == -1) return _item;

        //アイテム情報がなければ入れる
        if (iteminfo[_sloat_num] == null) 
        {
            iteminfo[_sloat_num] = _item;
            iteminventoryinfo[_sloat_num] = new ItemInventoryInformation();
        }

        //スタック上限または取得できる物がなくなるまで
        while (iteminventoryinfo[_sloat_num].stack_num != _item.stack_max)
        {
            //取得できるものがなくなれば終了
            if (_item.get_num == 0) break;

            iteminventoryinfo[_sloat_num].stack_num++;
            _item.get_num--;
        }

        //スロットにアイテム情報を入れる
        if (iteminfo[_sloat_num].type == ItemInformation.ITEM_TYPE.NON)
        {
            iteminfo[_sloat_num] = _item;

            //取得できるものが残っている場合空いているスロットを調べる
            if (iteminfo[_sloat_num].get_num != 0)
            {
                ItemGet(_item, SloatCheck(_item));
            }
        }

        //更新したものを返す
        return iteminfo[_sloat_num];
    }

    public ItemInformation ItemCheck(ItemInformation _item)
    {

        Debug.Log(_item);
        Debug.Log(_item.type);
        Debug.Log(_item.id);
        Debug.Log(_item.get_num);
        Debug.Log(_item.stack_max);

        //アイテムを調べる
        if (_item.type == ItemInformation.ITEM_TYPE.WEAPON) 
        {
            switch(_item.id)
            {
                case ItemInformation.ITEM_ID.PISTOL:
                    //すでに所持している

                    //所持していない
                    _item.id = ItemInformation.ITEM_ID.BULLET;
                    return ItemGet(_item, SloatCheck(_item));
                default:
                    return ItemGet(_item, SloatCheck(_item));
            }
        }
        else
        {
            return ItemGet(_item, SloatCheck(_item));
        }
    }

    int SloatCheck(ItemInformation _item)
    {
        //アイテムを入れる場所を調べる
        for (int sloat = 0; sloat < SLOAT_MAX; sloat++) 
        {
            //アイテムが空
            if (iteminfo[sloat] == null)                                    
            {
                return sloat;
            }

            //アイテムIDが同じ 
            if (_item.id == iteminfo[sloat].id)                                           
            {
                //スタック上限ではない場合のみ
                if (_item.stack_max != iteminventoryinfo[sloat].stack_num)
                {
                    return sloat;
                }          
            }
        }

        Debug.Log("aaaa");
        return -1;  //無し
    }

    // Update is called once per frame
    void Update()
    {

    }
}

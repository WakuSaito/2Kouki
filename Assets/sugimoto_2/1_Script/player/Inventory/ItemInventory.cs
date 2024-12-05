using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemInventorySloat:ItemInformation
{
    //public ItemInformation iteminfo;
    public int stack_num = 0;
}

public class ItemInventory : MonoBehaviour
{
    const int SLOAT_MAX = 10;

    public ItemInventorySloat[] sloats = new ItemInventorySloat[SLOAT_MAX];

    bool set_flag = false;

    //アイテム情報を保存
    //public ItemInformation[] iteminfo = new ItemInformation[SLOAT_MAX];

    /*UI関連*/
    [SerializeField] Transform[] item_sprite_pos;                                         //インベントリUI

    private void Start()
    {
        
    }

    private void Update()
    {
        sloats[0] = Set(0);
    }

    ItemInventorySloat Set(int _sloat)
    {
        Debug.Log("testa");

        if (set_flag)
        {
            Debug.Log("test");
            sloats[0] = new ItemInventorySloat() { id = ITEM_ID.FOOD_1 };
            set_flag = false;
        }
        return sloats[0];
    }

    void SetItemUI(ItemInformation _item)
    {
        for (int sloat = 0; sloat < SLOAT_MAX; sloat++)
        {
            //if (sloats[sloat] != ITEM_ID.NON)
            //{
            //    item_sprite_pos[sloat].GetComponent<Image>().sprite = ;
            //}
        }
    }
    public bool ItemGet(ItemInformation _item)
    {
        set_flag = ItemCheck(_item);
        //SetItemUI(_item);

        return set_flag;
    }

    bool ItemCheck(ItemInformation _item)
    {
        if (_item.type == ITEM_TYPE.WEAPON)
        {
            switch (_item.id)
            {
                case ITEM_ID.PISTOL:
                    //所持していない場合
                    /*武器インベントリに入れる*/

                    //所持している場合
                    _item.id = ITEM_ID.BULLET;
                    break;
            }
        }

        return InSloat(_item, SloatCheck(_item));
    }

    int SloatCheck(ItemInformation _item)
    {
        for (int sloat = 0; sloat < SLOAT_MAX; sloat++)
        {
            if (sloats[sloat] == null)
            {
                sloats[sloat] = new ItemInventorySloat();
                return sloat;
            }

            if (sloats[sloat].id == _item.id || sloats[sloat].id == ITEM_ID.NON)
            {
                if (_item.stack_max != sloats[sloat].stack_num)
                {
                    return sloat;
                }
            }
        }

        return -1;
    }

    bool InSloat(ItemInformation _item, int _sloat)
    {
        Debug.Log(_sloat);
        //スロットに入れない
        if (_sloat == -1) return false;

        //スタック上限または取得できる物がなくなるまで
        while (sloats[_sloat].stack_num != _item.stack_max)
        {
            //取得できるものがなくなれば終了
            if (_item.get_num == 0) return true;

            sloats[_sloat].stack_num++;
            _item.get_num--;
        }

        //取得できるものが残っている場合空いているスロットを調べる
        if (_item.get_num != 0)
        {
            InSloat(_item, SloatCheck(_item));
        }
        else
        {
            return true;
        }

        return false;
    }


    //bool ItemGet(ItemInformation _item ,int _sloat_num)
    //{
    //    //スロットに入れれなければそのまま返す
    //    if (_sloat_num == -1) return false;

    //    //アイテム情報がなければ入れる
    //    if (iteminfo[_sloat_num] == null) 
    //    {
    //        iteminfo[_sloat_num] = _item;
    //        iteminventoryinfo[_sloat_num] = new ItemInventoryInformation();
    //    }

    //    //スタック上限または取得できる物がなくなるまで
    //    while (iteminventoryinfo[_sloat_num].stack_num != _item.stack_max)
    //    {
    //        //取得できるものがなくなれば終了
    //        if (_item.get_num == 0) break;

    //        iteminventoryinfo[_sloat_num].stack_num++;
    //        _item.get_num--;
    //    }

    //    //スロットにアイテム情報を入れる
    //    if (iteminfo[_sloat_num].type == ITEM_TYPE.NON)
    //    {
    //        Debug.Log("aa");

    //        iteminfo[_sloat_num] = _item;

    //        //取得できるものが残っている場合空いているスロットを調べる
    //        if (_item.get_num != 0)
    //        {
    //            ItemGet(_item, SloatCheck(_item));
    //        }
    //        else
    //        {
    //            Debug.Log("aa");
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public bool ItemCheck(ItemInformation _item)
    //{

    //    Debug.Log(_item);
    //    Debug.Log(_item.type);
    //    Debug.Log(_item.id);
    //    Debug.Log(_item.get_num);
    //    Debug.Log(_item.stack_max);

    //    //アイテムを調べる
    //    if (_item.type == ITEM_TYPE.WEAPON) 
    //    {
    //        switch(_item.id)
    //        {
    //            case ITEM_ID.PISTOL:
    //                //すでに所持している

    //                //所持していない
    //                _item.id = ITEM_ID.BULLET;
    //                return ItemGet(_item, SloatCheck(_item));
    //            default:
    //                return ItemGet(_item, SloatCheck(_item));
    //        }
    //    }
    //    else
    //    {
    //        return ItemGet(_item, SloatCheck(_item));
    //    }
    //}

    //int SloatCheck(ItemInformation _item)
    //{
    //    //アイテムを入れる場所を調べる
    //    for (int sloat = 0; sloat < SLOAT_MAX; sloat++) 
    //    {
    //        //アイテムが空
    //        if (iteminfo[sloat] == null)                                    
    //        {
    //            return sloat;
    //        }

    //        //アイテムIDが同じ 
    //        if (_item.id == iteminfo[sloat].id)                                           
    //        {
    //            //スタック上限ではない場合のみ
    //            if (_item.stack_max != iteminventoryinfo[sloat].stack_num)
    //            {
    //                return sloat;
    //            }          
    //        }
    //    }

    //    Debug.Log("aaaa");
    //    return -1;  //無し
    //}

    // Update is called once per frame
}

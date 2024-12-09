using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySloat
{
    public ItemInformation ItemInfo { get; set; } // スロットに格納されるアイテム
    public int Item_Num { get; set; } // スロット内のアイテム数

    public Transform Set_Pos { get; set; }

    public Text Text { get; set; }

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
        //アイテム情報がなければ入れる
        if(ItemInfo == null)
        {
            ItemInfo = _iteminfo;
            Item_Num = 0;           //アイテム数を後で数えて入れなおすため
        }

        //アイテムが入っていて、IDが違う場合は入れれない
        if(ItemInfo.id != _iteminfo.id)
        {
            return ItemInfo.get_num;
        }


        //スロットの空き容量を調べる
        int stack_space = ItemInfo.stack_max - Item_Num;
        //追加できるアイテム数を調べる
        int add_num = Mathf.Min(_iteminfo.get_num, stack_space);//取得可能数がはいるか、空き容量の数しか入らないか

        //スロットのアイテム数を更新
        Item_Num += add_num;
        _iteminfo.get_num -= add_num;

        return _iteminfo.get_num;
    }

    public void SetSloatUI()
    {
        //UI表示
        Set_Pos.gameObject.SetActive(true);
        Set_Pos.GetComponent<Image>().sprite = ItemInfo.sprite;
        Text.GetComponent<Text>().text = Item_Num + "";
    }

    public void SetSloatItemInfo()
    {
        //獲得可能数は所持している数
        ItemInfo.get_num = Item_Num;
    }

    public void InMouseSloat(GameObject _hit_sloat)
    {
        Debug.Log("test");

        if (_hit_sloat == Set_Pos.gameObject)
        {
            if (Input.GetMouseButton(0))
            {
                _hit_sloat.transform.position = Input.mousePosition;
            }
            else
            {
                _hit_sloat.transform.position = Set_Pos.position;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    const int GUN_SLOT = 3;

    //インベントリの要素
    InventoryClass Inventory;
    [SerializeField] int slot_size = 10;
    [SerializeField] Transform[] sprite_pos;
    [SerializeField] Transform[] slot_box;
    [SerializeField] Text[] text;

    //オブジェクト
    [SerializeField] GameObject item_inventory_obj; //インベントリ
    [SerializeField] GameObject player_obj; 

    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryClass(slot_size);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool AddInventory_PickUP_Item(ItemInformation _item, ref WeaponInventory _weapon)//アイテム取得
    {
        //武器の場合の処理を追加
        /*
        所持している武器オブジェクトの中に、取得したアイテムと同じ種類じゃなければ取得
        情報を入れる場所はスロット３
        オブジェクトは消さずに、武器インベントリに保存する（捨てることをしない以外は表示非表示で対応）
        同じ種類の場合アイテム情報を弾丸に変更

        最初に武器インベントリを調べる
        武器インベントリに入っていた場合アイテムインベントリを調べる
         */

        //種類が武器の場合
        if (_item.type == ITEM_TYPE.WEAPON)
        {
            //武器インベントリに銃がない場合
            if (_weapon.Inventory.Sloats[GUN_SLOT].ItemInfo == null)
            {
                //銃をインベントリに入れる
                _weapon.Inventory.Sloats[GUN_SLOT].ItemInfo = new ItemInformation(_item);

                //武器をプレイヤーの子にしておく
                _weapon.WeaponGet(_item.weaponitem_info.weapon_obj);

                return false;
            }
            //アイテムインベントリにあるか調べる
            else
            {
                //インベントリに入れれるか調べる
                bool in_flag = true;

                //武器インベントリと同じIDか調べる
                if (_weapon.Inventory.Sloats[GUN_SLOT].ItemInfo.id == _item.id)
                {
                    in_flag = false;
                }

                //インベントリの中身と比べる
                for (int sloat = 0; sloat < Inventory.Slots.Length; sloat++)
                {
                    if (Inventory.Slots[sloat].ItemInfo != null && Inventory.Slots[sloat].ItemInfo.id == _item.id)
                    {
                        in_flag = false;
                        break;
                    }
                }

                //インベントリに入れれなければ弾丸に変更
                if (!in_flag)
                {
                    //アイテム情報を弾丸に変更
                    _item.BulletInfo();
                }
                else
                {
                    //武器をプレイヤーの子にしておく
                    _weapon.WeaponGet(_item.weaponitem_info.weapon_obj);
                }
            }
        }

        //アイテムをインベントリに
        for (int sloat = 0; sloat < Inventory.Slots.Length; sloat++)
        {
            if (Inventory.Slots[sloat].CanAdd_PickUPItem(_item))
            {
                int remaining_num = Inventory.Slots[sloat].Add_PickUPItem(_item);

                //すべて追加できた場合
                if (remaining_num <= 0)
                {
                    return true;
                }
                else
                {
                    //追加できなかった分を更新
                    _item.get_num = remaining_num;
                }
            }
        }
        return false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour
{
    const int GUN_SLOT = 3;

    //インベントリマネージャー
    inventoryManager mInventoryManager;

    //インベントリの要素
    public InventoryClass Inventory;
    public int slot_size = 10;
    public Transform[] sprite;
    public Transform[] slot_box;
    public Text[] text;

    //オブジェクト
    [SerializeField] GameObject mInventoryManagerObj;
    [SerializeField] GameObject item_inventory_obj; //インベントリ
    [SerializeField] GameObject mFoodGageObj; //インベントリ
    [SerializeField] GameObject mHpGageObj; //インベントリ

    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryClass(slot_size, slot_box);
        mInventoryManager = mInventoryManagerObj.GetComponent<inventoryManager>();
    }

    /// <summary>
    /// アップデート
    /// アイテムインベントリの操作
    /// UIの変更
    /// </summary>
    void Update()
    {
        if (mInventoryManager.inventory_state == INVENTORY.ITEM)
        {
            Inventory.SetUI(sprite, text);
        }
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
        //if (_item.type == ITEM_TYPE.WEAPON)
        //{
        //    //武器インベントリに銃がない場合
        //    if (_weapon.Inventory.Sloats[GUN_SLOT].ItemInfo == null)
        //    {
        //        //銃をインベントリに入れる
        //        _weapon.Inventory.Sloats[GUN_SLOT].ItemInfo = new ItemInformation(_item);

        //        //武器をプレイヤーの子にしておく
        //        _weapon.WeaponGet(_item.weaponitem_info.weapon_obj);

        //        return false;
        //    }
        //    //アイテムインベントリにあるか調べる
        //    else
        //    {
        //        //インベントリに入れれるか調べる
        //        bool in_flag = true;

        //        //武器インベントリと同じIDか調べる
        //        if (_weapon.Inventory.Sloats[GUN_SLOT].ItemInfo.id == _item.id)
        //        {
        //            in_flag = false;
        //        }

        //        //インベントリの中身と比べる
        //        for (int sloat = 0; sloat < Inventory.Slots.Length; sloat++)
        //        {
        //            if (Inventory.Slots[sloat].ItemInfo != null && Inventory.Slots[sloat].ItemInfo.id == _item.id)
        //            {
        //                in_flag = false;
        //                break;
        //            }
        //        }

        //        //インベントリに入れれなければ弾丸に変更
        //        if (!in_flag)
        //        {
        //            //アイテム情報を弾丸に変更
        //            _item.BulletInfo();
        //        }
        //        else
        //        {
        //            //武器をプレイヤーの子にしておく
        //            _weapon.WeaponGet(_item.weaponitem_info.weapon_obj);
        //        }
        //    }
        //}

        //アイテムをインベントリに
        for (int sloat = 0; sloat < Inventory.Slots.Length; sloat++)
        {
            if (Inventory.Slots[sloat].Can_Add_Slot(_item))
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

    /// <summary>
    /// ゲージ回復
    /// 食料ゲージ、体力ゲージを回復させる
    /// </summary>
    public void Recovery_Gage()
    {
        //カーソルのあっているオブジェクトを取得
        foreach (RaycastResult result in mInventoryManager.HitResult())
        {
            for (int slot = 0; slot < slot_size; slot++)
            {
                if (result.gameObject == sprite[slot].gameObject)
                {
                    ITEM_ID id = Inventory.Slots[slot].ItemInfo.id;

                    if (id >= ITEM_ID.FOOD_1 && id <= ITEM_ID.EMERGENCY_PACK)
                    {
                        int recovery_num = Inventory.Slots[slot].ItemInfo.recoveryitem_info.recovery_num;

                        //食料
                        {
                            if (id >= ITEM_ID.FOOD_1 && id <= ITEM_ID.FOOD_4)
                            {
                                mFoodGageObj.GetComponent<Gauge>().Increase_Gauge(recovery_num);
                                //playerSound.PlayEat();//SE
                            }
                            if (id >= ITEM_ID.DRINK_1 && id <= ITEM_ID.DRINK_2)
                            {
                                mFoodGageObj.GetComponent<Gauge>().Increase_Gauge(recovery_num);
                                //playerSound.PlayDrink();//SE
                            }
                        }

                        //体力
                        {
                            if (id >= ITEM_ID.EMERGENCY_PACK)
                            {
                                mHpGageObj.GetComponent<Gauge>().Increase_Gauge(recovery_num);
                                //playerSound.PlayHeal();//SE
                            }
                        }
                        //アイテム消費
                        UseItem(slot);
                    }

                    return;
                }
            }
        }
    }

    void UseItem(int _slot)
    {
        Inventory.Slots[_slot].ItemInfo.get_num--;
        
        if (Inventory.Slots[_slot].CheckEmpty())
        {
            Inventory.Slots[_slot].initializationSlot();
        }
    }
}

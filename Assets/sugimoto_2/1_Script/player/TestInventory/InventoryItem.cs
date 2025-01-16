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
    public GameObject item_inventory_obj; //インベントリUI

    // Start is called before the first frame update
    void Start()
    {
        //インベントリのインストラクタ作成
        Inventory = new InventoryClass(slot_size, slot_box);
        //インベントリマネージャー取得
        mInventoryManager = mInventoryManagerObj.GetComponent<inventoryManager>();
    }

    /// <summary>
    /// アップデート
    /// アイテムインベントリの操作
    /// UIの変更
    /// </summary>
    void Update()
    {
        //アイテムインベントリを開いている間
        if (mInventoryManager.inventory_state == INVENTORY.ITEM|| mInventoryManager.inventory_state == INVENTORY.CHEST)
        {
            Inventory.SetUI(sprite, text);//UI更新
        }
    }

    public bool AddInventory_PickUP_Item(ItemInformation _item, ref InventoryWeapon _weapon)//アイテム取得
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
            //アイテム情報からスロットの位置を取得
            int slot_num = _weapon.CanWeaponGet(_item.weaponitem_info.weapon_obj);
            Debug.Log(slot_num);

            if (_weapon.mWeaponSlotObj[slot_num] == null)
            {
                //武器インベントリに銃がない場合
                
                _weapon.WeaponGet(_item.weaponitem_info.weapon_obj);//武器取得

                return false;
            }
            else
            {
                //インベントリに入れれるか調べる

                bool in_flag = true;

                //武器インベントリと同じIDか調べる
                if (_weapon.Inventory.Slots[slot_num].ItemInfo.id == _item.id)
                {
                    in_flag = false;
                }

                //アイテムインベントリの中身と比べる
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
    /// <param name="_food_gage_obj">食料ゲージのscriptを持ったオブジェクト</param>
    /// <param name="_hp_gage_obj">体力ゲージのscriptを持ったオブジェクト</param>
    public void Recovery_Gage(GameObject _food_gage_obj,GameObject _hp_gage_obj)
    {
        //カーソルのあっているオブジェクトを取得
        foreach (RaycastResult result in mInventoryManager.HitResult())
        {
            //カーソルの当たっているスロットを取得
            for (int slot = 0; slot < slot_size; slot++)
            {
                if (result.gameObject == sprite[slot].gameObject)
                {
                    //IDを取得
                    ITEM_ID id = Inventory.Slots[slot].ItemInfo.id;

                    if (id >= ITEM_ID.FOOD_1 && id <= ITEM_ID.EMERGENCY_PACK)
                    {
                        //回復する値を取得
                        int recovery_num = Inventory.Slots[slot].ItemInfo.recoveryitem_info.recovery_num;

                        //食料
                        {
                            if (id >= ITEM_ID.FOOD_1 && id <= ITEM_ID.DRINK_2)
                            {
                                _food_gage_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);//ゲージを増やす
                                //playerSound.PlayEat();//SE
                            }
                        }

                        //体力
                        {
                            if (id >= ITEM_ID.EMERGENCY_PACK)
                            {
                                _hp_gage_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);//ゲージを増やす
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

    /// <summary>
    /// アイテム消費
    /// スロットの場所を指定し、そのスロットのアイテムを１つ減らす
    /// アイテムがすべてなくなったらそのスロットを初期化
    /// </summary>
    /// <param name="_slot">スロットの場所</param>
    void UseItem(int _slot)
    {
        //スロットのアイテムの数を減らす
        Inventory.Slots[_slot].ItemInfo.get_num--;
        
        //空になったか調べる
        if (Inventory.Slots[_slot].CheckEmpty())
        {
            Inventory.Slots[_slot].initializationSlot();//初期化
        }
    }

    /// <summary>
    /// 弾丸を消費する
    /// 銃のリロードの際に使用
    /// 弾丸が入っているスロットを調べ、増減処理をする
    /// </summary>
    /// <param name="_amount">銃に入る弾数</param>
    /// <returns>銃に入れた弾数</returns>
    public int UseBullet(int _amount)
    {
        //銃に入る弾数を取得
        int addAmount = _amount;
        //増やした弾数を保存
        int add_num = 0;

        //インベントリの中身を調べる
        for (int slot = 0; slot < slot_size; slot++)
        {
            //スロットの中身が空なら次のスロットへ
            if (Inventory.Slots[slot].CheckEmpty()) continue;

            //中身が弾丸
            if (Inventory.Slots[slot].ItemInfo.id == ITEM_ID.BULLET)
            {
                //弾丸の数を取得
                int bullet_num = Inventory.Slots[slot].ItemInfo.get_num;

                //銃スロットにある銃に入る弾数よりもスロットの弾数のほうが多い
                //銃に入る弾数入れたら即終了
                if (bullet_num > addAmount)
                {
                    //銃スロットにある銃に入る弾数数だけスロットの弾丸を減らす
                    Inventory.Slots[slot].ItemInfo.get_num -= addAmount;
                    return addAmount + add_num;//増やした弾数（+add_numいらないかも）
                }
                else
                {
                    //スロットの弾数のほうが少ない
                    //スロットの中身すべて銃に入れたら次のスロットを調べる

                    //銃スロットにある銃に入る弾数をスロットの弾数の減らす
                    addAmount -= Inventory.Slots[slot].ItemInfo.get_num;
                    //増やした数は弾数をスロットの弾数
                    add_num += Inventory.Slots[slot].ItemInfo.get_num;
                    //スロットの中身を０にする
                    Inventory.Slots[slot].ItemInfo.get_num = 0;

                    //空になったら
                    if (Inventory.Slots[slot].CheckEmpty())
                    {
                        Inventory.Slots[slot].initializationSlot();//初期化
                    }
                }
            }

        }

        return _amount - addAmount;//増やした弾数
    }

    /// <summary>
    /// インベントリに弾丸があるか調べる
    /// リロードできるかどうかに使用
    /// </summary>
    /// <returns>あるtrue、ないfalse</returns>
    public bool CheckBullet()
    {
        for (int slot = 0; slot < slot_size; slot++)
        {
            if (Inventory.Slots[slot].ItemInfo == null) continue;

            if (Inventory.Slots[slot].ItemInfo.id == ITEM_ID.BULLET)
            {
                return true;
            }
        }
        return false;
    }

}

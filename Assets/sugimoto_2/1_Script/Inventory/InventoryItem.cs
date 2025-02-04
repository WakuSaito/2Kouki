using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// アイテムインベントリ
/// プレイヤーが所持するアイテムのインベントリ
/// </summary>
public class InventoryItem : MonoBehaviour
{
    const int GUN_SLOT = 3;

    //インベントリマネージャー
    InventoryManager m_inventoryManager;

    //インベントリの要素
    public InventoryClass m_inventory;
    public int m_slotSize = 10;
    public Transform[] m_BoxTrans;
    public Transform[] m_spriteTrans;
    public Transform[] m_noUseMarkTrans;
    public Text[] m_Text;

    //オブジェクト
    [SerializeField] GameObject m_inventoryManagerObj;
    public GameObject m_uiObj; //インベントリUI

    [SerializeField]//サウンド用
    private PlayerSound m_playerSound;

    /// <summary>
    /// スタート関数
    /// インベントリクラス作成
    /// インベントリマネージャー取得
    /// </summary>
    void Start()
    {
        //インベントリのインストラクタ作成
        m_inventory = new InventoryClass(m_slotSize, m_BoxTrans);
        //インベントリマネージャー取得
        m_inventoryManager = m_inventoryManagerObj.GetComponent<InventoryManager>();
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

            if (_weapon.m_weaponSlotObj[slot_num] == null)
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
                if (_weapon.m_Inventory.Slots[slot_num].ItemInfo.id == _item.id)
                {
                    in_flag = false;
                }

                //アイテムインベントリの中身と比べる
                for (int sloat = 0; sloat < m_inventory.Slots.Length; sloat++)
                {
                    if (m_inventory.Slots[sloat].ItemInfo != null && m_inventory.Slots[sloat].ItemInfo.id == _item.id)
                    {
                        in_flag = false;
                        break;
                    }
                }

                

                //インベントリに入れれなければ弾丸に変更
                if (!in_flag)
                {
                    //弾丸にして入れられるか調べる
                    for (int slot = 0; slot < m_inventory.Slots.Length; slot++)
                    {
                        if(m_inventory.Slots[slot].CanAddItem(ITEM_ID.BULLET))
                        {
                            //アイテム情報を弾丸に変更
                            _item.BulletInfo();
                            break;
                        }

                    }

                }
                else
                {
                    //武器をプレイヤーの子にしておく
                    _weapon.WeaponGet(_item.weaponitem_info.weapon_obj);
                }
            }
        }

        //アイテムをインベントリに
        for (int sloat = 0; sloat < m_inventory.Slots.Length; sloat++)
        {
            if (m_inventory.Slots[sloat].Can_Add_Slot(_item))
            {
                int remaining_num = m_inventory.Slots[sloat].Add_PickUPItem(_item);

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
    public void Recovery_Gage(GameObject _food_gage_obj,GameObject _hp_gage_obj,bool _area_flag)
    {
        bool recovery_flag = false;

        //カーソルのあっているオブジェクトを取得
        foreach (RaycastResult result in m_inventoryManager.HitResult())
        {
            //カーソルの当たっているスロットを取得
            for (int slot = 0; slot < m_slotSize; slot++)
            {
                if (result.gameObject == m_spriteTrans[slot].gameObject)
                {
                    //IDを取得
                    ITEM_ID id = m_inventory.Slots[slot].ItemInfo.id;

                    if (id >= ITEM_ID.FOOD_1 && id <= ITEM_ID.EMERGENCY_PACK)
                    {
                        //回復する値を取得
                        int recovery_num = m_inventory.Slots[slot].ItemInfo.recoveryitem_info.recovery_num;

                        //食料
                        {
                            if (_area_flag)
                            {
                                if (id >= ITEM_ID.FOOD_1 && id <= ITEM_ID.DRINK_2)
                                {
                                    _food_gage_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);//ゲージを増やす
                                    recovery_flag = true;
                                    m_playerSound.PlayEat();//SE
                                }
                            }
                            else
                            {
                                if (id >= ITEM_ID.DRINK_1 && id <= ITEM_ID.DRINK_2)
                                {
                                    _food_gage_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);//ゲージを増やす
                                    recovery_flag = true;
                                    m_playerSound.PlayEat();//SE
                                }
                            }
                            
                        }

                        //体力
                        {
                            if (id >= ITEM_ID.EMERGENCY_PACK)
                            {
                                _hp_gage_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);//ゲージを増やす
                                recovery_flag = true;
                                m_playerSound.PlayHeal();//SE
                            }
                        }
                        //アイテム消費
                        if (recovery_flag) UseItem(slot);
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
        m_inventory.Slots[_slot].ItemInfo.get_num--;
        
        //空になったか調べる
        if (m_inventory.Slots[_slot].CheckEmpty())
        {
            m_inventory.Slots[_slot].initializationSlot();//初期化
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
        for (int slot = 0; slot < m_slotSize; slot++)
        {
            //スロットの中身が空なら次のスロットへ
            if (m_inventory.Slots[slot].CheckEmpty()) continue;

            //中身が弾丸
            if (m_inventory.Slots[slot].ItemInfo.id == ITEM_ID.BULLET)
            {
                //弾丸の数を取得
                int bullet_num = m_inventory.Slots[slot].ItemInfo.get_num;

                //銃スロットにある銃に入る弾数よりもスロットの弾数のほうが多い
                //銃に入る弾数入れたら即終了
                if (bullet_num > addAmount)
                {
                    //銃スロットにある銃に入る弾数数だけスロットの弾丸を減らす
                    m_inventory.Slots[slot].ItemInfo.get_num -= addAmount;
                    return addAmount + add_num;//増やした弾数（+add_numいらないかも）
                }
                else
                {
                    //スロットの弾数のほうが少ない
                    //スロットの中身すべて銃に入れたら次のスロットを調べる

                    //銃スロットにある銃に入る弾数をスロットの弾数の減らす
                    addAmount -= m_inventory.Slots[slot].ItemInfo.get_num;
                    //増やした数は弾数をスロットの弾数
                    add_num += m_inventory.Slots[slot].ItemInfo.get_num;
                    //スロットの中身を０にする
                    m_inventory.Slots[slot].ItemInfo.get_num = 0;

                    //空になったら
                    if (m_inventory.Slots[slot].CheckEmpty())
                    {
                        m_inventory.Slots[slot].initializationSlot();//初期化
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
        for (int slot = 0; slot < m_slotSize; slot++)
        {
            if (m_inventory.Slots[slot].ItemInfo == null) continue;

            if (m_inventory.Slots[slot].ItemInfo.id == ITEM_ID.BULLET)
            {
                return true;
            }
        }
        return false;
    }

}

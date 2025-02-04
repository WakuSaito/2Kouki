using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPickUpItem
{
    /*[SerializeField] インスペクターから設定*/
    /// <summary> インベントリオブジェクト </summary>
    [SerializeField] GameObject m_playerObj;

    /*プライベート　private*/
    /// <summary> InventoryWeaponクラス </summary>
    InventoryItem m_inventoryItem;
    InventoryWeapon m_inventoryWeapon;
    /// <summary> SearchViewAreaクラス </summary>
    SearchViewArea m_searchViewArea;
    /// <summary> PlayerSoundクラス </summary
    PlayerSound m_playerSound;

    /// <summary>
    /// アイテム取得準備
    /// </summary>
    public void SetPickUp()
    {
        m_inventoryItem = m_playerObj.GetComponent<InventoryItem>();
        m_inventoryWeapon = m_playerObj.GetComponent<InventoryWeapon>();
        m_searchViewArea = m_playerObj.GetComponent<SearchViewArea>();
        m_playerSound = m_playerObj.GetComponent<PlayerSound>();
    }

    /// <summary>
    /// アイテム取得
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    /// <param name="_search">SearchViewAreaクラス</param>
    /// <param name="_se">PlayerSoundクラス</param>
    /// <returns>すべて取得できたオブジェクト、出来なければnull</returns>
    public GameObject PickUpItem(bool _phsh)
    {  
        //アイテム取得
        GameObject search_obj = m_searchViewArea.GetObjUpdate("item", 1.5f, 0.03f);
        if (!_phsh || search_obj == null) return null;

        //ID取得
        ITEM_ID id = search_obj.GetComponent<ItemSetting>().iteminfo.id;

        bool all_get_flag = m_inventoryItem.AddInventory_PickUP_Item(search_obj.GetComponent<ItemSetting>().iteminfo, ref m_inventoryWeapon);

        m_playerSound.PlayPickUp();//SE

        if (all_get_flag)
        {
            //すべて拾えたら削除
            return search_obj;
        }
        else
        {
            //すべて拾えなかったら何もしない：武器を拾った場合はプレイヤーの所持品にする

            if (id >= ITEM_ID.PISTOL && id <= ITEM_ID.SHOTGUN)
            {
                search_obj.GetComponent<GunManager>().m_handPlayerObj = m_playerObj;
            }
        }

        return null;
    }
}

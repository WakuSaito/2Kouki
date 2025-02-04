using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 できること
 ・アイテム保存
 */

public class InventoryChest : MonoBehaviour
{
    public InventoryClass m_inventory;

    public int m_sloatSize = 10;
    public Transform[] m_spriteTrans;
    public Transform[] m_slotBoxTrans;
    public Text[] m_Text;
    public Transform m_weaponObjParent;
    [SerializeField] bool m_tutorialFlag;

    //オブジェクト
    [SerializeField] GameObject m_PlayerObj;
    [SerializeField] GameObject m_inventoryManagerObj;
    public GameObject m_ChestUIObj;

    [SerializeField] ItemInformation m_tutorialItemInfo;

    /// <summary>
    /// スタート関数
    /// インベントリクラス作成
    /// </summary>
    void Start()
    {
        //インベントリクラス作成
        m_inventory = new InventoryClass(m_sloatSize, m_slotBoxTrans);

        if(m_tutorialFlag)
        {
            int rand = Random.Range(0, m_sloatSize);
            m_inventory.Slots[rand].ItemInfo = m_tutorialItemInfo;

            if(m_tutorialItemInfo.type==ITEM_TYPE.WEAPON)
            {
                m_tutorialItemInfo.weaponitem_info.weapon_obj.transform.parent = m_weaponObjParent;
            }
        }
    }
}

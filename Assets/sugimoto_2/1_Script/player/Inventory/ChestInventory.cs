using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 できること
 ・アイテム保存
 */

public class ChestInventory : MonoBehaviour
{
    public InventoryClass m_inventory;

    public int m_sloatSize = 10;
    public Transform[] m_spriteTrans;
    public Transform[] m_slotBoxTrans;
    public Text[] m_Text;


    //オブジェクト
    [SerializeField] GameObject m_PlayerObj;
    [SerializeField] GameObject m_inventoryManagerObj;
    public GameObject m_ChestUIObj;

    /// <summary>
    /// スタート関数
    /// インベントリクラス作成
    /// </summary>
    void Start()
    {
        //インベントリクラス作成
        m_inventory = new InventoryClass(m_sloatSize, m_slotBoxTrans);
    }
}

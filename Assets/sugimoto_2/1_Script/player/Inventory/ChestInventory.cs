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
    inventoryManager m_inventoryManager;
    player m_Player;

    public int m_sloatSize = 10;
    public Transform[] m_spriteTrans;
    public Transform[] m_slotBoxTrans;
    public Text[] m_Text;


    //オブジェクト
    [SerializeField] GameObject m_PlayerObj;
    [SerializeField] GameObject m_inventoryManagerObj;
    public GameObject m_ChestUIObj;

    // Start is called before the first frame update
    void Start()
    {
        m_inventory = new InventoryClass(m_sloatSize, m_slotBoxTrans);
        m_inventoryManager = m_inventoryManagerObj.GetComponent<inventoryManager>();
        m_Player = m_PlayerObj.GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_inventoryManager.inventory_state == INVENTORY.CHEST)
        {
            m_inventory.SetUI(m_spriteTrans, m_Text);
        }
    }
}

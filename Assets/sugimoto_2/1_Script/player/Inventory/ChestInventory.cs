using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 �ł��邱��
 �E�A�C�e���ۑ�
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


    //�I�u�W�F�N�g
    [SerializeField] GameObject m_PlayerObj;
    [SerializeField] GameObject m_ChestObj;
    [SerializeField] GameObject m_ItemInventoryUIObj;
    [SerializeField] GameObject m_ChestUIObj;

    // Start is called before the first frame update
    void Start()
    {
        m_inventory = new InventoryClass(m_sloatSize, m_slotBoxTrans);
        m_inventoryManager = m_ChestObj.GetComponent<inventoryManager>();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 �ł��邱��
 �E�A�C�e���ۑ�
 */

public class InventoryChest : MonoBehaviour
{
    public InventoryClass m_inventory;

    public int m_sloatSize = 10;
    public Transform[] m_spriteTrans;
    public Transform[] m_slotBoxTrans;
    public Text[] m_Text;


    //�I�u�W�F�N�g
    [SerializeField] GameObject m_PlayerObj;
    [SerializeField] GameObject m_inventoryManagerObj;
    public GameObject m_ChestUIObj;

    /// <summary>
    /// �X�^�[�g�֐�
    /// �C���x���g���N���X�쐬
    /// </summary>
    void Start()
    {
        //�C���x���g���N���X�쐬
        m_inventory = new InventoryClass(m_sloatSize, m_slotBoxTrans);
    }
}

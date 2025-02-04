using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPickUpItem
{
    /*[SerializeField] �C���X�y�N�^�[����ݒ�*/
    /// <summary> �C���x���g���I�u�W�F�N�g </summary>
    [SerializeField] GameObject m_playerObj;

    /*�v���C�x�[�g�@private*/
    /// <summary> InventoryWeapon�N���X </summary>
    InventoryItem m_inventoryItem;
    InventoryWeapon m_inventoryWeapon;
    /// <summary> SearchViewArea�N���X </summary>
    SearchViewArea m_searchViewArea;
    /// <summary> PlayerSound�N���X </summary
    PlayerSound m_playerSound;

    /// <summary>
    /// �A�C�e���擾����
    /// </summary>
    public void SetPickUp()
    {
        m_inventoryItem = m_playerObj.GetComponent<InventoryItem>();
        m_inventoryWeapon = m_playerObj.GetComponent<InventoryWeapon>();
        m_searchViewArea = m_playerObj.GetComponent<SearchViewArea>();
        m_playerSound = m_playerObj.GetComponent<PlayerSound>();
    }

    /// <summary>
    /// �A�C�e���擾
    /// </summary>
    /// <param name="_phsh">���͂���Ă��邩</param>
    /// <param name="_search">SearchViewArea�N���X</param>
    /// <param name="_se">PlayerSound�N���X</param>
    /// <returns>���ׂĎ擾�ł����I�u�W�F�N�g�A�o���Ȃ����null</returns>
    public GameObject PickUpItem(bool _phsh)
    {  
        //�A�C�e���擾
        GameObject search_obj = m_searchViewArea.GetObjUpdate("item", 1.5f, 0.03f);
        if (!_phsh || search_obj == null) return null;

        //ID�擾
        ITEM_ID id = search_obj.GetComponent<ItemSetting>().iteminfo.id;

        bool all_get_flag = m_inventoryItem.AddInventory_PickUP_Item(search_obj.GetComponent<ItemSetting>().iteminfo, ref m_inventoryWeapon);

        m_playerSound.PlayPickUp();//SE

        if (all_get_flag)
        {
            //���ׂďE������폜
            return search_obj;
        }
        else
        {
            //���ׂďE���Ȃ������牽�����Ȃ��F������E�����ꍇ�̓v���C���[�̏����i�ɂ���

            if (id >= ITEM_ID.PISTOL && id <= ITEM_ID.SHOTGUN)
            {
                search_obj.GetComponent<GunManager>().m_handPlayerObj = m_playerObj;
            }
        }

        return null;
    }
}

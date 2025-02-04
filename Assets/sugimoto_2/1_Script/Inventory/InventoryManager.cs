using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �C���x���g���֘A�Ɏg�p
/// �C���x���g���̏��/�A�C�e���ړ�
/// </summary>
public enum INVENTORY
{
    NON,    //�����Ȃ�
    ITEM,   //�A�C�e���C���x���g��
    CHEST,  //�`�F�X�g�C���x���g��
    WEAPON, //����C���x���g��
}

public class InventoryManager : MonoBehaviour
{
    //�e��C���x���g��
    InventoryItem m_inventoryItem;
    InventoryWeapon mInventoryWeapon;
    InventoryChest[] m_chestInventory;
    [SerializeField] StopObjectAction m_stopObjectAction;
    player m_player;

    //�萔
    const int GUN_SLOT = 2; //����X���b�g�̈ʒu

    /*�I�u�W�F�N�g�C���x���g���֘A*/
    /// <summary>�`�F�X�g�I�u�W�F�N�g</summary>
    public GameObject[] m_chestObj;
    /// <summary>�v���C���[�I�u�W�F�N�g</summary>
    [SerializeField] GameObject m_playerObj;
    /// <summary>�J���Ă���`�F�X�g�I�u�W�F�N�g</summary>
    GameObject m_openChestObj = null;

    /*�I�u�W�F�N�g�A�C�e���֘A*/
    /// <summary>�͂�ł���A�C�e���̐e�I�u�W�F�N�g�i�`�揇�ʕύX�p�j</summary>
    [SerializeField] GameObject m_catchItemParent;
    /// <summary>�h���b�v�A�C�e���iITEM_ID���j</summary>
    [SerializeField] GameObject[] m_dropItemObj;
    /// <summary>�h���b�v�����A�C�e���̐e�I�u�W�F�N�g�i�܂Ƃ߂�p�j</summary>
    [SerializeField] GameObject m_dropItemsParent;
    /// <summary>�`�F�X�g�ɓ��ꂽ����̐e�I�u�W�F�N�g�i�܂Ƃ߂�p�j</summary>
    [SerializeField] GameObject[] m_chestWeaponParent;

    /// <summary>�C���x���g���̏��</summary>
    public INVENTORY m_inventoryState = INVENTORY.NON;

    //�A�C�e���ړ�
    SELECT_SLOAT can_catch_slot;     //�͂ނ��Ƃ��\�ȃX���b�g�̏��
    SELECT_SLOAT catch_slot;         //�͂�ł���X���b�g�̏��
    SELECT_SLOAT destination_slot;   //�ړ���̃X���b�g���
    [SerializeField] GameObject m_backObj;               //�ړ��悪�w�i


    /// <summary>�ǂ̃X���b�g��I�����Ă���̂���ۑ�</summary>
    struct SELECT_SLOAT
    {
        /// <summary>�I�����Ă���I�u�W�F�N�g</summary>
        public GameObject m_selectObj;
        /// <summary>�I�����Ă���X���b�g�ԍ�</summary>
        public int m_slotNum;
        /// <summary>�I�����Ă���A�C�e���̃C���x���g��</summary>
        public int m_selectInventory;
        /// <summary>�I�����Ă���`�F�X�g</summary>
        public int m_chestNum;
    }

    /// <summary>
    /// �X�^�[�g����
    /// �e�R���|�[�l���g�擾
    /// </summary>
    void Start()
    {
        m_inventoryItem = m_playerObj.GetComponent<InventoryItem>();
        mInventoryWeapon = m_playerObj.GetComponent<InventoryWeapon>();
        m_player = m_playerObj.GetComponent<player>();

        //�`�F�X�g�I�u�W�F�N�g�̐������p��
        m_chestInventory = new InventoryChest[m_chestObj.Length];
        for (int i = 0; i < m_chestObj.Length; i++)
        {
            m_chestInventory[i] = m_chestObj[i].GetComponent<InventoryChest>();
        }
    }

    /// <summary>
    /// �e�C���x���g������
    /// �A�C�e���ړ�
    /// UI�\��
    /// </summary>
    void Update()
    {
        //�����̃C���x���g�����J����Ă���Ώ���
        if (m_inventoryState != INVENTORY.NON)
        {
            CheckInventoryItem();
            MoveItem();
            m_stopObjectAction.ChangeStopState(true);
        }
        else
        {
            m_stopObjectAction.ChangeStopState(false);
        }

        //�A�C�e���C���x���g��
        if (m_inventoryState == INVENTORY.ITEM)
        {
            //�A�C�e���C���x���g���ʏ�\��
            m_inventoryItem.m_inventory.SetUI(m_inventoryItem.m_spriteTrans, m_inventoryItem.m_Text);

            //�G���A���Ȃ�ʏ�\���A�O�Ȃ�g�p�ł��Ȃ��A�C�e���\��
            for (int slot = 0; slot < m_inventoryItem.m_slotSize; slot++)
            {
                if (m_inventoryItem.m_inventory.Slots[slot].ItemInfo == null) continue;

                ITEM_ID id = m_inventoryItem.m_inventory.Slots[slot].ItemInfo.id;

                //if�g�p�ł��Ȃ��A�C�e���Aelse�ł���A�C�e��
                if (id >= ITEM_ID.FOOD_1 && id <= ITEM_ID.FOOD_4 && !m_player.m_inSafeAreaFlag)
                {
                    m_inventoryItem.m_noUseMarkTrans[slot].gameObject.SetActive(true);
                }
                else
                {
                    m_inventoryItem.m_noUseMarkTrans[slot].gameObject.SetActive(false);
                }
            }
        }

        //�`�F�X�g�C���x���g��
        if (m_inventoryState == INVENTORY.CHEST)
        {
            //�J���Ă���`�F�X�g�̃X�N���v�g�擾
            InventoryChest chest_inventory = m_openChestObj.GetComponent<InventoryChest>();

            //UI�ʏ�\��
            m_inventoryItem.m_inventory.SetUI(m_inventoryItem.m_spriteTrans, m_inventoryItem.m_Text);
            //�g�p�֎~mark��\��
            for (int slot = 0; slot < m_inventoryItem.m_slotSize; slot++)
            {
                m_inventoryItem.m_noUseMarkTrans[slot].gameObject.SetActive(false);
            }
            chest_inventory.m_inventory.SetUI(chest_inventory.m_spriteTrans, chest_inventory.m_Text);
        }
    }

    void CheckInventoryItem()    //�J�[�\���̂����Ă���A�C�e���𒲂ׂ�
    {
        SlotInfoInitialization(ref destination_slot);   //������
        SlotInfoInitialization(ref can_catch_slot);     //������

        //�͂ރA�C�e��������
        foreach (RaycastResult result in HitResult())
        {
            //�A�C�e��������ł��Ȃ�
            if (can_catch_slot.m_selectInventory == (int)INVENTORY.NON)
            {
                //�A�C�e���C���x���g��
                for (int i = 0; i < m_inventoryItem.m_slotSize; i++)
                {
                    if (result.gameObject == m_inventoryItem.m_spriteTrans[i].gameObject)
                    {
                        can_catch_slot.m_selectObj = m_inventoryItem.m_spriteTrans[i].gameObject;
                        can_catch_slot.m_slotNum = i;
                        can_catch_slot.m_selectInventory = (int)INVENTORY.ITEM;
                        break;
                    }
                }


                //�`�F�X�g�C���x���g��
                for (int j = 0; j < m_chestObj.Length; j++)
                {
                    for (int i = 0; i < m_chestInventory[j].m_sloatSize; i++)
                    {
                        if (result.gameObject == m_chestInventory[j].m_spriteTrans[i].gameObject)
                        {
                            can_catch_slot.m_selectObj = m_chestInventory[j].m_spriteTrans[i].gameObject;
                            can_catch_slot.m_slotNum = i;
                            can_catch_slot.m_chestNum = j;
                            can_catch_slot.m_selectInventory = (int)INVENTORY.CHEST;
                            Debug.Log(can_catch_slot.m_selectObj);
                            break;
                        }
                    }
                    if (can_catch_slot.m_selectInventory != (int)INVENTORY.NON) break;
                }
            }
        }
        foreach (RaycastResult result in HitResult())
        {
            //�A�C�e���C���x���g��
            for (int i = 0; i < m_inventoryItem.m_slotSize; i++)
            {
                if (result.gameObject == m_inventoryItem.m_BoxTrans[i].gameObject)
                {
                    destination_slot.m_selectObj = m_inventoryItem.m_BoxTrans[i].gameObject;
                    destination_slot.m_slotNum = i;
                    destination_slot.m_selectInventory = (int)INVENTORY.ITEM;
                    break;
                }
            }

            //����C���x���g��
            if (result.gameObject == mInventoryWeapon.m_SlotBoxTrans[GUN_SLOT].gameObject)
            {
                destination_slot.m_selectObj = mInventoryWeapon.m_SlotBoxTrans[GUN_SLOT].gameObject;
                destination_slot.m_slotNum = GUN_SLOT;
                destination_slot.m_selectInventory = (int)INVENTORY.WEAPON;
                break;
            }

            //�`�F�X�g�C���x���g��
            for (int j = 0; j < m_chestObj.Length; j++)
            {
                for (int i = 0; i < m_chestInventory[j].m_sloatSize; i++)
                {
                    if (result.gameObject == m_chestInventory[j].m_slotBoxTrans[i].gameObject)
                    {
                        destination_slot.m_selectObj = m_chestInventory[j].m_slotBoxTrans[i].gameObject;
                        destination_slot.m_slotNum = i;
                        destination_slot.m_chestNum = j;
                        destination_slot.m_selectInventory = (int)INVENTORY.CHEST;
                        break;
                    }
                }

                if (destination_slot.m_selectInventory != (int)INVENTORY.NON) break;
            }

            //�ړ��悪�Ȃ��A�C���x���g���̔w�i�ɓ������Ă���Δw�i������
            if (destination_slot.m_selectObj == null) 
            {
                if (result.gameObject == m_backObj)
                {
                    destination_slot.m_selectObj = m_backObj;
                }
            }
        }

    }


    /// <summary>
    /// �A�C�e���ړ�
    /// CheckInventoryItem()�Œ͂߂�A�C�e�����擾������Ŏg������
    /// �ړ�������A�C�e���I���A�I�������A�C�e�����ړ�
    /// �ړ���ɂ���ď����������قȂ�
    /// ���g�������Ȃ���΂��̂܂܈ړ�
    /// �����A�C�e�����m�̏ꍇ�X�^�b�N����łȂ���΃X�^�b�N���A�c�����ꍇ�͎c�����������������܂܌��̏ꏊ�֖߂�
    /// �قȂ�A�C�e�����m�̏ꍇ�A�ꏊ�����ւ���
    /// ����̏ꍇ�A�A�C�e�������ւ���
    /// </summary>
    void MoveItem()
    {
        //���N���b�N���ꂽ���������
        if(Input.GetMouseButtonDown(0))
        {
            if (can_catch_slot.m_selectObj == null) return;
            catch_slot = can_catch_slot;
            ParentChildren(m_catchItemParent, catch_slot.m_selectObj);
        }

        if (catch_slot.m_selectObj == null) return;

        //���N���b�N�������̊ԃ}�E�X�ɒǏ]
        if (Input.GetMouseButton(0))
        {
            catch_slot.m_selectObj.transform.position = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("1");

            //�N���b�N��������Ă�����
            if (catch_slot.m_selectObj == null) return;

            //�͂�ł���X���b�g�̃C���x���g�����A�C�e���C���x���g��
            if (catch_slot.m_selectInventory == (int)INVENTORY.ITEM)
            {
                //���̐e�ɖ߂�
                ParentChildren(m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].gameObject, catch_slot.m_selectObj);

                //�I�u�W�F�N�g�͌��̈ʒu�ɁA��񂾂��n��
                catch_slot.m_selectObj.transform.position = m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].position;

                //�ړ���
                if (destination_slot.m_selectInventory == (int)INVENTORY.ITEM)
                {
                    MoveItemInfo(ref m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum], ref m_inventoryItem.m_inventory.Slots[destination_slot.m_slotNum]);
                }
                else if (destination_slot.m_selectInventory == (int)INVENTORY.WEAPON)
                {
                    //����ȊO�͕ύX�s��
                    if (m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum].ItemInfo.type != ITEM_TYPE.WEAPON) return;

                    //����I�u�W�F�N�g�A�A�C�e��������ւ�
                    mInventoryWeapon.GunObjChenge(m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum].ItemInfo);
                    ItemInfoChange(ref m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum], ref mInventoryWeapon.m_Inventory.Slots[destination_slot.m_slotNum]);
                }
                else if (destination_slot.m_selectInventory == (int)INVENTORY.CHEST)
                {
                    //ParentChildren(m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum].ItemInfo.weaponitem_info.weapon_obj, m_chestInventory[destination_slot.m_chestNum].m_weaponObjParent.gameObject);
                    MoveItemInfo(ref m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum], ref m_chestInventory[destination_slot.m_chestNum].m_inventory.Slots[destination_slot.m_slotNum]);
                }
                else if (destination_slot.m_selectObj != m_backObj && m_inventoryState == INVENTORY.ITEM)
                {
                    DropItem();
                }

            }
            else if (catch_slot.m_selectInventory == (int)INVENTORY.CHEST)
            {
                //�͂�ł���X���b�g�̃C���x���g�����`�F�X�g�C���x���g��

                //���̐e�ɖ߂�
                ParentChildren(m_chestInventory[catch_slot.m_chestNum].m_slotBoxTrans[catch_slot.m_slotNum].gameObject, catch_slot.m_selectObj);

                //�I�u�W�F�N�g�͌��̈ʒu�ɁA��񂾂��n��
                catch_slot.m_selectObj.transform.position = m_chestInventory[catch_slot.m_chestNum].m_slotBoxTrans[catch_slot.m_slotNum].position;

                //�ړ���
                if (destination_slot.m_selectInventory == (int)INVENTORY.ITEM)
                {
                    MoveItemInfo(ref m_chestInventory[catch_slot.m_chestNum].m_inventory.Slots[catch_slot.m_slotNum], ref m_inventoryItem.m_inventory.Slots[destination_slot.m_slotNum]);
                    //ParentChildren(m_chestInventory[catch_slot.m_chestNum].m_inventory.Slots[catch_slot.m_slotNum].ItemInfo.weaponitem_info.weapon_obj, m_player.GetComponent<InventoryWeapon>().m_weaponParent);
                }
                else if (destination_slot.m_selectInventory == (int)INVENTORY.CHEST)
                {
                    MoveItemInfo(ref m_chestInventory[catch_slot.m_chestNum].m_inventory.Slots[catch_slot.m_slotNum], ref m_chestInventory[destination_slot.m_chestNum].m_inventory.Slots[destination_slot.m_slotNum]);
                }
            }


            //��񏉊���
            SlotInfoInitialization(ref catch_slot);
        }
    }

    /// <summary>
    /// �A�C�e���𗎂Ƃ�
    /// �A�C�e���C���x���g���݂̂̋@�\
    /// </summary>
    public void DropItem()
    {
        //�X���b�g�̒��̃A�C�e��ID�擾
        ItemInformation iteminfo;

        if (catch_slot.m_selectInventory == (int)INVENTORY.ITEM)
        {
            //�X���b�g�̃C���x���g�����擾
            iteminfo = m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum].ItemInfo;

            Vector3 set_pos = m_playerObj.transform.position;
            set_pos.y += 0.5f;
            set_pos += m_playerObj.transform.forward / 2;
            //����ȊO�͐���
            if (iteminfo.id >= ITEM_ID.FOOD_1 && iteminfo.id <= ITEM_ID.BULLET)
            {
                GameObject obj = Instantiate(m_dropItemObj[(int)iteminfo.id], set_pos, Quaternion.identity);              //����
                obj.GetComponent<ItemSetting>().iteminfo = iteminfo;                        //�A�C�e�������
                ParentChildren(m_dropItemsParent, obj);
                obj.transform.position = set_pos;
                obj.GetComponent<ItemSetting>().drop_flag = true;
            }
            //����̏ꍇ�̓v���C���[���������Ă���I�u�W�F�N�g�𗎂Ƃ�
            if (iteminfo.id >= ITEM_ID.PISTOL && iteminfo.id <= ITEM_ID.SHOTGUN)
            {
                GameObject weapon_obj = iteminfo.weaponitem_info.weapon_obj;
                weapon_obj.GetComponent<ItemSetting>().iteminfo = iteminfo;                        //�A�C�e�������
                ParentChildren(m_dropItemsParent, weapon_obj);
                weapon_obj.transform.position = set_pos;
                weapon_obj.GetComponent<GunManager>().DropItemSetting();
            }

            m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum].ItemInfo = null;      //�A�C�e�����폜
            m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum].initializationSlot(); //������
        }
    }

    void MoveItemInfo(ref SlotClass _catch, ref SlotClass _destination)
    {
        if (_destination.CanAddStackItem(_catch))
        {
            //�A�C�e�����d�˂���ꍇ�̏���
            _destination.AddStackItem(ref _catch);
        }
        else
        {
            //�ł��Ȃ��ꍇ�̏���
            ItemInfoChange(ref _catch, ref _destination);
        }
    }

    /// <summary>
    /// �A�C�e��������ւ�
    /// �X���b�g�̏������ւ���
    /// </summary>
    /// <param name="_slot1">����ւ������X���b�g�P��</param>
    /// <param name="_slot2">����ւ������X���b�g�Q��</param>
    void ItemInfoChange(ref SlotClass _slot1 ,ref SlotClass _slot2)
    {
        SlotClass temp = _slot1;
        _slot1 = _slot2;
        _slot2 = temp;
    }

    /// <summary>
    /// �X���b�g��񏉊���
    /// �I��ł����X���b�g�̒��g��������
    /// </summary>
    /// <param name="_slot">���������������g</param>
    void SlotInfoInitialization(ref SELECT_SLOAT _slot)
    {
        //������
        _slot.m_selectObj = null;
        _slot.m_selectInventory = (int)INVENTORY.NON;
        _slot.m_slotNum = -1;
        _slot.m_chestNum = -1;
    }

    /// <summary>
    /// �q�b�g����
    /// ���C���������Ă���I�u�W�F�N�g���擾
    /// </summary>
    /// <returns>���������I�u�W�F�N�g�����X�g�ŕԂ�</returns>
    public List<RaycastResult> HitResult()
    {
        //�}�E�X�̈ʒu����UI���擾����
        //RaycastAll�̈����iPointerEventData�j�쐬
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        //RaycastAll�̌��ʊi�[�pList
        List<RaycastResult> RayResult = new List<RaycastResult>();

        //PointerEventData�Ƀ}�E�X�̈ʒu���Z�b�g
        pointData.position = Input.mousePosition;

        //RayCast�i�X�N���[�����W�j
        EventSystem.current.RaycastAll(pointData, RayResult);

        return RayResult;
    }

    public bool OpenClose(bool _flag,GameObject _item)
    {
        if(_flag)
        {
            if (m_inventoryState == INVENTORY.ITEM)
            {
                m_inventoryItem.m_uiObj.SetActive(false);

                if (catch_slot.m_selectObj != null)
                {
                    //���̐e�ɖ߂�
                    ParentChildren(m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].gameObject, catch_slot.m_selectObj);
                    //�I�u�W�F�N�g�͌��̈ʒu�ɁA��񂾂��n��
                    catch_slot.m_selectObj.transform.position = m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].position;
                }
            }
            if (m_openChestObj != null)
            {
                m_inventoryItem.m_uiObj.SetActive(false);
                m_openChestObj.GetComponent<InventoryChest>().m_ChestUIObj.SetActive(false);
                m_openChestObj = null;

                if (catch_slot.m_selectObj != null)
                {
                    if (catch_slot.m_selectInventory == (int)INVENTORY.ITEM)
                    {
                        //���̐e�ɖ߂�
                        ParentChildren(m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].gameObject, catch_slot.m_selectObj);
                        //�I�u�W�F�N�g�͌��̈ʒu�ɁA��񂾂��n��
                        catch_slot.m_selectObj.transform.position = m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].position;
                    }
                    if (catch_slot.m_selectInventory == (int)INVENTORY.CHEST)
                    {
                        //���̐e�ɖ߂�
                        ParentChildren(m_chestInventory[catch_slot.m_chestNum].m_slotBoxTrans[catch_slot.m_slotNum].gameObject, catch_slot.m_selectObj);
                        //�I�u�W�F�N�g�͌��̈ʒu�ɁA��񂾂��n��
                        catch_slot.m_selectObj.transform.position = m_chestInventory[catch_slot.m_chestNum].m_slotBoxTrans[catch_slot.m_slotNum].position;
                    }

                }

            }
            Screen.lockCursor = true;
            m_inventoryState = INVENTORY.NON;
            return false;
        }
        else
        {
            if (m_inventoryState == INVENTORY.ITEM)
            {
                m_inventoryItem.m_uiObj.SetActive(true);
            }
            if(m_inventoryState == INVENTORY.CHEST)
            {
                m_inventoryItem.m_uiObj.SetActive(true);
                _item.GetComponent<InventoryChest>().m_ChestUIObj.SetActive(true);
                m_openChestObj = _item;
            }
            Screen.lockCursor = false;
            return true;
        }
    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        //�e�q�֌W�ɐݒ�

        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

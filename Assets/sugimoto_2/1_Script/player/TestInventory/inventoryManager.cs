using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum INVENTORY
{
    NON,
    ITEM,
    CHEST,
    WEAPON,
}

public class inventoryManager : MonoBehaviour
{
    InventoryItem m_inventoryItem;
    InventoryWeapon mInventoryWeapon;
    ChestInventory[] ChestInventory;
    player m_player;

    //�萔
    const int GUN_SLOT = 2;

    //�C���x���g�����������Ă���I�u�W�F�N�g
    public GameObject player_obj;
    public GameObject[] chest_inventory;
    //�J�����`�F�X�g�I�u�W�F�N�g
    public GameObject m_openChestObj = null;

    //�C���x���g���̏��(���Ă���A�ǂ̃C���x���g�����J���Ă��邩)
    public INVENTORY inventory_state = INVENTORY.NON;

    //�A�C�e���ړ�
    SELECT_SLOAT can_catch_slot;     //�͂ނ��Ƃ��\�ȃX���b�g�̏��
    SELECT_SLOAT catch_slot;         //�͂�ł���X���b�g�̏��
    SELECT_SLOAT destination_slot;   //�ړ���̃X���b�g���
    [SerializeField] GameObject m_backObj;               //�ړ��悪�w�i

    //�`�揇�Ԃ�ύX���邽�߂̃I�u�W�F�N�g
    [SerializeField] GameObject m_catchItemParent;
    //�h���b�v�A�C�e��(ITEM_ID�̏��ɕ��ׂ�)
    [SerializeField] GameObject[] m_dropItemObj;

    /// <summary>
    /// �ǂ̃X���b�g��I�����Ă���̂���ۑ�
    /// </summary>
    struct SELECT_SLOAT
    {
        public GameObject sloat_obj;   //�͂ރI�u�W�F�N�g
        public int slot_no;            //�X���b�g�̈ʒu
        public int slot_inventory;     //�ǂ̃C���x���g���̃X���b�g��
        public int chest_no;           //�ǂ̃`�F�X�g��
    }

    // Start is called before the first frame update
    void Start()
    {
        m_inventoryItem = player_obj.GetComponent<InventoryItem>();
        mInventoryWeapon = player_obj.GetComponent<InventoryWeapon>();
        m_player = player_obj.GetComponent<player>();

        ChestInventory = new ChestInventory[chest_inventory.Length];
        for (int i = 0; i < chest_inventory.Length; i++)
        {
            ChestInventory[i] = chest_inventory[i].GetComponent<ChestInventory>();
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
        if (inventory_state!=INVENTORY.NON)
        {
            CheckInventoryItem();
            MoveItem();
        }

        //�A�C�e���C���x���g��
        if (inventory_state == INVENTORY.ITEM)
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
        if (inventory_state == INVENTORY.CHEST)
        {
            //�J���Ă���`�F�X�g�̃X�N���v�g�擾
            ChestInventory chest_inventory = m_openChestObj.GetComponent<ChestInventory>();

            //UI�ʏ�\��
            m_inventoryItem.m_inventory.SetUI(m_inventoryItem.m_spriteTrans, m_inventoryItem.m_Text);
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
            if (can_catch_slot.slot_inventory == (int)INVENTORY.NON)
            {
                //�A�C�e���C���x���g��
                for (int i = 0; i < m_inventoryItem.m_slotSize; i++)
                {
                    if (result.gameObject == m_inventoryItem.m_spriteTrans[i].gameObject)
                    {
                        can_catch_slot.sloat_obj = m_inventoryItem.m_spriteTrans[i].gameObject;
                        can_catch_slot.slot_no = i;
                        can_catch_slot.slot_inventory = (int)INVENTORY.ITEM;
                        break;
                    }
                }


                //�`�F�X�g�C���x���g��
                for (int j = 0; j < chest_inventory.Length; j++)
                {
                    for (int i = 0; i < ChestInventory[j].m_sloatSize; i++)
                    {
                        if (result.gameObject == ChestInventory[j].m_spriteTrans[i].gameObject)
                        {
                            can_catch_slot.sloat_obj = ChestInventory[j].m_spriteTrans[i].gameObject;
                            can_catch_slot.slot_no = i;
                            can_catch_slot.chest_no = j;
                            can_catch_slot.slot_inventory = (int)INVENTORY.CHEST;
                            Debug.Log(can_catch_slot.sloat_obj);
                            break;
                        }
                    }
                    if (can_catch_slot.slot_inventory != (int)INVENTORY.NON) break;
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
                    destination_slot.sloat_obj = m_inventoryItem.m_BoxTrans[i].gameObject;
                    destination_slot.slot_no = i;
                    destination_slot.slot_inventory = (int)INVENTORY.ITEM;
                    break;
                }
            }

            //����C���x���g��
            if (result.gameObject == mInventoryWeapon.slot_box[GUN_SLOT].gameObject)
            {
                destination_slot.sloat_obj = mInventoryWeapon.slot_box[GUN_SLOT].gameObject;
                destination_slot.slot_no = GUN_SLOT;
                destination_slot.slot_inventory = (int)INVENTORY.WEAPON;
                break;
            }

            //�`�F�X�g�C���x���g��
            for (int j = 0; j < chest_inventory.Length; j++)
            {
                for (int i = 0; i < ChestInventory[j].m_sloatSize; i++)
                {
                    if (result.gameObject == ChestInventory[j].m_slotBoxTrans[i].gameObject)
                    {
                        destination_slot.sloat_obj = ChestInventory[j].m_slotBoxTrans[i].gameObject;
                        destination_slot.slot_no = i;
                        destination_slot.chest_no = j;
                        destination_slot.slot_inventory = (int)INVENTORY.CHEST;
                        break;
                    }
                }

                if (destination_slot.slot_inventory != (int)INVENTORY.NON) break;
            }

            //�ړ��悪�Ȃ��A�C���x���g���̔w�i�ɓ������Ă���Δw�i������
            if (destination_slot.sloat_obj == null) 
            {
                if (result.gameObject == m_backObj)
                {
                    destination_slot.sloat_obj = m_backObj;
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
            if (can_catch_slot.sloat_obj == null) return;
            catch_slot = can_catch_slot;
            ParentChildren(m_catchItemParent, catch_slot.sloat_obj);
        }

        //���N���b�N�������̊ԃ}�E�X�ɒǏ]
        if (Input.GetMouseButton(0))
        {
            catch_slot.sloat_obj.transform.position = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("1");

            //�N���b�N��������Ă�����
            if (catch_slot.sloat_obj == null) return;

            //�͂�ł���X���b�g�̃C���x���g�����A�C�e���C���x���g��
            if (catch_slot.slot_inventory == (int)INVENTORY.ITEM)
            {
                //���̐e�ɖ߂�
                ParentChildren(m_inventoryItem.m_BoxTrans[catch_slot.slot_no].gameObject, catch_slot.sloat_obj);

                //�I�u�W�F�N�g�͌��̈ʒu�ɁA��񂾂��n��
                catch_slot.sloat_obj.transform.position = m_inventoryItem.m_BoxTrans[catch_slot.slot_no].position;

                //�ړ���
                if (destination_slot.slot_inventory == (int)INVENTORY.ITEM)
                {
                    MoveItemInfo(ref m_inventoryItem.m_inventory.Slots[catch_slot.slot_no], ref m_inventoryItem.m_inventory.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.WEAPON)
                {
                    //����ȊO�͕ύX�s��
                    if (m_inventoryItem.m_inventory.Slots[catch_slot.slot_no].ItemInfo.type != ITEM_TYPE.WEAPON) return;

                    //����I�u�W�F�N�g�A�A�C�e��������ւ�
                    mInventoryWeapon.GunObjChenge(m_inventoryItem.m_inventory.Slots[catch_slot.slot_no].ItemInfo);
                    ItemInfoChange(ref m_inventoryItem.m_inventory.Slots[catch_slot.slot_no], ref mInventoryWeapon.Inventory.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.CHEST)
                {
                    MoveItemInfo(ref m_inventoryItem.m_inventory.Slots[catch_slot.slot_no], ref ChestInventory[destination_slot.chest_no].m_inventory.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.sloat_obj != m_backObj)
                {
                    DropItem();
                }

            }
            else if (catch_slot.slot_inventory == (int)INVENTORY.CHEST)
            {
                //�͂�ł���X���b�g�̃C���x���g�����`�F�X�g�C���x���g��

                //���̐e�ɖ߂�
                ParentChildren(ChestInventory[catch_slot.chest_no].m_slotBoxTrans[catch_slot.slot_no].gameObject, catch_slot.sloat_obj);

                //�I�u�W�F�N�g�͌��̈ʒu�ɁA��񂾂��n��
                catch_slot.sloat_obj.transform.position = ChestInventory[catch_slot.chest_no].m_slotBoxTrans[catch_slot.slot_no].position;

                //�ړ���
                if (destination_slot.slot_inventory == (int)INVENTORY.ITEM)
                {
                    MoveItemInfo(ref ChestInventory[catch_slot.chest_no].m_inventory.Slots[catch_slot.slot_no], ref m_inventoryItem.m_inventory.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.CHEST)
                {
                    MoveItemInfo(ref ChestInventory[catch_slot.chest_no].m_inventory.Slots[catch_slot.slot_no], ref ChestInventory[destination_slot.chest_no].m_inventory.Slots[destination_slot.slot_no]);
                }
            }


            //��񏉊���
            SlotInfoInitialization(ref catch_slot);
            Debug.Log(catch_slot.sloat_obj);
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

        if (catch_slot.slot_inventory == (int)INVENTORY.ITEM)
        {
            //�X���b�g�̃C���x���g�����擾
            iteminfo = m_inventoryItem.m_inventory.Slots[catch_slot.slot_no].ItemInfo;

            Vector3 set_pos = player_obj.transform.position;
            set_pos.y += 0.5f;
            set_pos += player_obj.transform.forward / 2;

            //����ȊO�͐���
            if (iteminfo.id >= ITEM_ID.FOOD_1 && iteminfo.id <= ITEM_ID.BULLET)
            {
                GameObject obj = Instantiate(m_dropItemObj[(int)iteminfo.id], set_pos, Quaternion.identity);              //����
                obj.GetComponent<ItemSetting>().iteminfo = iteminfo;                        //�A�C�e�������
            }
            //����̏ꍇ�̓v���C���[���������Ă���I�u�W�F�N�g�𗎂Ƃ�
            if (iteminfo.id >= ITEM_ID.PISTOL && iteminfo.id <= ITEM_ID.SHOTGUN)
            {
                iteminfo.weaponitem_info.weapon_obj.transform.parent = null;
                iteminfo.weaponitem_info.weapon_obj.SetActive(true);
                iteminfo.weaponitem_info.weapon_obj.GetComponent<BoxCollider>().enabled = true;
                iteminfo.weaponitem_info.weapon_obj.GetComponent<DestoryRigitbody>().enabled = true;
                iteminfo.weaponitem_info.weapon_obj.AddComponent<Rigidbody>();
            }

            m_inventoryItem.m_inventory.Slots[catch_slot.slot_no].ItemInfo = null;      //�A�C�e�����폜
            m_inventoryItem.m_inventory.Slots[catch_slot.slot_no].initializationSlot(); //������
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
        _slot.sloat_obj = null;
        _slot.slot_inventory = (int)INVENTORY.NON;
        _slot.slot_no = -1;
        _slot.chest_no = -1;
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
            if (inventory_state == INVENTORY.ITEM)
            {
                m_inventoryItem.m_itemInventoryObj.SetActive(false);
            }
            if(m_openChestObj != null)
            {
                m_inventoryItem.m_itemInventoryObj.SetActive(false);
                m_openChestObj.GetComponent<ChestInventory>().m_ChestUIObj.SetActive(false);
                m_openChestObj = null;
            }
            Screen.lockCursor = true;
            inventory_state = INVENTORY.NON;
            return false;
        }
        else
        {
            if (inventory_state == INVENTORY.ITEM)
            {
                m_inventoryItem.m_itemInventoryObj.SetActive(true);
            }
            if(inventory_state == INVENTORY.CHEST)
            {
                m_inventoryItem.m_itemInventoryObj.SetActive(true);
                _item.GetComponent<ChestInventory>().m_ChestUIObj.SetActive(true);
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

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
    InventoryItem InventoryItem;
    WeaponInventory WeaponInventory;
    ChestInventory[] ChestInventory;

    //�萔
    const int GUN_SLOT = 3;

    //�C���x���g�����������Ă���I�u�W�F�N�g
    public GameObject player_obj;
    public GameObject[] chest_inventory;
    //�C���x���g���̏��(���Ă���A�ǂ̃C���x���g�����J���Ă��邩)
    public INVENTORY inventory_state = INVENTORY.NON;

    //�A�C�e���ړ�
    SELECT_SLOAT can_catch_slot;     //�͂ނ��Ƃ��\�ȃX���b�g�̏��
    SELECT_SLOAT catch_slot;         //�͂�ł���X���b�g�̏��
    SELECT_SLOAT destination_slot;   //�ړ���̃X���b�g���

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
        InventoryItem = player_obj.GetComponent<InventoryItem>();
        WeaponInventory = player_obj.GetComponent<WeaponInventory>();

        //ChestInventory = new ChestInventory[chest_inventory.Length];
        //for (int i = 0; i < chest_inventory.Length; i++)
        //{
        //    ChestInventory[i] = ChestInventory[i].GetComponent<ChestInventory>();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory_state!=INVENTORY.NON)
        {
            CheckInventoryItem();
            MoveItem();
        }
    }

    void CheckInventoryItem()    //�J�[�\���̂����Ă���A�C�e���𒲂ׂ�
    {
        SlotInfoInitialization(ref destination_slot);
        SlotInfoInitialization(ref can_catch_slot);

        //�͂ރA�C�e��������
        foreach (RaycastResult result in HitResult())
        {
            //�A�C�e��������ł��Ȃ�
            if (can_catch_slot.slot_inventory == (int)INVENTORY.NON)
            {
                //�A�C�e���C���x���g��
                for (int i = 0; i < InventoryItem.slot_size; i++)
                {
                    if (result.gameObject == InventoryItem.sprite[i].gameObject)
                    {
                        if (InventoryItem.Inventory.Slots[i].ItemInfo != null)
                        {
                            Debug.Log(InventoryItem.Inventory.Slots[i].ItemInfo + " " + i);
                        }
                        else
                        {
                            Debug.Log("null " + i);
                        }

                        can_catch_slot.sloat_obj = InventoryItem.sprite[i].gameObject;
                        can_catch_slot.slot_no = i;
                        can_catch_slot.slot_inventory = (int)INVENTORY.ITEM;
                        break;
                    }
                }

                ////�`�F�X�g�C���x���g��
                //for (int j = 0; j < chest_inventory.Length; j++)
                //{
                //    for (int i = 0; i < ChestInventory[j].sloat_size; i++)
                //    {
                //        if (result.gameObject == ChestInventory[j].sprite_pos[i].gameObject)
                //        {
                //            can_catch_slot.sloat_obj = InventoryItem.sprite[i].gameObject;
                //            can_catch_slot.slot_no = i;
                //            can_catch_slot.chest_no = j;
                //            can_catch_slot.slot_inventory = (int)INVENTORY.CHEST;
                //            break;
                //        }
                //    }
                //    if (can_catch_slot.slot_inventory != (int)INVENTORY.NON) break;
                //}
            }
        }
        foreach (RaycastResult result in HitResult())
        {
            //�A�C�e��������ł���

            //�A�C�e���C���x���g��
            for (int i = 0; i < InventoryItem.slot_size; i++)
            {
                if (result.gameObject == InventoryItem.slot_box[i].gameObject)
                {
                    destination_slot.sloat_obj = InventoryItem.slot_box[i].gameObject;
                    destination_slot.slot_no = i;
                    destination_slot.slot_inventory = (int)INVENTORY.ITEM;
                    break;
                }
            }

            ////����C���x���g��
            //if (result.gameObject == WeaponInventory.sloat_box[GUN_SLOT].gameObject)
            //{
            //    destination_slot.sloat_obj = WeaponInventory.sloat_box[GUN_SLOT].gameObject;
            //    destination_slot.slot_no = GUN_SLOT;
            //    destination_slot.slot_inventory = (int)INVENTORY.WEAPON;
            //    break;
            //}

            ////�`�F�X�g�C���x���g��
            //for (int j = 0; j < chest_inventory.Length; j++)
            //{
            //    for (int i = 0; i < ChestInventory[j].sloat_size; i++)
            //    {
            //        if (result.gameObject == ChestInventory[j].slot_box[i].gameObject)
            //        {
            //            destination_slot.sloat_obj = ChestInventory[j].slot_box[i].gameObject;
            //            destination_slot.slot_no = i;
            //            destination_slot.chest_no = j;
            //            destination_slot.slot_inventory = (int)INVENTORY.CHEST;
            //            break;
            //        }
            //    }

            //    if (destination_slot.slot_inventory != (int)INVENTORY.NON) break;
            //}
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
            catch_slot = can_catch_slot;
        }

        //��񂪂Ȃ���ΏI��
        if (catch_slot.slot_inventory == (int)INVENTORY.NON) return;

        //���N���b�N�������̊ԃ}�E�X�ɒǏ]
        if (Input.GetMouseButton(0))
        {
            catch_slot.sloat_obj.transform.position = Input.mousePosition;
        }
        else
        {
            //�N���b�N�������ꂽ��

            //�͂�ł���X���b�g�̃C���x���g�����A�C�e���C���x���g��
            if (catch_slot.slot_inventory == (int)INVENTORY.ITEM)
            {
                //�I�u�W�F�N�g�͌��̈ʒu�ɁA��񂾂��n��
                catch_slot.sloat_obj.transform.position = InventoryItem.slot_box[catch_slot.slot_no].position;

                //�ړ��悪�A�C�e���C���x���g��
                if (destination_slot.slot_inventory == (int)INVENTORY.ITEM)
                {
                    //�A�C�e�����d�˂���ꍇ�̏���
                    if (InventoryItem.Inventory.Slots[destination_slot.slot_no].CanAddStackItem(InventoryItem.Inventory.Slots[catch_slot.slot_no]))
                    {
                        InventoryItem.Inventory.Slots[destination_slot.slot_no].AddStackItem(ref InventoryItem.Inventory.Slots[catch_slot.slot_no]);
                    }
                    else
                    {
                        //�ł��Ȃ��ꍇ�̏���
                        ItemInfoChange(ref InventoryItem.Inventory.Slots[catch_slot.slot_no], ref InventoryItem.Inventory.Slots[destination_slot.slot_no]);
                    }
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.WEAPON)
                {
                    ItemInfoChange(ref InventoryItem.Inventory.Slots[catch_slot.slot_no], ref WeaponInventory.InventoryClass.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.CHEST)
                {
                    //�A�C�e�����d�˂���ꍇ�̏���

                    //�ł��Ȃ��ꍇ�̏���
                    ItemInfoChange(ref InventoryItem.Inventory.Slots[catch_slot.slot_no], ref ChestInventory[destination_slot.chest_no].InventoryClass.Slots[destination_slot.slot_no]);
                }

            }

            SlotInfoInitialization(ref catch_slot);
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

}

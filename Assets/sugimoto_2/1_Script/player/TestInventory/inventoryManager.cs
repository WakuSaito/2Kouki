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
    slot_info can_catch_slot;     //�͂ނ��Ƃ��\�ȃX���b�g�̏��
    slot_info catch_slot;         //�͂�ł���X���b�g�̏��
    slot_info destination_slot;   //�ړ���̃X���b�g���

    struct slot_info
    {
        public GameObject sloat_obj;    //�͂ރI�u�W�F�N�g
        public int slot_no;             //�X���b�g�̈ʒu
        public int slot_inventory;     //�ǂ̃C���x���g���̃X���b�g��
        public int chest_no;            //�ǂ̃`�F�X�g��
    }

    // Start is called before the first frame update
    void Start()
    {
        InventoryItem = player_obj.GetComponent<InventoryItem>();
        WeaponInventory = player_obj.GetComponent<WeaponInventory>();

        ChestInventory = new ChestInventory[chest_inventory.Length];
        for (int i = 0; i < chest_inventory.Length; i++)
        {
            ChestInventory[i] = ChestInventory[i].GetComponent<ChestInventory>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(inventory_state!=INVENTORY.NON)
        {
            CheckInventoryItem();
            MoveItem();
        }
    }

    void CheckInventoryItem()    //�J�[�\���̂����Ă���A�C�e���𒲂ׂ�
    {
        SlotInfoInitialization(destination_slot);
        SlotInfoInitialization(can_catch_slot);
        //can_catch_slot.slot_inventory = (int)INVENTORY.NON;

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
                        can_catch_slot.sloat_obj = InventoryItem.sprite[i].gameObject;
                        can_catch_slot.slot_no = i;
                        can_catch_slot.slot_inventory = (int)INVENTORY.ITEM;
                        break;
                    }
                }

                //�`�F�X�g�C���x���g��
                for (int j = 0; j < chest_inventory.Length; j++)
                {
                    for (int i = 0; i < ChestInventory[j].sloat_size; i++)
                    {
                        if (result.gameObject == ChestInventory[j].sprite_pos[i].gameObject)
                        {
                            can_catch_slot.sloat_obj = InventoryItem.sprite[i].gameObject;
                            can_catch_slot.slot_no = i;
                            can_catch_slot.chest_no = j;
                            can_catch_slot.slot_inventory = (int)INVENTORY.CHEST;
                            break;
                        }
                    }
                    if (can_catch_slot.slot_inventory != (int)INVENTORY.NON) break;
                }
            }
            //�A�C�e��������ł���
            else
            {
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

                //����C���x���g��
                if (result.gameObject == WeaponInventory.sloat_box[GUN_SLOT].gameObject)
                {
                    destination_slot.sloat_obj = WeaponInventory.sloat_box[GUN_SLOT].gameObject;
                    destination_slot.slot_no = GUN_SLOT;
                    destination_slot.slot_inventory = (int)INVENTORY.WEAPON;
                    break;
                }

                //�`�F�X�g�C���x���g��
                for (int j = 0; j < chest_inventory.Length; j++)
                {
                    for (int i = 0; i < ChestInventory[j].sloat_size; i++)
                    {
                        if (result.gameObject == ChestInventory[j].slot_box[i].gameObject)
                        {
                            destination_slot.sloat_obj = ChestInventory[j].slot_box[i].gameObject;
                            destination_slot.slot_no = i;
                            destination_slot.chest_no = j;
                            destination_slot.slot_inventory = (int)INVENTORY.CHEST;
                            break;
                        }
                    }

                    if (destination_slot.slot_inventory != (int)INVENTORY.NON) break;
                }
            }
        }
    }

    void MoveItem()
    {
        if (can_catch_slot.slot_inventory == (int)INVENTORY.NON ) return;

        //���N���b�N���ꂽ��͂�
        if(Input.GetMouseButtonDown(0))
        {
            catch_slot = can_catch_slot;
        }

        //���N���b�N�������̊ԃ}�E�X�ɒǏ]
        if (Input.GetMouseButton(0))
        {
            catch_slot.sloat_obj.transform.position = Input.mousePosition;
        }
        //�N���b�N�������ꂽ��
        else
        {
            //�͂�ł���X���b�g�̃C���x���g�����A�C�e���C���x���g��
            if (catch_slot.slot_inventory == (int)INVENTORY.ITEM)
            {
                //�I�u�W�F�N�g�͌��̈ʒu�ɁA��񂾂��n��
                catch_slot.sloat_obj.transform.position = InventoryItem.slot_box[catch_slot.slot_no].position;
                
                //�ړ��悪�A�C�e���C���x���g��
                if (destination_slot.slot_inventory == (int)INVENTORY.ITEM)
                {
                    //�A�C�e�����d�˂���ꍇ�̏���

                    //�ł��Ȃ��ꍇ�̏���
                    ItemInfoChange(ref InventoryItem.Inventory.Slots[catch_slot.slot_no], ref InventoryItem.Inventory.Slots[destination_slot.slot_no]);
                }
                else if(destination_slot.slot_inventory == (int)INVENTORY.WEAPON)
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
        }
    }

    void ItemInfoChange(ref SlotClass _slot1 ,ref SlotClass _slot2)
    {
        SlotClass temp = _slot1;
        _slot1 = _slot2;
        _slot2 = temp;
    }

    void SlotInfoInitialization(slot_info _slot)
    {
        //������
        _slot.sloat_obj = null;
        _slot.slot_inventory = (int)INVENTORY.NON;
        _slot.slot_no = -1;
        _slot.chest_no = -1;
    }

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

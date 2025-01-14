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

    //定数
    const int GUN_SLOT = 3;

    //インベントリ情報を持っているオブジェクト
    public GameObject player_obj;
    public GameObject[] chest_inventory;
    //インベントリの状態(閉じている、どのインベントリを開いているか)
    public INVENTORY inventory_state = INVENTORY.NON;

    //アイテム移動
    slot_info can_catch_slot;     //掴むことが可能なスロットの情報
    slot_info catch_slot;         //掴んでいるスロットの情報
    slot_info destination_slot;   //移動先のスロット情報

    struct slot_info
    {
        public GameObject sloat_obj;    //掴むオブジェクト
        public int slot_no;             //スロットの位置
        public int slot_inventory;     //どのインベントリのスロットか
        public int chest_no;            //どのチェストか
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

    void CheckInventoryItem()    //カーソルのあっているアイテムを調べる
    {
        SlotInfoInitialization(destination_slot);
        SlotInfoInitialization(can_catch_slot);
        //can_catch_slot.slot_inventory = (int)INVENTORY.NON;

        //掴むアイテムを決定
        foreach (RaycastResult result in HitResult())
        {
            //アイテムをつかんでいない
            if (can_catch_slot.slot_inventory == (int)INVENTORY.NON) 
            {
                //アイテムインベントリ
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

                //チェストインベントリ
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
            //アイテムをつかんでいる
            else
            {
                //アイテムインベントリ
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

                //武器インベントリ
                if (result.gameObject == WeaponInventory.sloat_box[GUN_SLOT].gameObject)
                {
                    destination_slot.sloat_obj = WeaponInventory.sloat_box[GUN_SLOT].gameObject;
                    destination_slot.slot_no = GUN_SLOT;
                    destination_slot.slot_inventory = (int)INVENTORY.WEAPON;
                    break;
                }

                //チェストインベントリ
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

        //左クリックされたら掴む
        if(Input.GetMouseButtonDown(0))
        {
            catch_slot = can_catch_slot;
        }

        //左クリック長押しの間マウスに追従
        if (Input.GetMouseButton(0))
        {
            catch_slot.sloat_obj.transform.position = Input.mousePosition;
        }
        //クリックが離されたら
        else
        {
            //掴んでいるスロットのインベントリがアイテムインベントリ
            if (catch_slot.slot_inventory == (int)INVENTORY.ITEM)
            {
                //オブジェクトは元の位置に、情報だけ渡す
                catch_slot.sloat_obj.transform.position = InventoryItem.slot_box[catch_slot.slot_no].position;
                
                //移動先がアイテムインベントリ
                if (destination_slot.slot_inventory == (int)INVENTORY.ITEM)
                {
                    //アイテムを重ねられる場合の処理

                    //できない場合の処理
                    ItemInfoChange(ref InventoryItem.Inventory.Slots[catch_slot.slot_no], ref InventoryItem.Inventory.Slots[destination_slot.slot_no]);
                }
                else if(destination_slot.slot_inventory == (int)INVENTORY.WEAPON)
                {
                    ItemInfoChange(ref InventoryItem.Inventory.Slots[catch_slot.slot_no], ref WeaponInventory.InventoryClass.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.CHEST)
                {
                    //アイテムを重ねられる場合の処理

                    //できない場合の処理
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
        //初期化
        _slot.sloat_obj = null;
        _slot.slot_inventory = (int)INVENTORY.NON;
        _slot.slot_no = -1;
        _slot.chest_no = -1;
    }

    public List<RaycastResult> HitResult()
    {
        //マウスの位置からUIを取得する
        //RaycastAllの引数（PointerEventData）作成
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        //RaycastAllの結果格納用List
        List<RaycastResult> RayResult = new List<RaycastResult>();

        //PointerEventDataにマウスの位置をセット
        pointData.position = Input.mousePosition;

        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointData, RayResult);

        return RayResult;
    }

}

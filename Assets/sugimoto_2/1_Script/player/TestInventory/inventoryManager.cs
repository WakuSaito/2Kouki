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
    InventoryWeapon mInventoryWeapon;
    ChestInventory[] ChestInventory;

    //定数
    const int GUN_SLOT = 2;

    //インベントリ情報を持っているオブジェクト
    public GameObject player_obj;
    public GameObject[] chest_inventory;
    public GameObject m_openChestObj = null;
    //インベントリの状態(閉じている、どのインベントリを開いているか)
    public INVENTORY inventory_state = INVENTORY.NON;

    //アイテム移動
    SELECT_SLOAT can_catch_slot;     //掴むことが可能なスロットの情報
    SELECT_SLOAT catch_slot;         //掴んでいるスロットの情報
    SELECT_SLOAT destination_slot;   //移動先のスロット情報

    /// <summary>
    /// どのスロットを選択しているのかを保存
    /// </summary>
    struct SELECT_SLOAT
    {
        public GameObject sloat_obj;   //掴むオブジェクト
        public int slot_no;            //スロットの位置
        public int slot_inventory;     //どのインベントリのスロットか
        public int chest_no;           //どのチェストか
    }

    // Start is called before the first frame update
    void Start()
    {
        InventoryItem = player_obj.GetComponent<InventoryItem>();
        mInventoryWeapon = player_obj.GetComponent<InventoryWeapon>();

        ChestInventory = new ChestInventory[chest_inventory.Length];
        for (int i = 0; i < chest_inventory.Length; i++)
        {
            ChestInventory[i] = chest_inventory[i].GetComponent<ChestInventory>();
        }
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

    void CheckInventoryItem()    //カーソルのあっているアイテムを調べる
    {
        SlotInfoInitialization(ref destination_slot);
        SlotInfoInitialization(ref can_catch_slot);

        //掴むアイテムを決定
        foreach (RaycastResult result in HitResult())
        {
            //アイテムをつかんでいない
            if (can_catch_slot.slot_inventory == (int)INVENTORY.NON)
            {
                //アイテムインベントリ
                for (int i = 0; i < InventoryItem.m_slotSize; i++)
                {
                    if (result.gameObject == InventoryItem.m_spriteTrans[i].gameObject)
                    {
                        can_catch_slot.sloat_obj = InventoryItem.m_spriteTrans[i].gameObject;
                        can_catch_slot.slot_no = i;
                        can_catch_slot.slot_inventory = (int)INVENTORY.ITEM;
                        break;
                    }
                }


                //チェストインベントリ
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
            //アイテムをつかんでいる

            //アイテムインベントリ
            for (int i = 0; i < InventoryItem.m_slotSize; i++)
            {
                if (result.gameObject == InventoryItem.m_BoxTrans[i].gameObject)
                {
                    destination_slot.sloat_obj = InventoryItem.m_BoxTrans[i].gameObject;
                    destination_slot.slot_no = i;
                    destination_slot.slot_inventory = (int)INVENTORY.ITEM;
                    break;
                }
            }

            //武器インベントリ
            if (result.gameObject == mInventoryWeapon.slot_box[GUN_SLOT].gameObject)
            {
                destination_slot.sloat_obj = mInventoryWeapon.slot_box[GUN_SLOT].gameObject;
                destination_slot.slot_no = GUN_SLOT;
                destination_slot.slot_inventory = (int)INVENTORY.WEAPON;
                break;
            }

            //チェストインベントリ
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
        }

    }


    /// <summary>
    /// アイテム移動
    /// CheckInventoryItem()で掴めるアイテムを取得した後で使う処理
    /// 移動させるアイテム選択、選択したアイテムを移動
    /// 移動先によって少し処理が異なる
    /// 中身が何もなければそのまま移動
    /// 同じアイテム同士の場合スタック上限でなければスタックし、残った場合は残った個数を所持したまま元の場所へ戻す
    /// 異なるアイテム同士の場合、場所を入れ替える
    /// 武器の場合、アイテムを入れ替える
    /// </summary>
    void MoveItem()
    {
        //左クリックされたら情報を入れる
        if(Input.GetMouseButtonDown(0))
        {
            catch_slot = can_catch_slot;
        }

        //情報がなければ終了
        if (catch_slot.slot_inventory == (int)INVENTORY.NON) return;

        //左クリック長押しの間マウスに追従
        if (Input.GetMouseButton(0))
        {
            catch_slot.sloat_obj.transform.position = Input.mousePosition;
        }
        else
        {
            //クリックが離されたら

            //掴んでいるスロットのインベントリがアイテムインベントリ
            if (catch_slot.slot_inventory == (int)INVENTORY.ITEM)
            {
                //オブジェクトは元の位置に、情報だけ渡す
                catch_slot.sloat_obj.transform.position = InventoryItem.m_BoxTrans[catch_slot.slot_no].position;

                //移動先
                if (destination_slot.slot_inventory == (int)INVENTORY.ITEM)
                {
                    MoveItemInfo(ref InventoryItem.m_inventory.Slots[catch_slot.slot_no], ref InventoryItem.m_inventory.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.WEAPON)
                {
                    //武器以外は変更不可
                    if (InventoryItem.m_inventory.Slots[catch_slot.slot_no].ItemInfo.type != ITEM_TYPE.WEAPON) return;

                    //武器オブジェクト、アイテム情報入れ替え
                    mInventoryWeapon.GunObjChenge(InventoryItem.m_inventory.Slots[catch_slot.slot_no].ItemInfo);
                    ItemInfoChange(ref InventoryItem.m_inventory.Slots[catch_slot.slot_no], ref mInventoryWeapon.Inventory.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.CHEST)
                {
                    MoveItemInfo(ref InventoryItem.m_inventory.Slots[catch_slot.slot_no], ref ChestInventory[destination_slot.chest_no].m_inventory.Slots[destination_slot.slot_no]);
                }
            }
            //掴んでいるスロットのインベントリがチェストインベントリ
            if (catch_slot.slot_inventory == (int)INVENTORY.CHEST)
            {
                //オブジェクトは元の位置に、情報だけ渡す
                catch_slot.sloat_obj.transform.position = ChestInventory[catch_slot.chest_no].m_slotBoxTrans[catch_slot.slot_no].position;

                //移動先
                if (destination_slot.slot_inventory == (int)INVENTORY.ITEM)
                {
                    MoveItemInfo(ref ChestInventory[catch_slot.chest_no].m_inventory.Slots[catch_slot.slot_no], ref InventoryItem.m_inventory.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.CHEST)
                {
                    MoveItemInfo(ref ChestInventory[catch_slot.chest_no].m_inventory.Slots[catch_slot.slot_no], ref ChestInventory[destination_slot.chest_no].m_inventory.Slots[destination_slot.slot_no]);
                }
            }
            SlotInfoInitialization(ref catch_slot);
        }
    }

    void MoveItemInfo(ref SlotClass _catch, ref SlotClass _destination)
    {
        if (_destination.CanAddStackItem(_catch))
        {
            //アイテムを重ねられる場合の処理
            _destination.AddStackItem(ref _catch);
        }
        else
        {
            //できない場合の処理
            ItemInfoChange(ref _catch, ref _destination);
        }
    }

    /// <summary>
    /// アイテム情報入れ替え
    /// スロットの情報を入れ替える
    /// </summary>
    /// <param name="_slot1">入れ替えたいスロット１つ目</param>
    /// <param name="_slot2">入れ替えたいスロット２つ目</param>
    void ItemInfoChange(ref SlotClass _slot1 ,ref SlotClass _slot2)
    {
        SlotClass temp = _slot1;
        _slot1 = _slot2;
        _slot2 = temp;
    }

    /// <summary>
    /// スロット情報初期化
    /// 選んでいたスロットの中身を初期化
    /// </summary>
    /// <param name="_slot">初期化したい中身</param>
    void SlotInfoInitialization(ref SELECT_SLOAT _slot)
    {
        //初期化
        _slot.sloat_obj = null;
        _slot.slot_inventory = (int)INVENTORY.NON;
        _slot.slot_no = -1;
        _slot.chest_no = -1;
    }

    /// <summary>
    /// ヒット結果
    /// レイが当たっているオブジェクトを取得
    /// </summary>
    /// <returns>当たったオブジェクトをリストで返す</returns>
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

    public bool OpenClose(bool _flag,GameObject _item)
    {
        if(_flag)
        {
            if (inventory_state == INVENTORY.ITEM)
            {
                InventoryItem.m_itemInventoryObj.SetActive(false);
            }
            if(m_openChestObj != null)
            {
                InventoryItem.m_itemInventoryObj.SetActive(false);
                m_openChestObj.SetActive(false);
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
                InventoryItem.m_itemInventoryObj.SetActive(true);
            }
            if(inventory_state == INVENTORY.CHEST)
            {
                InventoryItem.m_itemInventoryObj.SetActive(true);
                _item.GetComponent<ChestInventory>().m_ChestUIObj.SetActive(true);
                m_openChestObj = _item.GetComponent<ChestInventory>().m_ChestUIObj;
            }
            Screen.lockCursor = false;
            return true;
        }
    }
}

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

    //定数
    const int GUN_SLOT = 2;

    //インベントリ情報を持っているオブジェクト
    public GameObject player_obj;
    public GameObject[] chest_inventory;
    //開いたチェストオブジェクト
    public GameObject m_openChestObj = null;

    //インベントリの状態(閉じている、どのインベントリを開いているか)
    public INVENTORY inventory_state = INVENTORY.NON;

    //アイテム移動
    SELECT_SLOAT can_catch_slot;     //掴むことが可能なスロットの情報
    SELECT_SLOAT catch_slot;         //掴んでいるスロットの情報
    SELECT_SLOAT destination_slot;   //移動先のスロット情報
    [SerializeField] GameObject m_backObj;               //移動先が背景

    //描画順番を変更するためのオブジェクト
    [SerializeField] GameObject m_catchItemParent;
    //ドロップアイテム(ITEM_IDの順に並べる)
    [SerializeField] GameObject[] m_dropItemObj;

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
    /// 各インベントリ操作
    /// アイテム移動
    /// UI表示
    /// </summary>
    void Update()
    {
        //何かのインベントリが開かれていれば処理
        if (inventory_state!=INVENTORY.NON)
        {
            CheckInventoryItem();
            MoveItem();
        }

        //アイテムインベントリ
        if (inventory_state == INVENTORY.ITEM)
        {
            //アイテムインベントリ通常表示
            m_inventoryItem.m_inventory.SetUI(m_inventoryItem.m_spriteTrans, m_inventoryItem.m_Text);

            //エリア内なら通常表示、外なら使用できないアイテム表示
            for (int slot = 0; slot < m_inventoryItem.m_slotSize; slot++)
            {
                if (m_inventoryItem.m_inventory.Slots[slot].ItemInfo == null) continue;

                ITEM_ID id = m_inventoryItem.m_inventory.Slots[slot].ItemInfo.id;

                //if使用できないアイテム、elseできるアイテム
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

        //チェストインベントリ
        if (inventory_state == INVENTORY.CHEST)
        {
            //開いているチェストのスクリプト取得
            ChestInventory chest_inventory = m_openChestObj.GetComponent<ChestInventory>();

            //UI通常表示
            m_inventoryItem.m_inventory.SetUI(m_inventoryItem.m_spriteTrans, m_inventoryItem.m_Text);
            chest_inventory.m_inventory.SetUI(chest_inventory.m_spriteTrans, chest_inventory.m_Text);
        }
    }

    void CheckInventoryItem()    //カーソルのあっているアイテムを調べる
    {
        SlotInfoInitialization(ref destination_slot);   //初期化
        SlotInfoInitialization(ref can_catch_slot);     //初期化

        //掴むアイテムを決定
        foreach (RaycastResult result in HitResult())
        {
            //アイテムをつかんでいない
            if (can_catch_slot.slot_inventory == (int)INVENTORY.NON)
            {
                //アイテムインベントリ
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
            //アイテムインベントリ
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

            //移動先がなく、インベントリの背景に当たっていれば背景を入れる
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
            if (can_catch_slot.sloat_obj == null) return;

            catch_slot = can_catch_slot;
            ParentChildren(m_catchItemParent, catch_slot.sloat_obj);
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
                //元の親に戻す
                ParentChildren(m_inventoryItem.m_BoxTrans[catch_slot.slot_no].gameObject, catch_slot.sloat_obj);

                //オブジェクトは元の位置に、情報だけ渡す
                catch_slot.sloat_obj.transform.position = m_inventoryItem.m_BoxTrans[catch_slot.slot_no].position;

                //武器インベントリに持っていったらバグる
                if (destination_slot.sloat_obj == null)
                {
                    DropItem();
                }

                //移動先
                if (destination_slot.slot_inventory == (int)INVENTORY.ITEM)
                {
                    MoveItemInfo(ref m_inventoryItem.m_inventory.Slots[catch_slot.slot_no], ref m_inventoryItem.m_inventory.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.WEAPON)
                {
                    //武器以外は変更不可
                    if (m_inventoryItem.m_inventory.Slots[catch_slot.slot_no].ItemInfo.type != ITEM_TYPE.WEAPON) return;

                    //武器オブジェクト、アイテム情報入れ替え
                    mInventoryWeapon.GunObjChenge(m_inventoryItem.m_inventory.Slots[catch_slot.slot_no].ItemInfo);
                    ItemInfoChange(ref m_inventoryItem.m_inventory.Slots[catch_slot.slot_no], ref mInventoryWeapon.Inventory.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.CHEST)
                {
                    MoveItemInfo(ref m_inventoryItem.m_inventory.Slots[catch_slot.slot_no], ref ChestInventory[destination_slot.chest_no].m_inventory.Slots[destination_slot.slot_no]);
                }
            }
            //掴んでいるスロットのインベントリがチェストインベントリ
            if (catch_slot.slot_inventory == (int)INVENTORY.CHEST)
            {
                //元の親に戻す
                ParentChildren(ChestInventory[catch_slot.chest_no].m_slotBoxTrans[catch_slot.slot_no].gameObject, catch_slot.sloat_obj);

                //オブジェクトは元の位置に、情報だけ渡す
                catch_slot.sloat_obj.transform.position = ChestInventory[catch_slot.chest_no].m_slotBoxTrans[catch_slot.slot_no].position;

                //移動先
                if (destination_slot.slot_inventory == (int)INVENTORY.ITEM)
                {
                    MoveItemInfo(ref ChestInventory[catch_slot.chest_no].m_inventory.Slots[catch_slot.slot_no], ref m_inventoryItem.m_inventory.Slots[destination_slot.slot_no]);
                }
                else if (destination_slot.slot_inventory == (int)INVENTORY.CHEST)
                {
                    MoveItemInfo(ref ChestInventory[catch_slot.chest_no].m_inventory.Slots[catch_slot.slot_no], ref ChestInventory[destination_slot.chest_no].m_inventory.Slots[destination_slot.slot_no]);
                }
            }

            //情報初期化
            SlotInfoInitialization(ref catch_slot);
        }
    }

    /// <summary>
    /// アイテムを落とす
    /// アイテムインベントリのみの機能
    /// </summary>
    public void DropItem()
    {
        //スロットの中のアイテムID取得
        ItemInformation iteminfo;

        if (catch_slot.slot_inventory == (int)INVENTORY.ITEM)
        {
            //スロットのインベントリ情報取得
            iteminfo = m_inventoryItem.m_inventory.Slots[catch_slot.slot_no].ItemInfo;

            Vector3 set_pos = player_obj.transform.position;
            set_pos.y += 0.5f;
            set_pos += player_obj.transform.forward / 2;
            GameObject obj = Instantiate(m_dropItemObj[(int)iteminfo.id], set_pos, Quaternion.identity);              //生成
            obj.GetComponent<ItemSetting>().iteminfo = iteminfo;                        //アイテム情報代入
            m_inventoryItem.m_inventory.Slots[catch_slot.slot_no].ItemInfo = null;      //アイテム情報削除
            m_inventoryItem.m_inventory.Slots[catch_slot.slot_no].initializationSlot(); //初期化
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
        //親子関係に設定

        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

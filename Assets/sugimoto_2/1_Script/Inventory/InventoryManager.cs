using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// インベントリ関連に使用
/// インベントリの状態/アイテム移動
/// </summary>
public enum INVENTORY
{
    NON,    //何もない
    ITEM,   //アイテムインベントリ
    CHEST,  //チェストインベントリ
    WEAPON, //武器インベントリ
}

public class InventoryManager : MonoBehaviour
{
    //各種インベントリ
    InventoryItem m_inventoryItem;
    InventoryWeapon mInventoryWeapon;
    InventoryChest[] m_chestInventory;
    [SerializeField] StopObjectAction m_stopObjectAction;
    player m_player;

    [SerializeField]//サウンド用
    private PlayerSound m_playerSound;

    //定数
    const int GUN_SLOT = 2; //武器スロットの位置

    /*オブジェクトインベントリ関連*/
    /// <summary>チェストオブジェクト</summary>
    public GameObject[] m_chestObj;
    /// <summary>プレイヤーオブジェクト</summary>
    [SerializeField] GameObject m_playerObj;
    /// <summary>開けているチェストオブジェクト</summary>
    GameObject m_openChestObj = null;

    /*オブジェクトアイテム関連*/
    /// <summary>掴んでいるアイテムの親オブジェクト（描画順位変更用）</summary>
    [SerializeField] GameObject m_catchItemParent;
    /// <summary>ドロップアイテム（ITEM_ID順）</summary>
    [SerializeField] GameObject[] m_dropItemObj;
    /// <summary>ドロップしたアイテムの親オブジェクト（まとめる用）</summary>
    [SerializeField] GameObject m_dropItemsParent;
    /// <summary>チェストに入れた武器の親オブジェクト（まとめる用）</summary>
    [SerializeField] GameObject[] m_chestWeaponParent;

    /// <summary>インベントリの状態</summary>
    public INVENTORY m_inventoryState = INVENTORY.NON;

    //アイテム移動
    SELECT_SLOAT can_catch_slot;     //掴むことが可能なスロットの情報
    SELECT_SLOAT catch_slot;         //掴んでいるスロットの情報
    SELECT_SLOAT destination_slot;   //移動先のスロット情報
    [SerializeField] GameObject m_backObj;               //移動先が背景


    /// <summary>どのスロットを選択しているのかを保存</summary>
    struct SELECT_SLOAT
    {
        /// <summary>選択しているオブジェクト</summary>
        public GameObject m_selectObj;
        /// <summary>選択しているスロット番号</summary>
        public int m_slotNum;
        /// <summary>選択しているアイテムのインベントリ</summary>
        public int m_selectInventory;
        /// <summary>選択しているチェスト</summary>
        public int m_chestNum;
    }

    /// <summary>
    /// スタート処理
    /// 各コンポーネント取得
    /// </summary>
    void Start()
    {
        m_inventoryItem = m_playerObj.GetComponent<InventoryItem>();
        mInventoryWeapon = m_playerObj.GetComponent<InventoryWeapon>();
        m_player = m_playerObj.GetComponent<player>();

        //チェストオブジェクトの数だけ用意
        m_chestInventory = new InventoryChest[m_chestObj.Length];
        for (int i = 0; i < m_chestObj.Length; i++)
        {
            m_chestInventory[i] = m_chestObj[i].GetComponent<InventoryChest>();
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

        //アイテムインベントリ
        if (m_inventoryState == INVENTORY.ITEM)
        {
            //アイテムインベントリ通常表示
            m_inventoryItem.m_inventory.SetUI(m_inventoryItem.m_spriteTrans, m_inventoryItem.m_Text);

            //エリア内なら通常表示、外なら使用できないアイテム表示
            for (int slot = 0; slot < m_inventoryItem.m_slotSize; slot++)
            {
                if (m_inventoryItem.m_inventory.Slots[slot].ItemInfo == null) continue;

                ITEM_ID id = m_inventoryItem.m_inventory.Slots[slot].ItemInfo.id;

                //if使用できないアイテム、elseできるアイテム
                if (id >= ITEM_ID.FOOD_1 && id <= ITEM_ID.FOOD_4  && !m_player.m_inSafeAreaFlag || id == ITEM_ID.EMERGENCY_PACK && !m_player.m_inSafeAreaFlag)
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
        if (m_inventoryState == INVENTORY.CHEST)
        {
            //開いているチェストのスクリプト取得
            InventoryChest chest_inventory = m_openChestObj.GetComponent<InventoryChest>();

            //UI通常表示
            m_inventoryItem.m_inventory.SetUI(m_inventoryItem.m_spriteTrans, m_inventoryItem.m_Text);
            //使用禁止mark非表示
            for (int slot = 0; slot < m_inventoryItem.m_slotSize; slot++)
            {
                m_inventoryItem.m_noUseMarkTrans[slot].gameObject.SetActive(false);
            }
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
            if (can_catch_slot.m_selectInventory == (int)INVENTORY.NON)
            {
                //アイテムインベントリ
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


                //チェストインベントリ
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
            //アイテムインベントリ
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

            //武器インベントリ
            if (result.gameObject == mInventoryWeapon.m_SlotBoxTrans[GUN_SLOT].gameObject)
            {
                destination_slot.m_selectObj = mInventoryWeapon.m_SlotBoxTrans[GUN_SLOT].gameObject;
                destination_slot.m_slotNum = GUN_SLOT;
                destination_slot.m_selectInventory = (int)INVENTORY.WEAPON;
                break;
            }

            //チェストインベントリ
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

            //移動先がなく、インベントリの背景に当たっていれば背景を入れる
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
            if (can_catch_slot.m_selectObj == null) return;
            catch_slot = can_catch_slot;
            ParentChildren(m_catchItemParent, catch_slot.m_selectObj);
        }

        if (catch_slot.m_selectObj == null) return;

        //左クリック長押しの間マウスに追従
        if (Input.GetMouseButton(0))
        {
            catch_slot.m_selectObj.transform.position = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("1");

            //クリックが離されていたら
            if (catch_slot.m_selectObj == null) return;

            //掴んでいるスロットのインベントリがアイテムインベントリ
            if (catch_slot.m_selectInventory == (int)INVENTORY.ITEM)
            {
                //元の親に戻す
                ParentChildren(m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].gameObject, catch_slot.m_selectObj);

                //オブジェクトは元の位置に、情報だけ渡す
                catch_slot.m_selectObj.transform.position = m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].position;

                //移動先
                if (destination_slot.m_selectInventory == (int)INVENTORY.ITEM)
                {
                    MoveItemInfo(ref m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum], ref m_inventoryItem.m_inventory.Slots[destination_slot.m_slotNum]);
                }
                else if (destination_slot.m_selectInventory == (int)INVENTORY.WEAPON)
                {
                    //武器以外は変更不可
                    if (m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum].ItemInfo.type != ITEM_TYPE.WEAPON) return;

                    //武器オブジェクト、アイテム情報入れ替え
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
                //掴んでいるスロットのインベントリがチェストインベントリ

                //元の親に戻す
                ParentChildren(m_chestInventory[catch_slot.m_chestNum].m_slotBoxTrans[catch_slot.m_slotNum].gameObject, catch_slot.m_selectObj);

                //オブジェクトは元の位置に、情報だけ渡す
                catch_slot.m_selectObj.transform.position = m_chestInventory[catch_slot.m_chestNum].m_slotBoxTrans[catch_slot.m_slotNum].position;

                //移動先
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

        if (catch_slot.m_selectInventory == (int)INVENTORY.ITEM)
        {
            //スロットのインベントリ情報取得
            iteminfo = m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum].ItemInfo;

            Vector3 set_pos = m_playerObj.transform.position;
            set_pos.y += 0.5f;
            set_pos += m_playerObj.transform.forward / 2;
            //武器以外は生成
            if (iteminfo.id >= ITEM_ID.FOOD_1 && iteminfo.id <= ITEM_ID.BULLET)
            {
                GameObject obj = Instantiate(m_dropItemObj[(int)iteminfo.id], set_pos, Quaternion.identity);              //生成
                obj.GetComponent<ItemSetting>().iteminfo = iteminfo;                        //アイテム情報代入
                ParentChildren(m_dropItemsParent, obj);
                obj.transform.position = set_pos;
                obj.GetComponent<ItemSetting>().drop_flag = true;
            }
            //武器の場合はプレイヤーが所持しているオブジェクトを落とす
            if (iteminfo.id >= ITEM_ID.PISTOL && iteminfo.id <= ITEM_ID.SHOTGUN)
            {
                GameObject weapon_obj = iteminfo.weaponitem_info.weapon_obj;
                weapon_obj.GetComponent<ItemSetting>().iteminfo = iteminfo;                        //アイテム情報代入
                ParentChildren(m_dropItemsParent, weapon_obj);
                weapon_obj.transform.position = set_pos;
                weapon_obj.GetComponent<GunManager>().DropItemSetting();
            }

            //アイテムを拾った時と同じを鳴らすSE
            m_playerSound.PlayPickUp();
            m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum].ItemInfo = null;      //アイテム情報削除
            m_inventoryItem.m_inventory.Slots[catch_slot.m_slotNum].initializationSlot(); //初期化
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
        _slot.m_selectObj = null;
        _slot.m_selectInventory = (int)INVENTORY.NON;
        _slot.m_slotNum = -1;
        _slot.m_chestNum = -1;
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

    public bool OpenClose(bool _flag, GameObject _item, INVENTORY _type)
    {
        if(_flag)
        {
            if (m_inventoryState == INVENTORY.ITEM)
            {
                m_inventoryItem.m_uiObj.SetActive(false);

                if (catch_slot.m_selectObj != null)
                {
                    //元の親に戻す
                    ParentChildren(m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].gameObject, catch_slot.m_selectObj);
                    //オブジェクトは元の位置に、情報だけ渡す
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
                        //元の親に戻す
                        ParentChildren(m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].gameObject, catch_slot.m_selectObj);
                        //オブジェクトは元の位置に、情報だけ渡す
                        catch_slot.m_selectObj.transform.position = m_inventoryItem.m_BoxTrans[catch_slot.m_slotNum].position;
                    }
                    if (catch_slot.m_selectInventory == (int)INVENTORY.CHEST)
                    {
                        //元の親に戻す
                        ParentChildren(m_chestInventory[catch_slot.m_chestNum].m_slotBoxTrans[catch_slot.m_slotNum].gameObject, catch_slot.m_selectObj);
                        //オブジェクトは元の位置に、情報だけ渡す
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
            m_inventoryState = _type;

            if (_type == INVENTORY.ITEM)
            {
                m_inventoryItem.m_uiObj.SetActive(true);
            }
            if(_type == INVENTORY.CHEST)
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
        //親子関係に設定

        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

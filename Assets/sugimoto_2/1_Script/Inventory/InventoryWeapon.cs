using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SLOT_ORDER
{
    HAND,
    KNIFE,
    GUN,
    DOG,
}

public class InventoryWeapon : MonoBehaviour
{
    /// <summary>スロットにある銃以外の種類の数</summary>
    const int OTHER_GUN_TYPE_NUM = 2;

    //インベントリの要素
    public InventoryClass m_Inventory;
    public int m_sloatSize = 4;
    public Transform[] m_SlotBoxTrans;
    public Transform[] m_spriteTrans;

    //オブジェクト
    [SerializeField] GameObject m_uiObj; //インベントリUI
    public GameObject[] m_weaponSlotObj;
    [SerializeField] Transform m_frame;
    [SerializeField] GameObject m_weaponParent;                 //銃の親オブジェクト
    [SerializeField] GameObject[] m_saveOtherGun = new GameObject[OTHER_GUN_TYPE_NUM];



    public SLOT_ORDER m_selectSlot = SLOT_ORDER.HAND;
    Color mColorAlphaHalf = new Color(1.0f, 1.0f, 1.0f, 0.5f);//半透明
    Color mColorAlphaFull = new Color(1.0f, 1.0f, 1.0f, 1.0f);//不透明

    // Start is called before the first frame update
    void Start()
    {
        //インベントリのインストラクタ作成
        m_Inventory = new InventoryClass(m_sloatSize, m_SlotBoxTrans);

        //mWeaponSlotObjにあるオブジェクトの情報をスロットに入れる
        ItemSlotSet();
    }

    // Update is called once per frame
    void Update()
    {
        SetUI(m_spriteTrans);
        SetWeapon();
    }

    void ItemSlotSet()
    {
        for (int i = 0; i < m_weaponSlotObj.Length; i++)
        {
            if (m_weaponSlotObj[i] == null) continue;

            ITEM_ID id = m_weaponSlotObj[i].GetComponent<ItemSetting>().iteminfo.id;

            //アイテムIDによって情報を入れるスロットが異なる
            switch (id)
            {
                case ITEM_ID.PISTOL:
                case ITEM_ID.ASSAULT:
                case ITEM_ID.SHOTGUN:
                    m_Inventory.Slots[(int)SLOT_ORDER.GUN].ItemInfo = m_weaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.HAND:
                    m_Inventory.Slots[(int)SLOT_ORDER.HAND].ItemInfo = m_weaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.KNIFE:
                    m_Inventory.Slots[(int)SLOT_ORDER.KNIFE].ItemInfo = m_weaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.DOG_DIRECTION:
                    m_Inventory.Slots[(int)SLOT_ORDER.DOG].ItemInfo = m_weaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
            }
        }
    }

    public GameObject ChangeWeapon()
    {
        //回転の取得
        float mouse_wheel = Input.GetAxis("Mouse ScrollWheel");

        //マウスホイールに動きがあったら変更
        //if (Mathf.Abs(mouse_wheel) != 0)
        //{
        //    //現在の武器非表示
        //    mWeaponSlotObj[(int)mSelectSlot].SetActive(false);
        //    sprite[(int)mSelectSlot].GetComponent<Image>().color = mColorAlphaHalf;
        //    ////インベントリ表示
        //    //display_timer = 0.0f;
        //    //display_flag = true;

        //    //if (weapon[(int)select_weapon].GetComponent<ItemInformation>().id == ITEM_ID.PISTOL)
        //    //{
        //    //    weapon[(int)select_weapon].GetComponent<GunManager>().StopReload();
        //    //}
        //}

        //マウスホイール下回し
        if (mouse_wheel < 0)
        {
            //次の武器インベントリへ
            m_selectSlot++;
            //武器インベントリの領域を超えたら最初に戻す
            if ((int)m_selectSlot >= m_sloatSize)
            {
                m_selectSlot = 0;
            }

            //武器インベントリの中身が何もなければ中身のある武器へ
            while (m_weaponSlotObj[(int)m_selectSlot] == null)
            {
                if (m_weaponSlotObj[(int)m_selectSlot] == null)
                {
                    //次の武器インベントリへ
                    m_selectSlot++;
                    //武器インベントリの領域を超えたら最初に戻す
                    if ((int)m_selectSlot >= m_sloatSize)
                    {
                        m_selectSlot = 0;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        //上回し
        if (mouse_wheel > 0)
        {
            //前の武器インベントリ
            m_selectSlot--;
            if (m_selectSlot < 0)
            {
                //武器インベントリの領域を超えたら最後にする
                m_selectSlot = (SLOT_ORDER)m_sloatSize - 1;
            }

            //武器インベントリの中身が何もなければ中身のある武器へ
            while (m_weaponSlotObj[(int)m_selectSlot] == null)
            {
                if (m_weaponSlotObj[(int)m_selectSlot] == null)
                {
                    //前の武器インベントリ
                    m_selectSlot--;
                    if (m_selectSlot < 0)
                    {
                        //武器インベントリの領域を超えたら最後にする
                        m_selectSlot = (SLOT_ORDER)m_sloatSize - 1;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        //マウスホイールに動きがあったら変更
        //if (Mathf.Abs(mouse_wheel) != 0)
        //{
        //    //持っている武器を変更
        //    mWeaponSlotObj[(int)mSelectSlot].SetActive(true);
        //    sprite[(int)mSelectSlot].GetComponent<Image>().color = mColorAlphaFull;
        //    mFrame.position = slot_box[(int)mSelectSlot].transform.position;
        //}
        return m_weaponSlotObj[(int)m_selectSlot];
    }

    public int CanWeaponGet(GameObject _item)
    {
        ITEM_ID id = _item.GetComponent<ItemSetting>().iteminfo.id;

        //取得する武器は銃と犬用アイテムのみなのでHANDとKNIFEは省略
        switch (id)
        {
            case ITEM_ID.PISTOL:
            case ITEM_ID.ASSAULT:
            case ITEM_ID.SHOTGUN:
                    return (int)SLOT_ORDER.GUN;
            case ITEM_ID.DOG_DIRECTION:
                    return (int)SLOT_ORDER.DOG;
        }

        return -1;
    }

    public void WeaponGet(GameObject _item)
    {
        ITEM_ID id = _item.GetComponent<ItemSetting>().iteminfo.id;

        //取得する武器は銃と犬用アイテムのみなのでHANDとKNIFEは省略
        switch (id)
        {
            case ITEM_ID.PISTOL:
            case ITEM_ID.ASSAULT:
            case ITEM_ID.SHOTGUN:
                if (m_weaponSlotObj[(int)SLOT_ORDER.GUN] == null)
                {
                    m_Inventory.Slots[(int)SLOT_ORDER.GUN].ItemInfo = _item.GetComponent<ItemSetting>().iteminfo;
                    m_weaponSlotObj[(int)SLOT_ORDER.GUN] = _item.GetComponent<ItemSetting>().iteminfo.weaponitem_info.weapon_obj;
                    //選んでいる武器がHANDの場合拾った武器を選んでいる武器に変更
                    if (m_selectSlot == SLOT_ORDER.HAND) m_selectSlot = SLOT_ORDER.GUN;
                }
                break;
            case ITEM_ID.DOG_DIRECTION:
                if (m_weaponSlotObj[(int)SLOT_ORDER.DOG] == null)
                {
                    m_Inventory.Slots[(int)SLOT_ORDER.DOG].ItemInfo = _item.GetComponent<ItemSetting>().iteminfo;
                    m_weaponSlotObj[(int)SLOT_ORDER.DOG] = _item.GetComponent<ItemSetting>().iteminfo.weaponitem_info.weapon_obj;
                    //選んでいる武器がHANDの場合拾った武器を選んでいる武器に変更
                    if (m_selectSlot == SLOT_ORDER.HAND) m_selectSlot = SLOT_ORDER.DOG;
                }
                break;
        }

        //武器取得の際の設定
        ParentChildren(m_weaponParent.gameObject, _item);
        _item.GetComponent<GunManager>().GetItemSetting();

    }

    public void GunObjChenge(ItemInformation _inventoryitem_item)
    {
        //今の武器を非表示
        m_weaponSlotObj[(int)SLOT_ORDER.GUN].SetActive(false);
        //入れ替えた武器に変更
        m_weaponSlotObj[(int)SLOT_ORDER.GUN] = _inventoryitem_item.weaponitem_info.weapon_obj;
    }


    /// <summary>
    /// UIをセットする
    /// インベントリのスロットのUIを決定
    /// インベントリクラスにあるのと違い、Textなしバージョン
    /// </summary>
    /// <param name="_sprite">スプライトを入れるトランスフォーム</param>
    public void SetUI(Transform[] _sprite)
    {
        //スプライト
        for (int slot = 0; slot < m_sloatSize; slot++)
        {
            if (m_Inventory.Slots[slot].ItemInfo == null)
            {
                //アイテム情報がない場合

                _sprite[slot].gameObject.SetActive(false);          //非表示
                _sprite[slot].GetComponent<Image>().sprite = null;  //スプライト初期化
            }
            else
            {
                //アイテム情報がある場合

                _sprite[slot].gameObject.SetActive(true);                                             //表示
                _sprite[slot].GetComponent<Image>().sprite = m_Inventory.Slots[slot].ItemInfo.sprite;   //スロットにあるアイテム情報からスプライトを代入
            }
        }
    }

    public void SetWeapon()
    {
        for (int slot = 0; slot < m_sloatSize; slot++)
        {
            //アイテム情報がない
            if (m_weaponSlotObj[slot] == null) continue;

            //選択しているスロットのオブジェクト表示
            if (slot == (int)m_selectSlot)
            {
                m_weaponSlotObj[(int)m_selectSlot].SetActive(true);
                m_spriteTrans[(int)m_selectSlot].GetComponent<Image>().color = mColorAlphaFull;
                m_frame.position = m_spriteTrans[(int)m_selectSlot].position;
            }
            else
            {
                //非表示

                m_weaponSlotObj[slot].SetActive(false);
                m_spriteTrans[slot].GetComponent<Image>().color = mColorAlphaHalf;
            }
        }
    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        //親子関係に設定

        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

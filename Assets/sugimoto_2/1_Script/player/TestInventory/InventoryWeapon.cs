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
    const int OTHER_GUN_TYPE_NUM = 2;

    //インベントリマネージャー
    inventoryManager mInventoryManager;

    //インベントリの要素
    public InventoryClass Inventory;
    public int slot_size = 4;
    public Transform[] sprite;
    public Transform[] slot_box;

    //オブジェクト
    [SerializeField] GameObject mInventoryManagerObj;
    [SerializeField] GameObject mWeapon_inventory_UI_obj; //インベントリUI
    public GameObject[] mWeaponSlotObj;
    [SerializeField] Transform mFrame;
    [SerializeField] GameObject mWeaponParent;                 //銃の親オブジェクト
    [SerializeField] GameObject[] m_saveOtherGun = new GameObject[OTHER_GUN_TYPE_NUM];



    public SLOT_ORDER mSelectSlot = SLOT_ORDER.HAND;
    Color mColorAlphaHalf = new Color(1.0f, 1.0f, 1.0f, 0.5f);//半透明
    Color mColorAlphaFull = new Color(1.0f, 1.0f, 1.0f, 1.0f);//不透明

    // Start is called before the first frame update
    void Start()
    {
        //インベントリのインストラクタ作成
        Inventory = new InventoryClass(slot_size, slot_box);
        //インベントリマネージャー取得
        mInventoryManager = mInventoryManagerObj.GetComponent<inventoryManager>();

        //mWeaponSlotObjにあるオブジェクトの情報をスロットに入れる
        ItemSlotSet();
    }

    // Update is called once per frame
    void Update()
    {
        SetUI(sprite);
        SetWeapon();
    }

    void ItemSlotSet()
    {
        for (int i = 0; i < mWeaponSlotObj.Length; i++)
        {
            if (mWeaponSlotObj[i] == null) continue;

            ITEM_ID id = mWeaponSlotObj[i].GetComponent<ItemSetting>().iteminfo.id;

            //アイテムIDによって情報を入れるスロットが異なる
            switch (id)
            {
                case ITEM_ID.PISTOL:
                case ITEM_ID.ASSAULT:
                case ITEM_ID.SHOTGUN:
                    Inventory.Slots[(int)SLOT_ORDER.GUN].ItemInfo = mWeaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.HAND:
                    Inventory.Slots[(int)SLOT_ORDER.HAND].ItemInfo = mWeaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.KNIFE:
                    Inventory.Slots[(int)SLOT_ORDER.KNIFE].ItemInfo = mWeaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
                    break;
                case ITEM_ID.DOG_DIRECTION:
                    Inventory.Slots[(int)SLOT_ORDER.DOG].ItemInfo = mWeaponSlotObj[i].GetComponent<ItemSetting>().iteminfo;
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
            mSelectSlot++;
            //武器インベントリの領域を超えたら最初に戻す
            if ((int)mSelectSlot >= slot_size)
            {
                mSelectSlot = 0;
            }

            //武器インベントリの中身が何もなければ中身のある武器へ
            while (mWeaponSlotObj[(int)mSelectSlot] == null)
            {
                if (mWeaponSlotObj[(int)mSelectSlot] == null)
                {
                    //次の武器インベントリへ
                    mSelectSlot++;
                    //武器インベントリの領域を超えたら最初に戻す
                    if ((int)mSelectSlot >= slot_size)
                    {
                        mSelectSlot = 0;
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
            mSelectSlot--;
            if (mSelectSlot < 0)
            {
                //武器インベントリの領域を超えたら最後にする
                mSelectSlot = (SLOT_ORDER)slot_size - 1;
            }

            //武器インベントリの中身が何もなければ中身のある武器へ
            while (mWeaponSlotObj[(int)mSelectSlot] == null)
            {
                if (mWeaponSlotObj[(int)mSelectSlot] == null)
                {
                    //前の武器インベントリ
                    mSelectSlot--;
                    if (mSelectSlot < 0)
                    {
                        //武器インベントリの領域を超えたら最後にする
                        mSelectSlot = (SLOT_ORDER)slot_size - 1;
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
        return mWeaponSlotObj[(int)mSelectSlot];
        {
            //武器インベントリ表示非表示
            //{
            //    display_timer += Time.deltaTime;

            //    if (display_timer >= 5.0f)
            //    {
            //        display_flag = false;
            //        color = 0.0f;
            //    }

            //    if (!display_flag)
            //    {
            //        color += Time.deltaTime;

            //        for (int i = 0; i < weapon_sprite_obj.Length; i++)
            //        {
            //            //親オブジェ
            //            weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color.a - color);
            //            //オブジェ本体
            //            weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, weapon_sprite_obj[i].GetComponent<Image>().color.a - color);
            //            //枠オブジェ
            //            frame_pos.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, frame_pos.GetComponent<Image>().color.a - color);
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < weapon_sprite_obj.Length; i++)
            //        {
            //            //親オブジェ
            //            weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            //            //オブジェ本体
            //            if (hand_weapon == (WEAPON_ID)i)
            //            {
            //                weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            //            }
            //            else if (weapon_hand_obj[i] != null)
            //            {
            //                weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            //            }
            //            else
            //            {
            //                weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            //            }

            //            //枠オブジェ
            //            frame_pos.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            //        }
            //    }
            //}

            //配置、UI設定
            //bullet_text_obj.SetActive(false);

            //if (hand_weapon != befor_weapon)
            //{
            //    switch (hand_weapon)
            //    {
            //        case WEAPON_ID.PISTOL:
            //            //bullet_text_obj.SetActive(true);
            //            //transform設定
            //            ParentChildren(hand_pos.gameObject, weapon_hand_obj[weapon_cnt]);
            //            weapon_hand_obj[weapon_cnt].transform.localRotation = Quaternion.identity;
            //            weapon_hand_obj[weapon_cnt].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            //            //weapon_hand_obj[weapon_cnt].transform.localPosition = hand_start_pos.localPosition;
            //            Debug.Log(weapon_hand_obj[weapon_cnt].transform);
            //            weapon_hand_obj[weapon_cnt].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //スケール変更
            //            break;
            //        case WEAPON_ID.KNIFE:
            //            //weapon_hand_obj[weapon_cnt].SetActive(true);
            //            break;
            //    }
            //}
        }
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
                if (mWeaponSlotObj[(int)SLOT_ORDER.GUN] == null)
                {
                    Inventory.Slots[(int)SLOT_ORDER.GUN].ItemInfo = _item.GetComponent<ItemSetting>().iteminfo;
                    mWeaponSlotObj[(int)SLOT_ORDER.GUN] = _item.GetComponent<ItemSetting>().iteminfo.weaponitem_info.weapon_obj;
                    //選んでいる武器がHANDの場合拾った武器を選んでいる武器に変更
                    if (mSelectSlot == SLOT_ORDER.HAND) mSelectSlot = SLOT_ORDER.GUN;
                }
                break;
            case ITEM_ID.DOG_DIRECTION:
                if (mWeaponSlotObj[(int)SLOT_ORDER.DOG] == null)
                {
                    Inventory.Slots[(int)SLOT_ORDER.DOG].ItemInfo = _item.GetComponent<ItemSetting>().iteminfo;
                    mWeaponSlotObj[(int)SLOT_ORDER.DOG] = _item.GetComponent<ItemSetting>().iteminfo.weaponitem_info.weapon_obj;
                    //選んでいる武器がHANDの場合拾った武器を選んでいる武器に変更
                    if (mSelectSlot == SLOT_ORDER.HAND) mSelectSlot = SLOT_ORDER.DOG;
                }
                break;
        }

        //当たり判定をOFFにする
        _item.GetComponent<BoxCollider>().enabled = false;
        _item.SetActive(false);

        //位置設定
        ParentChildren(mWeaponParent.gameObject, _item);
        _item.transform.localRotation = Quaternion.identity;
        _item.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        _item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //スケール変更

    }

    public void GunObjChenge(ItemInformation _inventoryitem_item)
    {
        //今の武器を非表示
        mWeaponSlotObj[(int)SLOT_ORDER.GUN].SetActive(false);
        //入れ替えた武器に変更
        mWeaponSlotObj[(int)SLOT_ORDER.GUN] = _inventoryitem_item.weaponitem_info.weapon_obj;
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
        for (int slot = 0; slot < slot_size; slot++)
        {
            if (Inventory.Slots[slot].ItemInfo == null)
            {
                //アイテム情報がない場合

                _sprite[slot].gameObject.SetActive(false);          //非表示
                _sprite[slot].GetComponent<Image>().sprite = null;  //スプライト初期化
            }
            else
            {
                //アイテム情報がある場合

                _sprite[slot].gameObject.SetActive(true);                                             //表示
                _sprite[slot].GetComponent<Image>().sprite = Inventory.Slots[slot].ItemInfo.sprite;   //スロットにあるアイテム情報からスプライトを代入
            }
        }
    }

    public void SetWeapon()
    {
        for (int slot = 0; slot < slot_size; slot++)
        {
            //アイテム情報がない
            if (mWeaponSlotObj[slot] == null) continue;

            //選択しているスロットのオブジェクト表示
            if (slot == (int)mSelectSlot)
            {
                mWeaponSlotObj[(int)mSelectSlot].SetActive(true);
                sprite[(int)mSelectSlot].GetComponent<Image>().color = mColorAlphaFull;
                mFrame.position = sprite[(int)mSelectSlot].position;
            }
            else
            {
                //非表示

                mWeaponSlotObj[slot].SetActive(false);
                sprite[slot].GetComponent<Image>().color = mColorAlphaHalf;
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

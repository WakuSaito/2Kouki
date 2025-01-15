using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWeapon : MonoBehaviour
{
    //インベントリマネージャー
    inventoryManager mInventoryManager;

    //インベントリの要素
    public InventoryClass Inventory;
    public int slot_size = 4;
    public Transform[] sprite;
    public Transform[] slot_box;
    public Text[] text;

    //オブジェクト
    [SerializeField] GameObject mInventoryManagerObj;
    [SerializeField] GameObject mWeapon_inventory_UI_obj; //インベントリUI
    [SerializeField] GameObject[] mWeaponSlotObj;
    [SerializeField] Transform mFrame;

    public enum SLOT_ORDER
    {
        HAND,
        KNIFE,
        GUN,
        DOG,
    }

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
    }

    void ItemSlotSet()
    {
        for (int i = 0; i < mWeaponSlotObj.Length; i++)
        {
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
        if (Mathf.Abs(mouse_wheel) != 0)
        {
            //現在の武器非表示
            mWeaponSlotObj[(int)mSelectSlot].SetActive(false);
            sprite[(int)mSelectSlot].GetComponent<Image>().color = mColorAlphaHalf;
            ////インベントリ表示
            //display_timer = 0.0f;
            //display_flag = true;

            //if (weapon[(int)select_weapon].GetComponent<ItemInformation>().id == ITEM_ID.PISTOL)
            //{
            //    weapon[(int)select_weapon].GetComponent<GunManager>().StopReload();
            //}
        }

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
        if (Mathf.Abs(mouse_wheel) != 0)
        {
            //持っている武器を変更
            mWeaponSlotObj[(int)mSelectSlot].SetActive(true);
            sprite[(int)mSelectSlot].GetComponent<Image>().color = mColorAlphaFull;
            mFrame.position = slot_box[(int)mSelectSlot].transform.position;
        }
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

    public void WeaponGet(GameObject _item)
    {
        if (mWeaponSlotObj[(int)SLOT_ORDER.GUN] == null)
        {

        }
    }

    /// <summary>
    /// UIをセットする
    /// インベントリのスロットのUIを決定
    /// インベントリクラスにあるのと違い、Textなしバージョン
    /// </summary>
    /// <param name="_sprite">スプライトを入れるトランスフォーム</param>
    public void SetUI(Transform[] _sprite)
    {
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

}

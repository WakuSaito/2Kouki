using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    public enum Sloat_Order
    {
        HAND,
        KNIFE,
        GUN,
        DOG,
        SAVE_1,
        SAVE_2,
    }

    public InventoryTest Inventory;
    public int sloat_size = 4;
    [SerializeField] Transform[] sprite_pos;
    [SerializeField] Transform[] sloat_box;
    [SerializeField] Transform frame_pos;
    [SerializeField] Transform gun_parent;

    public GameObject[] weapon;

    public Sloat_Order select_weapon = Sloat_Order.HAND;
    Color semitransparent = new Color(1.0f, 1.0f, 1.0f, 0.5f);//半透明
    Color opacity = new Color(1.0f, 1.0f, 1.0f, 1.0f);//不透明

    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryTest(sloat_size, sloat_box);

        //初期設定
        ItemInformation hand = weapon[(int)Sloat_Order.HAND].GetComponent<ItemSetting>().iteminfo;
        Inventory.Sloats[(int)Sloat_Order.HAND].ItemInfo = new ItemInformation(hand.type, hand.id, hand.get_num, hand.stack_max, hand.sprite);
        hand = weapon[(int)Sloat_Order.KNIFE].GetComponent<ItemSetting>().iteminfo;
        Inventory.Sloats[(int)Sloat_Order.KNIFE].ItemInfo = new ItemInformation(hand.type, hand.id, hand.get_num, hand.stack_max, hand.sprite);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) { Inventory.PrintInventory(); }

        Inventory.SetWeaponUI();
    }

    public void WeaponGet(GameObject _item)
    {
        if (weapon[(int)Sloat_Order.GUN] == null)
        {
            weapon[(int)Sloat_Order.GUN] = _item;
        }
        else if (weapon[(int)Sloat_Order.SAVE_1] == null)
        {
            weapon[(int)Sloat_Order.SAVE_1] = _item;
        }
        else if (weapon[(int)Sloat_Order.SAVE_2] == null)
        {
            weapon[(int)Sloat_Order.SAVE_2] = _item;
        }

        //当たり判定をOFFにする
        _item.GetComponent<BoxCollider>().enabled = false;
        _item.SetActive(false);

        //transform設定
        ParentChildren(gun_parent.gameObject, _item);
        _item.transform.localRotation = Quaternion.identity;
        _item.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        _item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //スケール変更

        if (select_weapon == Sloat_Order.HAND) 
        {
            select_weapon = Sloat_Order.GUN;
            _item.SetActive(true);
        }

        Inventory.Add_WeaponInventory(weapon);
    }

    public GameObject ChangeWeapon()
    {
        //ホイール処理
        {
            //回転の取得
            float mouse_wheel = Input.GetAxis("Mouse ScrollWheel");

            //マウスホイールに動きがあったら変更
            if (Mathf.Abs(mouse_wheel) != 0)
            {
                //現在の武器非表示
                weapon[(int)select_weapon].SetActive(false);
                sprite_pos[(int)select_weapon].GetComponent<Image>().color = semitransparent;
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
                select_weapon++;
                //武器インベントリの領域を超えたら最初に戻す
                if ((int)select_weapon >= sloat_size)
                {
                    select_weapon = 0;
                }

                //武器インベントリの中身が何もなければ中身のある武器へ
                while (weapon[(int)select_weapon] == null)
                {
                    if (weapon[(int)select_weapon] == null)
                    {
                        //次の武器インベントリへ
                        select_weapon++;
                        //武器インベントリの領域を超えたら最初に戻す
                        if ((int)select_weapon >= sloat_size)
                        {
                            select_weapon = 0;
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
                select_weapon--;
                if (select_weapon < 0)
                {
                    //武器インベントリの領域を超えたら最後にする
                    select_weapon = (Sloat_Order)(sloat_size - 1);
                }

                //武器インベントリの中身が何もなければ中身のある武器へ
                while (weapon[(int)select_weapon]== null)
                {
                    if (weapon[(int)select_weapon]== null)
                    {
                        //前の武器インベントリ
                        select_weapon--;
                        if (select_weapon < 0)
                        {
                            //武器インベントリの領域を超えたら最後にする
                            select_weapon = (Sloat_Order)(sloat_size - 1);
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
                weapon[(int)select_weapon].SetActive(true);
                sprite_pos[(int)select_weapon].GetComponent<Image>().color = opacity;
                frame_pos.position = sloat_box[(int)select_weapon].transform.position;
            }

        }

        return weapon[(int)select_weapon];
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

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        //親子関係に設定

        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }


}

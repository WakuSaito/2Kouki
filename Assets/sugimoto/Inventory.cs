using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : ID
{
    //定数
    public const int INVENTORY_MAX = 10;        //アイテムインベントリの最大枠
    public const int WEAPON_INVENTORY_MAX = 4;  //武器インベントリの最大枠
    const int ITEM_MAX = 30;                    //スタックできる最大数

    //武器インベントリ
    public GameObject[] weapon_hand_obj = new GameObject[WEAPON_INVENTORY_MAX] { null, null, null, null };
    int weapon_cnt = 0;


    //アイテムインベントリ 
    public int[] item_num = new int[INVENTORY_MAX] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };                    //アイテムの数保存
    public int[] item_type_id = new int[INVENTORY_MAX] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };      //アイテムの種類保存
    [SerializeField] Transform hand_pos;        //手の位置
    /*UI関連*/
    [SerializeField] GameObject item_inventory;                                         //インベントリUI
    bool item_inventory_flag = false;                                                   //インベントリ開いてるか閉じてるか  
    [SerializeField] GameObject[] item_sprite_obj = new GameObject[INVENTORY_MAX];      //アイテムのスプライトを入れるオブジェ
    [SerializeField] Sprite[] item_sprite;                                              //種類別アイテムスプライト
    [SerializeField] Text[] item_num_text;                                              //アイテムの個数表示

    public enum WEAPON_ID
    {
        HAND,
        KNIFE,
        PISTOL,
        DOG,
    }

    public WEAPON_ID hand_weapon = WEAPON_ID.HAND;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    
    public void ItemInventory()
    {
        //インベントリ開閉
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!item_inventory_flag)
            {
                Screen.lockCursor = false;
                item_inventory_flag = true;
                item_inventory.SetActive(true);
            }
            else
            {
                Screen.lockCursor = true;
                item_inventory_flag = false;
                item_inventory.SetActive(false);
            }
        }

    }

    public int PistolBulletNum()
    {
        //ピストルに入っている弾数を取得
        if (weapon_hand_obj[(int)WEAPON_ID.PISTOL] != null)
        {
            return weapon_hand_obj[(int)WEAPON_ID.PISTOL].GetComponent<Pistol>().pistol_bullet_num;
        }
        else
        {
            return 0;
        }
    }

    public int InventoryBulletNum()
    {
        //インベントリに入っているすべての弾数を取得

        int bullet_num = 0;

        for (int i = 0; i < INVENTORY_MAX; i++)
        {
            if (item_type_id[i] == (int)ITEM_ID.BULLET)
            {
                bullet_num += item_num[i];
            }
        }

        return bullet_num;
    }

    public void ReduceInventory(int _i)
    {
        //インベントリの中身を減らす
        item_num_text[_i].text = item_num[_i] + "";

        //アイテムがなくなればUI初期化
        if (item_num[_i] == 0)
        {
            item_sprite_obj[_i].GetComponent<Image>().sprite = null;
            item_sprite_obj[_i].SetActive(false);
        }
    }

    public void ItemGet(GameObject _item)
    {
        //アイテム取得処理(同じアイテムは最大スタック数じゃなけれ優先的にスタックさせる)

        //アイテムからID取得
        int item_id = (int)_item.GetComponent<ItemSet_ID>().id;

        //アイテムがピストルだった場合のみ取得するアイテム変更
        switch(item_id)
        {
            case (int)ITEM_ID.PISTOL:
                item_id = (int)ITEM_ID.BULLET;
                break;
        }

        //取得可能なアイテムの数
        int get_num = _item.GetComponent<ItemSet_ID>().get_num;

        Debug.Log(get_num);


        //取得可能数が0になるまでループ
        while (get_num != 0)
        {
            bool input_flag = false;    //インベントリに入れるか調べる
            int input_pos = -1;         //入れる位置を保存

            //インベントリに同じアイテムがあるか調べる
            for (int i = 0; i < INVENTORY_MAX; i++)
            {
                //インベントリのアイテムと同じIDだったら
                if (item_type_id[i] == item_id && item_num[i] != ITEM_MAX)
                {
                    //すでにアイテム欄にあり、スタック上限じゃなければ
                    input_flag = true;
                    input_pos = i;
                    break;
                }
            }

            //同じアイテムがなかった場合
            if (!input_flag)
            {
                for (int i = 0; i < INVENTORY_MAX; i++)
                { 
                    //空白(-1)があればID保存してアイテムを入れる
                    if (item_type_id[i] == -1)
                    {
                        item_type_id[i] = item_id;
                        input_flag = true;
                        input_pos = i;
                        break;
                    }
                }
                //空白が見つからなければ終了
                Debug.Log("アイテムがMAXです");
                break;
            }

            if(input_flag)
            {
                //取得可能最大数を保存
                int get_max = get_num;

                for (int cnt = 1; cnt <= get_max; cnt++)
                {
                    //アイテム数がMaxじゃなければ
                    if (item_num[input_pos] == ITEM_MAX)
                    {

                    }
                    else
                    {
                        item_num[input_pos]++;
                        get_num--;
                    }
                }

                //取得可能なアイテム数がなくなれば終了
                if (get_num <= 0)
                {
                    //アイテムインベントリUI変更処理
                    item_sprite_obj[input_pos].SetActive(true);
                    item_sprite_obj[input_pos].GetComponent<Image>().sprite = item_sprite[input_pos];
                    item_num_text[input_pos].text = item_num[input_pos] + "";
                }
            }

            //インベントリを最後まで見たら獲得可能なアイテムを0にする
            if(input_pos==INVENTORY_MAX)
                get_num = 0;
        }

        

        //確認用
        //for (int i = 0; i <INVENTORY_MAX;i++)
        //{
        //    Debug.Log(item_num[i]);
        //    Debug.Log(item_type_id[i]);
        //}
    }


    public void HandWeapon()
    {
        //武器の切り替え

        weapon_hand_obj[weapon_cnt].SetActive(false);

        //回転の取得
        float mouse_wheel = Input.GetAxis("Mouse ScrollWheel");

        //マウスホイール上回し
        if (mouse_wheel > 0)
        {
            //次のweaponインベントリへ
            weapon_cnt++;
            //武器インベントリの領域を超えたら最初に戻す
            if (weapon_cnt >= WEAPON_INVENTORY_MAX)
            {
                weapon_cnt = 0;
            }

            //インベントリの中身が何もなければ中身のあるインベントリへ
            while (weapon_hand_obj[weapon_cnt] == null)
            {
                if (weapon_hand_obj[weapon_cnt] == null)
                {
                    weapon_cnt++;

                    if (weapon_cnt >= WEAPON_INVENTORY_MAX)
                    {
                        weapon_cnt = 0;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        if (mouse_wheel < 0)
        {
            //次のweaponインベントリへ
            weapon_cnt--;
            if (weapon_cnt < 0)
            {
                weapon_cnt = WEAPON_INVENTORY_MAX - 1;
            }

            //インベントリの中身が何もなければ中身のあるインベントリへ
            while (weapon_hand_obj[weapon_cnt] == null)
            {
                if (weapon_hand_obj[weapon_cnt] == null)
                {
                    weapon_cnt--;
                    if (weapon_cnt < 0)
                    {
                        weapon_cnt = WEAPON_INVENTORY_MAX - 1;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        //持っている武器を変更
        hand_weapon = (WEAPON_ID)weapon_cnt;
        weapon_hand_obj[weapon_cnt].SetActive(true);

        switch (hand_weapon)
        {
            case WEAPON_ID.PISTOL:
                //transform設定
                ParentChildren(hand_pos.gameObject, weapon_hand_obj[weapon_cnt]);
                weapon_hand_obj[weapon_cnt].transform.position = hand_pos.position;
                weapon_hand_obj[weapon_cnt].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //スケール変更
                break;
        }

    }

    public void HandWeapon(WEAPON_ID _weapon_id)
    {
        weapon_hand_obj[weapon_cnt].SetActive(false);
        hand_weapon = _weapon_id;
        weapon_cnt = (int)_weapon_id;
        weapon_hand_obj[weapon_cnt].SetActive(true);
    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        //親子関係に設定

        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

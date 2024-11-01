using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int INVENTORY_MAX = 10;
    public const int WEAPON_INVENTORY_MAX = 4;
    const int ITEM_MAX = 30;
    
    //アイテムの数保存
    public int[] item_num = new int[INVENTORY_MAX] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //アイテムの種類保存
    public int[] item_type_id = new int[INVENTORY_MAX] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

    [SerializeField] Transform hand_pos;

    public enum WEAPON_ID
    {
        HAND,
        KNIFE,
        PISTOL,
        DOG,
    }

    public WEAPON_ID hand_weapon = WEAPON_ID.HAND;

    //武器のインベントリ
    public GameObject[] weapon_hand_obj = new GameObject[WEAPON_INVENTORY_MAX] { null, null, null, null };
    int weapon_cnt = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public int PistolBulletNum()
    {
        if (weapon_hand_obj[(int)WEAPON_ID.PISTOL] != null)
        {
            return weapon_hand_obj[(int)WEAPON_ID.PISTOL].GetComponent<Pistol>().bullet_num;
        }
        else
        {
            return 0;
        }
    }

    public int InventoryBulletNum()
    {
        int bullet_num = 0;

        for (int i = 0; i < INVENTORY_MAX; i++)
        {
            if (item_type_id[i] == (int)Item.ITEM_ID.BULLET)
            {
                bullet_num += item_num[i];
            }
        }

        return bullet_num;
    }

    public void HandWeapon()
    {
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
                weapon_cnt = WEAPON_INVENTORY_MAX-1;
            }

            //インベントリの中身が何もなければ中身のあるインベントリへ
            while (weapon_hand_obj[weapon_cnt] == null)
            {
                if (weapon_hand_obj[weapon_cnt] == null)
                {
                    weapon_cnt--;
                    if (weapon_cnt < 0)
                    {
                        weapon_cnt = WEAPON_INVENTORY_MAX-1;
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

        switch(hand_weapon)
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

    public void ItemGet(GameObject _item)
    {
        //アイテムスクリプトからアイテムのID取得
        int item_id = (int)_item.GetComponent<Item>().id;

        //アイテムがピストルだった場合のみ取得するアイテム変更
        if (item_id == (int)Item.ITEM_ID.PISTOL)
        {
            item_id = (int)Item.ITEM_ID.BULLET;
        }

        //取得可能なアイテムの数
        int get_num = _item.GetComponent<Item>().get_num[item_id];

        while (get_num != 0)
        {

            for (int i = 0; i < INVENTORY_MAX; i++)
            {
                //インベントリのアイテム欄が空白(-1)または同じIDだったら
                if (item_type_id[i] == -1 || item_type_id[i] == item_id)
                {
                    if (item_type_id[i] == -1)
                    {
                        item_type_id[i] = item_id;
                    }

                    int get_max = get_num;
                    for (int cnt = 1; cnt <= get_max; cnt++)
                    {
                        //アイテム数がMaxじゃなければ
                        if (item_num[i] == ITEM_MAX)
                        {
                            break;
                        }
                        else
                        {
                            item_num[i]++;
                            get_num--;
                        }
                    }
                }

                //取得可能なアイテム数がなくなれば終了
                if (get_num <= 0)
                    break;
            }

            //インベントリを最後まで見たら獲得可能なアイテムを0にする
            get_num = 0;
        }

        //確認用
        //for (int i = 0; i <INVENTORY_MAX;i++)
        //{
        //    Debug.Log(item_num[i]);
        //    Debug.Log(item_type_id[i]);
        //}
    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

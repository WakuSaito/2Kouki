using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : ID
{
    //定数
    public const int INVENTORY_MAX = 10;        //アイテムインベントリの最大枠
    public const int WEAPON_INVENTORY_MAX = 4;  //武器インベントリの最大枠

    //食料ゲージ
    [SerializeField] GameObject food_gauge_obj;
    [SerializeField] GameObject hp_gauge_obj;

    //武器インベントリ
    public GameObject[] weapon_hand_obj = new GameObject[WEAPON_INVENTORY_MAX] { null, null, null, null };  //武器配列
    [SerializeField] Transform hand_start_pos;



    int weapon_cnt = 0;
    /*UI関連*/
    [SerializeField] GameObject[] weapon_sprite_obj = new GameObject[WEAPON_INVENTORY_MAX]; //アイテムのスプライトを入れるオブジェ
    [SerializeField] Sprite[] weapon_sprite;                                                //種類別武器スプライト
    [SerializeField] Transform frame_pos;
    [SerializeField] GameObject bullet_text_obj;
    [SerializeField] Text bullet_text;
    float color = 0.0f;
    float display_timer = 0.0f;
    bool display_flag = true;

    public enum WEAPON_ID
    {
        HAND,
        KNIFE,
        PISTOL,
        DOG,
    }

    public WEAPON_ID hand_weapon = WEAPON_ID.HAND;
    public WEAPON_ID befor_weapon = WEAPON_ID.HAND;


    //アイテムインベントリ 
    public int[] item_num = new int[INVENTORY_MAX] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };                    //アイテムの数保存
    public int[] item_type_id = new int[INVENTORY_MAX] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };      //アイテムの種類保存
    [SerializeField] Transform hand_pos;        //手の位置
    /*UI関連*/
    [SerializeField] GameObject item_inventory;                                         //インベントリUI
    public bool item_inventory_flag = false;                                                   //インベントリ開いてるか閉じてるか  
    [SerializeField] GameObject[] item_sprite_obj = new GameObject[INVENTORY_MAX];      //アイテムのスプライトを入れるオブジェ
    [SerializeField] Sprite[] item_sprite;                                              //種類別アイテムスプライト
    [SerializeField] Text[] item_num_text;                                              //アイテムの個数表示

    //SE再生用
    [SerializeField]//サウンド用
    private PlayerSound playerSound;

    // Update is called once per frame
    void Update()
    {
        BulletText();
    }

    public void CheckInventoryItem()    //カーソルのあっているアイテムを調べる
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

        foreach (RaycastResult result in RayResult)
        {
            GetComponent<Inventory>().InventoryOperation(result.gameObject);
        }
    }

    void InventoryOperation(GameObject _item)   //アイテム別処理
    {
        //インベントリ操作

        for (int i = 0; i < INVENTORY_MAX; i++)
        {
            //現在カーソルがあっているInventoryの中身が空じゃなければ
            if (item_sprite_obj[i] == _item && item_type_id[i] != -1) 
            {
                //ID取得
                ITEM_ID _id = (ITEM_ID)item_type_id[i];

                //IDが食料の場合
                if (_id >= ITEM_ID.FOOD_1 && _id <= ITEM_ID.DRINK_2)
                {
                    EatFood(i, _id);
                }
                if(_id>=ITEM_ID.EMERGENCY_PACK)
                {
                    HpRecovery(i, _id);
                }
            }
        }
    }

    void EatFood(int _i, ITEM_ID _id)   //食事処理
    {
        //食事処理
        if (Input.GetMouseButtonDown(0))
        {
            //食料ゲージを増やす
            if (_id >= ITEM_ID.FOOD_1 && _id <= ITEM_ID.FOOD_4)
            {
                food_gauge_obj.GetComponent<Gauge>().Increase_Gauge(10);
                playerSound.PlayEat();//SE
            }
            if (_id >= ITEM_ID.DRINK_1 && _id <= ITEM_ID.DRINK_2)
            {
                food_gauge_obj.GetComponent<Gauge>().Increase_Gauge(5);
                playerSound.PlayDrink();//SE
            }

            //スプライト初期化
            SpriteInitializing(_i);
        }
    }

    void HpRecovery(int _i, ITEM_ID _id)
    {
        //応急処置
        if (Input.GetMouseButtonDown(0))
        {
            //食料ゲージを増やす
            if (_id >= ITEM_ID.EMERGENCY_PACK)
            {
                hp_gauge_obj.GetComponent<Gauge>().Increase_Gauge(3);
                playerSound.PlayHeal();//SE
            }

            //スプライト初期化
            SpriteInitializing(_i);
        }
    }

    void SpriteInitializing(int _i)
    {
        //スプライト初期化
        //スプライト削除
        item_sprite_obj[_i].GetComponent<Image>().sprite = null;
        item_sprite_obj[_i].SetActive(false);
        //アイテム数を減らす
        item_num[_i]--;
        //アイテム数UIを変更
        item_num_text[_i].text = item_num[_i] + "";

        //アイテムがなくなればインベントリ初期化
        if (item_num[_i] == 0)
        {
            item_type_id[_i] = -1;
        }
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

        if(item_inventory_flag)
        {
            //武器インベントリ表示
            display_flag = true;
            display_timer = 0.0f;
        }
    }

    public int PistolBulletNum()
    {
        //ピストルに入っている弾数を取得
        if (weapon_hand_obj[(int)WEAPON_ID.PISTOL] != null)
        {
            int num= weapon_hand_obj[(int)WEAPON_ID.PISTOL].GetComponent<GunManager>().GetCurrentMagazine();
            return num;
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
                //武器インベントリになかった場合
                if (weapon_hand_obj[(int)WEAPON_ID.PISTOL] == null)
                    WeaponGet(_item);
                else
                    item_id = (int)ITEM_ID.BULLET;

                break;
        }

        //取得可能なアイテムの数
        int get_num = _item.GetComponent<ItemSet_ID>().get_num;
        int max_get = _item.GetComponent<ItemSet_ID>().get_max;

        //取得可能数が0になるまでループ
        while (get_num != 0)
        {
            bool input_flag = false;    //インベントリに入れるか調べる
            int input_pos = -1;         //入れる位置を保存

            //最初にインベントリに同じアイテムがあるか調べる
            for (int i = 0; i < INVENTORY_MAX; i++)
            {
                //インベントリのアイテムと同じIDだったら
                if (item_type_id[i] == item_id && item_num[i] != max_get)
                {
                    //すでにアイテム欄にあり、スタック上限じゃなければ
                    input_flag = true;
                    input_pos = i;
                    break;
                }
            }

            //次に同じアイテムがなかった場合
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
            }

            if (input_flag)
            {
                //取得可能最大数を保存
                int get_max = get_num;

                for (int cnt = 1; cnt <= get_max; cnt++)
                {
                    //アイテム数がMaxじゃなければ
                    if (item_num[input_pos] != max_get)
                    {
                        item_num[input_pos]++;
                        get_num--;
                    }
                }

                //アイテムインベントリUI変更処理
                item_sprite_obj[input_pos].SetActive(true);
                item_sprite_obj[input_pos].GetComponent<Image>().sprite = item_sprite[item_id];
                item_num_text[input_pos].text = item_num[input_pos] + "";
            }
            else
            {
                //入れる場所が見つからなければ終了             
                Debug.Log("アイテムがMAXです");
                break;
            }

        }

        Destroy(_item);

        //確認用
        //for (int i = 0; i <INVENTORY_MAX;i++)
        //{
        //    Debug.Log(item_num[i]);
        //    Debug.Log(item_type_id[i]);
        //}
    }

    private void WeaponGet(GameObject _item)
    {
        Debug.Log("銃ゲット");

        //アイテムからID取得
        ITEM_ID item_id = _item.GetComponent<ItemSet_ID>().id;

        //インベントリ表示
        display_timer = 0.0f;
        display_flag = true;

        //IDごとに処理
        switch (item_id)
        {
            //ピストル
            case ITEM_ID.PISTOL:

                //当たり判定をOFFにする
                _item.GetComponent<BoxCollider>().enabled = false;
                _item.GetComponent<GunManager>().hand_player_obj = gameObject;
                //tagをweaponに
                _item.tag = "weapon";

                //ピストルの弾数表示
                bullet_text_obj.SetActive(true);
                //武器インベントリに入れる
                weapon_hand_obj[(int)WEAPON_ID.PISTOL] = _item;

                //手に何も持っていなければ自動的に持つ
                if (hand_weapon == WEAPON_ID.HAND)
                {
                    //武器入れ替え
                    HandWeapon(WEAPON_ID.PISTOL);
                    //プレイヤーの現在の武器をピストルに変更
                    GetComponent<player>().hand_weapon = weapon_hand_obj[(int)WEAPON_ID.PISTOL];
                }
                else
                {
                    _item.SetActive(false);
                }

                break;
        }

    }

    public void BulletText()
    {
        bullet_text.text = PistolBulletNum() + "／" + InventoryBulletNum();
    }

    public void ChangeWeapon()
    {
        //ホイール処理
        {
            //回転の取得
            float mouse_wheel = Input.GetAxis("Mouse ScrollWheel");

            //マウスホイールに動きがあったら変更
            if (Mathf.Abs(mouse_wheel) != 0)
            {
                //現在の武器非表示
                weapon_hand_obj[weapon_cnt].SetActive(false);
                weapon_sprite_obj[weapon_cnt].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                //インベントリ表示
                display_timer = 0.0f;
                display_flag = true;

                if(hand_weapon == WEAPON_ID.PISTOL)
                {
                    weapon_hand_obj[weapon_cnt].GetComponent<GunManager>().StopReload();
                }
            }

            //マウスホイール下回し
            if (mouse_wheel < 0)
            {
                //次の武器インベントリへ
                weapon_cnt++;
                //武器インベントリの領域を超えたら最初に戻す
                if (weapon_cnt >= WEAPON_INVENTORY_MAX)
                {
                    weapon_cnt = 0;
                }

                //武器インベントリの中身が何もなければ中身のある武器へ
                while (weapon_hand_obj[weapon_cnt] == null)
                {
                    if (weapon_hand_obj[weapon_cnt] == null)
                    {
                        //次の武器インベントリへ
                        weapon_cnt++;
                        //武器インベントリの領域を超えたら最初に戻す
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
            //上回し
            if (mouse_wheel > 0)
            {
                //前の武器インベントリ
                weapon_cnt--;
                if (weapon_cnt < 0)
                {
                    //武器インベントリの領域を超えたら最後にする
                    weapon_cnt = WEAPON_INVENTORY_MAX - 1;
                }

                //武器インベントリの中身が何もなければ中身のある武器へ
                while (weapon_hand_obj[weapon_cnt] == null)
                {
                    if (weapon_hand_obj[weapon_cnt] == null)
                    {
                        //前の武器インベントリ
                        weapon_cnt--;
                        if (weapon_cnt < 0)
                        {
                            //武器インベントリの領域を超えたら最後にする
                            weapon_cnt = WEAPON_INVENTORY_MAX - 1;
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
                hand_weapon = (WEAPON_ID)weapon_cnt;
                weapon_hand_obj[weapon_cnt].SetActive(true);
                weapon_sprite_obj[weapon_cnt].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                frame_pos.position = weapon_sprite_obj[weapon_cnt].transform.position;
                GetComponent<player>().hand_weapon = weapon_hand_obj[weapon_cnt];
            }

        }

        //武器インベントリ表示非表示
        {
            display_timer += Time.deltaTime;

            if (display_timer >= 5.0f)
            {
                display_flag = false;
                color = 0.0f;
            }

            if (!display_flag)
            {
                color += Time.deltaTime;

                for (int i = 0; i < weapon_sprite_obj.Length; i++)
                {
                    //親オブジェ
                    weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color.a - color);
                    //オブジェ本体
                    weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, weapon_sprite_obj[i].GetComponent<Image>().color.a - color);
                    //枠オブジェ
                    frame_pos.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, frame_pos.GetComponent<Image>().color.a - color);
                }
            }
            else
            {
                for (int i = 0; i < weapon_sprite_obj.Length; i++)
                {
                    //親オブジェ
                    weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

                    //オブジェ本体
                    if (hand_weapon == (WEAPON_ID)i)
                    {
                        weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }
                    else if (weapon_hand_obj[i] != null)
                    {
                        weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    }
                    else
                    {
                        weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    }

                    //枠オブジェ
                    frame_pos.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                }
            }
        }

        //配置、UI設定
        //bullet_text_obj.SetActive(false);

        //if (hand_weapon != befor_weapon)
        {
            switch (hand_weapon)
            {
                case WEAPON_ID.PISTOL:
                    //bullet_text_obj.SetActive(true);
                    //transform設定
                    ParentChildren(hand_pos.gameObject, weapon_hand_obj[weapon_cnt]);
                    weapon_hand_obj[weapon_cnt].transform.localRotation = Quaternion.identity;
                    weapon_hand_obj[weapon_cnt].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    //weapon_hand_obj[weapon_cnt].transform.localPosition = hand_start_pos.localPosition;
                    Debug.Log(weapon_hand_obj[weapon_cnt].transform);
                    weapon_hand_obj[weapon_cnt].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //スケール変更
                    break;
                case WEAPON_ID.KNIFE:
                    //weapon_hand_obj[weapon_cnt].SetActive(true);
                    break;
            }
        }

        befor_weapon = hand_weapon;

    }

    public void HandWeapon(WEAPON_ID _weapon_id)
    {
        //現在の武器非表示
        weapon_hand_obj[weapon_cnt].SetActive(false);
        weapon_sprite_obj[weapon_cnt].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        //現在の武器のIDを変更した武器のIDに変更
        hand_weapon = _weapon_id;
        weapon_cnt = (int)_weapon_id;
        weapon_hand_obj[weapon_cnt].SetActive(true);
        weapon_sprite_obj[weapon_cnt].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        frame_pos.position = weapon_sprite_obj[weapon_cnt].transform.position;
        //武器インベントリ表示
        display_flag = true;
        display_timer = 0.0f;
    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        //親子関係に設定

        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }

    //所持している食料の数取得
    public int GetFoodItemSum()
    {
        int foodSum = 0;//返り値用合計

        //所持アイテム全体から食料を探す
        for (int i = 0; i < INVENTORY_MAX; i++)
        {
            if (item_type_id[i] >= (int)ITEM_ID.FOOD_1 && 
                item_type_id[i] <= (int)ITEM_ID.DRINK_2)
            {
                foodSum += item_num[i];
            }
        }
        return foodSum;
    }

    //引数番目のアイテムアイコンの座標取得
    public Vector2 GetItemIconPos(int _num)
    {
        //引数が範囲内か調べる
        if (_num < 0 || _num >= item_sprite_obj.Length) return Vector2.zero;

        //座標を返す
        return item_sprite_obj[_num].transform.position;
    }
}

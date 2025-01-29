using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;
//using UnityEngine.EventSystems;

public class player : PlayerFunction, IStopObject
{
    Animator Animator;  // アニメーターコンポーネント取得用
    Rigidbody Rigidbody;

    const float Walk_Speed = 2.0f;
    const float Run_Speed = 3.0f;
    const int Damage_Num = 1;
    const int Item_Distance = 2;

    //移動
    bool run_flag = false;  //走っているかどうかフラグ

    //視点移動
    [SerializeField] float cameraSensitivity = 90;
    float rot_x = 0.0f;
    float rot_y = 0.0f;

    [SerializeField] GameObject rot_obj;　  //弾丸生成位置用
    [SerializeField] GameObject dir_obj;    //向きを制御したいObject
    [SerializeField] GameObject camera_obj; 

    //アイテムを拾う
    [SerializeField] GameObject hand;//設置場所
    public GameObject hand_weapon;//手にある武器
    bool item_all_get_flag = false;

    //判定
    public bool attacked_zonbi_flag = false;//ダメージ判定
    public bool bitten_zonbi_flag = false;//ゲームオーバー判定
    bool game_clear_flag = false;//ゲームクリア判定

    //アニメーション
    [SerializeField] GameObject anim_obj;
    bool idle_flag = false;
    bool hand_pistol_flag = false;

    //犬
    [SerializeField] GameObject dog;

    //ゲージ
    [SerializeField] GameObject food_gage;  //食料ゲージ
    [SerializeField] float food_num_max;    //食料ゲージの最大値
    [SerializeField] float food_num_now;    //食料ゲージの現在値
    [SerializeField] GameObject hp_gague;   //体力ゲージ
    public float hp_num_max;      //体力ゲージの最大値
    public float hp_num_now;      //体力ゲージの現在値

    //視点方向探索用
    private SearchViewArea searchViewArea;
    [SerializeField]//サウンド用
    private PlayerSound playerSound;

    [SerializeField]//ゲームオーバー用
    private GameObject fadeOutUI;
    [SerializeField]
    private SceneChanger sceneChanger;

    //インベントリ
    InventoryItem m_inventoryItem;
    InventoryWeapon m_inventoryWeapon;
    InventoryManager m_inventoryManager;

    //オブジェクト
    [SerializeField] GameObject m_inventoryManagerObj;

    //セーフエリア
    public bool m_inSafeAreaFlag = true;

    //フラグ
    bool m_openInventoryFlag = false;

    //ポーズ用停止フラグ
    private bool is_pause = false;


    public INVENTORY inventory_status = INVENTORY.NON;
    public InventoryChest chest;

    // Start is called before the first frame update
    void Start()
    {
        //インベントリ
        m_inventoryItem = GetComponent<InventoryItem>();
        m_inventoryWeapon = GetComponent<InventoryWeapon>();
        m_inventoryManager = m_inventoryManagerObj.GetComponent<InventoryManager>();


        //コンポーネント取得
        Animator = anim_obj.GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();

        searchViewArea = GetComponent<SearchViewArea>();

        //ゲージ設定
        food_num_now = food_gage.GetComponent<Gauge>().GaugeSetting(food_num_max);
        hp_num_now   = hp_gague.GetComponent<Gauge>().GaugeSetting(hp_num_max);

        //カーソルキー非表示
        Screen.lockCursor = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (game_clear_flag) return;

        if (!DowmPlayer())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_inventoryManager.m_inventoryState = INVENTORY.ITEM;
                m_openInventoryFlag = m_inventoryManager.OpenClose(m_openInventoryFlag, null);
            }
        }

        //インベントリ開いているとき
        if (m_openInventoryFlag)
        {
            //ゲージ回復処理
            if (Input.GetMouseButtonDown(1))
            {
                m_inventoryItem.Recovery_Gage(food_gage, hp_gague, m_inSafeAreaFlag);
            }

        }
        if (is_pause)
        {
            //移動を止める
            Rigidbody.velocity = new Vector3(0, 0);

            run_flag = false;
            idle_flag = true;
            Animation();
            Rigidbody.velocity = new Vector3(0, 0);
            return;
        }

        //プレイヤーが倒されていない場合
        if (!DowmPlayer())
        {
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    m_inventoryManager.m_inventoryState = INVENTORY.ITEM;
            //    m_openInventoryFlag = m_inventoryManager.OpenClose(m_openInventoryFlag, null);
            //}

            hand_weapon = m_inventoryWeapon.m_weaponSlotObj[(int)m_inventoryWeapon.m_selectSlot];

            //インベントリ閉じている
            if (!m_openInventoryFlag)
            {
                //ゲージ処理
                Gauge();

                //移動処理
                MoveSet();

                //視点移動
                ViewpointMove();
                //マウスカーソル表示非表示
                MouseCursorVisibility();

                //アイテムを拾う
                PickUpItem();

                //武器の入れ替え
                hand_weapon = m_inventoryWeapon.ChangeWeapon();

                //武器別処理
                AttackWeapon();
            }
            //Inventoryを開いている
            else
            {
                ////ゲージ回復処理
                //if (Input.GetMouseButtonDown(1))
                //{
                //    m_inventoryItem.Recovery_Gage(food_gage, hp_gague, m_inSafeAreaFlag);
                //}

                ////移動を止める
                //Rigidbody.velocity = new Vector3(0, 0);

                //run_flag = false;
                //idle_flag = true;
                ////インベントリのアイテムを調べる
                ////Inventory.CheckInventoryItem();
            }
        }
        else//ゲームオーバー
        {
            if (game_clear_flag) return;
            //ゾンビの向いている向きによって倒れる方向を変える（ゾンビの向いている方向の逆方向へ倒れる（後ろ））
            //か、画面フェードアウト
            fadeOutUI.GetComponent<FadeImage>().StartFade();
            sceneChanger.LoadGameOverScene();
        }

        //アニメーション
        Animation();
    }

    void MoveSet()
    {
        //ダッシュ判定処理
        run_flag = (RunFlag());

        if (run_flag)//走り移動
        {
            idle_flag = Move(Run_Speed, Rigidbody);
        }
        else//歩き移動
        {
            idle_flag = Move(Walk_Speed, Rigidbody);
        }
    }

    void ViewpointMove()//視点移動
    {
        rot_x += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        rot_y += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        rot_y = Mathf.Clamp(rot_y, -60, 60);

        dir_obj.transform.localRotation = Quaternion.AngleAxis(rot_x, Vector3.up);
        camera_obj.transform.localRotation = Quaternion.AngleAxis(rot_y, Vector3.left);//*を外した
    }

    void　PickUpItem()  //アイテムを拾う
    {
        //視点方向のアイテムを探す
        GameObject item = searchViewArea.GetObjUpdate("item", Item_Distance,0.03f);
        Debug.Log(item);
        if (Input.GetMouseButtonDown(1))

        {        
            if (item == null) return;

            if (item.GetComponent<ItemSetting>().iteminfo.id == ITEM_ID.CHEST)
            {
                m_inventoryManager.m_inventoryState = INVENTORY.CHEST;
                m_openInventoryFlag = m_inventoryManager.OpenClose(m_openInventoryFlag, item);
                return;
            }

            playerSound.PlayPickUp();//SE

            //アイテム取得
            bool all_get_flag = false;
            all_get_flag = m_inventoryItem.AddInventory_PickUP_Item(item.GetComponent<ItemSetting>().iteminfo,ref m_inventoryWeapon);

            ITEM_ID id = item.GetComponent<ItemSetting>().iteminfo.id;

            if (id >= ITEM_ID.PISTOL && id <= ITEM_ID.SHOTGUN)
            {
                item.GetComponent<GunManager>().m_handPlayerObj = gameObject;
            }

            if (all_get_flag)
            {
                if (id >= ITEM_ID.PISTOL && id <= ITEM_ID.SHOTGUN) 
                {
                    item.GetComponent<GunManager>().m_handPlayerObj = gameObject;
                }
                else
                {
                    Debug.Log("w_get");
                    Destroy(item);
                }
            }

        }
    }


    void AttackWeapon() //武器別処理
    {
        //フラグ初期化
        hand_pistol_flag = false;

        searchViewArea.ResetColor("Zombie");

        switch (m_inventoryWeapon.m_selectSlot)
        {
            case SLOT_ORDER.KNIFE:
                //攻撃、animation処理
                hand_weapon.GetComponent<knifeAttackAnimetion>().AttackAnimation(camera_obj);
                break;
            //ピストル
            case SLOT_ORDER.GUN:

                hand_pistol_flag = true;

                //アニメーション起動
                hand_weapon.GetComponent<Animator>().enabled = true;

                //リロード処理
                if (Input.GetKeyDown(KeyCode.R))
                {
                    //リロード処理
                    hand_weapon.GetComponent<GunManager>().Reload();
                }

                //攻撃
                if (Input.GetMouseButtonDown(0))
                {
                    hand_weapon.GetComponent<GunManager>().PullTriggerDown();
                }

                //攻撃
                if (Input.GetMouseButton(0))
                {

                    hand_weapon.GetComponent<GunManager>().PullTrigger();
                }

                break;
            //犬
            case SLOT_ORDER.DOG:

                //攻撃するオブジェクト取得
                GameObject attack_obj = searchViewArea.GetObjUpdate("Zombie", 20f, 0.5f);
                Debug.Log("dog" + attack_obj);

                //攻撃
                if (Input.GetMouseButtonDown(0))
                {
                    if (attack_obj != null)
                    {
                        if (attack_obj.tag == "Zombie")
                        {
                            playerSound.PlayWhistleAttack();//se
                            dog.GetComponent<DogManager>().OrderAttack(attack_obj.GetComponentInParent<ZombieManager>().gameObject);
                        }
                    }
                }
                //仮で探知
                else if(Input.GetMouseButtonDown(1))
                {
                    playerSound.PlayWhistleDetect();//se
                    dog.GetComponent<DogManager>().OrderDetection();
                }

                break;
        }
    }

    void Gauge()        //各ゲージ処理
    {
        //食料ゲージ減少
        food_num_now = food_gage.GetComponent<Gauge>().DurationReduce(3.0f, 1.0f);
        //食料ゲージがなくなった場合持続ダメージ
        hp_num_now = (int)hp_gague.GetComponent<Gauge>().DurationDamage(6.0f, 1, food_gage, hp_gague);
    }

    void Animation() //アニメーション
    {
        Animator.SetBool("Run", run_flag);  //走る
        Animator.SetBool("Idle", idle_flag);  //idle
        Animator.SetBool("HandPislol", hand_pistol_flag);  //pistol所持状態
    }


    public GameObject Ray(float _distance)//レイの先にあるオブジェクト取得
    {
        GameObject hit_obj;

        //ビューポート座標のレイを飛ばす
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            //アイテムまでの距離を調べる
            float distance = Vector3.Distance(hit.transform.position, transform.position);
            hit_obj = hit.collider.gameObject;

            //距離が範囲内なら
            if (distance <= _distance)
            {
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);
                return hit_obj;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    public void DamagePlayer()  //プレイヤーがダメージを受ける
    {
        hp_num_now = hp_gague.GetComponent<Gauge>().ReduceGauge(Damage_Num);

        playerSound.PlayDamage();//SE
    }

    public void GameClear() //ゲームクリアかどうか
    {
        //クリアフラグON
        game_clear_flag = true;
        Debug.Log("CLEAR");
    }

    bool DowmPlayer()   //体力が残っているか調べる
    {
        if (hp_num_now <= 0) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 休息（休息後の体力割合、消費する食料）
    /// </summary>
    public void TakeRest(float _setHpPer, float _useFoodPer)
    {
        //回復後の体力計算
        float nextHp = hp_num_max * _setHpPer;
        //現在の体力の方が少ないなら回復
        if (nextHp > hp_num_now)
        {
            //体力ゲージ減少
            hp_gague.GetComponent<Gauge>().Increase_Gauge(nextHp - hp_num_now);
        }

        //消費後の食料ゲージ計算
        float nextFood = food_num_now - (food_num_max * _useFoodPer);
        //下限を決める
        float nextFoodMin = food_num_max * 0.1f;

        //回復はしないように
        if (food_num_now <= nextFoodMin) return;

        //下限を下回る
        if (nextFood <= nextFoodMin)
        {
            nextFood = nextFoodMin;
        }
        //食料ゲージ減少
        food_gage.GetComponent<Gauge>().ReduceGauge(food_num_now - nextFood);
    }

    public void Pause()
    {
        is_pause = true;
    }

    public void Resume()
    {
        is_pause = false;
    }
}


////Y軸制御
////angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
////dir_obj.transform.localEulerAngles = new Vector3(dir_obj.transform.localEulerAngles.x, angle.y);
////if (mouse_pos.y <= Max_X_angle)
////{
////    mouse_pos.y = Max_X_angle;
////}
////else if (mouse_pos.y >= -Max_X_angle)
////{
////    //mouse_pos.y = -Max_X_angle;
////}
////横方向
//float rot_character = mouse_pos.x;
//rot_character += 2.0f * Time.deltaTime;
//dir_obj.transform.localRotation = Quaternion.Euler(0.0f, rot_character, 0.0f);
////縦方向制御
//float rot = mouse_start.y - mouse_pos.y;
//if (Mathf.Abs(rot) <= Max_X_angle)
//{
//    rot += 2.0f * Time.deltaTime;
//    Quaternion rotation = Quaternion.Euler(rot, 0.0f, 0.0f);
//    camera_obj.transform.localRotation = rotation;
//}
////camera_obj.transform.localRotation = rotation;
////アングル制御したいObjectに代入
////for (int i = 0; i < angle_change_obj.Length; i++)
////{
////    angle_change_obj[i].transform.localEulerAngles = angle;
////}
//mouse_pos = Input.mousePosition;


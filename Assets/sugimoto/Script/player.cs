using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;
//using UnityEngine.EventSystems;

public class player : MonoBehaviour
{
    Inventory Inventory;
    Animator Animator;  // アニメーターコンポーネント取得用
    Rigidbody Rigidbody;

    const float Attacked_Speed = 1.5f;
    const float Walk_Speed = 5.0f;
    const float Run_Speed = 10.0f;
    const float Max_X_angle = 60.0f;
    const int Damage_Num = 1;
    const int Attack_Distance = 180;
    const int Item_Distance = 5;

    Vector3 Pistol_angle { get { return new Vector3(315.0f, 14.999999f, 44.9999924f); } }

    //移動
    bool run_flag = false;  //走っているかどうかフラグ
    int key_push_cnt = 0;   //キー入力された回数
    float push_timer = 0.0f;//ダブル入力カウント用
    Vector3 before_pos;

    //視点移動
    [SerializeField] float cameraSensitivity = 90;
    float rot_x = 0.0f;
    float rot_y = 0.0f;

    Vector3 mouse_pos;                      //マウスの位置
    [SerializeField] GameObject rot_obj;　  //弾丸生成位置用
    [SerializeField] GameObject dir_obj;    //向きを制御したいObject
    [SerializeField] GameObject camera_obj; 

    Vector3 mouse_start;

    //アイテムを拾う
    [SerializeField] GameObject hand;//設置場所
    public GameObject hand_weapon;//手にある武器

    //判定
    public int hp;
    public int MAX_HP;
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
    [SerializeField] float hp_num_max;      //体力ゲージの最大値
    [SerializeField] float hp_num_now;      //体力ゲージの現在値

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネント取得
        Inventory = GetComponent<Inventory>();
        Animator = anim_obj.GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();

        //後で消す
        hp = (int)hp_num_max;

        //ゲージ設定
        food_num_now = food_gage.GetComponent<Gauge>().GaugeSetting(food_num_max);
        hp_num_now   = hp_gague.GetComponent<Gauge>().GaugeSetting(hp_num_max);

        //カーソルキー非表示
        Screen.lockCursor = true;

        //マウスの位置情報保存
        mouse_pos = Input.mousePosition;
        mouse_start = Input.mousePosition;
        before_pos = gameObject.transform.position;
        //angle = this.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DowmPlayer())
        {
            //アイテムInventory開閉
            GetComponent<Inventory>().ItemInventory();

            if (!GetComponent<Inventory>().item_inventory_flag)
            {
                //食料ゲージ減少
                food_num_now = food_gage.GetComponent<Gauge>().DurationReduce(2.0f, 100.0f);
                Debug.Log(food_num_now);
                //食料ゲージがなくなった場合持続ダメージ
                hp = (int)hp_gague.GetComponent<Gauge>().DurationDamage(2.0f, 1, food_gage, hp_gague);

                //移動処理
                {
                    //ダッシュ判定処理
                    if (!attacked_zonbi_flag)
                    {
                        //移動キーが入力されていないかつダッシュコマンド入力１回目じゃなければ移動初期化
                        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && key_push_cnt != 1)
                        {
                            key_push_cnt = 0;
                            push_timer = 0.0f;
                            run_flag = false;
                        }

                        //Wキーが２回入力されたらダッシュ
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            key_push_cnt++;
                        }

                        //ダッシュコマンド１回目の場合
                        if (key_push_cnt == 1)
                        {
                            run_flag = false;

                            //ダブル入力されなければ歩き（短い時間以内に２回入力）
                            push_timer += Time.deltaTime;
                            if (push_timer >= 1)
                            {
                                key_push_cnt = 0;
                            }
                        }
                        //ダブル入力されればダッシュ
                        else if (key_push_cnt >= 2)
                        {
                            run_flag = true;
                        }


                        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
                        {
                            run_flag = true;
                        }


                        //走り移動
                        if (run_flag)
                        {
                            Move(Run_Speed);
                        }
                        //歩き移動
                        else
                        {
                            Move(Walk_Speed);
                        }
                    }
                    else
                    {
                        Move(Attacked_Speed);
                    }

                }

                //視点移動
                {
                    rot_x += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
                    rot_y += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
                    rot_y = Mathf.Clamp(rot_y, -90, 90);

                    dir_obj.transform.localRotation = Quaternion.AngleAxis(rot_x, Vector3.up);
                    camera_obj.transform.localRotation = Quaternion.AngleAxis(rot_y, Vector3.left);//*を外した

                    if (Input.GetKeyDown(KeyCode.End))
                    {
                        if (Screen.lockCursor == false)
                        {
                            Screen.lockCursor = true;
                        }
                        else
                        {
                            Screen.lockCursor = false;
                        }
                    }
                }

                //アイテムを拾う
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        //アイテム取得
                        GameObject item = Ray(Item_Distance);

                        if (item != null)
                        {
                            switch (item.tag)
                            {
                                /*アイテムについて
                                 *各武器はのタグをweaponに設定
                                 *各アイテムはitemのタグに設定
                                 *武器オブジェクトにID設定用のスクリプトをアタッチ
                                 *そこでIDを設定する
                                 */

                                case "weapon":
                                    GetComponent<Inventory>().WeaponGet(item);
                                    break;
                                case "item":
                                    GetComponent<Inventory>().ItemGet(item);
                                    break;
                            }
                        }
                    }
                }

                //武器
                {
                    //武器の入れ替え
                    GetComponent<Inventory>().ChangeWeapon();

                    //全ゾンビオブジェクト
                    GameObject[] zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");

                    //距離が一定以下のゾンビを全て取得
                    foreach (var zombie in zombieObjects)
                    {
                        //全てのゾンビの色を通常に戻す 処理が重いかも
                        zombie.GetComponent<ZombieManager>().ChangeColorAlpha(0.0f);
                    }


                        //フラグ初期化
                        hand_pistol_flag = false;

                    switch (Inventory.hand_weapon)
                    {
                        //ピストル
                        case Inventory.WEAPON_ID.PISTOL:

                            hand_pistol_flag = true;

                            //リロード処理
                            if (Input.GetKeyDown(KeyCode.R))
                            {
                                hand_weapon.GetComponent<Pistol>().Reload(GetComponent<Inventory>());
                            }
                            //攻撃
                            if (Input.GetMouseButtonDown(0))
                            {
                                hand_weapon.GetComponent<Pistol>().Attack(rot_obj, hand_weapon);

                            }
                            break;
                        //犬
                        case Inventory.WEAPON_ID.DOG:

                            GameObject attack_obj=LockOnUpdate();

                            //攻撃
                            if (Input.GetMouseButtonDown(0))
                            {
                                ////攻撃するオブジェクト取得
                               // GameObject attack_obj = Ray(Attack_Distance);

                                if (attack_obj != null)
                                {
                                    if (attack_obj.tag == "Zombie"/* || attack_obj.tag == "Head"*/)
                                    {
                                        dog.GetComponent<DogManager>().OrderAttack(attack_obj.GetComponentInParent<ZombieManager>().gameObject);
                                        Debug.Log(attack_obj + "a");
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            //Inventoryを開いている状態なら
            else
            {
                run_flag = false;
                idle_flag = true;
                //インベントリのアイテムを調べる
                GetComponent<Inventory>().CheckInventoryItem();
            }
        }
        else//ゲームオーバー
        {
            //ゾンビの向いている向きによって倒れる方向を変える（ゾンビの向いている方向の逆方向へ倒れる（後ろ））
            //か、画面フェードアウト
        }

        //アニメーション
        Animator.SetBool("Run", run_flag);  //走る
        Animator.SetBool("Idle", idle_flag);  //idle
        Animator.SetBool("HandPislol", hand_pistol_flag);  //pistol所持状態

        //位置保存
        before_pos = transform.position;
    }

    public GameObject LockOnUpdate()
    {
        //全ゾンビオブジェクト
        GameObject[] zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");

        //対象の条件　後でクラス変数化
        float activeAngle = 20.0f;
        float activeDistance = 20.0f;

        Vector3 playerPos = dir_obj.transform.position;
        Vector3 cameraPos = camera_obj.transform.position;
        Vector3 eyeDir = camera_obj.transform.forward;//視点方向ベクトル

        List<GameObject> targetZombies = new List<GameObject>();

        //距離が一定以下のゾンビを全て取得
        foreach (var zombie in zombieObjects)
        {
            ////全てのゾンビの色を通常に戻す 処理が重いかも
            //zombie.GetComponent<ZombieManager>().ChangeColorAlpha(0.0f);

            Debug.Log("ゾンビ発見");
            //距離を調べる
            Vector3 zombiePos = zombie.transform.position;
            //zombiePos.y += 2.0f;
            if (Vector3.Distance(playerPos, zombiePos) > activeDistance) continue;

            targetZombies.Add(zombie);//リストに追加
        }

        if (targetZombies.Count != 0)
        {
            //ゾンビの中心位置調整用
            Vector3 zombieCenterAd = Vector3.up * 2.0f;

            //対象のゾンビの中から視点から角度が一番近いオブジェクトを取得
            GameObject nearestEnemy =
                targetZombies.OrderBy(p =>
                Vector3.Angle(((p.transform.position + zombieCenterAd) - cameraPos).normalized, eyeDir)).First();

            Debug.Log("角度:" + Vector3.Angle(((nearestEnemy.transform.position + zombieCenterAd) - cameraPos).normalized, eyeDir));

            //取得したオブジェクトまでと視点の角度が一定以下なら
            if (Vector3.Angle(((nearestEnemy.transform.position + zombieCenterAd) - cameraPos).normalized, eyeDir) <= activeAngle)
            {
                //対象の色を変更
                nearestEnemy.GetComponent<ZombieManager>().ChangeColorAlpha(0.25f);

                //nearestEnemyが犬の攻撃対象

                return nearestEnemy;
            }

        }
        return null;
    }


    //レイの先にあるオブジェクト取得
    public GameObject Ray(float _distance)
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


    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody.velocity = Vector3.zero;
    }

    private void Move(float _speed)
    {
        idle_flag = true;
        {
            //リジットボディーでの移動
            Vector3 vec = Vector3.zero;

            // Wキー（前方移動）
            if (Input.GetKey(KeyCode.W))
            {
                idle_flag = false;
                vec += transform.forward;
            }
            
            // Sキー（後方移動）
            if (Input.GetKey(KeyCode.S))
            {
                idle_flag = false;
                vec += -transform.forward;
            }

            // Dキー（右移動）
            if (Input.GetKey(KeyCode.D))
            {
                idle_flag = false;
                vec += transform.right;
            }

            // Aキー（左移動）
            if (Input.GetKey(KeyCode.A))
            {
                idle_flag = false;
                vec += -transform.right;
            }

            //斜め移動の速度を一定にするため正規化
            vec.Normalize();

            //yはそのまま（代入すると重力に影響があるため）
            Rigidbody.velocity = new Vector3(vec.x * _speed, Rigidbody.velocity.y, vec.z * _speed);

            //transformでの移動
            //// Wキー（前方移動）
            //if (Input.GetKey(KeyCode.W))
            //{
            //    idle_flag = false;
            //    transform.position += _speed * transform.forward * Time.deltaTime;
            //}

            //// Sキー（後方移動）
            //if (Input.GetKey(KeyCode.S))
            //{
            //    idle_flag = false;
            //    transform.position -= _speed * transform.forward * Time.deltaTime;
            //}

            //// Dキー（右移動）
            //if (Input.GetKey(KeyCode.D))
            //{
            //    idle_flag = false;
            //    transform.position += _speed * transform.right * Time.deltaTime;
            //}

            //// Aキー（左移動）
            //if (Input.GetKey(KeyCode.A))
            //{
            //    idle_flag = false;
            //    transform.position -= _speed * transform.right * Time.deltaTime;
            //}
        }

    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }

    public void DamagePlayer()  //プレイヤーがダメージを受ける
    {
        hp = (int)hp_gague.GetComponent<Gauge>().ReduceGauge(Damage_Num);
    }

    public void GameClear() //ゲームクリアかどうか
    {
        //クリアフラグON
        game_clear_flag = true;
        Debug.Log("CLEAR");
    }

    bool DowmPlayer()   //体力が残っているか調べる
    {
        if (hp <= 0) 
        {
            return true;
        }
        else
        {
            return false;
        }
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


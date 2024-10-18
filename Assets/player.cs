using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class player : MonoBehaviour
{
    MapCreate MapCreate;
    [SerializeField]Inventory Inventory;

    const float Attacked_Speed = 1.5f;
    const float Walk_Speed = 5.0f;
    const float Run_Speed = 20.0f;
    const float Max_X_angle = 60.0f;
    const int Damage_Num = 1;

    Vector3 Pistol_angle { get { return new Vector3(0, -15, 0); } }

    //移動
    bool run_flag = false;  //走っているかどうかフラグ
    int key_push_cnt = 0;   //キー入力された回数
    float push_timer = 0.0f;//ダブル入力カウント用

    //視点移動
    Vector3 mouse_pos;                      //マウスの位置
    [SerializeField] GameObject rot_obj;　  //弾丸生成位置用
    [SerializeField] GameObject dir_obj;    //向きを制御したいObject
    [SerializeField] GameObject camera_obj; 

    Vector3 mouse_start;

    //アイテムを拾う
    [SerializeField] GameObject hand;//設置場所
    GameObject hand_weapon;//手にある武器


    //ダメージ判定
    public int hp;
    public bool attacked_zonbi_flag = false;//ダメージ判定
    public bool bitten_zonbi_flag = false;//ゲームオーバー判定


    // Start is called before the first frame update
    void Start()
    {
        Inventory = GetComponent<Inventory>();

        mouse_pos = Input.mousePosition;
        mouse_start = Input.mousePosition;
        //angle = this.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bitten_zonbi_flag)
        {
            //移動処理
            {
                //ダッシュ判定処理
                if (!attacked_zonbi_flag)
                {
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


                        if(Input.GetKey(KeyCode.W)&& Input.GetKey(KeyCode.LeftShift))
                        {
                            run_flag = true;
                        }

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
                //Y軸制御
                //angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
                //dir_obj.transform.localEulerAngles = new Vector3(dir_obj.transform.localEulerAngles.x, angle.y);

                //if (mouse_pos.y <= Max_X_angle)
                //{
                //    mouse_pos.y = Max_X_angle;
                //}
                //else if (mouse_pos.y >= -Max_X_angle)
                //{
                //    //mouse_pos.y = -Max_X_angle;
                //}

                //横方向

                float rot_character = mouse_pos.x;
                rot_character += 2.0f * Time.deltaTime;


                dir_obj.transform.localRotation = Quaternion.Euler(0.0f, rot_character, 0.0f);

                //縦方向制御
                float rot = mouse_start.y - mouse_pos.y;
                if (Mathf.Abs(rot) <= Max_X_angle)
                {
                    rot += 2.0f * Time.deltaTime;
                    Quaternion rotation = Quaternion.Euler(rot, 0.0f, 0.0f);

                    camera_obj.transform.localRotation = rotation;
                }
                //camera_obj.transform.localRotation = rotation;
                //アングル制御したいObjectに代入
                //for (int i = 0; i < angle_change_obj.Length; i++)
                //{
                //    angle_change_obj[i].transform.localEulerAngles = angle;
                //}


                mouse_pos = Input.mousePosition;
            }

            //アイテムを拾う
            {
                if (Input.GetMouseButtonDown(1))
                {
                    //ビューポート座標のレイを飛ばす
                    Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                    RaycastHit hit = new RaycastHit();

                    if (Physics.Raycast(ray, out hit))
                    {
                        //アイテムまでの距離を調べる
                        float distance = Vector3.Distance(hit.transform.position, transform.position);

                        //距離近ければ拾う
                        if (distance <= 5.0f)
                        {
                            GameObject item = hit.collider.gameObject;

                            switch (item.tag)
                            {
                                case "pistol":
                                    //遠距離武器を持っていない場合取得
                                    if (GetComponent<Inventory>().weapon_hand_obj[(int)Inventory.WEAPON_ID.PISTOL] == null)
                                    {
                                        //武器インベントリに入れる
                                        GetComponent<Inventory>().weapon_hand_obj[(int)Inventory.WEAPON_ID.PISTOL] = item;

                                        //手に何も持っていなければ自動的に持つ
                                        if (hand_weapon == null)
                                        {
                                            //手にある武器をピストルへ変更
                                            GetComponent<Inventory>().weapon_cnt = (int)Inventory.WEAPON_ID.PISTOL;
                                            hand_weapon = GetComponent<Inventory>().weapon_hand_obj[(int)Inventory.WEAPON_ID.PISTOL];

                                            //transform設定
                                            ParentChildren(hand, hand_weapon);                                //手の子にする
                                            hand_weapon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //スケール変更
                                            hand_weapon.transform.localEulerAngles = Pistol_angle;            //ピストル用のアングルへ変更
                                        }
                                    }
                                    //持っていない場合は弾丸を取得
                                    else
                                    {
                                        //弾丸(アイテム)を取得(Inventoryに弾丸があって最大数じゃないまたはInventoryに弾丸はないが空いていれば)
                                        Inventory.ItemGet(item);

                                        Destroy(item);
                                    }

                                    break;
                            }
                        }
                    }
                }
            }

            //武器
            {
                //武器の入れ替え
                GetComponent<Inventory>().HandWeapon();

                switch(GetComponent<Inventory>().weapon_cnt)
                {
                    //手にある武器がピストル
                    case (int)Inventory.WEAPON_ID.PISTOL:

                        //リロード処理
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            hand_weapon.GetComponent<Pistol>().Reload();
                        }
                        //攻撃
                        if(Input.GetMouseButtonDown(0))
                        {
                            hand_weapon.GetComponent<Pistol>().Attack(rot_obj, hand_weapon);

                        }
                        break;
                }
            }

            //ダメージ
        }
        else//ゲームオーバー
        {
            //ゾンビの向いている向きによって倒れる方向を変える（ゾンビの向いている方向の逆方向へ倒れる（後ろ））
            //か、画面フェードアウト
        }
    }

    private void Move(float _speed)
    {
        {
            // Wキー（前方移動）
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += _speed * transform.forward * Time.deltaTime;
            }

            // Sキー（後方移動）
            if (Input.GetKey(KeyCode.S))
            {
                transform.position -= _speed * transform.forward * Time.deltaTime;
            }

            // Dキー（右移動）
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += _speed * transform.right * Time.deltaTime;
            }

            // Aキー（左移動）
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= _speed * transform.right * Time.deltaTime;
            }
        }

    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }

    public void DamagePlayer()
    {
        GetComponent<HpGage>().HpDamageGage(Damage_Num);
    }
}

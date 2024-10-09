using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : CharacterBase
{
    const float Attacked_Speed = 1.5f;
    const float Walk_Speed = 5.0f;
    const float Run_Speed = 10.0f;
    const float Max_Y_angle = 60.0f;
    const float Max_X_angle = 90.0f;
    Vector3 Pistol_angle { get { return new Vector3(0, -15, 0); } }

    //移動
    bool run_flag = false;  //走っているかどうかフラグ
    int key_push_cnt = 0;   //キー入力された回数
    float push_timer = 0.0f;//ダブル入力カウント用
    //視点移動
    Vector3 mouse_pos;                      //マウスの位置
    Vector3 angle = new Vector3(0, 0, 0);　 //角度
    [SerializeField] GameObject rot_obj;　  //弾丸生成位置用
    [SerializeField] GameObject dir_obj;    //向きを制御したいObject
    [SerializeField] GameObject[] angle_change_obj;

    //アイテムを拾う
    [SerializeField] GameObject hand;
    [SerializeField] GameObject hand_item;
    [SerializeField] GameObject camera_obj;
    GameObject item;

    //ダメージ判定
    public bool attacked_zonbi_flag = false;
    public bool bitten_zonbi_flag = false;
    bool targe_get_flag = false;
    Vector3 targe;

    //攻撃
    //Pistol
    [SerializeField] GameObject bullet;



    // Start is called before the first frame update
    void Start()
    {
        mouse_pos = Input.mousePosition;
        angle = this.transform.localEulerAngles;
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
                angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
                dir_obj.transform.localEulerAngles = new Vector3(dir_obj.transform.localEulerAngles.x, angle.y);
                //if (angle.y <= Max_Y_angle && angle.y >= -Max_Y_angle)
                //{
                //    angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
                //}
                //else
                //{
                //    if (angle.y > Max_Y_angle) 
                //    {
                //        //angle.y = Max_Y_angle;
                //        angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
                //        dir_obj.transform.localEulerAngles = new Vector3(dir_obj.transform.localEulerAngles.x, angle.y);
                //    }
                //    else if(angle.y<-Max_Y_angle)
                //    {
                //        //angle.y = -Max_Y_angle;
                //        angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
                //        dir_obj.transform.localEulerAngles = new Vector3(dir_obj.transform.localEulerAngles.x, angle.y);

                //    }
                //}

                //X軸制御
                if (angle.x <= Max_X_angle && angle.x >= -Max_X_angle)
                {
                    angle.x -= (Input.mousePosition.y - mouse_pos.y) * 0.2f;
                }
                else
                {
                    if (angle.x > Max_X_angle)
                    {
                        angle.x = Max_X_angle;
                    }
                    else if (angle.x < -Max_X_angle)
                    {
                        angle.x = -Max_X_angle;
                    }
                }

                //アングル制御したいObjectに代入
                for (int i = 0; i < angle_change_obj.Length; i++)
                {
                    angle_change_obj[i].transform.localEulerAngles = angle;
                }


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
                            item = hit.collider.gameObject;

                            switch (item.tag)
                            {
                                case "pistol":
                                    if (hand_item == null)
                                    {
                                        hand_item = item;
                                        ParentChildren(hand, hand_item);                                //手の子にする
                                        hand_item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //スケール変更
                                        hand_item.transform.localEulerAngles = Pistol_angle;            //ピストル用のアングルへ変更
                                    }
                                    break;
                            }

                            Debug.Log(item);
                        }
                    }
                }
            }

            //攻撃
            {
                if (Input.GetMouseButtonDown(0) && hand_item != null)
                {
                    switch (hand_item.tag)
                    {
                        case "pistol":
                            //位置
                            Vector3 pos = transform.position;
                            //向き
                            Quaternion rot = rot_obj.transform.rotation;
                            //弾丸生成
                            Instantiate(bullet, hand_item.transform.position, rot);
                            break;
                    }
                }
            }

        }
        else//ゲームオーバー
        {
            float speed = 2.0f;

            //ゾンビの向いている向きによって倒れる方向を変える（ゾンビの向いている方向の逆方向へ倒れる（後ろ））

            //if(transform.localEulerAngles>=90)
            //angle.x += (-transform.forward.y - mouse_pos.y) * 0.2f;
            //angle.y = 0;
            //transform.localEulerAngles = angle;

            //if(!targe_get_flag)
            //{
            //    targe = new Vector3(0, 90, 0);
            //    targe_get_flag = true;
            //}

            //// 正面に目標を捉えてから90度回転させた回転を表すQuaternion
            //Quaternion targetQ = Quaternion.LookRotation(targe, Vector3.up) * Quaternion.AngleAxis(0, Vector3.right);

            //// 徐々に回転
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQ, speed);
        }
    }

    private void Move(float _speed)
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

    void ParentChildren(GameObject _parent,GameObject _child)
    {
        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

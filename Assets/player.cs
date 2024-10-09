using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : CharacterBase
{
    const float Walk_Speed = 5.0f;
    const float Run_Speed = 10.0f;
    const float Max_Y_angle = 60.0f;
    const float Max_X_angle = 90.0f;

    //Pistol
    [SerializeField] GameObject bullet;
    [SerializeField] Transform pistol;
    [SerializeField] GameObject[] angle_change_obj;


    //移動
    bool run_flag = false;  //走っているかどうかフラグ
    int key_push_cnt = 0;   //キー入力された回数
    float push_timer = 0.0f;//ダブル入力カウント用
    //視点移動
    Vector3 mouse_pos;                      //マウスの位置
    Vector3 angle = new Vector3(0, 0, 0);　 //角度
    [SerializeField] GameObject rot_obj;　  //弾丸生成位置用
    [SerializeField] GameObject dir_obj;    //向きを制御したいObject

    // Start is called before the first frame update
    void Start()
    {
        mouse_pos = Input.mousePosition;
        angle = this.transform.localEulerAngles;

    }

    // Update is called once per frame
    void Update()
    {
        //移動処理
        {
            //ダッシュ処理
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
                Run();
            }
            //歩き移動
            else
            {
                Walk();
            }
        }

        //視点移動
        {
            //Y軸制御
            if (angle.y <= Max_Y_angle && angle.y >= -Max_Y_angle)
            {
                angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
            }
            else
            {
                if (angle.y > Max_Y_angle) 
                {
                    //angle.y = Max_Y_angle;
                    angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
                    dir_obj.transform.localEulerAngles = new Vector3(dir_obj.transform.localEulerAngles.x, angle.y);
                }
                else if(angle.y<-Max_Y_angle)
                {
                    //angle.y = -Max_Y_angle;
                    angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
                    dir_obj.transform.localEulerAngles = new Vector3(dir_obj.transform.localEulerAngles.x, angle.y);

                }
            }

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

        //攻撃
        {
            //Pistol
            if (Input.GetMouseButtonDown(0))
            {
                //位置
                Vector3 pos = transform.position;
                //向き
                Quaternion rot = rot_obj.transform.rotation;
                //弾丸生成
                Instantiate(bullet, pistol.position, rot);
            }
        }
    }


    void Walk()
    {
        // Wキー（前方移動）
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Walk_Speed * transform.forward * Time.deltaTime;
        }

        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Walk_Speed * transform.forward * Time.deltaTime;
        }

        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Walk_Speed * transform.right * Time.deltaTime;
        }

        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Walk_Speed * transform.right * Time.deltaTime;
        }
    }
    void Run()
    {
        // Wキー（前方移動）
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Run_Speed * transform.forward * Time.deltaTime;
        }

        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Run_Speed * transform.forward * Time.deltaTime;
        }

        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Run_Speed * transform.right * Time.deltaTime;
        }

        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Run_Speed * transform.right * Time.deltaTime;
        }
    }

}

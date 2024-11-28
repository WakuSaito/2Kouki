using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFunction : MonoBehaviour
{
    //キー入力
    int key_push_cnt = 0;   //押した回数
    float push_timer = 0.0f;//ダブル入力のTimer

    //移動
    public bool Move(float _speed,Rigidbody _rb)
    {
        bool idle_flag = true;

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
        _rb.velocity = new Vector3(vec.x * _speed, _rb.velocity.y, vec.z * _speed);

        return idle_flag;
    }
    //走る
    public bool RunFlag()
    {
        bool run_flag = false;

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

        return run_flag;
    }

    //マウスカーソル表示非表示
    public void MouseCursorVisibility()
    {
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
}

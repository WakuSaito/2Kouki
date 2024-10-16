using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

public class Zombie : MonoBehaviour
{
    [SerializeField]//探知範囲
    float detection_range = 20.0f;
    [SerializeField]//掴む距離
    float grap_range = 1.0f;
    [SerializeField]//走る速度
    float run_speed = 6.0f;
    [SerializeField]//歩く速度
    float walk_speed = 1.0f;
    [SerializeField]//攻撃のディレイ
    double attack_delay_sec = 3.0;

    float random_walk_time = 0.0f;//ランダムウォークの目標時間用
    float random_walk_count = 0.0f;//ランダムウォークの時間計測用

    Rigidbody rb;

    GameObject PlayerObj;//プレイヤー

    [SerializeField]
    bool on_move_stop = false;//移動不可フラグ

    //async用トークン（遅延実行のキャンセル用）
    private CancellationTokenSource _cancellationTokenSource =
            new CancellationTokenSource();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //プレイヤーの位置取得
        PlayerObj = GameObject.FindGameObjectWithTag("Player");

        //on_move_stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) DamageBody();
        if (Input.GetKeyDown(KeyCode.B)) DamageHead();

        //移動不可なら処理しない
        if (on_move_stop) {
            rb.velocity = Vector3.zero;
            return;
        }

        //座標取得
        Vector3 pos = transform.position;
        Vector3 player_pos = PlayerObj.transform.position;
        //プレイヤーとの距離計算
        float player_distance = Vector3.Distance(pos, player_pos);

        float current_speed;


        if (player_distance <= detection_range)//プレイヤーとの距離が一定以下
        {
            //プレイヤーの方を向く
            var direction = player_pos - pos;
            direction.y = 0;

            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);

            //transform.LookAt(PlayerObj.transform, transform.up);

            current_speed = run_speed;//速度変更
        }
        else//通常の動作
        {
            if (random_walk_count >= random_walk_time)
            {
                random_walk_count = 0.0f;//リセット
                random_walk_time = UnityEngine.Random.Range(4.0f, 8.0f);//次に向きを変えるまでの時間
                //ランダムに向きを変更
                Vector3 course = new Vector3(0, UnityEngine.Random.Range(0, 180), 0);
                transform.localRotation = Quaternion.Euler(course);
            }
            else
            {
                //時間カウント
                random_walk_count += Time.deltaTime;
            }

            current_speed = walk_speed;//速度変更
        }

        //y軸を無視する
        Vector3 vec = transform.forward;
        vec.y = 0.0f;
        Vector3.Normalize(vec);

        rb.velocity = vec * current_speed;

        //向いている方向に移動
        //transform.Translate(Vector3.forward * walk_speed * Time.deltaTime);

        if (player_distance <= grap_range)//掴みかかる距離
        {
            GrapPlayer();
        }
    }
    //プレイヤーをつかむ
    private void GrapPlayer()
    {
        Debug.Log("Grap");
        on_move_stop = true;//移動停止

        //ここでプレイヤー側のつかまれる関数を呼び出したい
        //その際このGameObjectの情報を渡したい

        // CancellationTokenを生成
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        //数秒後
        //この間にダメージを受けるなどでキャンセル
        DelayRunAsync(token, attack_delay_sec,
            () => BitePlayer() //噛みつく
            );    
    }
    //プレイヤーに噛みつく
    private void BitePlayer()
    {
        //プレイヤーをゲームオーバーにする
        Debug.Log("Attack");

        on_move_stop = false;//移動再開（動作テスト用）
    }

    //体にダメージを受けた
    public void DamageBody()
    {
        Debug.Log("Body");
        //スタン
        Stan();
    }
    //頭にダメージを受けた
    public void DamageHead()
    {
        Debug.Log("Head");
        Dead();//死亡
    }
    //スタン
    private void Stan()
    {
        on_move_stop = true;
        //のけぞるor一定時間停止
        _cancellationTokenSource.Cancel();//遅延実行中のasync停止

        //のけぞり
        rb.AddForce(transform.forward * -1.0f * 20.0f, ForceMode.Impulse);

        DelayRunAsync(1.5,
        () => on_move_stop = false//移動再開
        );
    }
    //死亡処理
    private void Dead()
    {
        _cancellationTokenSource.Cancel();//遅延実行中のasync停止

        //Destroy(gameObject);
    }
    //遅らせて実行する
    private async ValueTask DelayRunAsync(CancellationToken token, double wait_sec, Action action)
    {
        // ディレイ処理
        await Task.Delay(TimeSpan.FromSeconds(wait_sec), token);
        action();
    }
    private async ValueTask DelayRunAsync(double wait_sec, Action action)
    {
        // ディレイ処理
        await Task.Delay(TimeSpan.FromSeconds(wait_sec));
        action();
    }
}

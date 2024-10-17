using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

//自動でアタッチされるスクリプト
[RequireComponent(typeof(ZombieMove))]
[RequireComponent(typeof(ZombieAnimation))]

/// <summary>
/// ゾンビの管理クラス
/// ZombieBaseを継承したクラスを扱う
/// </summary>
public class ZombieManager : MonoBehaviour
{
    /// <summary>
    /// 操作するクラス
    /// </summary>
    [SerializeField]
    private ZombieBase[] zombieBases;

    private ZombieMove zombieMove;
    private ZombieAttack zombieAttack;
    private ZombieAnimation zombieAnimation;

    GameObject playerObj;

    [SerializeField]//プレイヤーの探知範囲
    float detectionPlayerRange = 10.0f;

    [SerializeField]//攻撃開始距離
    float attackStartRange = 1.0f;

    //awaitでフラグのオンオフをしたほうが良さそう
    float randomWalkTime = 0.0f;//ランダムウォークの目標時間用
    float randomWalkCount = 0.0f;//ランダムウォークの時間計測用

    //攻撃のクールタイム中
    private bool isAttackCoolDown = false;

    private void Awake()
    {
        //プレイヤーオブジェクト取得
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var zombie in zombieBases){
            Debug.Log(zombie);

            //各クラスでオーバーライドした初期設定実行
            zombie.SetUpZombie();

            //下記クラスに該当するか確認し代入
            //どうやら違うクラスでTryGetComponentするとnullが代入されるっぽい
            if (zombieMove == null) zombie.TryGetComponent(out zombieMove);
            if (zombieAttack == null) zombie.TryGetComponent(out zombieAttack);
            if (zombieAnimation == null) zombie.TryGetComponent(out zombieAnimation);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //座標取得
        Vector3 pos = transform.position;
        Vector3 playerPos = playerObj.transform.position;
        pos.y = 0; playerPos.y = 0;//仮でy座標を無視
        //プレイヤーとの距離計算
        float playerDistance = Vector3.Distance(pos, playerPos);

        //移動
        {
            if(playerDistance < 0.5f)
            {
                //とりあえず近づきすぎないようにした
                zombieMove.StopMove();
            }
            else if (playerDistance < detectionPlayerRange)
            {
                //プレイヤーの方の向きを求める
                Vector3 direction = playerPos - pos;
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
                //向きの変更
                zombieMove.ChangeDirection(lookRotation);

                //走る
                zombieMove.RunFront();
            }
            else
            {
                if (randomWalkCount >= randomWalkTime)
                {
                    randomWalkCount = 0.0f;//リセット
                                           //次に向きを変えるまでの時間を決める
                    randomWalkTime = UnityEngine.Random.Range(4.0f, 8.0f);

                    //ランダムに向きを設定
                    Vector3 direction = new Vector3(0, UnityEngine.Random.Range(0, 180), 0);
                    //向きを変更
                    zombieMove.ChangeDirection(Quaternion.Euler(direction));
                }
                else
                {
                    //時間カウント
                    randomWalkCount += Time.deltaTime;
                }

                //歩く
                zombieMove.WalkFront();
            }
        }

        //攻撃
        {
            if (isAttackCoolDown) return;//クールタイムチェック

            if (playerDistance < attackStartRange) 
            {
                //攻撃開始
                zombieAttack.StartAttack();

                //攻撃のクールタイム中にする
                isAttackCoolDown = true;
                //数秒後クールタイム解除
                DelayRunAsync(
                    3.0,
                    () => isAttackCoolDown = false
                    );
            }
        }
    }


    private async ValueTask DelayRunAsync(double wait_sec, Action action)
    {
        // ディレイ処理
        await Task.Delay(TimeSpan.FromSeconds(wait_sec));
        action();
    }

}

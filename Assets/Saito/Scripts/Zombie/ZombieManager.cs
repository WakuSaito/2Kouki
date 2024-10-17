using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

//自動でアタッチされるスクリプト
[RequireComponent(typeof(ZombieMove))]
[RequireComponent(typeof(ZombieAnimation))]
[RequireComponent(typeof(ZombieAction))]

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
    private ZombieAction zombieAction;

    GameObject playerObj;

    [SerializeField]//プレイヤーの探知範囲
    float detectionPlayerRange = 10.0f;

    [SerializeField]//攻撃開始距離
    float attackStartRange = 1.0f;

    //攻撃のクールタイム中
    private bool isAttackCoolDown = false;
    //ランダムに向きを変えるクールタイム中
    private bool isChangeDirCoolDown = false;

    //目標とする向き
    Quaternion targetRotation;

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
            //どうやら違うクラスでTryGetComponentするとnullが代入されるっぽいのでnullチェック
            if (zombieMove == null) zombie.TryGetComponent(out zombieMove);
            if (zombieAttack == null) zombie.TryGetComponent(out zombieAttack);
            if (zombieAnimation == null) zombie.TryGetComponent(out zombieAnimation);
            if (zombieAction == null) zombie.TryGetComponent(out zombieAction);
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
                zombieAnimation.Idle();//停止モーション
            }
            else if (playerDistance < detectionPlayerRange)
            {
                //プレイヤーの方の向きを求める
                Vector3 direction = playerPos - pos;
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                //角度を補間
                //歩きと走りで同じようなスクリプトなので改善の余地あり 補間方法も一考の余地あり
                var qua = Quaternion.RotateTowards(transform.rotation, targetRotation, 500*Time.deltaTime);
                //向きを変更
                zombieMove.ChangeDirection(qua);

                //走る
                zombieMove.RunFront();
                zombieAnimation.Run();//移動モーション
            }
            else
            {
                //向き変更
                if (!isChangeDirCoolDown)
                {
                    isChangeDirCoolDown = true;//クールタイム中に
                    DelayRunAsync(
                        UnityEngine.Random.Range(4.0f, 8.0f),//次に向きを変えるまでの時間を決める
                        () => isChangeDirCoolDown = false  //フラグオフ
                        );          

                    //ランダムに向きを設定
                    Vector3 direction = new Vector3(0, UnityEngine.Random.Range(-180, 180), 0);
                    targetRotation = Quaternion.Euler(direction);
                }

                //角度を補間
                var qua = Quaternion.RotateTowards(transform.rotation, targetRotation, 100*Time.deltaTime);
                //向きを変更
                zombieMove.ChangeDirection(qua);

                //歩く
                zombieMove.WalkFront();
                zombieAnimation.Walk();//移動モーション
            }
        }

        //攻撃
        {
            if (isAttackCoolDown) return;//クールタイムチェック

            if (playerDistance < attackStartRange) 
            {
                //攻撃開始
                zombieAttack.StartAttack();
                //攻撃モーション再生
                zombieAnimation.Attack();

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

    //体にダメージを受けた
    public void DamageBody()
    {
        Debug.Log("Body");
        //スタン
        zombieAction.KnockBack();
    }
    //頭にダメージを受けた
    public void DamageHead()
    {
        Debug.Log("Head");
        zombieAction.Dead();//死亡
    }

    private async ValueTask DelayRunAsync(double wait_sec, Action action)
    {
        // ディレイ処理
        await Task.Delay(TimeSpan.FromSeconds(wait_sec));
        action();
    }

}

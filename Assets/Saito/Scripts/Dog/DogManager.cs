using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

/*
後ほどクラスを分割したい
・移動クラス
・アニメーションクラス
・その他アクションごとに分割
*/

//自動でアタッチされるスクリプト
[RequireComponent(typeof(DogMove))]
[RequireComponent(typeof(DogAnimation))]

/// <summary>
/// 犬のマネージャークラス
/// </summary>
public class DogManager : MonoBehaviour
{
    /// <summary>
    /// 操作するクラス
    /// </summary>
    [SerializeField]
    private DogBase[] dogBases;

    private DogMove dogMove;
    private DogAnimation dogAnimation;

    [SerializeField]//噛みついている時間
    private double biteStaySec = 4.0;

    [SerializeField]//待機状態になるプレイヤーとの距離
    private float stayPlayerDistance = 5.0f;


    //攻撃対象オブジェクト
    private GameObject attackTargetObj;
    //プレイヤー
    private GameObject playerObj;

    //攻撃対象に突進中
    private bool isChargeTarget = false;
    //行動停止
    private bool isStopAction = false;

    private void Awake()
    {
        //プレイヤー取得
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var dog in dogBases)
        {
            //各クラスでオーバーライドした初期設定実行
            dog.SetUpDog();

            //下記クラスに該当するか確認し代入
            //どうやら違うクラスでTryGetComponentするとnullが代入されるっぽいのでnullチェック
            if (dogMove == null) dog.TryGetComponent(out dogMove);
            if (dogAnimation == null) dog.TryGetComponent(out dogAnimation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopAction) {
            dogMove.StopMove();
            return; 
        }

        if(Input.GetKeyDown(KeyCode.O))//デバッグ用
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Zombie");
            OrderAttack(obj);
        }

        if (isChargeTarget)//突進
        {
            //攻撃対象が存在しないならreturn
            //攻撃対象が途中でDestroyされた場合の挙動注意
            if (attackTargetObj == null){
                isChargeTarget = false;//突進中断
                return;
            }

            //attackTargetObjの位置に向かって移動する
            dogMove.LookAtObject(attackTargetObj);
            dogMove.RunFront();//移動

            dogAnimation.Run();//アニメーション

            //攻撃対象に限りなく近づいたら
            if (GetObjectDistance(attackTargetObj) < 0.5f)
            {
                //噛みつく
                BiteZombie(attackTargetObj);
            }
        }
        else//通常時の移動
        {

            //if(プレイヤーとの距離が離れている)
            if (GetObjectDistance(playerObj) > stayPlayerDistance)
            {
                //プレイヤーの方に移動する
                //障害物の判定に注意
                dogMove.LookAtObject(playerObj);
                dogMove.RunFront();

                dogAnimation.Run();//アニメーション

            }
            else
            {
                dogAnimation.Idle();
                dogMove.StopMove();
            }
            //else if(近くなら)
            //とりあえずは何もしない
            //プレイヤーの近くでウロウロさせたい　範囲内のランダム位置を目標として移動みたいな
        }
    }

    /// <summary>
    /// 対象のオブジェクトとの距離を求める
    /// </summary>
    private float GetObjectDistance(GameObject _target)
    {
        if (_target == null) return 0.0f;

        Vector3 pos = transform.position;
        Vector3 targetPos = _target.transform.position;
        pos.y = 0; targetPos.y = 0;//y方向を計算しない
        return Vector3.Distance(targetPos, pos);
    }

    /// <summary>
    /// 攻撃指示を受けたとき
    /// </summary>
    public void OrderAttack(GameObject _obj)
    {
        isChargeTarget = true;

        attackTargetObj = _obj;//攻撃対象を取得
    }

    /// <summary>
    /// ゾンビに噛みつく
    /// </summary>
    private void BiteZombie(GameObject _zombieObj)
    {
        ZombieManager zombieManager;
        //attackTargetObjからZombieManagerを取得し、FreezePositionを呼び出し
        attackTargetObj.TryGetComponent(out zombieManager);
        if (zombieManager == null) return;

        zombieManager.FreezePosition(biteStaySec);//ゾンビを停止

        dogAnimation.Attack();
        isStopAction = true;
        isChargeTarget = false;

        DelayRunAsync(
            biteStaySec,
            () => {
                isStopAction = false; 
            });
    }

    /// <summary>
    /// 遅らせてActionを実行するasync
    /// </summary>
    private async ValueTask DelayRunAsync(double wait_sec, Action action)
    {
        // ディレイ処理
        await Task.Delay(TimeSpan.FromSeconds(wait_sec));
        action();
    }
}

/*
・待機時の通常行動
プレイヤー周囲一定距離のランダムな待機エリア内位置を決める
↓
そこまで歩く
↓
ランダムな時間停止　→　上へ戻る

・待機時にプレイヤーから離れたときの行動
待機時の通常行動をキャンセル
↓
プレイヤー周囲一定距離のランダムな待機エリア内位置を決める
↓
そこまで走る
↓
ランダムな時間停止　→　通常行動へ戻る

・攻撃時の行動
攻撃指示を受ける
↓
対象に突進
↓
噛みつく
↓
暫くしたら離して、ちょっとスタン　→　離れた時の行動へ

通常行動中のみ他の指示を受ける

指示は一つの関数の引数で指定するのがいいのでは
現在の行動を他から参照できるようにしたい

 */
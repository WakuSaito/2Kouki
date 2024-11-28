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

    private TargetMark targetMark;

    [SerializeField]//噛みついている時間
    private double biteStaySec = 4.0;

    [SerializeField]//待機状態になるプレイヤーとの距離
    private float stayPlayerDistance = 5.0f;

    [SerializeField]//探知距離
    private float detectRange = 30.0f;


    //攻撃対象オブジェクト
    private GameObject attackTargetObj;
    //プレイヤー
    private GameObject playerObj;

    //移動目標座標
    private Vector3 targetPos;

    //移動停止フラグ
    private bool onFreezeMove = false;

    //行動停止
    private bool isStopAction = false;
    //指示を受けないフラグ
    private bool IsIgnoreOrder = false;

    //攻撃対象に突進中
    private bool isChargeTarget = false;
    //移動方法を歩行にする
    private bool isMoveTypeWalk = false;


    private void Awake()
    {
        //プレイヤー取得
        playerObj = GameObject.FindGameObjectWithTag("Player");

        targetMark = gameObject.GetComponent<TargetMark>();
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

        RandomTargetPos();
    }

    //デバッグ用
    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Zombie");
            OrderAttack(obj);
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            OrderDetection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopAction) {
            dogMove.StopMove();//移動停止
            return; 
        }

        DebugUpdate();//デバッグ用

        if (isChargeTarget)//突進
        {
            //攻撃対象が存在しないならreturn
            //攻撃対象が途中でDestroyされた場合の挙動注意
            if (attackTargetObj == null){
                isChargeTarget = false;//突進中断
                return;
            }

            //attackTargetObjの位置に向かって移動する
            dogMove.LookAtPosition(attackTargetObj.transform.position);
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
            NomalUpdate();
        }
    }

    //通常行動用の仮関数
    private void NomalUpdate()
    {
        //移動先座標がプレイヤーから離れているなら決めなおす
        float playerTargetDistance = Vector3.Distance(playerObj.transform.position, targetPos);
        if (playerTargetDistance > stayPlayerDistance)
        {
            RandomTargetPos();
            onFreezeMove = false;//停止中でも解除
            isMoveTypeWalk = false;//走るようにする
        }

        if (onFreezeMove) return;

        //目標座標までの位置を求める
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        //プレイヤーと自身の距離
        float playerDistance = Vector3.Distance(playerObj.transform.position, pos);

        //ここで移動
        dogMove.LookAtPosition(targetPos);//向き変更
        //プレイヤーとの距離によって速度変更
        if (isMoveTypeWalk)
        {
            dogMove.WalkFront();
            dogAnimation.Walk();
        }
        else
        {
            dogMove.RunFront();
            dogAnimation.Run();
        }

        float distance = Vector3.Distance(pos, targetPos);
        //到着したら
        if (distance < 0.1f)
        {
            //停止
            dogMove.StopMove();
            dogAnimation.Idle();

            //停止時間をランダムに決める
            //変数は後でクラス変数にする
            double freezeSec = UnityEngine.Random.Range(2.0f, 5.0f);

            onFreezeMove = true;
            _ = DelayRunAsync(
                        freezeSec,
                        () => {
                            onFreezeMove = false;
                            RandomTargetPos();
                            isMoveTypeWalk = true;
                        }
                        );
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
    public void OrderAttack(GameObject _obj)//zombieの子のパーツが渡されたとき動かない可能性アリ
    {
        if (IsIgnoreOrder) return;
        Debug.Log("攻撃指示を受け付けた");

        isChargeTarget = true;

        attackTargetObj = _obj;//攻撃対象を取得
    }
    /// <summary>
    /// 指示：周囲の探知
    /// </summary>
    public void OrderDetection()
    {
        if (IsIgnoreOrder) return;
        Debug.Log("探知開始");

        //一定範囲の対象のオブジェクトをマーク
        targetMark.RangeMark();
    }

    /// <summary>
    /// ゾンビに噛みつく
    /// </summary>
    private void BiteZombie(GameObject _zombieObj)
    {
        Debug.Log("ゾンビに噛みつき");

        ZombieManager zombieManager;
        //attackTargetObjからZombieManagerを取得し、FreezePositionを呼び出し
        attackTargetObj.TryGetComponent(out zombieManager);
        if (zombieManager == null) return;

        zombieManager.FreezePosition(biteStaySec);//ゾンビを停止

        dogAnimation.Attack();
        isStopAction = true;
        isChargeTarget = false;

        _ = DelayRunAsync(
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



    //プレイヤー一定範囲のランダム位置を目標座標に設定する
    private void RandomTargetPos()
    {
        //プレイヤーの周囲のランダム位置を求める
        Vector3 pPos = playerObj.transform.position;
        //移動先位置をランダムに決める
        targetPos = UnityEngine.Random.insideUnitCircle * stayPlayerDistance;
        targetPos.z = targetPos.y;//平面上に生成するため入れ替え
        targetPos.y = 0.5f;//y方向は一律にする
        //アタッチしたオブジェクトを基準にする
        targetPos.x += pPos.x;
        targetPos.z += pPos.z;
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
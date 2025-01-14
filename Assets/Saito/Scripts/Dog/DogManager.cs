using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//自動でアタッチされるスクリプト
[RequireComponent(typeof(DogMove))]
[RequireComponent(typeof(DogAnimation))]
[RequireComponent(typeof(DogSound))]

/// <summary>
/// 犬マネージャークラス
/// </summary>
public class DogManager : MonoBehaviour, IStopObject
{
    /// <summary>
    /// 操作するクラス
    /// </summary>
    [SerializeField]
    private DogBase[] mDogBases;

    private DogMove mDogMove;
    private DogAnimation mDogAnimation;
    private DogSound mDogSound;

    private TargetMark mTargetMark;

    [SerializeField]//噛みついている時間
    private float mBiteStaySec = 4.0f;

    [SerializeField]//待機状態になるプレイヤーとの距離
    private float mStayPlayerDistance = 5.0f;

    [SerializeField]//探知のクールタイム
    private float mDetectCooldownSec = 60.0f;


    //攻撃対象オブジェクト
    private GameObject mAttackTargetObj;
    //プレイヤー
    private GameObject mPlayerObj;

    //移動目標座標
    private Vector3 mTargetPos;

    //移動停止フラグ
    private bool mOnFreezeMove = false;

    //行動停止
    [SerializeField]
    private bool mIsStopAction = false;
    //指示を受けないフラグ
    private bool mIsIgnoreOrder = false;
    //探知のクールタイム中
    private bool mIsDetectCooldown = false;

    //攻撃対象に突進中
    private bool mIsChargeTarget = false;
    //移動方法を歩行にする
    private bool mIsMoveTypeWalk = false;

    //ポーズ用停止フラグ
    private bool mIsPause = false;

    //動作中の遅延動作
    List<IEnumerator> mInActionDelays = new List<IEnumerator>();


    private void Awake()
    {
        //プレイヤー取得
        mPlayerObj = GameObject.FindGameObjectWithTag("Player");

        mTargetMark = gameObject.GetComponent<TargetMark>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var dog in mDogBases)
        {
            //各クラスでオーバーライドした初期設定実行
            dog.SetUpDog();

            //下記クラスに該当するか確認し代入
            //どうやら違うクラスでTryGetComponentするとnullが代入されるっぽいのでnullチェック
            if (mDogMove == null) dog.TryGetComponent(out mDogMove);
            if (mDogAnimation == null) dog.TryGetComponent(out mDogAnimation);
            if (mDogSound == null) dog.TryGetComponent(out mDogSound);
        }

        RandomTargetPos();
    }


    // Update is called once per frame
    void Update()
    {
        if (mIsStopAction || mIsPause) {
            mDogMove.StopMove();//移動停止
            return; 
        }

        if (mIsChargeTarget)//突進
        {
            //攻撃対象が存在しないならreturn
            //攻撃対象が途中でDestroyされた場合の挙動注意
            if (mAttackTargetObj == null){
                mIsChargeTarget = false;//突進中断
                return;
            }

            //attackTargetObjの位置に向かって移動する
            mDogMove.LookAtPosition(mAttackTargetObj.transform.position);
            mDogMove.RunFront();//移動

            mDogAnimation.Run();//アニメーション

            //攻撃対象に限りなく近づいたら
            if (GetObjectDistance(mAttackTargetObj) < 0.5f)
            {
                //噛みつく
                BiteZombie(mAttackTargetObj);
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
        float player_target_distance = Vector3.Distance(mPlayerObj.transform.position, mTargetPos);
        if (player_target_distance > mStayPlayerDistance)
        {
            RandomTargetPos();
            mOnFreezeMove = false;//停止中でも解除
            mIsMoveTypeWalk = false;//走るようにする
        }

        if (mOnFreezeMove) return;

        //目標座標までの位置を求める
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        //プレイヤーと自身の距離
        float player_distance = Vector3.Distance(mPlayerObj.transform.position, pos);
        //距離が遠い場合は指示を受け付けない
        if (player_distance <= mStayPlayerDistance)
        {
            mIsIgnoreOrder = false;
        }
        else
        {
            mIsIgnoreOrder = true;
        }

        //ここで移動
        mDogMove.LookAtPosition(mTargetPos);//向き変更
        //プレイヤーとの距離によって速度変更
        if (mIsMoveTypeWalk)
        {
            mDogMove.WalkFront();
            mDogAnimation.Walk();
        }
        else
        {
            mDogMove.RunFront();
            mDogAnimation.Run();
        }

        float distance = Vector3.Distance(pos, mTargetPos);
        //到着したら
        if (distance < 0.1f)
        {
            //停止
            mDogMove.StopMove();
            mDogAnimation.Idle();

            //停止時間をランダムに決める
            //変数は後でクラス変数にする
            float freeze_sec = UnityEngine.Random.Range(2.0f, 5.0f);

            mOnFreezeMove = true;
            //一定時間停止
            mInActionDelays.Add(
                        DelayRunCoroutine(
                        freeze_sec,
                        () => {
                            mOnFreezeMove = false;
                            RandomTargetPos();
                            mIsMoveTypeWalk = true;
                        }
                        ));
            StartCoroutine(mInActionDelays[mInActionDelays.Count - 1]);
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
        if (!CanOrderAttack()) return;
        Debug.Log("攻撃指示を受け付けた");

        mIsChargeTarget = true;

        mAttackTargetObj = _obj;//攻撃対象を取得

        mDogSound.PlayAttackBark();//鳴き声
    }
    /// <summary>
    /// 指示：周囲の探知
    /// </summary>
    public void OrderDetection()
    {
        if (!CanOrderDetection()) return;
        Debug.Log("探知開始");

        //一定範囲の対象のオブジェクトをマーク
        mTargetMark.RangeMark();

        mDogSound.PlayDetectBark();//鳴き声

        mIsChargeTarget = false;//攻撃はキャンセル

        //クールタイム
        mIsDetectCooldown = true;

        mInActionDelays.Add(
                        DelayRunCoroutine(
                        mDetectCooldownSec,
                        () => {
                            mIsDetectCooldown = false;
                        }
                        ));
        StartCoroutine(mInActionDelays[mInActionDelays.Count - 1]);
    }
    //攻撃指示可能か
    public bool CanOrderAttack()
    {
        if (mIsStopAction) return false;
        if (mIsIgnoreOrder) return false;

        return true;
    }
    //攻撃指示可能か
    public bool CanOrderDetection()
    {
        if (mIsStopAction) return false;
        if (mIsIgnoreOrder) return false;
        if (mIsDetectCooldown) return false;

        return true;
    }

    //チュートリアル用のクールタイム監視用
    public bool UsedOrderDetection()
    {
        return mIsDetectCooldown;
    }

    /// <summary>
    /// ゾンビに噛みつく
    /// </summary>
    private void BiteZombie(GameObject _zombie_obj)
    {
        Debug.Log("ゾンビに噛みつき");

        ZombieManager zombie_manager;
        //attackTargetObjからZombieManagerを取得し、FreezePositionを呼び出し
        mAttackTargetObj.TryGetComponent(out zombie_manager);
        if (zombie_manager == null) return;

        zombie_manager.FreezePosition((float)mBiteStaySec);//ゾンビを停止

        mDogAnimation.Attack();
        mIsStopAction = true;
        mIsChargeTarget = false;

        //一定時間停止
        mInActionDelays.Add(
                        DelayRunCoroutine(
                        mBiteStaySec,
                        () => {
                            mIsStopAction = false;
                        }
                        ));
        StartCoroutine(mInActionDelays[mInActionDelays.Count - 1]);
    }


    /// <summary>
    /// 遅らせてActionを実行するコルーチン
    /// </summary>
    private IEnumerator DelayRunCoroutine(float _wait_sec, Action _action)
    {
        //このコルーチンの情報取得 出来ればリスト追加もここでやりたい
        IEnumerator this_cor = mInActionDelays[mInActionDelays.Count - 1];

        //コルーチンを再開しても待機時間情報が消えないようにする
        for (float i = 0; i < _wait_sec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);

        _action();
        //終了時にこのコルーチン情報を削除
        mInActionDelays.Remove(this_cor);
    }



    //プレイヤー一定範囲のランダム位置を目標座標に設定する
    private void RandomTargetPos()
    {
        //プレイヤーの周囲のランダム位置を求める
        Vector3 p_pos = mPlayerObj.transform.position;
        //移動先位置をランダムに決める
        mTargetPos = UnityEngine.Random.insideUnitCircle * mStayPlayerDistance;
        mTargetPos.z = mTargetPos.y;//平面上に生成するため入れ替え
        mTargetPos.y = 0.5f;//y方向は一律にする
        //アタッチしたオブジェクトを基準にする
        mTargetPos.x += p_pos.x;
        mTargetPos.z += p_pos.z;
    }

    //外部から行動の停止を変える用
    public void OnStopAction(bool _flag)
    {
        mIsStopAction = _flag;
    }


    //インターフェースでの停止処理用
    //一時停止
    public void Pause()
    {
        mIsPause = true;

        //ループ中に要素が変わらないようにクッションを噛ます
        List<IEnumerator> tmp_list = new List<IEnumerator>(mInActionDelays);
        foreach (var cor in tmp_list)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
    }
    //再開
    public void Resume()
    {
        mIsPause = false;

        List<IEnumerator> tmp_list = new List<IEnumerator>(mInActionDelays);
        foreach (var cor in tmp_list)
        {
            if (cor == null) continue;

            StartCoroutine(cor);
        }
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
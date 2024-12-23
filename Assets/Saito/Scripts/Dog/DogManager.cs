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
[RequireComponent(typeof(DogSound))]

/// <summary>
/// 犬のマネージャークラス
/// </summary>
public class DogManager : MonoBehaviour, IStopObject
{
    /// <summary>
    /// 操作するクラス
    /// </summary>
    [SerializeField]
    private DogBase[] dogBases;

    private DogMove dogMove;
    private DogAnimation dogAnimation;
    private DogSound dogSound;

    private TargetMark targetMark;

    [SerializeField]//噛みついている時間
    private float biteStaySec = 4.0f;

    [SerializeField]//待機状態になるプレイヤーとの距離
    private float stayPlayerDistance = 5.0f;

    [SerializeField]//探知距離
    private float detectRange = 30.0f;

    [SerializeField]//探知のクールタイム
    private float detectCooldownSec = 60.0f;


    //攻撃対象オブジェクト
    private GameObject attackTargetObj;
    //プレイヤー
    private GameObject playerObj;

    //移動目標座標
    private Vector3 targetPos;

    //移動停止フラグ
    private bool onFreezeMove = false;

    //行動停止
    [SerializeField]
    private bool isStopAction = false;
    //指示を受けないフラグ
    private bool isIgnoreOrder = false;
    //探知のクールタイム中
    private bool isDetectCooldown = false;

    //攻撃対象に突進中
    private bool isChargeTarget = false;
    //移動方法を歩行にする
    private bool isMoveTypeWalk = false;

    //ポーズ用停止フラグ
    private bool isPause = false;

    //動作中の遅延動作
    List<IEnumerator> inActionDelays = new List<IEnumerator>();


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
            if (dogSound == null) dog.TryGetComponent(out dogSound);
        }

        RandomTargetPos();
    }


    // Update is called once per frame
    void Update()
    {
        if (isStopAction || isPause) {
            dogMove.StopMove();//移動停止
            return; 
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
        //距離が遠い場合は指示を受け付けない
        if (playerDistance <= stayPlayerDistance)
        {
            isIgnoreOrder = false;
        }
        else
        {
            isIgnoreOrder = true;
        }

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
            float freezeSec = UnityEngine.Random.Range(2.0f, 5.0f);

            onFreezeMove = true;
            //一定時間停止
            inActionDelays.Add(
                        DelayRunCoroutine(
                        freezeSec,
                        () => {
                            onFreezeMove = false;
                            RandomTargetPos();
                            isMoveTypeWalk = true;
                        }
                        ));
            StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
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

        isChargeTarget = true;

        attackTargetObj = _obj;//攻撃対象を取得

        dogSound.PlayAttackBark();//鳴き声
    }
    /// <summary>
    /// 指示：周囲の探知
    /// </summary>
    public void OrderDetection()
    {
        if (!CanOrderDetection()) return;
        Debug.Log("探知開始");

        //一定範囲の対象のオブジェクトをマーク
        targetMark.RangeMark();

        dogSound.PlayDetectBark();//鳴き声

        isChargeTarget = false;//攻撃はキャンセル

        //クールタイム
        isDetectCooldown = true;

        inActionDelays.Add(
                        DelayRunCoroutine(
                        detectCooldownSec,
                        () => {
                            isDetectCooldown = false;
                        }
                        ));
        StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
    }
    //攻撃指示可能か
    public bool CanOrderAttack()
    {
        if (isStopAction) return false;
        if (isIgnoreOrder) return false;

        return true;
    }
    //攻撃指示可能か
    public bool CanOrderDetection()
    {
        if (isStopAction) return false;
        if (isIgnoreOrder) return false;
        if (isDetectCooldown) return false;

        return true;
    }

    //チュートリアル用のクールタイム監視用
    public bool UsedOrderDetection()
    {
        return isDetectCooldown;
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

        zombieManager.FreezePosition((float)biteStaySec);//ゾンビを停止

        dogAnimation.Attack();
        isStopAction = true;
        isChargeTarget = false;

        //一定時間停止
        inActionDelays.Add(
                        DelayRunCoroutine(
                        biteStaySec,
                        () => {
                            isStopAction = false;
                        }
                        ));
        StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
    }


    /// <summary>
    /// 遅らせてActionを実行するコルーチン
    /// </summary>
    private IEnumerator DelayRunCoroutine(float _wait_sec, Action _action)
    {
        //このコルーチンの情報取得 出来ればリスト追加もここでやりたい
        IEnumerator thisCor = inActionDelays[inActionDelays.Count - 1];

        //コルーチンを再開しても待機時間情報が消えないようにする
        for (float i = 0; i < _wait_sec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);

        _action();
        //終了時にこのコルーチン情報を削除
        inActionDelays.Remove(thisCor);
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

    //外部から行動の停止を変える用
    public void OnStopAction(bool _flag)
    {
        isStopAction = _flag;
    }


    //インターフェースでの停止処理用
    //一時停止
    public void Pause()
    {
        isPause = true;

        //ループ中に要素が変わらないようにクッションを噛ます
        List<IEnumerator> tmpList = new List<IEnumerator>(inActionDelays);
        foreach (var cor in tmpList)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
    }
    //再開
    public void Resume()
    {
        isPause = false;

        List<IEnumerator> tmpList = new List<IEnumerator>(inActionDelays);
        foreach (var cor in tmpList)
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
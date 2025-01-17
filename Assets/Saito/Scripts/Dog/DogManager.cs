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
/// 犬ベースクラスを継承しているスクリプトの管理
/// ポーズ時に停止するため、IStopObjectを継承
/// </summary>
public class DogManager : MonoBehaviour, IStopObject
{
    /// <summary>
    /// 操作するクラス
    /// </summary>
    [SerializeField] private DogBase[] m_dogBases;

    private DogMove m_dogMove;
    private DogAnimation m_dogAnimation;
    private DogSound m_dogSound;

    private TargetMark m_targetMark;

    //噛みついている時間
    [SerializeField] private float m_biteStaySec = 4.0f;
    //探知のクールタイム
    [SerializeField] private float m_detectCooldownSec = 60.0f;

    //待機状態になるプレイヤーとの距離
    [SerializeField] private float m_stayPlayerDistance = 5.0f;

    //攻撃対象オブジェクト
    private GameObject m_attackTargetObj;
    //プレイヤー
    private GameObject m_playerObj;

    //移動目標座標
    private Vector3 m_targetPos;

    //移動停止フラグ
    private bool m_onFreezeMove = false;

    //行動停止 初期状態設定可
    [SerializeField] private bool m_isStopAction = false;
    //指示を受けないフラグ
    private bool m_isIgnoreOrder = false;
    //探知のクールタイム中
    private bool m_isDetectCooldown = false;

    //攻撃対象に突進中
    private bool m_isChargeTarget = false;
    //移動方法を歩行にする
    private bool m_isMoveTypeWalk = false;

    //ポーズ用停止フラグ
    private bool m_isPause = false;

    //動作中の遅延動作
    List<IEnumerator> m_inActionDelays = new List<IEnumerator>();


    //コンポーネントの取得
    private void Awake()
    {
        //プレイヤー取得
        m_playerObj = GameObject.FindGameObjectWithTag("Player");

        m_targetMark = gameObject.GetComponent<TargetMark>();
    }

    //DogBaseを継承したクラスの取得と移動先決定
    void Start()
    {
        foreach(var dog in m_dogBases)
        {
            //各クラスでオーバーライドした初期設定実行
            dog.SetUpDog();

            //下記クラスに該当するか確認し代入
            //どうやら違うクラスでTryGetComponentするとnullが代入されるっぽいのでnullチェック
            if (m_dogMove == null) dog.TryGetComponent(out m_dogMove);
            if (m_dogAnimation == null) dog.TryGetComponent(out m_dogAnimation);
            if (m_dogSound == null) dog.TryGetComponent(out m_dogSound);
        }

        RandomTargetPos();//移動先設定
    }


    // 状態によってアクションを決めるメインループ
    void Update()
    {
        if (m_isStopAction || m_isPause) {
            m_dogMove.StopMove();//移動停止
            return; 
        }

        if (m_isChargeTarget)//突進
        {
            //攻撃対象が存在しないならreturn
            //攻撃対象が途中でDestroyされた場合の挙動注意
            if (m_attackTargetObj == null){
                m_isChargeTarget = false;//突進中断
                return;
            }

            //attackTargetObjの位置に向かって移動する
            m_dogMove.LookAtPosition(m_attackTargetObj.transform.position);
            m_dogMove.RunFront();//移動

            m_dogAnimation.Run();//アニメーション

            //攻撃対象に限りなく近づいたら
            if (GetObjectDistance(m_attackTargetObj) < 0.5f)
            {
                //噛みつく
                BiteZombie(m_attackTargetObj);
            }
        }
        else//通常時の移動
        {
            NomalMoveUpdate();
        }
    }

    /// <summary>
    /// 通常時移動
    /// 通常時の移動を決める関数
    /// </summary>
    private void NomalMoveUpdate()
    {
        //移動先座標がプレイヤーから離れているなら決めなおす
        float player_target_distance = Vector3.Distance(m_playerObj.transform.position, m_targetPos);
        if (player_target_distance > m_stayPlayerDistance)
        {
            RandomTargetPos();//移動先決め
            m_onFreezeMove = false;  //停止中でも解除
            m_isMoveTypeWalk = false;//走るようにする
        }

        if (m_onFreezeMove) return;

        //目標座標までの位置を求める
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        //プレイヤーと自身の距離
        float player_distance = Vector3.Distance(m_playerObj.transform.position, pos);
        //距離が遠い場合は指示を受け付けない
        if (player_distance <= m_stayPlayerDistance)
        {
            m_isIgnoreOrder = false;
        }
        else
        {
            m_isIgnoreOrder = true;
        }

        //ここで移動
        m_dogMove.LookAtPosition(m_targetPos);//向き変更
        //プレイヤーとの距離によって速度変更
        if (m_isMoveTypeWalk)
        {
            m_dogMove.WalkFront();
            m_dogAnimation.Walk();
        }
        else
        {
            m_dogMove.RunFront();
            m_dogAnimation.Run();
        }

        float distance = Vector3.Distance(pos, m_targetPos);
        //到着したら
        if (distance < 0.1f)
        {
            //停止
            m_dogMove.StopMove();
            m_dogAnimation.Idle();

            //停止時間をランダムに決める
            //変数は後でクラス変数にする
            float freeze_sec = UnityEngine.Random.Range(2.0f, 5.0f);

            m_onFreezeMove = true;
            //一定時間停止
            m_inActionDelays.Add(
                        DelayRunCoroutine(
                        freeze_sec,
                        () => {
                            m_onFreezeMove = false;
                            RandomTargetPos();
                            m_isMoveTypeWalk = true;
                        }
                        ));
            StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
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
    /// 攻撃指示の受け付け
    /// プレイヤー側で呼び出し、攻撃を開始する
    /// </summary>
    /// <param name="_target_obj">攻撃対象オブジェクト</param>
    public void OrderAttack(GameObject _target_obj)
    {
        //攻撃可能か調べる
        if (!CanOrderAttack()) return;
        Debug.Log("攻撃指示を受け付けた");

        m_isChargeTarget = true;//攻撃状態に
        m_attackTargetObj = _target_obj;//攻撃対象を保存

        m_dogSound.PlayAttackBark();//鳴き声
    }
    /// <summary>
    /// 探知指示の受け付け
    /// プレイヤー側で呼び出し、探知を開始する
    /// </summary>
    public void OrderDetection()
    {
        //探知が可能か調べる
        if (!CanOrderDetection()) return;
        Debug.Log("探知開始");

        //一定範囲の対象のオブジェクトをマークする
        m_targetMark.RangeMark();

        m_isChargeTarget = false;//攻撃はキャンセル      

        m_dogSound.PlayDetectBark();//鳴き声

        //一定時間クールタイム
        m_isDetectCooldown = true;
        m_inActionDelays.Add(
                        DelayRunCoroutine(
                        m_detectCooldownSec,
                        () => {
                            m_isDetectCooldown = false;
                        }
                        ));
        StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
    }
    /// <summary>
    /// 攻撃指示の受け付けが可能か
    /// 受け付け可能な場合可視化したいためUI関連のクラスでも参照
    /// </summary>
    /// <returns>行動可能 : true</returns>
    public bool CanOrderAttack()
    {
        if (m_isStopAction) return false;
        if (m_isIgnoreOrder) return false;

        return true;
    }
    /// <summary>
    /// 探知指示の受け付けが可能か
    /// 受け付け可能な場合可視化したいためUI関連のクラスでも参照
    /// </summary>
    /// <returns>行動可能 : true</returns>
    public bool CanOrderDetection()
    {
        if (m_isStopAction) return false;
        if (m_isIgnoreOrder) return false;
        if (m_isDetectCooldown) return false;

        return true;
    }
    /// <summary>
    /// 探知を受け付けたか
    /// チュートリアル用のクールタイム監視用
    /// </summary>
    public bool UsedOrderDetection()
    {
        return m_isDetectCooldown;
    }

    /// <summary>
    /// ゾンビに噛みつき
    /// 攻撃中に使用　ゾンビ側の停止関数を呼び出す
    /// </summary>
    /// <param name="_zombie_obj">噛みつくゾンビ</param>
    private void BiteZombie(GameObject _zombie_obj)
    {
        Debug.Log("ゾンビに噛みつき");

        //attackTargetObjからZombieManagerを取得
        ZombieManager zombie_manager;
        m_attackTargetObj.TryGetComponent(out zombie_manager);
        if (zombie_manager == null) return;

        zombie_manager.FreezePosition((float)m_biteStaySec);//ゾンビを停止

        m_dogAnimation.Attack(); //アニメーション再生
        m_isChargeTarget = false;//攻撃終了

        //一定時間行動停止
        m_isStopAction = true;
        m_inActionDelays.Add(
                        DelayRunCoroutine(
                        m_biteStaySec,
                        () => {
                            m_isStopAction = false;
                        }
                        ));
        StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
    }

    /// <summary>
    /// 遅延実行するコルーチン
    /// ラムダ式で指定した処理を一定時間後に実行
    /// </summary>
    /// <param name="_delay_sec">待機時間</param>
    /// <param name="_action">実行する処理</param>
    private IEnumerator DelayRunCoroutine(float _delay_sec, Action _action)
    {
        //このコルーチンの情報取得 出来ればリスト追加もここでやりたい
        IEnumerator this_cor = m_inActionDelays[m_inActionDelays.Count - 1];

        //コルーチンを再開しても待機時間情報が消えないようにする
        for (float i = 0; i < _delay_sec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);

        _action();
        //終了時にこのコルーチン情報を削除
        m_inActionDelays.Remove(this_cor);
    }

    /// <summary>
    /// 移動先座標をランダムに決める
    /// プレイヤーの周囲一定範囲内のランダム位置を目標座標に設定する
    /// </summary>
    private void RandomTargetPos()
    {
        //プレイヤーの周囲のランダム位置を求める
        Vector3 p_pos = m_playerObj.transform.position;
        //移動先位置をランダムに決める
        m_targetPos = UnityEngine.Random.insideUnitCircle * m_stayPlayerDistance;
        m_targetPos.z = m_targetPos.y;//平面上に生成するため入れ替え
        m_targetPos.y = 0.5f;//y方向は一律にする
        //アタッチしたオブジェクトを基準にする
        m_targetPos.x += p_pos.x;
        m_targetPos.z += p_pos.z;
    }

    /// <summary>
    /// 行動停止の切り替え
    /// 外部から行動停止状態を変える用
    /// </summary>
    /// <param name="_flag">停止 : true</param>
    public void OnStopAction(bool _flag)
    {
        m_isStopAction = _flag;
    }


    /// <summary>
    /// 一時停止
    /// インターフェースでの停止処理用
    /// </summary>
    public void Pause()
    {
        m_isPause = true;

        //ループ中に要素が変わらないようにクッションを噛ます
        List<IEnumerator> tmp_list = new List<IEnumerator>(m_inActionDelays);
        foreach (var cor in tmp_list)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
    }

    /// <summary>
    /// 再開
    /// インターフェースでの停止解除用
    /// </summary>
    public void Resume()
    {
        m_isPause = false;

        List<IEnumerator> tmp_list = new List<IEnumerator>(m_inActionDelays);
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
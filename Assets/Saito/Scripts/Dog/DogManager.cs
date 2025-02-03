using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//自動でアタッチされるスクリプト
[RequireComponent(typeof(DogMove))]
[RequireComponent(typeof(DogAnimation))]
[RequireComponent(typeof(DogSound))]

/// <summary>
/// <para>犬マネージャークラス</para>
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
    //追跡でどこまで近づくか
    [SerializeField] private float m_chasePlayerDistanceMin = 3.0f;
    //ワープを開始するプレイヤーとの距離
    [SerializeField] private float m_startWarpPlayerDistance = 20.0f;

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

    //ポーズ用停止フラグ
    private bool m_isPause = false;

    //動作中の遅延動作(停止、再開する用)
    List<IEnumerator> m_inActionDelays = new List<IEnumerator>();
    //名指しでコルーチンをキャンセルする用
    IEnumerator m_stayActionDelay;
    IEnumerator m_safeAreaActionDelay;

    //セーフエリアで待機する場所
    [SerializeField] private Transform m_safeAreaStayTransform;

    /// <summary>
    /// 移動ループの種類
    /// </summary>
    enum MOVE_UPDATE_TYPE
    {
        STAY,
        CHASE,
        SAFE_AREA,
        CHARGE,
        NULL
    }
    //前回実行された移動アップデート
    MOVE_UPDATE_TYPE m_prevMoveUpdateType = MOVE_UPDATE_TYPE.NULL;

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
        if (m_isStopAction || m_isPause)
        {
            m_dogMove.StopMove();//移動停止
            return;
        }

        //プレイヤーとの距離
        float player_distance = Vector3.Distance(m_playerObj.transform.position, transform.position);
        //移動ループ情報保存用
        MOVE_UPDATE_TYPE current_move_type;

        //移動タイプを決める
        if (m_isChargeTarget)
        {
            //突進
            current_move_type = MOVE_UPDATE_TYPE.CHARGE;

            //攻撃対象が存在しないならreturn
            //攻撃対象が途中でDestroyされた場合の挙動注意
            if (m_attackTargetObj == null)
            {
                m_isChargeTarget = false;//突進中断
                return;
            }

            //attackTargetObjの位置に向かって移動する
            m_dogMove.LookAtPosition(m_attackTargetObj.transform.position);
            m_dogMove.RunFront();//移動

            m_dogAnimation.Run();//アニメーション

            //攻撃対象に限りなく近づいたら
            if (GetObjectDistance(m_attackTargetObj) < 0.6f)
            {
                //噛みつく
                BiteZombie(m_attackTargetObj);
            }
        }
        else if (m_playerObj.GetComponent<player>().m_inSafeAreaFlag)
        {
            current_move_type = MOVE_UPDATE_TYPE.SAFE_AREA;
            //プレイヤーがセーフエリア内
            SafeAreaUpdate();
        }
        else if ((m_prevMoveUpdateType != MOVE_UPDATE_TYPE.CHASE && player_distance >= m_stayPlayerDistance) ||
                 (m_prevMoveUpdateType == MOVE_UPDATE_TYPE.CHASE && player_distance >= m_chasePlayerDistanceMin))
        {
            current_move_type = MOVE_UPDATE_TYPE.CHASE;
            //プレイヤーと一定以上離れている
            ChasePlayerUpdate();
        }
        else
        {
            current_move_type = MOVE_UPDATE_TYPE.STAY;
            //待機時
            StayUpdate();
        }

        //移動ループが変わったとき実行中のコルーチンをキャンセル
        if(m_prevMoveUpdateType == MOVE_UPDATE_TYPE.STAY &&
            current_move_type != MOVE_UPDATE_TYPE.STAY)
        {
            StopCoroutine(m_stayActionDelay);//停止
            m_inActionDelays.Remove(m_stayActionDelay);//再開しないようにRemove
            m_stayActionDelay = null;
        }
        else if(m_prevMoveUpdateType == MOVE_UPDATE_TYPE.SAFE_AREA &&
            current_move_type != MOVE_UPDATE_TYPE.SAFE_AREA)
        {
            StopCoroutine(m_safeAreaActionDelay);//停止
            m_inActionDelays.Remove(m_safeAreaActionDelay);//再開しないようにRemove
            m_safeAreaActionDelay = null;
        }

        m_prevMoveUpdateType = current_move_type;//移動タイプ情報保存
    }

    /// <summary>
    /// <para>セーフエリアでの行動</para>
    /// プレイヤーがセーフエリア内にいる時の挙動
    /// </summary>
    private void SafeAreaUpdate()
    {
        //連続実行でない
        if(m_prevMoveUpdateType != MOVE_UPDATE_TYPE.SAFE_AREA)
        {
            //一定時間後に所定の位置にワープ
            //セーフエリアから出たときキャンセルしないといけない lerpでもよさげ
            m_inActionDelays.Add(
                m_safeAreaActionDelay =//キャンセル用に保存
                        DelayRunCoroutine(
                        2.0f,//仮
                        () => {
                            m_dogMove.Warp(m_safeAreaStayTransform.position);//ワープさせる
                            m_dogMove.ChangeDirection(m_safeAreaStayTransform.rotation);//向きを変える
                            m_dogMove.StopMove();
                            m_dogAnimation.Idle();
                        }
                        ));
            StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
        }
        //移動先との距離
        float target_distance = Vector3.Distance(transform.position, m_safeAreaStayTransform.position);

        if (target_distance > 0.3f)
        {
            //セーフエリア所定位置に移動 この辺DogMoveにまとめた方が良さそう
            m_dogMove.LookAtPosition(m_safeAreaStayTransform.position);
            m_dogMove.RunFront();
            m_dogAnimation.Run();
        }
        else
        {
            m_dogMove.ChangeDirection(m_safeAreaStayTransform.rotation);
            m_dogMove.StopMove();
            m_dogAnimation.Idle();
        }
    }

    /// <summary>
    /// <para>プレイヤー追跡ループ</para>
    /// プレイヤーを追うように移動する 離れすぎたらワープする
    /// </summary>
    private void ChasePlayerUpdate()
    {
        //プレイヤーとの距離
        float player_distance = Vector3.Distance(m_playerObj.transform.position, transform.position);
        if(player_distance >= m_startWarpPlayerDistance)//プレイヤーとの距離が一定以上
        {
            Vector3 player_behind_pos = m_playerObj.transform.position - m_playerObj.transform.forward * -1.0f;

            m_dogMove.Warp(player_behind_pos);//ワープさせる
        }
        //目標地点設定
        m_targetPos = m_playerObj.transform.position;
      
        m_dogMove.LookAtPosition(m_targetPos);//向き変更
        m_dogMove.RunFront();//移動
        m_dogAnimation.Run();//アニメーション設定
    }

    /// <summary>
    /// <para>待機時ループ</para>
    /// 平常時の行動　基本停止でたまにプレイヤーの周囲をうろつく
    /// </summary>
    private void StayUpdate()
    {
        //連続実行でない
        if(m_prevMoveUpdateType != MOVE_UPDATE_TYPE.STAY)
        {
            m_targetPos = transform.position;
            m_onFreezeMove = false;
        }

        if (m_onFreezeMove) return;

        //目標地点に到着
        if (Vector3.Distance(m_targetPos, transform.position) <= 1.0f)
        {
            //移動停止
            m_dogMove.StopMove();
            m_dogAnimation.Idle();
            //停止時間をランダムに決める
            float freeze_sec = UnityEngine.Random.Range(2.0f, 5.0f);
            //一定時間停止 別のループ時キャンセルしたい
            m_onFreezeMove = true;
            m_inActionDelays.Add(
                m_stayActionDelay =//キャンセル出来るように保存
                        DelayRunCoroutine(
                        freeze_sec,
                        () => {
                            m_onFreezeMove = false;
                            RandomTargetPos();//移動先決め
                        }
                        ));
            StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
        }
        else
        {
            m_dogMove.LookAtPosition(m_targetPos);//向き変更
            m_dogMove.WalkFront();//移動
            m_dogAnimation.Walk();
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
    /// <para>攻撃指示の受け付け</para>
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
    /// <para>探知指示の受け付け</para>
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
    /// <para>攻撃指示の受け付けが可能か</para>
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
    /// <para>探知指示の受け付けが可能か</para>
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
    /// <para>探知を受け付けたか</para>
    /// チュートリアル用のクールタイム監視用
    /// </summary>
    public bool UsedOrderDetection()
    {
        return m_isDetectCooldown;
    }

    /// <summary>
    /// <para>ゾンビに噛みつき</para>
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
    /// <para>遅延実行するコルーチン</para>
    /// ラムダ式で指定した処理を一定時間後に実行 一時停止可
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
    /// <para>移動先座標をランダムに決める</para>
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
    /// <para>行動停止の切り替え</para>
    /// 外部から行動停止状態を変える用
    /// </summary>
    /// <param name="_flag">停止 : true</param>
    public void OnStopAction(bool _flag)
    {
        m_isStopAction = _flag;
    }


    /// <summary>
    /// <para>一時停止</para>
    /// インターフェースでの停止処理用
    /// </summary>
    public void Pause()
    {
        m_isPause = true;

        m_dogMove.StopMove();
        m_dogAnimation.Pause();

        //ループ中に要素が変わらないようにクッションを噛ます
        List<IEnumerator> tmp_list = new List<IEnumerator>(m_inActionDelays);
        foreach (var cor in tmp_list)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
    }

    /// <summary>
    /// <para>再開</para>
    /// インターフェースでの停止解除用
    /// </summary>
    public void Resume()
    {
        m_isPause = false;

        m_dogAnimation.Resume();

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

/
攻撃処理

プレイヤーがセーフエリア内
　何秒かで指定位置に向かう
　上が実行し終わったら指定位置、指定向きでワープし座る

プレイヤーが範囲内
　基本止まる
　一定時間この状態が続いたら、範囲内を自由に動く

プレイヤーが範囲外
　プレイヤーに向かう
　もし離れすぎたら
　　プレイヤーの背後にワープ
/
 */
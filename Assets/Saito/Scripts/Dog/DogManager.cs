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
    private DogBase[] m_dogBases;

    private DogMove m_dogMove;
    private DogAnimation m_dogAnimation;
    private DogSound m_dogSound;

    private TargetMark m_targetMark;

    [SerializeField]//噛みついている時間
    private float m_biteStaySec = 4.0f;

    [SerializeField]//待機状態になるプレイヤーとの距離
    private float m_stayPlayerDistance = 5.0f;

    [SerializeField]//探知のクールタイム
    private float m_detectCooldownSec = 60.0f;


    //攻撃対象オブジェクト
    private GameObject m_attackTargetObj;
    //プレイヤー
    private GameObject m_playerObj;

    //移動目標座標
    private Vector3 m_targetPos;

    //移動停止フラグ
    private bool m_onFreezeMove = false;

    //行動停止
    [SerializeField]
    private bool m_isStopAction = false;
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


    private void Awake()
    {
        //プレイヤー取得
        m_playerObj = GameObject.FindGameObjectWithTag("Player");

        m_targetMark = gameObject.GetComponent<TargetMark>();
    }

    // Start is called before the first frame update
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

        RandomTargetPos();
    }


    // Update is called once per frame
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
            NomalUpdate();
        }
    }

    //通常行動用の仮関数
    private void NomalUpdate()
    {
        //移動先座標がプレイヤーから離れているなら決めなおす
        float player_target_distance = Vector3.Distance(m_playerObj.transform.position, m_targetPos);
        if (player_target_distance > m_stayPlayerDistance)
        {
            RandomTargetPos();
            m_onFreezeMove = false;//停止中でも解除
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
    /// 攻撃指示を受けたとき
    /// </summary>
    public void OrderAttack(GameObject _obj)//zombieの子のパーツが渡されたとき動かない可能性アリ
    {
        if (!CanOrderAttack()) return;
        Debug.Log("攻撃指示を受け付けた");

        m_isChargeTarget = true;

        m_attackTargetObj = _obj;//攻撃対象を取得

        m_dogSound.PlayAttackBark();//鳴き声
    }
    /// <summary>
    /// 指示：周囲の探知
    /// </summary>
    public void OrderDetection()
    {
        if (!CanOrderDetection()) return;
        Debug.Log("探知開始");

        //一定範囲の対象のオブジェクトをマーク
        m_targetMark.RangeMark();

        m_dogSound.PlayDetectBark();//鳴き声

        m_isChargeTarget = false;//攻撃はキャンセル

        //クールタイム
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
    //攻撃指示可能か
    public bool CanOrderAttack()
    {
        if (m_isStopAction) return false;
        if (m_isIgnoreOrder) return false;

        return true;
    }
    //攻撃指示可能か
    public bool CanOrderDetection()
    {
        if (m_isStopAction) return false;
        if (m_isIgnoreOrder) return false;
        if (m_isDetectCooldown) return false;

        return true;
    }

    //チュートリアル用のクールタイム監視用
    public bool UsedOrderDetection()
    {
        return m_isDetectCooldown;
    }

    /// <summary>
    /// ゾンビに噛みつく
    /// </summary>
    private void BiteZombie(GameObject _zombie_obj)
    {
        Debug.Log("ゾンビに噛みつき");

        ZombieManager zombie_manager;
        //attackTargetObjからZombieManagerを取得し、FreezePositionを呼び出し
        m_attackTargetObj.TryGetComponent(out zombie_manager);
        if (zombie_manager == null) return;

        zombie_manager.FreezePosition((float)m_biteStaySec);//ゾンビを停止

        m_dogAnimation.Attack();
        m_isStopAction = true;
        m_isChargeTarget = false;

        //一定時間停止
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
    /// 遅らせてActionを実行するコルーチン
    /// </summary>
    private IEnumerator DelayRunCoroutine(float _wait_sec, Action _action)
    {
        //このコルーチンの情報取得 出来ればリスト追加もここでやりたい
        IEnumerator this_cor = m_inActionDelays[m_inActionDelays.Count - 1];

        //コルーチンを再開しても待機時間情報が消えないようにする
        for (float i = 0; i < _wait_sec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);

        _action();
        //終了時にこのコルーチン情報を削除
        m_inActionDelays.Remove(this_cor);
    }



    //プレイヤー一定範囲のランダム位置を目標座標に設定する
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

    //外部から行動の停止を変える用
    public void OnStopAction(bool _flag)
    {
        m_isStopAction = _flag;
    }


    //インターフェースでの停止処理用
    //一時停止
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
    //再開
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
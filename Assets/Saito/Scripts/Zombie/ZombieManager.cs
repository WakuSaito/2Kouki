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
[RequireComponent(typeof(ZombieHP))]

//ゾンビ関連のクラスをnameofでまとめてもいいかも


/// <summary>
/// <para>ゾンビの管理クラス</para>
/// ZombieBaseを継承したクラスを扱う
/// </summary>
public class ZombieManager : MonoBehaviour, IStopObject
{
    /// <summary>
    /// 操作するクラス
    /// </summary>
    [SerializeField] private ZombieBase[] m_zombieBases;

    private ZombieMove m_zombieMove;
    private ZombieAttack m_zombieAttack;
    private ZombieAnimation m_zombieAnimation;
    private ZombieAction m_zombieAction;
    private ZombieHP m_zombieHP;
    private ZombieSound m_zombieSound;

    GameObject m_playerObj;

    //プレイヤーの探知範囲
    [SerializeField] float m_detectionPlayerRangeMin = 10.0f;//最小
    [SerializeField] float m_detectionPlayerRangeMax = 30.0f;//最大

    //現在のプレイヤー探知範囲
    private float m_currentDetectionRange;

    //攻撃開始距離
    [SerializeField] float m_attackStartRange = 2.0f;

    //このオブジェクトを削除するプレイヤーとの距離
    //スポナーの最有効化と同じくらいの距離にする
    [SerializeField] float m_despawnPlayerDistance = 100.0f;

    //攻撃対象を発見している
    private bool m_isFoundTarget = false;
    //ランダムに向きを変えるクールタイム中
    private bool m_isChangeDirCoolDown = false;
    //移動不可フラグ
    private bool m_isFreezePos = false;
    //死亡済フラグ
    private bool m_isDead = false;
    //スタンフラグ
    private bool m_isStan = false;
    //一時停止
    private bool m_isStop = false;

    //チュートリアル用オブジェクトか
    [SerializeField] private bool m_isTutorialObj = false;

    //スタン処理キャンセル用トークン
    private IEnumerator m_stanCoroutine;

    //Meshがアタッチされたオブジェクト
    [SerializeField] GameObject m_meshObj;

    //動作中の遅延動作
    List<IEnumerator> m_inActionDelays = new List<IEnumerator>();

    //オブジェクトの取得
    private void Awake()
    {
        //プレイヤーオブジェクト取得
        m_playerObj = GameObject.FindGameObjectWithTag("Player");

        m_currentDetectionRange = m_detectionPlayerRangeMin;
    }

    //ゾンビベースを継承したクラスの取得と初期設定
    void Start()
    {
        foreach(var zombie in m_zombieBases)
        {
            Debug.Log(zombie);

            //各クラスでオーバーライドした初期設定実行
            zombie.SetUpZombie();

            //下記クラスに該当するか確認し代入
            //どうやら違うクラスでTryGetComponentするとnullが代入されるっぽいのでnullチェック
            if (m_zombieMove == null) zombie.TryGetComponent(out m_zombieMove);
            if (m_zombieAttack == null) zombie.TryGetComponent(out m_zombieAttack);
            if (m_zombieAnimation == null) zombie.TryGetComponent(out m_zombieAnimation);
            if (m_zombieAction == null) zombie.TryGetComponent(out m_zombieAction);
            if (m_zombieHP == null) zombie.TryGetComponent(out m_zombieHP);
            if (m_zombieSound == null) zombie.TryGetComponent(out m_zombieSound);
        }
        
        Debug.Log("ゾンビ初期体力:"+ m_zombieHP.GetCurrentHP());

        if (m_isTutorialObj)
            m_zombieAnimation.Idle();
    }


    //行動パターンを決めるメインループ
    void Update()
    {
        if (m_isStop) return;
        if (m_playerObj == null) return;

        //死亡チェック
        if (m_zombieHP.IsDead())
            Dead();

        if (m_isTutorialObj)
        {
            Attack();//攻撃のみ繰り返す
            return;
        }

        if (m_isDead) return;//死亡済なら動かさない
        if (m_isStan) return;//スタン時は動かさない

        //座標取得
        Vector3 pos = transform.position;
        Vector3 player_pos = m_playerObj.transform.position;
        pos.y = 0; player_pos.y = 0;//仮でy座標を無視
        //プレイヤーとの距離計算
        float player_distance = Vector3.Distance(pos, player_pos);

        //プレイヤーから離れすぎたら削除
        if (player_distance > m_despawnPlayerDistance)
        {
            Destroy(gameObject);

            return;
        }


        ChangeDetectRange();//探知範囲計算


        //攻撃対象を見つけているか
        if (player_distance < m_currentDetectionRange)
        {
            m_isFoundTarget = true;//発見
        }
        else
        {
            m_isFoundTarget = false;
        }

        //移動
        {
            if (m_isFreezePos || m_zombieAttack.m_isAttack)//停止
            {
                //プレイヤーの方を向く
                m_zombieMove.LookAtPosition(player_pos);

                m_zombieMove.StopMove();
                m_zombieAnimation.Idle();//停止モーション
            }
            else if (m_isFoundTarget)
            {
                //プレイヤーの方を向く
                m_zombieMove.LookAtPosition(player_pos);

                if (player_distance < m_attackStartRange)
                {
                    //とりあえず近づきすぎないようにした
                    m_zombieMove.StopMove();
                    m_zombieAnimation.Idle();//停止モーション

                    Attack();//攻撃
                }
                else
                {
                    //走る
                    m_zombieMove.RunFront();
                    m_zombieAnimation.Run();//移動モーション
                }

            }
            else//通常の行動
            {
                //向き変更
                if (!m_isChangeDirCoolDown)
                {
                    m_isChangeDirCoolDown = true;//クールタイム中に
                    m_inActionDelays.Add(
                        DelayRunCoroutine(
                        UnityEngine.Random.Range(4.0f, 8.0f),//次に向きを変えるまでの時間を決める
                        () => m_isChangeDirCoolDown = false  //フラグオフ
                        ));
                    StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);

                    //ランダムに向きを設定
                    Vector3 direction = new Vector3(0, UnityEngine.Random.Range(-180, 180), 0);
                    //向きを変更
                    m_zombieMove.ChangeDirection(Quaternion.Euler(direction));
                }

                //歩く
                m_zombieMove.WalkFront();
                m_zombieAnimation.Walk();//移動モーション
            }

        }
    }
    /// <summary>
    /// <para>攻撃</para>
    /// フラグチェックと関数呼び出し
    /// </summary>
    private void Attack()
    {
        if (m_zombieAttack.m_isAttack) return;//クールタイムチェック
        if (m_isDead) return;

        //攻撃開始
        m_zombieAttack.StartAttack();
        //攻撃モーション再生
        m_zombieAnimation.Attack();
    }

    /// <summary>
    /// 探知範囲の変更
    /// プレイヤーの体力が低いほどプレイヤーを見つけやすくなる
    /// </summary>
    private void ChangeDetectRange()
    {
        //プレイヤーのスクリプト取得
        player player_script = m_playerObj.GetComponent<player>();
        if (player_script == null) return;

        //プレイヤーの体力取得
        float max_hp = player_script.hp_num_max;
        float current_hp = player_script.hp_num_now;

        //現在の体力の割合取得
        float current_hp_per = current_hp / max_hp;

        //現在の体力割合から探知範囲を補間で計算
        m_currentDetectionRange = m_detectionPlayerRangeMin * current_hp_per +
            m_detectionPlayerRangeMax * (1.0f - current_hp_per);
    }

    /// <summary>
    /// <para>体にダメージを受ける</para>
    /// ダメージ計算とアニメーション等の再生
    /// </summary>
    /// <param name="_hit_pos">被弾地点 (アニメーション用)</param>
    /// <param name="_damage">ダメージ量</param>
    public void DamageBody(Vector3 _hit_pos, int _damage)
    {
        Debug.Log("Body");

        m_zombieAttack.AttackCancel();//攻撃処理のキャンセル

        Vector3 vec = _hit_pos - transform.position;

        Vector3 axis = Vector3.Cross(transform.forward, vec);

        if (axis.y < 0)
        {
            Debug.Log("左側");
            //zombieAnimation.DamageHitLeft();
        }
        else
        {
            Debug.Log("右側");
            //zombieAnimation.DamageHitRight();
        }
        //エフェクト表示
        m_zombieAnimation.DamagedEffect(_hit_pos);

        m_zombieHP.Damage(_damage);//ダメージ

        Stan(0.1f);//スタン
    }
    /// <summary>
    /// </para>頭にダメージを受けた</para>
    /// ダメージ計算とアニメーション等再生
    /// </summary>
    /// <param name="_hit_pos">被弾地点 (アニメーション用)</param>
    /// <param name="_damage">ダメージ量</param>
    public void DamageHead(Vector3 _hit_pos, int _damage)
    {
        Debug.Log("Head");

        m_zombieAttack.AttackCancel();//攻撃処理のキャンセル

        m_zombieHP.Damage(_damage * 2);//ダメージ

        //アニメーション
        m_zombieAnimation.DamageHitRight();
        //エフェクト表示
        m_zombieAnimation.DamagedEffect(_hit_pos);

        Stan(0.3f);//スタン
    }

    /// <summary>
    /// <para>スタン</para>
    /// 一定時間行動不可になる
    /// </summary>
    /// <param name="_sec">スタンする秒数</param>
    private void Stan(float _sec)
    {
        if (m_isDead) return;

        //現在動作中のスタンコルーチンをキャンセル
        if (m_isStan && m_stanCoroutine != null)
        {
            StopCoroutine(m_stanCoroutine);
            m_inActionDelays.Remove(m_stanCoroutine);
            m_stanCoroutine = null;
        }

        m_zombieAttack.AttackCancel();//攻撃処理のキャンセル

        m_isStan = true;

        //移動ベクトルをゼロにする
        m_zombieMove.StopMove();
        m_zombieAnimation.Idle();//停止モーション


        m_inActionDelays.Add(
            DelayRunCoroutine(
            _sec,
            () => m_isStan = false
            ));
        m_stanCoroutine = m_inActionDelays[m_inActionDelays.Count - 1];
        StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
        
    }


    /// <summary>
    /// <para>死亡処理</para>
    /// 当たり判定を消し、アニメーション再生
    /// </summary>
    private void Dead()
    {
        if (m_isDead) return;

        m_isDead = true;//別の動作を止めるためにフラグオン
        m_isFreezePos = true;//移動停止

        m_zombieAnimation.Die();//アニメーション
        m_zombieSound.PlayDead();//サウンド
        GetComponent<Rigidbody>().velocity = Vector3.zero;//動きを止める

        EnableCollider();//コライダー無効化

        //アニメーションが終わるころにオブジェクトを消す
        m_inActionDelays.Add(
            DelayRunCoroutine(
                    2.5f,//後で定数化したい
                    () => m_zombieAction.Dead()//死亡
                    ));
        StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
    }
    /// <summary>
    /// コライダー無効化
    /// </summary>
    private void EnableCollider()
    {
        //全ての子オブジェクトのコライダー取得
        Collider[] colliders = transform.GetComponentsInChildren<Collider>();

        foreach(var col in colliders)
        {
            col.enabled = false;//無効化
        }

        GetComponent<Rigidbody>().useGravity = false;
    }

    /// <summary>
    /// </para>移動不可状態にする</para>
    /// 犬側で呼び出す
    /// </summary>
    /// <param name="_sec">停止する秒数</param>
    public void FreezePosition(float _sec)
    {
        //移動停止フラグオン
        m_isFreezePos = true;
        //しばらくしたらオフにする
        m_inActionDelays.Add(
            DelayRunCoroutine(
                    _sec,
                    () => m_isFreezePos = false
                    ));
        StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
    }


    /// <summary>
    /// 遅らせてActionを実行するコルーチン
    /// キャンセル可
    /// </summary>
    /// <param name="_wait_sec">遅延秒数</param>
    /// <param name="_action">遅延後実行する処理</param>
    private IEnumerator DelayRunCoroutine(float _wait_sec, Action _action)
    {
        //このコルーチンの情報取得 出来ればリスト追加もここでやりたい
        IEnumerator this_coroutine = m_inActionDelays[m_inActionDelays.Count - 1];

        //コルーチンを再開しても待機時間情報が消えないようにする
        for (float i = 0; i < _wait_sec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);

        _action();
        //終了時にこのコルーチン情報を削除
        m_inActionDelays.Remove(this_coroutine);
    }

    /// <summary>
    /// <para>一時停止</para>
    /// インターフェースでの停止処理用
    /// </summary>
    public void Pause()
    {
        m_isStop = true;

        m_zombieAttack.Pause();
        m_zombieMove.StopMove();
        m_zombieAnimation.Pause();

        //ループ中に要素が変わらないようにクッションを噛ます
        List<IEnumerator> tmp_list = new List<IEnumerator>(m_inActionDelays);
        foreach(var cor in tmp_list)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
        
    }
    /// <summary>
    /// <para>再開</para>
    /// インターフェースでの停止解除処理用
    /// </summary>
    public void Resume()
    {
        m_isStop = false;

        m_zombieAttack.Resume();
        m_zombieAnimation.Resume();

        List<IEnumerator> tmp_list = new List<IEnumerator>(m_inActionDelays);
        foreach (var cor in tmp_list)
        {
            if (cor == null) continue;

            StartCoroutine(cor);
        }
    }

}

/*
ゾンビの行動

①プレイヤーを発見したとき
②プレイヤーへ移動
③至近距離まできたら攻撃
④プレイヤーから少し離れているなら、②へ
　プレイヤーからとても離れているなら、通常の行動に

 */
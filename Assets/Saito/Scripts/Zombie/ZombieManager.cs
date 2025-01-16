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
/// ゾンビの管理クラス
/// ZombieBaseを継承したクラスを扱う
/// </summary>
public class ZombieManager : MonoBehaviour, IStopObject
{
    /// <summary>
    /// 操作するクラス
    /// </summary>
    [SerializeField]
    private ZombieBase[] m_zombieBases;

    private ZombieMove m_zombieMove;
    private ZombieAttack m_zombieAttack;
    private ZombieAnimation m_zombieAnimation;
    private ZombieAction m_zombieAction;
    private ZombieHP m_zombieHP;
    private ZombieSound m_zombieSound;

    GameObject m_playerObj;

    [SerializeField]//プレイヤーの探知範囲
    float m_detectionPlayerRangeMin = 10.0f;
    [SerializeField]
    float m_detectionPlayerRangeMax = 30.0f;

    //現在のプレイヤー探知範囲
    private float m_currentDetectionRange;

    [SerializeField]//攻撃開始距離
    float m_attackStartRange = 2.0f;

    [SerializeField]//このオブジェクトを削除するプレイヤーとの距離
    float m_despawnPlayerDistance = 120.0f;

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

    [SerializeField] //チュートリアル用か
    private bool m_isTutorialObj = false;

    //スタン処理キャンセル用トークン
    private IEnumerator m_stanCoroutine;

    [SerializeField]//Meshがアタッチされたオブジェクト
    GameObject m_meshObj;
    //現在の色のアルファ値
    private float m_currentAlpha;

    //動作中の遅延動作
    List<IEnumerator> m_inActionDelays = new List<IEnumerator>();

    private void Awake()
    {
        //プレイヤーオブジェクト取得
        m_playerObj = GameObject.FindGameObjectWithTag("Player");

        //カラーのアルファ値取得
        m_currentAlpha = m_meshObj.GetComponent<Renderer>().materials[1].color.a;

        m_currentDetectionRange = m_detectionPlayerRangeMin;

    }

    // Start is called before the first frame update
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


    // Update is called once per frame
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

        //プレイヤーから離れすぎたら動かさない
        if (player_distance > m_despawnPlayerDistance)
        {
            m_zombieMove.StopMove();
            m_zombieAnimation.Idle();//停止モーション
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
    //攻撃
    private void Attack()
    {
        if (m_zombieAttack.m_isAttack) return;//クールタイムチェック
        if (m_isDead) return;

        //攻撃開始
        m_zombieAttack.StartAttack();
        //攻撃モーション再生
        m_zombieAnimation.Attack();
    }

    //探知範囲変更
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
    /// 体にダメージを受けた
    /// </summary>
    //被弾地点からアニメーションを変更させる用
    public void DamageBody(Vector3 _hit_pos, int _damage)
    {
        Debug.Log("Body");

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
    /// 頭にダメージを受けた
    /// </summary>
    public void DamageHead(int _damage)
    {
        Debug.Log("Head");

        m_zombieAttack.AttackCancel();//攻撃処理のキャンセル

        m_zombieHP.Damage(_damage * 2);//ダメージ

        m_zombieAnimation.DamageHitRight();

        Stan(0.3f);//スタン
    }
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

    //一定時間スタン
    private void Stan(float _sec)
    {
        if (m_isDead) return;

        if (m_isStan && m_stanCoroutine != null)
        {
            StopCoroutine(m_stanCoroutine);
            m_inActionDelays.Remove(m_stanCoroutine);
            m_stanCoroutine = null;
        }
        // stanCancellTokenSource.Cancel();//現在動いているスタン処理のキャンセル

        m_zombieAttack.AttackCancel();//攻撃処理のキャンセル

        //stanCancellTokenSource = new CancellationTokenSource();

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
    /// 死亡処理
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
    //コライダー無効化
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
    /// 移動不可状態にする
    /// (停止する時間)
    /// 犬側で呼び出してね
    /// </summary>
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

    //色のアルファ値変更
    public void ChangeColorAlpha(float _alpha)
    {
        //色が変わらない場合処理を行わないようにする
        if (m_currentAlpha == _alpha) return;
        m_currentAlpha = _alpha;

        Color current_color = m_meshObj.GetComponent<Renderer>().materials[1].color;
        m_meshObj.GetComponent<Renderer>().materials[1].color = new Color(current_color.r, current_color.g, current_color.b,_alpha);
    }

    /// <summary>
    /// 遅らせてActionを実行するコルーチン
    /// </summary>
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

    //インターフェースでの停止処理用
    //一時停止
    public void Pause()
    {
        m_isStop = true;

        m_zombieAttack.Pause();

        //ループ中に要素が変わらないようにクッションを噛ます
        List<IEnumerator> tmp_list = new List<IEnumerator>(m_inActionDelays);
        foreach(var cor in tmp_list)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
        
    }
    //再開
    public void Resume()
    {
        m_isStop = false;

        m_zombieAttack.Resume();

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
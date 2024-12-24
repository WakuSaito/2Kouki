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
    private ZombieBase[] zombieBases;

    private ZombieMove zombieMove;
    private ZombieAttack zombieAttack;
    private ZombieAnimation zombieAnimation;
    private ZombieAction zombieAction;
    private ZombieHP zombieHP;
    private ZombieSound zombieSound;

    GameObject playerObj;

    [SerializeField]//プレイヤーの探知範囲
    float detectionPlayerRangeMin = 10.0f;
    [SerializeField]
    float detectionPlayerRangeMax = 30.0f;

    //現在のプレイヤー探知範囲
    private float currentDetectionRange;

    [SerializeField]//攻撃開始距離
    float attackStartRange = 2.0f;

    [SerializeField]//このオブジェクトを削除するプレイヤーとの距離
    float despawnPlayerDistance = 120.0f;

    //攻撃対象を発見している
    private bool isFoundTarget = false;
    //ランダムに向きを変えるクールタイム中
    private bool isChangeDirCoolDown = false;
    //移動不可フラグ
    private bool isFreezePos = false;
    //死亡済フラグ
    private bool isDead = false;
    //スタンフラグ
    private bool isStan = false;
    //一時停止
    private bool isStop = false;

    [SerializeField] //チュートリアル用か
    private bool isTutorialObj = false;

    //スタン処理キャンセル用トークン
    private IEnumerator stanCoroutine;

    [SerializeField]//Meshがアタッチされたオブジェクト
    GameObject meshObj;
    //現在の色のアルファ値
    private float currentAlpha;

    //動作中の遅延動作
    List<IEnumerator> inActionDelays = new List<IEnumerator>();

    private void Awake()
    {
        //プレイヤーオブジェクト取得
        playerObj = GameObject.FindGameObjectWithTag("Player");

        //カラーのアルファ値取得
        currentAlpha = meshObj.GetComponent<Renderer>().materials[1].color.a;

        currentDetectionRange = detectionPlayerRangeMin;

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
            if (zombieHP == null) zombie.TryGetComponent(out zombieHP);
            if (zombieSound == null) zombie.TryGetComponent(out zombieSound);
        }
        
        Debug.Log("ゾンビ初期体力:"+zombieHP.GetCurrentHP());

        if (isTutorialObj)
            zombieAnimation.Idle();
    }


    // Update is called once per frame
    void Update()
    {
        if (isStop) return;
        if (playerObj == null) return;

        //死亡チェック
        if (zombieHP.IsDead())
            Dead();

        if (isTutorialObj)
        {
            Attack();//攻撃のみ繰り返す
            return;
        }

        if (isDead) return;//死亡済なら動かさない
        if (isStan) return;//スタン時は動かさない

        //座標取得
        Vector3 pos = transform.position;
        Vector3 playerPos = playerObj.transform.position;
        pos.y = 0; playerPos.y = 0;//仮でy座標を無視
        //プレイヤーとの距離計算
        float playerDistance = Vector3.Distance(pos, playerPos);

        //プレイヤーから離れすぎたら動かさない
        if (playerDistance > despawnPlayerDistance)
        {
            zombieMove.StopMove();
            zombieAnimation.Idle();//停止モーション
            return;
        }


        ChangeDetectRange();//探知範囲計算


        //攻撃対象を見つけているか
        if (playerDistance < currentDetectionRange)
        {
            isFoundTarget = true;//発見
        }
        else
        {
            isFoundTarget = false;
        }

        //移動
        {
            if (isFreezePos || zombieAttack.IsAttack)//停止
            {
                //プレイヤーの方を向く
                zombieMove.LookAtPosition(playerPos);

                zombieMove.StopMove();
                zombieAnimation.Idle();//停止モーション
            }
            else if (isFoundTarget)
            {
                //プレイヤーの方を向く
                zombieMove.LookAtPosition(playerPos);

                if (playerDistance < attackStartRange)
                {
                    //とりあえず近づきすぎないようにした
                    zombieMove.StopMove();
                    zombieAnimation.Idle();//停止モーション

                    Attack();//攻撃
                }
                else
                {
                    //走る
                    zombieMove.RunFront();
                    zombieAnimation.Run();//移動モーション
                }

            }
            else//通常の行動
            {
                //向き変更
                if (!isChangeDirCoolDown)
                {
                    isChangeDirCoolDown = true;//クールタイム中に
                    inActionDelays.Add(
                        DelayRunCoroutine(
                        UnityEngine.Random.Range(4.0f, 8.0f),//次に向きを変えるまでの時間を決める
                        () => isChangeDirCoolDown = false  //フラグオフ
                        ));
                    StartCoroutine(inActionDelays[inActionDelays.Count - 1]);

                    //ランダムに向きを設定
                    Vector3 direction = new Vector3(0, UnityEngine.Random.Range(-180, 180), 0);
                    //向きを変更
                    zombieMove.ChangeDirection(Quaternion.Euler(direction));
                }

                //歩く
                zombieMove.WalkFront();
                zombieAnimation.Walk();//移動モーション
            }

        }
    }
    //攻撃
    private void Attack()
    {
        if (zombieAttack.IsAttack) return;//クールタイムチェック
        if (isDead) return;

        //攻撃開始
        zombieAttack.StartAttack();
        //攻撃モーション再生
        zombieAnimation.Attack();
    }

    //探知範囲変更
    private void ChangeDetectRange()
    {
        //プレイヤーのスクリプト取得
        player playerScript = playerObj.GetComponent<player>();
        if (playerScript == null) return;

        //プレイヤーの体力取得
        float maxHP = playerScript.hp_num_max;
        float currentHP = playerScript.hp_num_now;

        //現在の体力の割合取得
        float currentHPPer = currentHP / maxHP;

        //現在の体力割合から探知範囲を補間で計算
        currentDetectionRange = detectionPlayerRangeMin * currentHPPer + 
            detectionPlayerRangeMax * (1.0f - currentHPPer);
    }

    /// <summary>
    /// 体にダメージを受けた
    /// </summary>
    //被弾地点からアニメーションを変更させる用
    public void DamageBody(Vector3 _hitPos, int _damage)
    {
        Debug.Log("Body");

        Vector3 vec = _hitPos - transform.position;

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
        zombieAnimation.DamagedEffect(_hitPos);

        zombieHP.Damage(_damage);//ダメージ

        Stan(0.1f);//スタン
    }
    /// <summary>
    /// 頭にダメージを受けた
    /// </summary>
    public void DamageHead(int _damage)
    {
        Debug.Log("Head");

        zombieAttack.AttackCancel();//攻撃処理のキャンセル

        zombieHP.Damage(_damage * 2);//ダメージ

        zombieAnimation.DamageHitRight();

        Stan(0.3f);//スタン
    }
    public void DamageHead(Vector3 _hitPos, int _damage)
    {
        Debug.Log("Head");
        
        zombieAttack.AttackCancel();//攻撃処理のキャンセル

        zombieHP.Damage(_damage * 2);//ダメージ

        //アニメーション
        zombieAnimation.DamageHitRight();
        //エフェクト表示
        zombieAnimation.DamagedEffect(_hitPos);

        Stan(0.3f);//スタン
    }

    //一定時間スタン
    private void Stan(float _sec)
    {
        if (isDead) return;

        if (isStan && stanCoroutine != null)
        {
            StopCoroutine(stanCoroutine);
            inActionDelays.Remove(stanCoroutine);
            stanCoroutine = null;
        }
           // stanCancellTokenSource.Cancel();//現在動いているスタン処理のキャンセル

        zombieAttack.AttackCancel();//攻撃処理のキャンセル

        //stanCancellTokenSource = new CancellationTokenSource();

        isStan = true;

        //移動ベクトルをゼロにする
        zombieMove.StopMove();
        zombieAnimation.Idle();//停止モーション


        inActionDelays.Add(
            DelayRunCoroutine(
            _sec,
            () => isStan = false
            ));
        stanCoroutine = inActionDelays[inActionDelays.Count - 1];
        StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
        
    }


    /// <summary>
    /// 死亡処理
    /// </summary>
    private void Dead()
    {
        if (isDead) return;

        isDead = true;//別の動作を止めるためにフラグオン
        isFreezePos = true;//移動停止

        zombieAnimation.Die();//アニメーション
        zombieSound.PlayDead();//サウンド
        GetComponent<Rigidbody>().velocity = Vector3.zero;//動きを止める

        EnableCollider();//コライダー無効化

        //アニメーションが終わるころにオブジェクトを消す
        inActionDelays.Add(
            DelayRunCoroutine(
                    2.5f,//後で定数化したい
                    () => zombieAction.Dead()//死亡
                    ));
        StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
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
        isFreezePos = true;
        //しばらくしたらオフにする
        inActionDelays.Add(
            DelayRunCoroutine(
                    _sec,
                    () => isFreezePos = false
                    ));
        StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
    }

    //色のアルファ値変更
    public void ChangeColorAlpha(float _alpha)
    {
        //色が変わらない場合処理を行わないようにする
        if (currentAlpha == _alpha) return;
        currentAlpha = _alpha;

        Color currentColor = meshObj.GetComponent<Renderer>().materials[1].color;
        meshObj.GetComponent<Renderer>().materials[1].color = new Color(currentColor.r,currentColor.g,currentColor.b,_alpha);
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

    //インターフェースでの停止処理用
    //一時停止
    public void Pause()
    {
        isStop = true;

        zombieAttack.Pause();

        //ループ中に要素が変わらないようにクッションを噛ます
        List<IEnumerator> tmpList = new List<IEnumerator>(inActionDelays);
        foreach(var cor in tmpList)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
        
    }
    //再開
    public void Resume()
    {
        isStop = false;

        zombieAttack.Resume();

        List<IEnumerator> tmpList = new List<IEnumerator>(inActionDelays);
        foreach (var cor in tmpList)
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
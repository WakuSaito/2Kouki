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
public class ZombieManager : MonoBehaviour
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

    GameObject playerObj;

    [SerializeField]//プレイヤーの探知範囲
    float detectionPlayerRangeMin = 10.0f;
    [SerializeField]
    float detectionPlayerRangeMax = 30.0f;

    //現在のプレイヤー探知範囲
    private float currentDetectionRange;

    [SerializeField]//探知範囲可視化用
    GameObject debugDetectionCirclePrefab;

    GameObject debugDetectionCircle;

    [SerializeField]//攻撃開始距離
    float attackStartRange = 3.0f;

    [SerializeField]//このオブジェクトを削除するプレイヤーとの距離
    float despawnPlayerDistance = 120.0f;

    //攻撃対象を発見している
    private bool isFoundTarget = false;
    //攻撃のクールタイム中
    private bool isAttackCoolDown = false;
    //ランダムに向きを変えるクールタイム中
    private bool isChangeDirCoolDown = false;
    //移動不可フラグ
    private bool isFreezePos = false;
    //死亡済フラグ
    private bool isDead = false;
    //スタンフラグ
    private bool isStan = false;

    //スタン処理キャンセル用トークン
    private CancellationTokenSource stanCancellTokenSource = new CancellationTokenSource();

    [SerializeField]//Meshがアタッチされたオブジェクト
    GameObject meshObj;
    //現在の色のアルファ値
    private float currentAlpha;

    private void Awake()
    {
        //プレイヤーオブジェクト取得
        playerObj = GameObject.FindGameObjectWithTag("Player");

        //カラーのアルファ値取得
        currentAlpha = meshObj.GetComponent<Renderer>().materials[1].color.a;

        currentDetectionRange = detectionPlayerRangeMin;

        //デバッグ用
        if (debugDetectionCirclePrefab != null)
            debugDetectionCircle = Instantiate(debugDetectionCirclePrefab,
                transform.position + transform.up * 0.2f, 
                Quaternion.AngleAxis(-90.0f,Vector3.left), 
                transform
                );
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
        }
        
        Debug.Log("ゾンビ初期体力:"+zombieHP.GetCurrentHP());
    }


    // Update is called once per frame
    void Update()
    {
        if (playerObj == null) return;

        //死亡チェック
        if (zombieHP.IsDead())
            Dead();

        if (isDead) return;//死亡済なら動かさない
        if (isStan) return;//スタン時は動かさない

        //座標取得
        Vector3 pos = transform.position;
        Vector3 playerPos = playerObj.transform.position;
        pos.y = 0; playerPos.y = 0;//仮でy座標を無視
        //プレイヤーとの距離計算
        float playerDistance = Vector3.Distance(pos, playerPos);

        //プレイヤーから離れすぎたら削除する
        //if(playerDistance > despawnPlayerDistance)
        //{
        //    Destroy(gameObject);
        //}


        ChangeDetectRange();//探知範囲計算

        //デバッグ用 範囲可視化
        if(debugDetectionCircle != null)
        {
            float scale =  currentDetectionRange;
            debugDetectionCircle.transform.localScale = new Vector3(scale, scale, 1);
        }

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
            if (isFreezePos || isAttackCoolDown)//停止
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
                    _ = DelayRunAsync(
                        UnityEngine.Random.Range(4.0f, 8.0f),//次に向きを変えるまでの時間を決める
                        () => isChangeDirCoolDown = false  //フラグオフ
                        );

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
        if (isAttackCoolDown) return;//クールタイムチェック
        if (isDead) return;

        //攻撃開始
        zombieAttack.StartAttack();
        //攻撃モーション再生
        zombieAnimation.Attack();

        //攻撃のクールタイム中にする
        isAttackCoolDown = true;
        //数秒後クールタイム解除
        _ = DelayRunAsync(
            3.0,
            () => isAttackCoolDown = false
            );
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
    public void DamageBody(int _damage)
    {
        Debug.Log("Body");

        zombieHP.Damage(_damage);
        zombieAnimation.DamageHitLeft();

        Stan(2.0);//スタン
    }
    //被弾地点からアニメーションを変更させる用
    public void DamageBody(Vector3 _hitPos)
    {
        Debug.Log("Body");

        Vector3 vec = _hitPos - transform.position;

        Vector3 axis = Vector3.Cross(transform.forward, vec);

        if (axis.y < 0)
        {
            Debug.Log("左側");
            zombieAnimation.DamageHitLeft();
        }
        else
        {
            Debug.Log("右側");
            zombieAnimation.DamageHitRight();
        }

        zombieHP.Damage(1);

        Stan(2.0);//スタン
    }
    public void DamageBody(Vector3 _hitPos, int _damage)
    {
        Debug.Log("Body");

        Vector3 vec = _hitPos - transform.position;

        Vector3 axis = Vector3.Cross(transform.forward, vec);

        if (axis.y < 0)
        {
            Debug.Log("左側");
            zombieAnimation.DamageHitLeft();
        }
        else
        {
            Debug.Log("右側");
            zombieAnimation.DamageHitRight();
        }

        zombieHP.Damage(_damage);//ダメージ

        Stan(2.0);//スタン
    }
    /// <summary>
    /// 頭にダメージを受けた
    /// </summary>
    public void DamageHead()
    {
        Debug.Log("Head");

        zombieAttack.AttackCancel();//攻撃処理のキャンセル

        zombieHP.Damage(1000);
    }
    public void DamageHead(int _damage)
    {
        Debug.Log("Head");

        zombieAttack.AttackCancel();//攻撃処理のキャンセル

        zombieHP.Damage(_damage * 2);//ダメージ
        
        Stan(2.5);//スタン
    }

    //一定時間スタン
    private void Stan(double _sec)
    {
        if (isDead) return;

        if (isStan)
            stanCancellTokenSource.Cancel();//現在動いているスタン処理のキャンセル

        zombieAttack.AttackCancel();//攻撃処理のキャンセル

        stanCancellTokenSource = new CancellationTokenSource();

        isStan = true;

        //移動ベクトルをゼロにする
        zombieMove.StopMove();
        zombieAnimation.Idle();//停止モーション

        _ = DelayRunAsync(
            _sec,
            stanCancellTokenSource.Token,
            () => isStan = false
            );
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

        //アニメーションが終わるころにオブジェクトを消す
        _ = DelayRunAsync(
                    3.5,//後で定数化したい
                    () => zombieAction.Dead()//死亡
                    );
    }

    /// <summary>
    /// 移動不可状態にする
    /// (停止する時間)
    /// 犬側で呼び出してね
    /// </summary>
    public void FreezePosition(double _sec)
    {
        //移動停止フラグオン
        isFreezePos = true;
        //しばらくしたらオフにする
        _ = DelayRunAsync(
                    _sec,
                    () => isFreezePos = false
                    );
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
    /// 遅らせてActionを実行するasync
    /// </summary>
    private async ValueTask DelayRunAsync(double _wait_sec, Action _action)
    {
        // ディレイ処理
        await Task.Delay(TimeSpan.FromSeconds(_wait_sec));
        _action();
    }
    //キャンセル用
    private async ValueTask DelayRunAsync(double _wait_sec, CancellationToken _token, Action _action)
    {
        // ディレイ処理
        await Task.Delay(TimeSpan.FromSeconds(_wait_sec), _token);
        _action();
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
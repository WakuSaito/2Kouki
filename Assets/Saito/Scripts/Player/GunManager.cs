using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// <para>銃マネージャークラス</para>
/// <para>インスペクターで変数を設定することで様々な銃種に対応出来る</para>
/// 手に持つアイテムなのでIWeaponを継承　インベントリ関連の仕様が変更されている場合必要無くなるかも
/// </summary>
public class GunManager : MonoBehaviour, IWeapon, IStopObject
{
    [SerializeField] private string m_soundType;//サウンドの種類（武器種）
    [SerializeField] private Transform m_muzzleTransform; //銃口位置

    [SerializeField] private GameObject m_bulletLine;         //弾道
    [SerializeField] private GameObject m_muzzleFlashPrefab;  //マズルフラッシュ用エフェクト
    [SerializeField] private GameObject m_bulletHitPrefab;    //着弾エフェクト

    [SerializeField] private int m_magazineSize = 10;//弾の容量
    [SerializeField] private int m_oneShotBulletAmount = 1; //一発で発射される量
    [SerializeField] private float m_bulletSpread = 0.03f;  //弾ブレ
    [SerializeField] private float m_bulletDistance = 20.0f;//弾の飛距離
    [SerializeField] private float m_rapidSpeed = 1.0f; //連射速度
    [SerializeField] private bool m_isCanRapid = false; //連射可能か
    [SerializeField] private float m_reloadSpeed = 2.8f;//リロード速度

    [SerializeField] private int m_bulletDamage = 5;  //弾が敵に与えるダメージ

    //プレイヤーが持った時に代入 インベントリの参照用？
    public GameObject m_handPlayerObj = null;
    bool m_setPlayerFlag = false;

    private int m_currentMagazineAmount;//現在のマガジンの弾数

    protected bool m_isShotCooldown = false;//発砲クールタイム中か
    protected bool m_isReload = false;  //リロード中か
    bool m_isActive = true;//手に持っている状態か

    GameObject m_cameraObj;
    protected InventoryItem m_inventoryItem;
    //サウンド再生用
    private GunSound m_gunSound;
    protected Animator m_animator;

    IEnumerator m_reloadCoroutine;


    //初期設定　コンポーネント取得
    private void Awake()
    {
        m_currentMagazineAmount = m_magazineSize;

        m_cameraObj = Camera.main.gameObject;
        m_animator = GetComponent<Animator>();
        m_gunSound = GetComponent<GunSound>();
    }

    private void Update()
    {
        if (m_handPlayerObj != null && !m_setPlayerFlag)
        {
            m_inventoryItem = m_handPlayerObj.GetComponent<InventoryItem>();
            m_setPlayerFlag = true;
        }
    }

    /// <summary>
    /// マガジンの最大容量取得
    /// </summary>
    /// <returns>マガジンの最大容量</returns>
    public int GetMagazineSize()
    {
        return m_magazineSize;
    }

    /// <summary>
    /// 現在の残弾数取得
    /// </summary>
    /// <returns>現在の残弾数</returns>
    public int GetCurrentMagazine()
    {
        return m_currentMagazineAmount;
    }

    /// <summary>
    /// <para>リロード</para>
    /// リロードを開始する
    /// </summary>
    public virtual void Reload()
    {
        if (m_isShotCooldown) return;
        if (m_isReload) return;
        //ピストルの弾丸が最大数じゃなければreload可能
        if (m_currentMagazineAmount >= m_magazineSize) return;

        if (m_inventoryItem == null)
        {
            Debug.Log("リロード開始　現在の残弾数:" + m_currentMagazineAmount);
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            m_reloadCoroutine = ReloadCoroutine();
            StartCoroutine(m_reloadCoroutine);
            return;
        }  
        else if (m_inventoryItem.CheckBullet())//インベントリに弾丸があればリロード開始
        {
            Debug.Log("リロード開始　現在の残弾数:" + m_currentMagazineAmount);
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            m_reloadCoroutine = ReloadCoroutine();
            StartCoroutine(m_reloadCoroutine);
            return;
        }
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(m_reloadSpeed);

        ReloadFin();
        m_reloadCoroutine = null;
    }

    /// <summary>
    /// <para>リロード完了</para>
    /// リロードし終わったとき
    /// </summary>
    void ReloadFin()
    {
        m_animator.SetBool("Reload", false);  //reload
        m_isReload = false;

        //ピストルに入る弾丸数を調べる
        int empty_amount = HowManyCanLoaded();

        Debug.Log("マガジンの空き:" + empty_amount);

        AddBullet(empty_amount);

        Debug.Log("リロード終了　現在の残弾数:" + m_currentMagazineAmount);
    }

    /// <summary>
    /// <para>リロード停止</para>
    /// リロードをキャンセルする
    /// </summary>
    public virtual void StopReload()
    {    
        if (m_reloadCoroutine != null)
        {
            Debug.Log("リロードキャンセル");
            m_isReload = false;
            m_animator.SetBool("Reload", false);  //reload
            //コルーチン完全停止
            StopCoroutine(m_reloadCoroutine);
            m_reloadCoroutine = null;
        }
    }

    /// <summary>
    /// <para>弾追加</para>
    /// 指定の数だけマガジンに弾を補充する
    /// </summary>
    /// <param name="_amount">補充する弾数</param>
    public void AddBullet(int _amount)
    {
        int add_amount = _amount;
        //最大は超えないように
        if (add_amount > m_magazineSize - m_currentMagazineAmount)
            add_amount = m_magazineSize - m_currentMagazineAmount;

        if (m_inventoryItem == null) {
            m_currentMagazineAmount += add_amount;
            return;
        }

        m_currentMagazineAmount += m_inventoryItem.UseBullet(_amount);
    }

    /// <summary>
    /// 弾が何発入るか計算
    /// </summary>
    /// <returns>入る弾数</returns>
    public int HowManyCanLoaded()
    {
        //マガジンに入る弾数計算
        return m_magazineSize - m_currentMagazineAmount; ;
    }

    /// <summary>
    /// <para>銃のトリガーが押された瞬間</para>
    /// 銃の発射ボタンを押した瞬間に呼び出す
    /// </summary>
    public void PullTriggerDown()
    {
        if (m_isShotCooldown) return;
        if (m_currentMagazineAmount <= 0)
        {
            m_gunSound.PlayBlankShot();//発射失敗音
            return;
        }
        if (m_isReload)
        {
            //アニメーション遷移を確定させるため1フレーム遅延実行
            StartCoroutine(DelayFrameCoroutine(
                1,
                () => StopReload()
            ));

        }

        Shot();
    }

    /// <summary>
    /// <para>銃のトリガーが押されている</para>
    /// 銃の発射ボタンを押している限り呼び出す
    /// </summary>
    public void PullTrigger()
    {
        if (m_isShotCooldown) return;
        if (!m_isCanRapid) return;
        if (m_currentMagazineAmount <= 0)
        {
            return;
        }

        Shot();
    }

    /// <summary>
    /// <para>発砲</para>
    /// 発砲時呼び出され、指定の回数弾を生成
    /// </summary>
    private void Shot()
    {
        //同時発射数分繰り返す
        for (int i = 0; i < m_oneShotBulletAmount; i++)
        {
            CreateBullet();
        }

        m_gunSound.PlayShot(m_soundType);//発射音

        m_animator.SetTrigger("Shot");//アニメーション

        //クールタイム
        StartCoroutine(CooldownCoroutine(m_rapidSpeed));

        m_currentMagazineAmount--;
    }


    /// <summary>
    /// <para>弾生成</para>
    /// 弾を一つ生成する
    /// </summary>
    private void CreateBullet()
    {
        //ばらつきをランダムに決める
        float x = UnityEngine.Random.Range(-m_bulletSpread, m_bulletSpread);
        float y = UnityEngine.Random.Range(-m_bulletSpread, m_bulletSpread);

        //視点ベクトルにばらつきを加算
        Vector3 gun_vec = m_cameraObj.transform.forward + new Vector3(x, y, 0);

        //当たったオブジェクト情報
        RaycastHit hit = new RaycastHit();

        //着弾地点
        Vector3 bullet_hit_pos;

        //なにかに当たった
        if (Physics.Raycast(m_cameraObj.transform.position, gun_vec, out hit))
        {
            bullet_hit_pos = hit.point;

            //アイテムまでの距離を調べる
            float distance = Vector3.Distance(hit.transform.position, transform.position);

            //距離が範囲内なら
            if (distance <= 30.0f)
            {
                GameObject hit_obj = hit.collider.gameObject;
                Debug.Log(hit_obj);
                if (hit_obj.tag == "Body")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageBody(hit.point, m_bulletDamage);
                }
                if (hit_obj.tag == "Head")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageHead(hit.point, m_bulletDamage);
                }
            }
        }
        //レイが何にも当たらなかったとき
        else
        {
            //弾丸のベクトルの終点を線の終点にする
            bullet_hit_pos = m_cameraObj.transform.position + (gun_vec * m_bulletDistance);
        }

        //アニメーション後にエフェクトを出したいので1フレーム遅らせる   
        StartCoroutine(DelayFrameCoroutine(
            1, 
            () => CreateBulletEffect(bullet_hit_pos)
        ));
    }

    /// <summary>
    /// <para>対象のアクションを一定フレーム後遅延実行させる</para>
    /// アニメーション制御用
    /// </summary>
    /// <param name="_frame">遅延フレーム</param>
    /// <param name="_action">遅延後実行する処理</param>
    private IEnumerator DelayFrameCoroutine(int _frame, Action _action)
    {
        for (int i = 0; i < _frame; i++)
        {
            yield return null;
        }

        //アクション実行
        _action();
    }

    /// <summary>
    /// <para>弾エフェクト生成</para>
    /// 発砲時にエフェクトを生成する
    /// </summary>
    /// <param name="_hit_pos">着弾地点</param>
    private void CreateBulletEffect(Vector3 _hit_pos)
    {
        //マズルフラッシュ
        if (m_muzzleFlashPrefab != null)
        {
            Instantiate(m_muzzleFlashPrefab,
                m_muzzleTransform.position,
                Quaternion.identity,
                transform
                ).transform.localRotation = Quaternion.identity;
        }

        //弾道
        if (m_bulletLine != null)
        {
            //弾道用のLineRendererを取得（見た目用）
            LineRenderer line_rend = Instantiate(
                m_bulletLine,
                Vector3.zero,
                Quaternion.identity
                ).GetComponent<LineRenderer>();

            //点の数
            line_rend.positionCount = 2;
            //始点の座標指定
            line_rend.SetPosition(0, m_muzzleTransform.position);

            //当たった場所を線の終点にする
            line_rend.SetPosition(1, _hit_pos);
        }

        //着弾
        if(m_bulletHitPrefab != null)
        {
            //着弾地点に生成
            GameObject hit_effect = 
                Instantiate(m_bulletHitPrefab,
                _hit_pos,
                Quaternion.identity
                );

            //発射方向に向ける
            hit_effect.transform.rotation = 
                Quaternion.LookRotation(m_muzzleTransform.position - hit_effect.transform.position);
        }
    }
    
    /// <summary>
    /// クールタイムコルーチン
    /// 発砲クールタイム管理用
    /// </summary>
    /// <param name="_sec">クールタイム秒数</param>
    private IEnumerator CooldownCoroutine(float _sec)
    {
        m_isShotCooldown = true;

        //連射速度分待つ
        yield return new WaitForSeconds(_sec);

        m_isShotCooldown = false;
    }

    /// <summary>
    /// <para>仕舞う</para>
    /// 武器切り替え時に呼び出し
    /// </summary>
    public void PutAway()
    {
        if (m_isActive == false) return;
        m_isActive = false;
        gameObject.SetActive(false);
        StopReload();//リロードをキャンセル
        Debug.Log("仕舞う");
    }
    /// <summary>
    /// <para>取り出す</para>>
    /// 武器切り替え時に呼び出し
    /// </summary>
    public void PutOut()
    {
        if (m_isActive == true) return;
        m_isActive = true;

        gameObject.SetActive(true);
        Debug.Log("取り出し");

        m_isReload = false;
        m_isShotCooldown = false;
    }

    /// <summary>
    /// アイテムを落とすときの設定
    /// オブジェクト表示、コライダーON、固定解除、スケール・向き調整
    /// </summary>
    public void DropItemSetting()
    {
        gameObject.SetActive(true);//表示
        GetComponent<ItemSetting>().drop_flag = true;//アイテムdrop
        GetComponent<ItemSetting>().m_getFlag = false;//獲得していない
        GetComponent<BoxCollider>().enabled = true;//コライダーON
        GetComponent<Rigidbody>().isKinematic = false;//固定解除
        GetComponent<Animator>().enabled = false;//アニメーションOFF
        tag = "item";//タグ変更

        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);//スケールをもとの大きさに
        transform.Rotate(new Vector3(0, 90, 45));//向きを調整
    }

    /// <summary>
    /// アイテム取得した時の設定
    /// コライダーOFF、固定する
    /// </summary>
    public void GetItemSetting()
    {
        //コンポーネント設定
        gameObject.SetActive(false);
        GetComponent<ItemSetting>().drop_flag = false;//アイテムdrop
        GetComponent<ItemSetting>().m_getFlag = true;//アイテム獲得済み
        GetComponent<BoxCollider>().enabled = false;//コライダーOFF
        GetComponent<Rigidbody>().isKinematic = true;//固定
        GetComponent<Animator>().enabled = true;//アニメーションON
        tag = "weapon";//タグ変更

        //位置設定
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //スケール変更
    }

    /// <summary>
    /// 一時停止
    /// </summary>
    public void Pause()
    {
        m_animator.speed = 0;
        if (m_reloadCoroutine != null)
            StopCoroutine(m_reloadCoroutine);
    }

    /// <summary>
    /// 再開
    /// </summary>
    public void Resume()
    {
        m_animator.speed = 1;
        if (m_reloadCoroutine != null)
            StartCoroutine(m_reloadCoroutine);
    }
}

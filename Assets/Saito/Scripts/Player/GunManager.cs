using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunManager : MonoBehaviour, IWeapon
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

    //プレイヤーが持った時に代入
    public GameObject m_handPlayerObj = null;
    bool m_setPlayerFlag = false;

    private int m_currentMagazineAmount;//現在のマガジンの弾数

    protected bool m_isShotCooldown = false;//発砲クールタイム中か
    protected bool m_isReload = false;  //リロード中か

    GameObject m_cameraObj;
    protected Inventory m_inventory;
    protected ItemInventory m_itemInventory;
    //サウンド再生用
    private GunSound m_gunSound;
    protected Animator m_animator;


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
            m_inventory = m_handPlayerObj.GetComponent<Inventory>();
            m_itemInventory = m_handPlayerObj.GetComponent<player>().ItemInventory;
            m_setPlayerFlag = true;
        }
    }

    public int GetMagazineSize()
    {
        return m_magazineSize;
    }

    public int GetCurrentMagazine()
    {
        return m_currentMagazineAmount;
    }

    public virtual void Reload()
    {
        if (m_isShotCooldown) return;
        if (m_isReload) return;
        //ピストルの弾丸が最大数じゃなければreload可能
        if (m_currentMagazineAmount >= m_magazineSize) return;

        if (m_inventory == null)
        {
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            Invoke(nameof(ReroadFin), m_reloadSpeed);
            return;
        }

        if (m_itemInventory.CheckInBullet())
        {
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            Invoke(nameof(ReroadFin), m_reloadSpeed);
            return;
        }

        //for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        //{
        //    //インベントリに弾丸があるか
        //    if (inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
        //    {
        //        anim.SetBool("Reload", true);  //reload
        //        isReload = true;
        //        Invoke(nameof(ReroadFin), reloadSpeed);
        //    }
        //}

    }

    void ReroadFin()
    {
        m_animator.SetBool("Reload", false);  //reload
        m_isReload = false;

        //ピストルに入る弾丸数を調べる
        int empty_amount = HowManyCanLoaded();

        AddBullet(empty_amount);
    }

    public virtual void StopReload()
    {
        if (IsInvoking(nameof(ReroadFin)))
        {
            m_isReload = false;
            m_animator.SetBool("Reload", false);  //reload
            CancelInvoke(nameof(ReroadFin));
        }
    }

    public void AddBullet(int _amount)
    {
        int add_amount = _amount;
        //最大は超えないように
        if (add_amount > m_magazineSize - m_currentMagazineAmount)
            add_amount = m_magazineSize - m_currentMagazineAmount;

        if (m_inventory == null) {
            m_currentMagazineAmount += add_amount;
            return;
        }

        m_currentMagazineAmount += m_itemInventory.SubBullet(_amount);

        //for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        //{
        //    //インベントリに弾丸があるか
        //    if (inventory.item_type_id[i] != (int)ID.ITEM_ID.BULLET) continue;

        //    for (int cnt = 0; cnt < addAmount; cnt++)
        //    {
        //        if (inventory.item_num[i] == 0)
        //        {
        //            //インベントリにあった弾丸の残りが0になったらidも初期化する
        //            inventory.item_type_id[i] = -1;
        //            break;
        //        }
        //        else
        //        {
        //            //下の関数にまとめて欲しい
        //            inventory.item_num[i]--;
        //            //インベントリの中身も減らす 複数減らせるようにしてほしい
        //            inventory.ReduceInventory(i);

        //            currentMagazineAmount++;
        //            addAmount--;
        //        }
        //    }
        //}
    }

    //後何発弾を入れられるか
    public int HowManyCanLoaded()
    {
        //マガジンに入る弾数計算
        int can_loaded_amount = m_magazineSize - m_currentMagazineAmount;

        if (m_inventory == null) return can_loaded_amount;

        for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        {
            //インベントリに弾丸があるか
            if (m_inventory.item_type_id[i] != (int)ID.ITEM_ID.BULLET) continue;

            for (int cnt = 0; cnt < can_loaded_amount; cnt++)
            {
                //インベントリに弾がある場合
                if (m_inventory.item_num[i] != 0)
                    can_loaded_amount--; 
                //弾が無い場合
                else          
                    break;
            }
        }

        return can_loaded_amount;
    }

    /// <summary>
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


    //弾発射
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

    //対象のアクションを一定フレーム後遅延実行させる
    private IEnumerator DelayFrameCoroutine(int _frame, Action _action)
    {
        for (int i = 0; i < _frame; i++)
        {
            yield return null;
        }

        //アクション実行
        _action();
    }

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
    
    //クールタイム用コルーチン
    private IEnumerator CooldownCoroutine(float _sec)
    {
        m_isShotCooldown = true;

        //連射速度分待つ
        yield return new WaitForSeconds(_sec);

        m_isShotCooldown = false;
    }

    //仕舞う
    public void PutAway()
    {
        gameObject.SetActive(false);
    }
    //取り出す
    public void PutOut()
    {
        gameObject.SetActive(true);

        m_isReload = false;
        m_isShotCooldown = false;
    }
}

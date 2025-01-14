using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunManager : MonoBehaviour, IWeapon
{
    [SerializeField] private string mSoundType;//サウンドの種類（武器種）
    [SerializeField] private Transform mMuzzleTransform; //銃口位置

    [SerializeField] private GameObject mBulletLine;         //弾道
    [SerializeField] private GameObject mMuzzleFlashPrefab;  //マズルフラッシュ用エフェクト
    [SerializeField] private GameObject mBulletHitPrefab;    //着弾エフェクト

    [SerializeField] private int mMagazineSize = 10;//弾の容量
    [SerializeField] private int mOneShotBulletAmount = 1; //一発で発射される量
    [SerializeField] private float mBulletSpread = 0.03f;  //弾ブレ
    [SerializeField] private float mBulletDistance = 20.0f;//弾の飛距離
    [SerializeField] private float mRapidSpeed = 1.0f; //連射速度
    [SerializeField] private bool mIsCanRapid = false; //連射可能か
    [SerializeField] private float mReloadSpeed = 2.8f;//リロード速度

    [SerializeField] private int mBulletDamage = 5;  //弾が敵に与えるダメージ

    //プレイヤーが持った時に代入
    public GameObject hand_player_obj = null;
    bool mSetPlayerFlag = false;

    private int mCurrentMagazineAmount;//現在のマガジンの弾数

    protected bool mIsShotCooldown = false;//発砲クールタイム中か
    protected bool mIsReload = false;  //リロード中か

    GameObject mCameraObj;
    protected Inventory mInventory;
    protected ItemInventory mIteminventory;
    //サウンド再生用
    private GunSound mGunSound;
    protected Animator mAnimator;


    private void Awake()
    {
        mCurrentMagazineAmount = mMagazineSize;

        mCameraObj = Camera.main.gameObject;
        mAnimator = GetComponent<Animator>();

        mGunSound = GetComponent<GunSound>();
    }

    private void Update()
    {
        if (hand_player_obj != null && !mSetPlayerFlag)
        {
            mInventory = hand_player_obj.GetComponent<Inventory>();
            mIteminventory = hand_player_obj.GetComponent<player>().ItemInventory;
            mSetPlayerFlag = true;
        }
    }

    public int GetMagazineSize()
    {
        return mMagazineSize;
    }

    public int GetCurrentMagazine()
    {
        return mCurrentMagazineAmount;
    }

    public virtual void Reload()
    {
        if (mIsShotCooldown) return;
        if (mIsReload) return;
        //ピストルの弾丸が最大数じゃなければreload可能
        if (mCurrentMagazineAmount >= mMagazineSize) return;

        if (mInventory == null)
        {
            mAnimator.SetBool("Reload", true);  //reload
            mIsReload = true;
            Invoke(nameof(ReroadFin), mReloadSpeed);
            return;
        }

        if (mIteminventory.CheckInBullet())
        {
            mAnimator.SetBool("Reload", true);  //reload
            mIsReload = true;
            Invoke(nameof(ReroadFin), mReloadSpeed);
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
        mAnimator.SetBool("Reload", false);  //reload
        mIsReload = false;

        //ピストルに入る弾丸数を調べる
        int empty_amount = HowManyCanLoaded();

        AddBullet(empty_amount);
    }

    public virtual void StopReload()
    {
        if (IsInvoking(nameof(ReroadFin)))
        {
            mIsReload = false;
            mAnimator.SetBool("Reload", false);  //reload
            CancelInvoke(nameof(ReroadFin));
        }
    }

    public void AddBullet(int _amount)
    {
        int add_amount = _amount;
        //最大は超えないように
        if (add_amount > mMagazineSize - mCurrentMagazineAmount)
            add_amount = mMagazineSize - mCurrentMagazineAmount;

        if (mInventory == null) {
            mCurrentMagazineAmount += add_amount;
            return;
        }

        mCurrentMagazineAmount += mIteminventory.SubBullet(_amount);

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
        int can_loaded_amount = mMagazineSize - mCurrentMagazineAmount;

        if (mInventory == null) return can_loaded_amount;

        for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        {
            //インベントリに弾丸があるか
            if (mInventory.item_type_id[i] != (int)ID.ITEM_ID.BULLET) continue;

            for (int cnt = 0; cnt < can_loaded_amount; cnt++)
            {
                //インベントリに弾がある場合
                if (mInventory.item_num[i] != 0)
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
        if (mIsShotCooldown) return;
        if (mCurrentMagazineAmount <= 0)
        {
            mGunSound.PlayBlankShot();//発射失敗音
            return;
        }
        if (mIsReload)
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
        if (mIsShotCooldown) return;
        if (!mIsCanRapid) return;
        if (mCurrentMagazineAmount <= 0)
        {
            return;
        }

        Shot();
    }

    private void Shot()
    {
        //同時発射数分繰り返す
        for (int i = 0; i < mOneShotBulletAmount; i++)
        {
            CreateBullet();
        }

        mGunSound.PlayShot(mSoundType);//発射音

        mAnimator.SetTrigger("Shot");//アニメーション

        //クールタイム
        StartCoroutine(CooldownCoroutine(mRapidSpeed));

        mCurrentMagazineAmount--;
    }


    //弾発射
    private void CreateBullet()
    {
        //ばらつきをランダムに決める
        float x = UnityEngine.Random.Range(-mBulletSpread, mBulletSpread);
        float y = UnityEngine.Random.Range(-mBulletSpread, mBulletSpread);

        //視点ベクトルにばらつきを加算
        Vector3 gun_vec = mCameraObj.transform.forward + new Vector3(x, y, 0);

        //当たったオブジェクト情報
        RaycastHit hit = new RaycastHit();

        //着弾地点
        Vector3 bullet_hit_pos;

        //なにかに当たった
        if (Physics.Raycast(mCameraObj.transform.position, gun_vec, out hit))
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
                    hit_obj.GetComponentInParent<ZombieManager>().DamageBody(hit.point, mBulletDamage);
                }
                if (hit_obj.tag == "Head")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageHead(hit.point, mBulletDamage);
                }
            }
        }
        //レイが何にも当たらなかったとき
        else
        {
            //弾丸のベクトルの終点を線の終点にする
            bullet_hit_pos = mCameraObj.transform.position + (gun_vec * mBulletDistance);
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
        if (mMuzzleFlashPrefab != null)
        {
            Instantiate(mMuzzleFlashPrefab,
                mMuzzleTransform.position,
                Quaternion.identity,
                transform
                ).transform.localRotation = Quaternion.identity;
        }

        //弾道
        if (mBulletLine != null)
        {
            //弾道用のLineRendererを取得（見た目用）
            LineRenderer line_rend = Instantiate(
                mBulletLine,
                Vector3.zero,
                Quaternion.identity
                ).GetComponent<LineRenderer>();

            //点の数
            line_rend.positionCount = 2;
            //始点の座標指定
            line_rend.SetPosition(0, mMuzzleTransform.position);

            //当たった場所を線の終点にする
            line_rend.SetPosition(1, _hit_pos);
        }

        //着弾
        if(mBulletHitPrefab != null)
        {
            //着弾地点に生成
            GameObject hit_effect = 
                Instantiate(mBulletHitPrefab,
                _hit_pos,
                Quaternion.identity
                );

            //発射方向に向ける
            hit_effect.transform.rotation = 
                Quaternion.LookRotation(mMuzzleTransform.position - hit_effect.transform.position);
        }
    }
    
    //クールタイム用コルーチン
    private IEnumerator CooldownCoroutine(float _sec)
    {
        mIsShotCooldown = true;

        //連射速度分待つ
        yield return new WaitForSeconds(_sec);

        mIsShotCooldown = false;
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

        mIsReload = false;
        mIsShotCooldown = false;
    }
}

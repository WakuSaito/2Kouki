using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunManager : MonoBehaviour
{
    [SerializeField] private Transform muzzleTransform; //銃口位置

    [SerializeField] private GameObject bulletLine;         //弾道
    [SerializeField] private GameObject muzzleFlashPrefab;  //マズルフラッシュ用エフェクト
    [SerializeField] private GameObject bulletHitPrefab;    //着弾エフェクト

    [SerializeField] private int magazineSize = 10;//弾の容量
    [SerializeField] private int oneShotBulletAmount = 1; //一発で発射される量
    [SerializeField] private float bulletSpread = 0.03f;  //弾ブレ
    [SerializeField] private float bulletDistance = 20.0f;//弾の飛距離
    [SerializeField] private float rapidSpeed = 1.0f; //連射速度
    [SerializeField] private bool isCanRapid = false; //連射可能か
    [SerializeField] private float reloadSpeed = 2.8f;//リロード速度

    [SerializeField] private int bulletDamage = 5;  //弾が敵に与えるダメージ

    //プレイヤーが持った時に代入
    public GameObject hand_player_obj = null;
    bool set_player_flag = false;

    private int currentMagazineAmount;//現在のマガジンの弾数

    protected bool isShotCooldown = false;//発砲クールタイム中か
    protected bool isReload = false;  //リロード中か

    GameObject cameraObj;
    protected Inventory inventory;
    protected ItemInventory iteminventory;
    //サウンド再生用
    private GunSound gunSound;
    protected Animator anim;


    private void Awake()
    {
        currentMagazineAmount = magazineSize;

        cameraObj = Camera.main.gameObject;
        anim = GetComponent<Animator>();

        gunSound = GetComponent<GunSound>();
    }

    private void Update()
    {
        if (hand_player_obj != null && !set_player_flag)
        {
            inventory = hand_player_obj.GetComponent<Inventory>();
            iteminventory = hand_player_obj.GetComponent<player>().ItemInventory;
            set_player_flag = true;
        }
    }

    public int GetMagazineSize()
    {
        return magazineSize;
    }

    public int GetCurrentMagazine()
    {
        return currentMagazineAmount;
    }

    public virtual void Reload()
    {
        if (isShotCooldown) return;
        if (isReload) return;
        //ピストルの弾丸が最大数じゃなければreload可能
        if (currentMagazineAmount >= magazineSize) return;

        //if(inventory == null)
        //{
        //    anim.SetBool("Reload", true);  //reload
        //    isReload = true;
        //    Invoke(nameof(ReroadFin), reloadSpeed);
        //    return;
        //}
        if (!iteminventory.CheckInBullet()) return;

        if (iteminventory.CheckInBullet())
        {
            anim.SetBool("Reload", true);  //reload
            isReload = true;
            Invoke(nameof(ReroadFin), reloadSpeed);
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
        anim.SetBool("Reload", false);  //reload
        isReload = false;

        //ピストルに入る弾丸数を調べる
        int emptyAmount = HowManyCanLoaded();

        AddBullet(emptyAmount);
    }

    public virtual void StopReload()
    {
        if (IsInvoking(nameof(ReroadFin)))
        {
            isReload = false;
            anim.SetBool("Reload", false);  //reload
            CancelInvoke(nameof(ReroadFin));
        }
    }

    public void AddBullet(int _amount)
    {
        int addAmount = _amount;
        //最大は超えないように
        if (addAmount > magazineSize - currentMagazineAmount)
            addAmount = magazineSize - currentMagazineAmount;

        currentMagazineAmount+=iteminventory.SubBullet(_amount);

        if (inventory == null) {
            currentMagazineAmount += addAmount;
            return;
        }


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
        int canLoadedAmount = magazineSize - currentMagazineAmount;

        if (inventory == null) return canLoadedAmount;

        for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        {
            //インベントリに弾丸があるか
            if (inventory.item_type_id[i] != (int)ID.ITEM_ID.BULLET) continue;

            for (int cnt = 0; cnt < canLoadedAmount; cnt++)
            {
                //インベントリに弾がある場合
                if (inventory.item_num[i] != 0)
                    canLoadedAmount--; 
                //弾が無い場合
                else          
                    break;
            }
        }

        return canLoadedAmount;
    }

    /// <summary>
    /// 銃の発射ボタンを押した瞬間に呼び出す
    /// </summary>
    public void PullTriggerDown()
    {
        if (isShotCooldown) return;
        if (currentMagazineAmount <= 0)
        {
            gunSound.PlayBlankShot();//発射失敗音
            return;
        }
        if (isReload)
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
        if (isShotCooldown) return;
        if (!isCanRapid) return;
        if (currentMagazineAmount <= 0)
        {
            return;
        }

        Shot();
    }

    private void Shot()
    {
        //同時発射数分繰り返す
        for (int i = 0; i < oneShotBulletAmount; i++)
        {
            CreateBullet();
        }

        gunSound.PlayShot();//発射音

        anim.SetTrigger("Shot");//アニメーション

        //クールタイム
        StartCoroutine(CooldownCoroutine(rapidSpeed));

        currentMagazineAmount--;
    }


    //弾発射
    private void CreateBullet()
    {
        //ばらつきをランダムに決める
        float x = UnityEngine.Random.Range(-bulletSpread, bulletSpread);
        float y = UnityEngine.Random.Range(-bulletSpread, bulletSpread);

        //視点ベクトルにばらつきを加算
        Vector3 gunVec = cameraObj.transform.forward + new Vector3(x, y, 0);

        //当たったオブジェクト情報
        RaycastHit hit = new RaycastHit();

        //着弾地点
        Vector3 bulletHitPos;

        //なにかに当たった
        if (Physics.Raycast(cameraObj.transform.position, gunVec, out hit))
        {
            bulletHitPos = hit.point;

            //アイテムまでの距離を調べる
            float distance = Vector3.Distance(hit.transform.position, transform.position);

            //距離が範囲内なら
            if (distance <= 30.0f)
            {
                GameObject hit_obj = hit.collider.gameObject;
                Debug.Log(hit_obj);
                if (hit_obj.tag == "Body")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageBody(hit.point, bulletDamage);
                }
                if (hit_obj.tag == "Head")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageHead(hit.point, bulletDamage);
                }
            }
        }
        //レイが何にも当たらなかったとき
        else
        {
            //弾丸のベクトルの終点を線の終点にする
            bulletHitPos = cameraObj.transform.position + (gunVec * bulletDistance);
        }

        //アニメーション後にエフェクトを出したいので1フレーム遅らせる   
        StartCoroutine(DelayFrameCoroutine(
            1, 
            () => CreateBulletEffect(bulletHitPos)
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

    private void CreateBulletEffect(Vector3 _hitPos)
    {
        //マズルフラッシュ
        if (muzzleFlashPrefab != null)
        {
            Instantiate(muzzleFlashPrefab,
                muzzleTransform.position,
                Quaternion.identity,
                transform
                ).transform.localRotation = Quaternion.identity;
        }

        //弾道
        if (bulletLine != null)
        {
        //弾道用のLineRendererを取得（見た目用）
        LineRenderer lineRend = Instantiate(
            bulletLine,
            Vector3.zero,
            Quaternion.identity
            ).GetComponent<LineRenderer>();

        //点の数
        lineRend.positionCount = 2;
        //始点の座標指定
        lineRend.SetPosition(0, muzzleTransform.position);

        //当たった場所を線の終点にする
        lineRend.SetPosition(1, _hitPos);
        }

        //着弾
        if(bulletHitPrefab != null)
        {
            //着弾地点に生成
            GameObject hitEffect = 
                Instantiate(bulletHitPrefab,
                _hitPos,
                Quaternion.identity
                );

            //発射方向に向ける
            hitEffect.transform.rotation = 
                Quaternion.LookRotation(muzzleTransform.position - hitEffect.transform.position);
        }
    }
    
    //クールタイム用コルーチン
    private IEnumerator CooldownCoroutine(float _sec)
    {
        isShotCooldown = true;

        //連射速度分待つ
        yield return new WaitForSeconds(_sec);

        isShotCooldown = false;
    }

}

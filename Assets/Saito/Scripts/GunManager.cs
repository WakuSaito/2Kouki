using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{

    [SerializeField] private Transform muzzleTransform;//銃口位置
    [SerializeField] private GameObject bulletLine;//弾道

    [SerializeField] private int magazineSize = 10;//弾の容量
    [SerializeField] private int oneShotBulletAmount = 1; //一発で発射される量
    [SerializeField] private float bulletSpread = 0.03f;  //弾ブレ
    [SerializeField] private float bulletDistance = 20.0f;//弾の飛距離
    [SerializeField] private float rapidSpeed = 1.0f;//連射速度
    [SerializeField] private bool isCanRapid = false;//連射可能か

    [SerializeField] private int bulletDamage = 5;  //弾が敵に与えるダメージ

    private int currentMagazineAmount;//現在のマガジンの弾数

    bool isCooldown = false;//クールタイム中か

    GameObject cameraObj;

    private void Awake()
    {
        currentMagazineAmount = magazineSize;

        cameraObj = Camera.main.gameObject;
    }

    private void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.R))
            currentMagazineAmount = magazineSize;

        if(Input.GetMouseButtonDown(0))
        {
            PullTriggerDown();
        }
        else if (Input.GetMouseButton(0))
        {
            PullTrigger();
        }
    }


    public void Reload(Inventory inventory)
    {
        if (isCooldown) return;

        //ピストルの弾丸が最大数じゃなければreload可能
        if (currentMagazineAmount < magazineSize)
        {
            for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
            {
                //インベントリに弾丸があるか　弾の種類を調べる必要あり
                if (inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
                {
                    //ピストルに入る弾丸数を調べる
                    int reload_num = magazineSize - currentMagazineAmount;
                    //reloadできる最大数を保存
                    int max_reload = reload_num;

                    for (int cnt = 0; cnt < max_reload; cnt++)
                    {
                        if (inventory.item_num[i] == 0)
                        {
                            //インベントリにあった弾丸の残りが0になったらidも初期化する
                            inventory.item_type_id[i] = -1;
                            break;
                        }
                        else
                        {
                            inventory.item_num[i]--;
                            magazineSize++;
                            reload_num--;
                        }
                        //インベントリの中身も減らす
                        inventory.ReduceInventory(i);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 銃の発射ボタンを押した瞬間に呼び出す
    /// </summary>
    public void PullTriggerDown()
    {
        if (currentMagazineAmount <= 0) return;
        if (isCooldown) return;

        //同時発射数分繰り返す
        for (int i = 0; i < oneShotBulletAmount; i++)
        {
            CreateBullet();
        }

        //出きれば反動を付けたい

        //クールタイム
        StartCoroutine(CooldownCoroutine(rapidSpeed));

        currentMagazineAmount--;
    }

    /// <summary>
    /// 銃の発射ボタンを押している限り呼び出す
    /// </summary>
    public void PullTrigger()
    {
        if (currentMagazineAmount <= 0) return;
        if (isCooldown) return;
        if (!isCanRapid) return;

        //同時発射数分繰り返す
        for (int i = 0; i < oneShotBulletAmount; i++)
        {
            CreateBullet();
        }

        //クールタイム
        StartCoroutine(CooldownCoroutine(rapidSpeed));

        currentMagazineAmount--;
    }

    //弾発射
    private void CreateBullet()
    {
        //ばらつきをランダムに決める
        float x = Random.Range(-bulletSpread, bulletSpread);
        float y = Random.Range(-bulletSpread, bulletSpread);

        //視点ベクトルにばらつきを加算
        Vector3 gunVec = cameraObj.transform.forward + new Vector3(x, y, 0);


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


        //ビューポート座標のレイを飛ばす
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit = new RaycastHit();


        if (Physics.Raycast(cameraObj.transform.position, gunVec, out hit))
        {
            //当たった場所を線の終点にする
            lineRend.SetPosition(1, hit.point);

            //アイテムまでの距離を調べる
            float distance = Vector3.Distance(hit.transform.position, transform.position);

            //距離が範囲内なら
            if (distance <= 30.0f)
            {
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);

                GameObject hit_obj = hit.collider.gameObject;
                Debug.Log(hit_obj);
                if (hit_obj.tag == "Body")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageBody(hit.point, bulletDamage);
                }
                if (hit_obj.tag == "Head")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageHead(bulletDamage);
                }
            }
        }
        //レイが何にも当たらなかったとき
        else
        {
            //弾丸のベクトルの終点を線の終点にする
            lineRend.SetPosition(1, cameraObj.transform.position + (gunVec * bulletDistance));
        }
    }
    
    //クールタイム用コルーチン
    private IEnumerator CooldownCoroutine(float _sec)
    {
        isCooldown = true;

        //連射速度分待つ
        yield return new WaitForSeconds(_sec);

        isCooldown = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    const int BULLET_MAX = 10;

    [SerializeField] GameObject bullet_obj;
    
    //プレイヤーが持った時に代入
    public GameObject hand_player_obj = null;
    bool set_player_flag = false;

    Animator anim;
    Inventory inventory;

    //もともとの弾数
    public int pistol_bullet_num = 10;

    [SerializeField] private Transform muzzleTransform;//銃口位置
    [SerializeField] private GameObject bulletLine;//弾道
    [SerializeField] private float bulletDistance = 20.0f;//弾の飛距離
    [SerializeField] private float bulletSpread = 0.03f;//弾ブレ

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hand_player_obj != null && !set_player_flag) 
        {
            inventory = hand_player_obj.GetComponent<Inventory>();
            set_player_flag = true;
        }
    }
        
    public void Reload()
    {
        //リロード処理
        if (Input.GetKeyDown(KeyCode.R))
        {
            //ピストルの弾丸が最大数じゃなければreload可能
            if (pistol_bullet_num < BULLET_MAX)
            {
                for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
                {
                    //インベントリに弾丸があるか
                    if (inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
                    {
                        anim.SetBool("Reload", true);  //reload
                        Invoke("ReroadFin", 2.8f);
                    }
                }
            }
        }
    }

    public void Reload(Inventory inventory)
    {
        //ピストルの弾丸が最大数じゃなければreload可能
        if (pistol_bullet_num < BULLET_MAX)
        {
            for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
            {
                //インベントリに弾丸があるか
                if (inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
                {
                    //ピストルに入る弾丸数を調べる
                    int reload_num = BULLET_MAX - pistol_bullet_num;
                    //reloadできる最大数を保存
                    int max_reload = reload_num;

                    //animation
                    anim.SetBool("Reload", true);  //reload
                    Invoke("ReroadFin", 2.8f);

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
                            pistol_bullet_num++;
                            reload_num--;
                        }
                        //インベントリの中身も減らす
                        inventory.ReduceInventory(i);
                    }
                }
            }
        }
    }

    public void Attack(GameObject _rot_obj,GameObject _hand_obj)
    {
        if (pistol_bullet_num > 0)
        {
            //ばらつきをランダムに決める
            float x = Random.Range(-bulletSpread, bulletSpread);
            float y = Random.Range(-bulletSpread, bulletSpread);

            //視点ベクトルにばらつきを加算
            Vector3 gunVec = _rot_obj.transform.forward + new Vector3(x, y, 0);


            //弾道用のLineRendererを取得（見た目用）
            LineRenderer lineRend = Instantiate(bulletLine,
                Vector3.zero,
                Quaternion.identity).GetComponent<LineRenderer>();

            //点の数
            lineRend.positionCount = 2;
            //始点の座標指定
            lineRend.SetPosition(0, muzzleTransform.position);


            //ビューポート座標のレイを飛ばす
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            RaycastHit hit = new RaycastHit();


            if (Physics.Raycast(ray, out hit))
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
                        hit_obj.GetComponentInParent<ZombieManager>().DamageBody(hit.point);
                    }
                    if (hit_obj.tag == "Head")
                    {
                        hit_obj.GetComponentInParent<ZombieManager>().DamageHead();
                    }
                }
            }
            //レイが何にも当たらなかったとき
            else
            {
                //弾丸のベクトルの終点を線の終点にする
                lineRend.SetPosition(1, _rot_obj.transform.position + (gunVec * bulletDistance));
            }

            ////向き発射される向き
            //Quaternion rot = _rot_obj.transform.rotation;
            ////弾丸生成
            //Instantiate(bullet_obj, _hand_obj.transform.position, rot);

            //Pistol内の弾丸を減らす
            pistol_bullet_num--;
            
        }
    }

    void ReroadFin()
    {
        anim.SetBool("Reload", false);  //reload

        //ピストルの弾丸が最大数じゃなければreload可能
        if (pistol_bullet_num < BULLET_MAX)
        {
            for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
            {
                //インベントリに弾丸があるか
                if (inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
                {
                    //ピストルに入る弾丸数を調べる
                    int reload_num = BULLET_MAX - pistol_bullet_num;
                    //reloadできる最大数を保存
                    int max_reload = reload_num;

                    //animation

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
                            pistol_bullet_num++;
                            reload_num--;
                        }
                        //インベントリの中身も減らす
                        inventory.ReduceInventory(i);
                    }
                }
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    const int BULLET_MAX = 10;

    [SerializeField] GameObject bullet_obj;

    //もともとの弾数
    public int pistol_bullet_num = 10;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

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
            //ビューポート座標のレイを飛ばす
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
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
                        hit_obj.GetComponentInParent<ZombieManager>().DamageBody();
                    }
                    if (hit_obj.tag == "Head")
                    {
                        hit_obj.GetComponentInParent<ZombieManager>().DamageHead();
                    }
                }
            }
            //向き発射される向き
            Quaternion rot = _rot_obj.transform.rotation;
            //弾丸生成
            Instantiate(bullet_obj, _hand_obj.transform.position, rot);

            //Pistol内の弾丸を減らす
            pistol_bullet_num--;
            
        }
    }
}

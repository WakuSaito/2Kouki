using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(GameObject _player)
    {
        //ビューポート座標のレイを飛ばす
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit = new RaycastHit();
        Debug.Log("a");

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("a");

            //アイテムまでの距離を調べる
            float distance = Vector3.Distance(hit.transform.position, _player.transform.position);
            Debug.Log(distance);
            //距離が範囲内なら
            if (distance <= 5.0f)
            {
                Debug.Log("b");

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
    }
}

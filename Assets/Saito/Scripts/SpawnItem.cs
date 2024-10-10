using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField]//ゾンビのプレハブ
    GameObject spawn_item_prefab;
    [SerializeField]//当たり判定のサイズの半分(円にしてもいい)
    Vector3 half_collider_size = new Vector3(0.5f,0.5f,0.5f);

    [SerializeField]//1度に生成する量
    int spawn_quantity_min = 3;
    [SerializeField]//1度に生成する量
    int spawn_quantity_max = 5;

    [SerializeField]//生成する範囲(半径)
    float spawn_area_radius = 5.0f;

    // アイテムのインスタンス
    List<GameObject> items = new List<GameObject>();

    private void Start()
    {
        StartSpawn();
    }

    //生成開始
    public void StartSpawn()
    {
        items.Clear();//配列リセット

        //生成する数を決める
        int quantity = Random.Range(spawn_quantity_min, spawn_quantity_max+1);

        //複数生成する
        for (int i = 0; i < quantity; i++) 
        {
            //無限ループが起きないように10回だけ試す
            for (int n = 0; n < 10; n++) 
            {
                //生成位置をランダムに決める
                Vector3 spawn_pos = Random.insideUnitCircle * spawn_area_radius;
                spawn_pos.z = spawn_pos.y;//平面上に生成するため入れ替え
                spawn_pos.y = 0.5f;//y方向は一律にする
                //アタッチしたオブジェクトを基準にする
                spawn_pos.x += transform.position.x;
                spawn_pos.z += transform.position.z;

                //当たり判定が他のものと重ならないとき
                //第4引数で判定しないレイヤーを設定可
                if (!Physics.CheckBox(spawn_pos, half_collider_size))
                {
                    //アイテムをインスタンス化
                    items.Add(Instantiate(
                        spawn_item_prefab,
                        spawn_pos,
                        Quaternion.identity
                        ));

                    break;
                }
            }
        }
    }
}

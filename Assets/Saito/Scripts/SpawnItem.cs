using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField]//生成させるプレハブ
    GameObject[] spawnItemPrefab;
    [SerializeField]//プレハブ生成される確率（重み）
    int[] spawnItemProbability;

    [SerializeField]//生成するオブジェクトのy座標
    float spawnPosY = 1.0f;

    [SerializeField]//当たり判定のサイズの半分(円にしてもいい)
    Vector3 half_collider_size = new Vector3(0.5f,0.5f,0.5f);

    [SerializeField]//1度に生成する量
    int spawn_quantity_min = 3;
    [SerializeField]//1度に生成する量
    int spawn_quantity_max = 5;

    [SerializeField]//生成する範囲(半径)
    float spawn_area_radius = 5.0f;

    [SerializeField]//生成するオブジェクトの親
    Transform spawnParent;

    // アイテムのインスタンス
    List<GameObject> items = new List<GameObject>();

    private void Start()
    {
        StartSpawn();
    }

    //何種類かをランダムで生成できるようにしたい
    //生成開始
    public void StartSpawn()
    {
        if (spawnItemPrefab == null) return;
        //オブジェクトの数より確率の数が少ない
        if (spawnItemPrefab.Length > spawnItemProbability.Length) return;

        items.Clear();//配列リセット

        //生成する数を決める
        int quantity = Random.Range(spawn_quantity_min, spawn_quantity_max+1);

        //確率関連
        int probMax = 0;
        for(int i=0;i< spawnItemPrefab.Length;i++)
        {
            probMax += spawnItemProbability[i];
        }

        //複数生成する
        for (int i = 0; i < quantity; i++) 
        {
            //無限ループが起きないように10回だけ試す
            for (int n = 0; n < 10; n++) 
            {
                //生成位置をランダムに決める
                Vector3 spawn_pos = Random.insideUnitCircle * spawn_area_radius;
                spawn_pos.z = spawn_pos.y;//平面上に生成するため入れ替え
                spawn_pos.y = spawnPosY;//y方向は一律にする
                //アタッチしたオブジェクトを基準にする
                spawn_pos.x += transform.position.x;
                spawn_pos.z += transform.position.z;

                //当たり判定が他のものと重ならないとき
                //第4引数で判定しないレイヤーを設定可
                if (!Physics.CheckBox(spawn_pos, half_collider_size))
                {
                    //確率から生成するオブジェクト決め
                    int randNum = Random.Range(0, probMax) + 1;
                    int tmp = 0;
                    int num = 0;
                    for (int j = 0; j < spawnItemPrefab.Length; j++)
                    {
                        tmp += spawnItemProbability[j];
                        if(randNum <= tmp)
                        {
                            num = j;
                            break;
                        }
                    }


                    if (spawnParent == null)
                    {
                        //アイテムをインスタンス化
                        items.Add(Instantiate(
                            spawnItemPrefab[num],
                            spawn_pos,
                            Quaternion.identity
                            ));

                        break;
                    }
                    else//親を指定
                    {
                        //アイテムをインスタンス化
                        items.Add(Instantiate(
                            spawnItemPrefab[num],
                            spawn_pos,
                            Quaternion.identity,
                            spawnParent
                            ));

                        break;
                    }
                }
            }
        }
    }
}

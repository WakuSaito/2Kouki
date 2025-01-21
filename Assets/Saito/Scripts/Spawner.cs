using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>スポナークラス</para>
/// プレイヤーが近づいたとき、特定のオブジェクトをスポーンさせる
/// </summary>
public class Spawner : MonoBehaviour
{
    //生成させるプレハブ
    [SerializeField] GameObject[] m_spawnObjPrefab;
    //プレハブが生成されるそれぞれの確率（重み）
    [SerializeField] int[] m_spawnObjProbability;

    //1度に生成する数
    [SerializeField] int m_spawnQuantityMin = 3;//最大
    [SerializeField] int m_spawnQuantityMax = 5;//最小

    //生成するオブジェクトのy座標
    [SerializeField] float m_spawnPosY = 1.0f;

    //生成するオブジェクトの当たり判定のサイズの半分(円にしてもいい)
    [SerializeField] Vector3 m_halfColliderSize = new Vector3(0.5f,0.5f,0.5f);
    //生成する範囲(半径)
    [SerializeField] float m_spawnAreaRadius = 5.0f;

    //生成するオブジェクトの親
    [SerializeField] Transform m_spawnParent;

    //生成を開始するプレイヤーとの距離
    [SerializeField] float m_startSpawnDistance = 100.0f;

    // アイテムのインスタンス
    List<GameObject> m_items = new List<GameObject>();

    private GameObject m_playerObj;//プレイヤーオブジェクト

    private void Awake()
    {
        //プレイヤー取得
        m_playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    //プレイヤーが近づいたら生成開始
    private void Update()
    {
        //プレイヤーとの距離計算
        float distance = Vector3.Distance(transform.position, m_playerObj.transform.position);

        //プレイヤーが生成開始範囲に入ったら
        if(distance < m_startSpawnDistance)
        {
            //生成
            StartSpawn();

            //このオブジェクトは必要なくなるので削除
            //変更する可能性アリ
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    /// <summary>
    /// <para>生成開始</para>
    /// インスペクタで指定した条件で、重ならないようにオブジェクトを生成する
    /// </summary>
    public void StartSpawn()
    {
        if (m_spawnObjPrefab == null) return;
        //オブジェクトの数より確率の数が少ない
        if (m_spawnObjPrefab.Length > m_spawnObjProbability.Length) return;

        m_items.Clear();//配列リセット

        //生成する数を決める
        int quantity = Random.Range(m_spawnQuantityMin, m_spawnQuantityMax + 1);

        //確率関連
        int prob_max = 0;
        for(int i=0;i< m_spawnObjPrefab.Length;i++)
        {
            prob_max += m_spawnObjProbability[i];
        }

        //複数生成する
        for (int i = 0; i < quantity; i++) 
        {
            //無限ループが起きないように10回だけ試す
            for (int n = 0; n < 10; n++) 
            {
                //生成位置をランダムに決める
                Vector3 spawn_pos = Random.insideUnitCircle * m_spawnAreaRadius;
                spawn_pos.z = spawn_pos.y;//平面上に生成するため入れ替え
                spawn_pos.y = m_spawnPosY;//y方向は一律にする
                //アタッチしたオブジェクトを基準にする
                spawn_pos.x += transform.position.x;
                spawn_pos.z += transform.position.z;

                //当たり判定が他のものと重ならないとき
                //第4引数で判定しないレイヤーを設定可
                if (!Physics.CheckBox(spawn_pos, m_halfColliderSize))
                {
                    //確率から生成するオブジェクト決め
                    int rand_num = Random.Range(0, prob_max) + 1;
                    int tmp = 0;
                    int num = 0;
                    for (int j = 0; j < m_spawnObjPrefab.Length; j++)
                    {
                        tmp += m_spawnObjProbability[j];
                        if(rand_num <= tmp)
                        {
                            num = j;
                            break;
                        }
                    }


                    if (m_spawnParent == null)
                    {
                        //アイテムをインスタンス化
                        m_items.Add(Instantiate(
                            m_spawnObjPrefab[num],
                            spawn_pos,
                            Quaternion.identity
                            ));

                        break;
                    }
                    else//親を指定
                    {
                        //アイテムをインスタンス化
                        m_items.Add(Instantiate(
                            m_spawnObjPrefab[num],
                            spawn_pos,
                            Quaternion.identity,
                            m_spawnParent
                            ));

                        break;
                    }
                }
            }
        }
    }
}

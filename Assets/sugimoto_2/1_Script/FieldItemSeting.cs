using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItemSeting : MonoBehaviour
{
    /// <summary> 生成する個数 </summary>
    [SerializeField] int m_setMax = 50;
    /// <summary> 生成するアイテム：ITEM_ID順 </summary>
    [SerializeField] GameObject[] m_items;
    /// <summary> まとめるオブジェクト </summary>
    [SerializeField] Transform m_parentTrans;
    /// <summary> プレイヤーオブジェクト </summary>
    [SerializeField] GameObject m_player;

    /// <summary>設置場所</summary>
    List<Transform> m_setPos = new List<Transform>();
    /// <summary>生成したオブジェクトを保存</summary>
    GameObject[] m_setObjSave;
    /// <summary> 設置した位置を保存 </summary>
    int[] m_setRandmNum;
    /// <summary> 再生成のクールタイム </summary>
    float m_spawnCoolTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //設置位置全取得(子要素のみにしてもいいかも)
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("ItemSpawnPos");
        Debug.Log(tmp.Length);
        foreach (var obj in tmp)
        {
            m_setPos.Add(obj.transform);
        }

        System.Array.Resize(ref m_setObjSave, m_setMax);
        System.Array.Resize(ref m_setRandmNum, m_setMax);

        RandomTrans();
        SetRondmItems();
    }

    // Update is called once per frame
    void Update()
    {
        m_spawnCoolTimer += Time.deltaTime;
        if (m_spawnCoolTimer >= 5)
        {
            Debug.Log("change");
            //エリア外にあるアイテムは削除
            m_player.GetComponent<AreaItemSetting>().DeleteOutsideAreaItems();
            GetOutSideAreaItemSpawner();
            RandomTrans();
            SetRondmItems();
            m_player.GetComponent<AreaItemSetting>().GetItemObj();
            m_spawnCoolTimer = 0.0f;
        }

    }

    /// <summary>
    /// 範囲外のスポナーを取得
    /// </summary>
    void GetOutSideAreaItemSpawner()
    {
        m_setPos.Clear();
        m_setPos = m_player.GetComponent<AreaItemSetting>().OutSideItemSpawner();

    }

    int SetMax()
    {
        if (m_setPos.Count > m_setMax)
        {
            return m_setMax;
        }
        else
        {
            return m_setPos.Count;
        }
    }

    /// <summary>
    /// ランダムで生成位置決定
    /// </summary>
    void RandomTrans()
    {
        int set_cnt = 0;
        int set_num = SetMax();

        //生成位置リセット
        ResetRandomTrans();

        while (set_cnt < set_num)
        {
            //Randomで設置場所を決定：同じ位置に生成しないようにする
            int random = Random.Range(0, m_setPos.Count);  //設置場所
            bool can_set_flag = false;

            //すでに決められていなか調べる
            for (int i = 0; i < set_num; i++)
            {
                for (int j = 0; j < set_num; j++)
                {
                    if (m_setRandmNum[i] == random)
                    {
                        can_set_flag = false;
                        break;
                    }
                    else
                    {
                        can_set_flag = true;
                    }
                }

                if (!can_set_flag) break;
            }

            //決定されていなければ設置場所に決定
            if (can_set_flag)
            {
                m_setRandmNum[set_cnt] = random;
                set_cnt++;
            }
        }
    }

    void ResetRandomTrans()
    {
        for (int i = 0; i < m_setMax; i++)
        {
            m_setRandmNum[i] = -1;
        }
    }

    void SetRondmItems()
    {
        int set_cnt = 0;
        int set_num = SetMax();

        while (set_cnt < set_num)
        {
            int item_rate_random = Random.Range(0, 16);//確率設定
            int set_item_random = -1;     //アイテム

            if (item_rate_random >= 0 && item_rate_random < 5)/*5/15*/
            {
                //弾丸
                set_item_random = (int)ITEM_ID.BULLET;
            }
            else if (item_rate_random >= 5 && item_rate_random < 10)/*5/15*/
            {
                //食料
                set_item_random = Random.Range((int)ITEM_ID.FOOD_1, (int)ITEM_ID.FOOD_4 + 1);
            }
            else if (item_rate_random >= 10 && item_rate_random < 12)/*2/15*/
            {
                //飲料
                set_item_random = Random.Range((int)ITEM_ID.DRINK_1, (int)ITEM_ID.DRINK_2 + 1);
            }
            else if (item_rate_random >= 12 && item_rate_random < 14)/*2/15*/
            {
                //回復キット、ピストル
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0: set_item_random = (int)ITEM_ID.EMERGENCY_PACK; break;
                    case 1: set_item_random = (int)ITEM_ID.PISTOL; break;
                }
            }
            else if (item_rate_random >= 15 && item_rate_random < 16)/*1/15*/
            {
                //ショットガン、アサルトライフル
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0: set_item_random = (int)ITEM_ID.ASSAULT; break;
                    case 1: set_item_random = (int)ITEM_ID.SHOTGUN; break;
                }
            }

            //アイテムが決まっていなければやり直す
            if (set_item_random == -1) continue;

            Debug.Log(set_cnt);
            Debug.Log(m_setPos[m_setRandmNum[set_cnt]]);
            //生成
            m_setObjSave[set_cnt] = Instantiate(m_items[set_item_random], m_setPos[m_setRandmNum[set_cnt]].position, Quaternion.identity, m_parentTrans);
            set_cnt++;
        }
    }
}

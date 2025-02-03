using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : ID
{
    const int MAX_SET = 5;

    //設置場所
    List<Transform> m_setPos = new List<Transform>();
    //設置アイテム
    [SerializeField] GameObject[] mItem;

    //設置場所保存
    int[] m_setPosSave;

    [SerializeField] Transform mParent;

    //設置する数
    [SerializeField] int mSetTimes = 0;

    bool m_deleteFlag = false;

    [SerializeField] GameObject m_player;

    /// <summary> 再生成のクールタイム </summary>
    float m_spawnCoolTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //設置位置全取得(子要素のみにしてもいいかも)
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("ItemSpawnPos");
        foreach (var obj in tmp)
            m_setPos.Add(obj.transform);

        //配列の長さ設定
        System.Array.Resize(ref m_setPosSave, m_setPos.Count);

        //配列を-1で初期化
        for (int i = 0; i < m_setPosSave.Length; i++)
        {
            m_setPosSave[i] = -1;
        }

        //設置
        SetItemPos(mSetTimes);
    }

    // Update is called once per frame
    void Update()
    {
        m_spawnCoolTimer += Time.deltaTime;

        //設置したアイテムが残っているかを調べる
        SearchPosItem();

        if (m_spawnCoolTimer >= 120)
        {
            m_player.GetComponent<AreaItemSetting>().OutsideArea();
        }

        //プレイヤーがセーフエリアに入ったらアイテムを削除
        if(m_player.GetComponent<player>().m_inSafeAreaFlag)
        {
            //設置
            DeleteSetItems();
        }
        else
        {
            //プレイヤーがセーフエリアから出ているとき、アイテムが削除されていれば生成
            if(m_deleteFlag)
            {
                SetItemPos(mSetTimes);
                m_player.GetComponent<AreaItemSetting>().GetItemObj();
            }
        }
    }

    public void SetItemPos(int _set_num)  //アイテム設置処理（再設置したいときこの関数を呼び出す）
    {
        int cnt = 0;

        //現在設置している数を調べる
        for (int i = 0; i < m_setPosSave.Length; i++)
        {
            if (m_setPosSave[i] != -1)
            {
                cnt++;
            }
        }


        while (mSetTimes >= cnt)
        {
            //ランダム
            int set_pos_random = Random.Range(0, m_setPos.Count);  //設置場所
            int set_item_random = -1;     //アイテム

            int item_rate_random = Random.Range(0, 16);//確率設定

            if (item_rate_random >= 0 && item_rate_random < 5)/*5/15*/
            {
                //弾丸
                set_item_random = (int)ITEM_ID.BULLET;
            }
            else if (item_rate_random >= 5 && item_rate_random < 9)/*4/15*/
            {
                //食料
                set_item_random = Random.Range((int)ITEM_ID.FOOD_1, (int)ITEM_ID.FOOD_4);
            }
            else if(item_rate_random >= 9 && item_rate_random < 12)/*3/15*/
            {
                //飲料
                set_item_random = Random.Range((int)ITEM_ID.DRINK_1, (int)ITEM_ID.DRINK_2);
            }
            else if(item_rate_random >= 12 && item_rate_random < 14)/*2/15*/
            {
                //回復キット、ピストル
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0: set_item_random = (int)ITEM_ID.EMERGENCY_PACK; break;
                    case 1: set_item_random = (int)ITEM_ID.PISTOL; break;
                }
            }
            else if(item_rate_random >= 15 && item_rate_random < 16)/*1/15*/
            {
                //ショットガン、アサルトライフル
                int random = Random.Range(0, 2);
                switch(random)
                {
                    case 0:set_item_random = (int)ITEM_ID.ASSAULT;break;
                    case 1:set_item_random = (int)ITEM_ID.SHOTGUN;break;
                }
            }

            if (set_item_random == -1) continue;

            //設置可能フラグ
            bool can_set_flag = false;

            //同じ場所に配置しないようにする
            for (int i = 0; i < m_setPosSave.Length; i++)
            {
                if (m_setPosSave[i] == set_pos_random)
                {
                    can_set_flag = false;
                    break;
                }
                else
                {
                    can_set_flag = true;
                }
            }

            if (can_set_flag)
            {
                //生成
                Instantiate(mItem[set_item_random], m_setPos[set_pos_random].position, Quaternion.identity, mParent);
                m_setPosSave[cnt] = set_pos_random;
                cnt++;

            }

            //設置回数が設置場所の最大数を超えたら終了（バグ対策）
            if (cnt >= m_setPos.Count)
            {
                break;
            }
        }

        m_deleteFlag = false;
    }

    void SearchPosItem()
    {
        //アイテムが設置場所にあるか
        for (int i = 0; i < m_setPosSave.Length; i++)
        {
            //設置位置の子が０ならなくなっているので要素を初期化
            if (m_setPosSave[i] != -1 && m_setPos[m_setPosSave[i]].childCount == 0)
            {
                m_setPosSave[i] = -1;
            }            
        }
    }

    /// <summary>
    /// 設置したアイテムをすべて削除
    /// </summary>
    void DeleteSetItems()
    {
        //設置したアイテムををまとめている親オブジェクトの子の数を数える
        int set_cnt = mParent.childCount;

        //残っていれば削除と初期化
        if (set_cnt > 0)
        {
            for (int i = 0; i < set_cnt; i++)
            {
                Destroy(mParent.GetChild(i).gameObject);
                m_setPosSave[i] = -1;
            }
            m_deleteFlag = true;
        }
    }
}

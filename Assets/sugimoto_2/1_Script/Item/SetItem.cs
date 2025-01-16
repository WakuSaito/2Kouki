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

    public bool m_test = false;

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
        SetItemPos();
    }

    // Update is called once per frame
    void Update()
    {
        //設置したアイテムが残っているかを調べる
        SearchPosItem();

        if(m_test)
        {
            //設置
            SetItemPos();
        }
    }

    public void SetItemPos()  //アイテム設置処理（再設置したいときこの関数を呼び出す）
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
            int set_item_random = Random.Range(0, mItem.Length);     //アイテム


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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : ID
{
    const int MAX_SET = 5;

    //設置場所
    [SerializeField] Transform[] set_pos;
    //設置アイテム
    [SerializeField] GameObject[] item;

    //設置場所保存
    int[] set_pos_save;

    [SerializeField] Transform Parent;

    //設置する数
    [SerializeField] int set_times = 0;

    public bool test = false;

    // Start is called before the first frame update
    void Start()
    {
        //配列の長さ設定
        System.Array.Resize(ref set_pos_save, set_pos.Length);

        //配列を-1で初期化
        for (int i = 0; i < set_pos_save.Length; i++)
        {
            set_pos_save[i] = -1;
        }

        //設置
        SetItemPos();
    }

    // Update is called once per frame
    void Update()
    {
        //設置したアイテムが残っているかを調べる
        SearchPosItem();

        if(test)
        {
            //設置
            SetItemPos();
        }
    }

    public void SetItemPos()  //アイテム設置処理（再設置したいときこの関数を呼び出す）
    {
        int cnt = 0;

        //現在設置している数を調べる
        for (int i = 0; i < set_pos_save.Length; i++)
        {
            if (set_pos_save[i] != -1)
            {
                cnt++;
            }
        }


        while (set_times >= cnt)
        {
            //ランダム
            int _set_pos_random = Random.Range(0, set_pos.Length);  //設置場所
            int _set_item_random = Random.Range(0, item.Length);     //アイテム


            //設置可能フラグ
            bool can_set_flag = false;

            //同じ場所に配置しないようにする
            for (int i = 0; i < set_pos_save.Length; i++)
            {
                if (set_pos_save[i] == _set_pos_random)
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
                Instantiate(item[_set_item_random], set_pos[_set_pos_random].position, Quaternion.identity, Parent);
                set_pos_save[cnt] = _set_pos_random;
                cnt++;

            }

            //設置回数が設置場所の最大数を超えたら終了（バグ対策）
            if (cnt >= set_pos.Length)
            {
                break;
            }
        }
    }

    void SearchPosItem()
    {
        //アイテムが設置場所にあるか
        for (int i = 0; i < set_pos_save.Length; i++)
        {
            //設置位置の子が０ならなくなっているので要素を初期化
            if (set_pos_save[i] != -1 && set_pos[set_pos_save[i]].childCount == 0)
            {
                set_pos_save[i] = -1;
            }            
        }
    }
}

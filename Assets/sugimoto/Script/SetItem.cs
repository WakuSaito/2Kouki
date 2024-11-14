using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : ID
{
    const int SET_POS_MAX = 5;

    //設置場所
    [SerializeField] Transform[] set_pos;
    //設置アイテム
    [SerializeField] GameObject[] item;

    //設置場所保存
    int[] set_pos_save = new int[SET_POS_MAX];

    [SerializeField] int set_times = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetItemPos(set_times);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetItemPos(int _set_times)
    {
        int cnt = 0;

        while (_set_times != cnt)
        {
            //ランダム
            int _set_pos_random = Random.Range(0, set_pos.Length);  //設置場所
            int _set_item_random = Random.Range(0, item.Length);     //アイテム

            Debug.Log(_set_pos_random);

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
                Instantiate(item[_set_item_random], set_pos[_set_pos_random].position, Quaternion.identity, set_pos[_set_pos_random]);
                set_pos_save[cnt] = _set_pos_random;
                cnt++;

                //設置回数が設置場所の最大数を超えたら終了（バグ対策）
                if (cnt >= SET_POS_MAX)
                {
                    break;
                }
            }
        }
    }
}

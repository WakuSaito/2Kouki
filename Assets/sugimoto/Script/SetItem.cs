using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : ID
{
    const int SET_POS_MAX = 5;

    //�ݒu�ꏊ
    [SerializeField] Transform[] set_pos;
    //�ݒu�A�C�e��
    [SerializeField] GameObject[] item;

    //�ݒu�ꏊ�ۑ�
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
            //�����_��
            int _set_pos_random = Random.Range(0, set_pos.Length);  //�ݒu�ꏊ
            int _set_item_random = Random.Range(0, item.Length);     //�A�C�e��

            Debug.Log(_set_pos_random);

            //�ݒu�\�t���O
            bool can_set_flag = false;

            //�����ꏊ�ɔz�u���Ȃ��悤�ɂ���
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
                //����
                Instantiate(item[_set_item_random], set_pos[_set_pos_random].position, Quaternion.identity, set_pos[_set_pos_random]);
                set_pos_save[cnt] = _set_pos_random;
                cnt++;

                //�ݒu�񐔂��ݒu�ꏊ�̍ő吔�𒴂�����I���i�o�O�΍�j
                if (cnt >= SET_POS_MAX)
                {
                    break;
                }
            }
        }
    }
}

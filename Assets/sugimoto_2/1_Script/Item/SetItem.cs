using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : ID
{
    const int MAX_SET = 5;

    //�ݒu�ꏊ
    [SerializeField] Transform[] set_pos;
    //�ݒu�A�C�e��
    [SerializeField] GameObject[] item;

    //�ݒu�ꏊ�ۑ�
    int[] set_pos_save;

    [SerializeField] Transform Parent;

    //�ݒu���鐔
    [SerializeField] int set_times = 0;

    public bool test = false;

    // Start is called before the first frame update
    void Start()
    {
        //�z��̒����ݒ�
        System.Array.Resize(ref set_pos_save, set_pos.Length);

        //�z���-1�ŏ�����
        for (int i = 0; i < set_pos_save.Length; i++)
        {
            set_pos_save[i] = -1;
        }

        //�ݒu
        SetItemPos();
    }

    // Update is called once per frame
    void Update()
    {
        //�ݒu�����A�C�e�����c���Ă��邩�𒲂ׂ�
        SearchPosItem();

        if(test)
        {
            //�ݒu
            SetItemPos();
        }
    }

    public void SetItemPos()  //�A�C�e���ݒu�����i�Đݒu�������Ƃ����̊֐����Ăяo���j
    {
        int cnt = 0;

        //���ݐݒu���Ă��鐔�𒲂ׂ�
        for (int i = 0; i < set_pos_save.Length; i++)
        {
            if (set_pos_save[i] != -1)
            {
                cnt++;
            }
        }


        while (set_times >= cnt)
        {
            //�����_��
            int _set_pos_random = Random.Range(0, set_pos.Length);  //�ݒu�ꏊ
            int _set_item_random = Random.Range(0, item.Length);     //�A�C�e��


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
                Instantiate(item[_set_item_random], set_pos[_set_pos_random].position, Quaternion.identity, Parent);
                set_pos_save[cnt] = _set_pos_random;
                cnt++;

            }

            //�ݒu�񐔂��ݒu�ꏊ�̍ő吔�𒴂�����I���i�o�O�΍�j
            if (cnt >= set_pos.Length)
            {
                break;
            }
        }
    }

    void SearchPosItem()
    {
        //�A�C�e�����ݒu�ꏊ�ɂ��邩
        for (int i = 0; i < set_pos_save.Length; i++)
        {
            //�ݒu�ʒu�̎q���O�Ȃ�Ȃ��Ȃ��Ă���̂ŗv�f��������
            if (set_pos_save[i] != -1 && set_pos[set_pos_save[i]].childCount == 0)
            {
                set_pos_save[i] = -1;
            }            
        }
    }
}

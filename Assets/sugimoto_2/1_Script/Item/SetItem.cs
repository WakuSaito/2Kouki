using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : ID
{
    const int MAX_SET = 5;

    //�ݒu�ꏊ
    List<Transform> m_setPos = new List<Transform>();
    //�ݒu�A�C�e��
    [SerializeField] GameObject[] mItem;

    //�ݒu�ꏊ�ۑ�
    int[] m_setPosSave;

    [SerializeField] Transform mParent;

    //�ݒu���鐔
    [SerializeField] int mSetTimes = 0;

    public bool m_test = false;

    // Start is called before the first frame update
    void Start()
    {
        //�ݒu�ʒu�S�擾(�q�v�f�݂̂ɂ��Ă���������)
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("ItemSpawnPos");
        foreach (var obj in tmp)
            m_setPos.Add(obj.transform);

        //�z��̒����ݒ�
        System.Array.Resize(ref m_setPosSave, m_setPos.Count);

        //�z���-1�ŏ�����
        for (int i = 0; i < m_setPosSave.Length; i++)
        {
            m_setPosSave[i] = -1;
        }

        //�ݒu
        SetItemPos();
    }

    // Update is called once per frame
    void Update()
    {
        //�ݒu�����A�C�e�����c���Ă��邩�𒲂ׂ�
        SearchPosItem();

        if(m_test)
        {
            //�ݒu
            SetItemPos();
        }
    }

    public void SetItemPos()  //�A�C�e���ݒu�����i�Đݒu�������Ƃ����̊֐����Ăяo���j
    {
        int cnt = 0;

        //���ݐݒu���Ă��鐔�𒲂ׂ�
        for (int i = 0; i < m_setPosSave.Length; i++)
        {
            if (m_setPosSave[i] != -1)
            {
                cnt++;
            }
        }


        while (mSetTimes >= cnt)
        {
            //�����_��
            int set_pos_random = Random.Range(0, m_setPos.Count);  //�ݒu�ꏊ
            int set_item_random = Random.Range(0, mItem.Length);     //�A�C�e��


            //�ݒu�\�t���O
            bool can_set_flag = false;

            //�����ꏊ�ɔz�u���Ȃ��悤�ɂ���
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
                //����
                Instantiate(mItem[set_item_random], m_setPos[set_pos_random].position, Quaternion.identity, mParent);
                m_setPosSave[cnt] = set_pos_random;
                cnt++;

            }

            //�ݒu�񐔂��ݒu�ꏊ�̍ő吔�𒴂�����I���i�o�O�΍�j
            if (cnt >= m_setPos.Count)
            {
                break;
            }
        }
    }

    void SearchPosItem()
    {
        //�A�C�e�����ݒu�ꏊ�ɂ��邩
        for (int i = 0; i < m_setPosSave.Length; i++)
        {
            //�ݒu�ʒu�̎q���O�Ȃ�Ȃ��Ȃ��Ă���̂ŗv�f��������
            if (m_setPosSave[i] != -1 && m_setPos[m_setPosSave[i]].childCount == 0)
            {
                m_setPosSave[i] = -1;
            }            
        }
    }
}

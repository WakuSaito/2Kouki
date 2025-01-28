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

    bool m_deleteFlag = false;

    [SerializeField] GameObject m_player;

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

        //�v���C���[���Z�[�t�G���A�ɓ�������A�C�e�����폜
        if(m_player.GetComponent<player>().m_inSafeAreaFlag)
        {
            //�ݒu
            DeleteSetItems();
        }
        else
        {
            //�v���C���[���Z�[�t�G���A����o�Ă���Ƃ��A�A�C�e�����폜����Ă���ΐ���
            if(m_deleteFlag)
            {
                SetItemPos();
                m_player.GetComponent<AreaItemSetting>().GetItemObj();
            }
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
            int set_item_random = -1;     //�A�C�e��

            int item_rate_random = Random.Range(0, 16);//�m���ݒ�

            if (item_rate_random >= 0 && item_rate_random < 5)/*5/15*/
            {
                //�e��
                set_item_random = (int)ITEM_ID.BULLET;
            }
            else if (item_rate_random >= 5 && item_rate_random < 9)/*4/15*/
            {
                //�H��
                set_item_random = Random.Range((int)ITEM_ID.FOOD_1, (int)ITEM_ID.FOOD_4);
            }
            else if(item_rate_random >= 9 && item_rate_random < 12)/*3/15*/
            {
                //����
                set_item_random = Random.Range((int)ITEM_ID.DRINK_1, (int)ITEM_ID.DRINK_2);
            }
            else if(item_rate_random >= 12 && item_rate_random < 14)/*2/15*/
            {
                //�񕜃L�b�g�A�s�X�g��
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0: set_item_random = (int)ITEM_ID.EMERGENCY_PACK; break;
                    case 1: set_item_random = (int)ITEM_ID.PISTOL; break;
                }
            }
            else if(item_rate_random >= 15 && item_rate_random < 16)/*1/15*/
            {
                //�V���b�g�K���A�A�T���g���C�t��
                int random = Random.Range(0, 2);
                switch(random)
                {
                    case 0:set_item_random = (int)ITEM_ID.ASSAULT;break;
                    case 1:set_item_random = (int)ITEM_ID.SHOTGUN;break;
                }
            }

            if (set_item_random == -1) continue;

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

        m_deleteFlag = false;
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

    /// <summary>
    /// �ݒu�����A�C�e�������ׂč폜
    /// </summary>
    void DeleteSetItems()
    {
        //�ݒu�����A�C�e�������܂Ƃ߂Ă���e�I�u�W�F�N�g�̎q�̐��𐔂���
        int set_cnt = mParent.childCount;

        //�c���Ă���΍폜�Ə�����
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

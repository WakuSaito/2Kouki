using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItemSeting : MonoBehaviour
{
    /// <summary> ��������� </summary>
    [SerializeField] int m_setMax = 50;
    /// <summary> ��������A�C�e���FITEM_ID�� </summary>
    [SerializeField] GameObject[] m_items;
    /// <summary> �܂Ƃ߂�I�u�W�F�N�g </summary>
    [SerializeField] Transform m_parentTrans;
    /// <summary> �v���C���[�I�u�W�F�N�g </summary>
    [SerializeField] GameObject m_player;

    /// <summary>�ݒu�ꏊ</summary>
    List<Transform> m_setPos = new List<Transform>();
    /// <summary>���������I�u�W�F�N�g��ۑ�</summary>
    GameObject[] m_setObjSave;
    /// <summary> �ݒu�����ʒu��ۑ� </summary>
    int[] m_setRandmNum;
    /// <summary> �Đ����̃N�[���^�C�� </summary>
    float m_spawnCoolTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //�ݒu�ʒu�S�擾(�q�v�f�݂̂ɂ��Ă���������)
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("ItemSpawnPos");

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
        if (m_spawnCoolTimer >= 240)
        {
            Debug.Log("change");
            //�G���A�O�ɂ���A�C�e���͍폜
            m_player.GetComponent<AreaItemSetting>().DeleteOutsideAreaItems();
            GetOutSideAreaItemSpawner();
            RandomTrans();
            SetRondmItems();
            m_player.GetComponent<AreaItemSetting>().GetItemObj();
            m_spawnCoolTimer = 0.0f;
        }

    }

    /// <summary>
    /// �͈͊O�̃X�|�i�[���擾
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
    /// �����_���Ő����ʒu����
    /// </summary>
    void RandomTrans()
    {
        int set_cnt = 0;
        int set_num = SetMax();

        //�����ʒu���Z�b�g
        ResetRandomTrans();

        while (set_cnt < set_num)
        {
            //Random�Őݒu�ꏊ������F�����ʒu�ɐ������Ȃ��悤�ɂ���
            int random = Random.Range(0, m_setPos.Count);  //�ݒu�ꏊ
            bool can_set_flag = false;

            //���łɌ��߂��Ă��Ȃ����ׂ�
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

            //���肳��Ă��Ȃ���ΐݒu�ꏊ�Ɍ���
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

        int rate_1 = 5;
        int rate_2 = 4 + rate_1;
        int rate_3 = 3 + rate_2;
        int rate_4 = 2 + rate_3;
        int rate_5 = 1 + rate_4;

        while (set_cnt < set_num)
        {
            int item_rate_random = Random.Range(0, 15 + 1);//�m���ݒ�
            int set_item_random = -1;     //�A�C�e��

            //����
            if (item_rate_random >= 0 && item_rate_random < 5)
            {
                //�H��
                set_item_random = Random.Range((int)ITEM_ID.FOOD_1, (int)ITEM_ID.FOOD_4 + 1);
            }

            if (item_rate_random >= 5 && item_rate_random < 9)
            {
                //�e��
                set_item_random = (int)ITEM_ID.BULLET;
            }
            
            if (item_rate_random >= 9 && item_rate_random < 12)
            {
                //�s�X�g��
                set_item_random = (int)ITEM_ID.PISTOL;
            }
            
            if (item_rate_random >= 12 && item_rate_random < 14)
            {
                //�����A�񕜃L�b�g
                int random = Random.Range(0, 2 + 1);

                switch (random)
                {
                    case 0:
                        set_item_random = (int)ITEM_ID.DRINK_1;
                        break;
                    case 1:
                        set_item_random = (int)ITEM_ID.DRINK_2;
                        break;
                    case 2:
                        set_item_random = (int)ITEM_ID.EMERGENCY_PACK;
                        break;
                }
            }

            if (item_rate_random >= 14 && item_rate_random < 15)
            {
                //�V���b�g�K���A�A�T���g���C�t��
                int random = Random.Range(0, 1 + 1);

                switch (random)
                {
                    case 0:
                        set_item_random = (int)ITEM_ID.ASSAULT;
                        break;
                    case 1:
                        set_item_random = (int)ITEM_ID.SHOTGUN;
                        break;
                }

            }
            //�Ⴂ

            //�A�C�e�������܂��Ă��Ȃ���΂�蒼��
            if (set_item_random == -1) continue;

            //����
            m_setObjSave[set_cnt] = Instantiate(m_items[set_item_random], m_setPos[m_setRandmNum[set_cnt]].position, Quaternion.identity, m_parentTrans);
            set_cnt++;
        }
    }
}

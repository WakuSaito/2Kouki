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

        SetRondmItems();
    }

    // Update is called once per frame
    void Update()
    {
        m_spawnCoolTimer += Time.deltaTime;
        if (m_spawnCoolTimer >= 5)
        {
            Debug.Log("aaaaaaaaaaaaaaaa");
            //�G���A�O�ɂ���A�C�e���͍폜
            m_player.GetComponent<AreaItemSetting>().OutsideArea();
            ResetRandomTrans();
            SetRondmItems();
            m_player.GetComponent<AreaItemSetting>().GetItemObj();
            m_spawnCoolTimer = 0.0f;
        }

    }

    void RandomTrans()
    {
        int set_cnt = 0;
        while (set_cnt < m_setMax)
        {
            //�I�u�W�F�N�g���c���Ă���ꍇ�̓X�L�b�v
            if (m_setObjSave[set_cnt] != null)
            {
                set_cnt++;
                continue;
            }

            //Random�Őݒu�ꏊ������F�����ʒu�ɐ������Ȃ��悤�ɂ���
            int random = Random.Range(0, m_setPos.Count);  //�ݒu�ꏊ
            bool can_set_flag = false;

            //���łɌ��߂��Ă��Ȃ����ׂ�
            for (int i = 0; i < m_setMax; i++)
            {
                for (int j = 0; j < m_setMax; j++)
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
            if (m_setObjSave[i] != null) continue;
            Debug.Log(m_setObjSave[i]);
            m_setRandmNum[i] = -1;
        }
    }

    void SetRondmItems()
    {
        //���g�����Z�b�g
        ResetRandomTrans();
        //�ݒu�ʒu����
        RandomTrans();

        int set_cnt = 0;

        while (set_cnt < m_setMax)
        {
            int item_rate_random = Random.Range(0, 16);//�m���ݒ�
            int set_item_random = -1;     //�A�C�e��

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
            else if (item_rate_random >= 9 && item_rate_random < 12)/*3/15*/
            {
                //����
                set_item_random = Random.Range((int)ITEM_ID.DRINK_1, (int)ITEM_ID.DRINK_2);
            }
            else if (item_rate_random >= 12 && item_rate_random < 14)/*2/15*/
            {
                //�񕜃L�b�g�A�s�X�g��
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0: set_item_random = (int)ITEM_ID.EMERGENCY_PACK; break;
                    case 1: set_item_random = (int)ITEM_ID.PISTOL; break;
                }
            }
            else if (item_rate_random >= 15 && item_rate_random < 16)/*1/15*/
            {
                //�V���b�g�K���A�A�T���g���C�t��
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0: set_item_random = (int)ITEM_ID.ASSAULT; break;
                    case 1: set_item_random = (int)ITEM_ID.SHOTGUN; break;
                }
            }

            //�A�C�e�������܂��Ă��Ȃ���΂�蒼��
            if (set_item_random == -1) continue;

            //����
            m_setObjSave[set_cnt] = Instantiate(m_items[set_item_random], m_setPos[set_cnt].position, Quaternion.identity, m_parentTrans);
            set_cnt++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]//����������v���n�u
    GameObject[] m_spawnObjPrefab;
    [SerializeField]//�v���n�u���������m���i�d�݁j
    int[] m_spawnObjProbability;

    [SerializeField]//��������I�u�W�F�N�g��y���W
    float m_spawnPosY = 1.0f;

    [SerializeField]//��������I�u�W�F�N�g�̓����蔻��̃T�C�Y�̔���(�~�ɂ��Ă�����)
    Vector3 m_halfColliderSize = new Vector3(0.5f,0.5f,0.5f);

    [SerializeField]//1�x�ɐ��������
    int m_spawnQuantityMin = 3;
    [SerializeField]//1�x�ɐ��������
    int m_spawnQuantityMax = 5;

    [SerializeField]//��������͈�(���a)
    float m_spawnAreaRadius = 5.0f;

    [SerializeField]//��������I�u�W�F�N�g�̐e
    Transform m_spawnParent;

    [SerializeField]//�������J�n����v���C���[�Ƃ̋���
    float m_startSpawnDistance = 100.0f;

    // �A�C�e���̃C���X�^���X
    List<GameObject> m_items = new List<GameObject>();

    private GameObject m_playerObj;//�v���C���[�I�u�W�F�N�g

    private void Awake()
    {
        //�v���C���[�擾
        m_playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        //�v���C���[�Ƃ̋����v�Z
        float distance = Vector3.Distance(transform.position, m_playerObj.transform.position);

        //�v���C���[�������J�n�͈͂ɓ�������
        if(distance < m_startSpawnDistance)
        {
            //����
            StartSpawn();

            //���̃I�u�W�F�N�g�͕K�v�Ȃ��Ȃ�̂ō폜
            //�ύX����\���A��
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    //����ނ��������_���Ő����ł���悤�ɂ�����
    //�����J�n
    public void StartSpawn()
    {
        if (m_spawnObjPrefab == null) return;
        //�I�u�W�F�N�g�̐����m���̐������Ȃ�
        if (m_spawnObjPrefab.Length > m_spawnObjProbability.Length) return;

        m_items.Clear();//�z�񃊃Z�b�g

        //�������鐔�����߂�
        int quantity = Random.Range(m_spawnQuantityMin, m_spawnQuantityMax + 1);

        //�m���֘A
        int prob_max = 0;
        for(int i=0;i< m_spawnObjPrefab.Length;i++)
        {
            prob_max += m_spawnObjProbability[i];
        }

        //������������
        for (int i = 0; i < quantity; i++) 
        {
            //�������[�v���N���Ȃ��悤��10�񂾂�����
            for (int n = 0; n < 10; n++) 
            {
                //�����ʒu�������_���Ɍ��߂�
                Vector3 spawn_pos = Random.insideUnitCircle * m_spawnAreaRadius;
                spawn_pos.z = spawn_pos.y;//���ʏ�ɐ������邽�ߓ���ւ�
                spawn_pos.y = m_spawnPosY;//y�����͈ꗥ�ɂ���
                //�A�^�b�`�����I�u�W�F�N�g����ɂ���
                spawn_pos.x += transform.position.x;
                spawn_pos.z += transform.position.z;

                //�����蔻�肪���̂��̂Əd�Ȃ�Ȃ��Ƃ�
                //��4�����Ŕ��肵�Ȃ����C���[��ݒ��
                if (!Physics.CheckBox(spawn_pos, m_halfColliderSize))
                {
                    //�m�����琶������I�u�W�F�N�g����
                    int rand_num = Random.Range(0, prob_max) + 1;
                    int tmp = 0;
                    int num = 0;
                    for (int j = 0; j < m_spawnObjPrefab.Length; j++)
                    {
                        tmp += m_spawnObjProbability[j];
                        if(rand_num <= tmp)
                        {
                            num = j;
                            break;
                        }
                    }


                    if (m_spawnParent == null)
                    {
                        //�A�C�e�����C���X�^���X��
                        m_items.Add(Instantiate(
                            m_spawnObjPrefab[num],
                            spawn_pos,
                            Quaternion.identity
                            ));

                        break;
                    }
                    else//�e���w��
                    {
                        //�A�C�e�����C���X�^���X��
                        m_items.Add(Instantiate(
                            m_spawnObjPrefab[num],
                            spawn_pos,
                            Quaternion.identity,
                            m_spawnParent
                            ));

                        break;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�X�|�i�[�N���X</para>
/// �v���C���[���߂Â����Ƃ��A����̃I�u�W�F�N�g���X�|�[��������
/// </summary>
public class Spawner : MonoBehaviour
{
    //����������v���n�u
    [SerializeField] GameObject[] m_spawnObjPrefab;
    //�v���n�u����������邻�ꂼ��̊m���i�d�݁j
    [SerializeField] int[] m_spawnObjProbability;

    //1�x�ɐ������鐔
    [SerializeField] int m_spawnQuantityMin = 3;//�ő�
    [SerializeField] int m_spawnQuantityMax = 5;//�ŏ�

    //��������I�u�W�F�N�g��y���W
    [SerializeField] float m_spawnPosY = 1.0f;

    //��������I�u�W�F�N�g�̓����蔻��̃T�C�Y�̔���(�~�ɂ��Ă�����)
    [SerializeField] Vector3 m_halfColliderSize = new Vector3(0.5f,0.5f,0.5f);
    //��������͈�(���a)
    [SerializeField] float m_spawnAreaRadius = 5.0f;

    //��������I�u�W�F�N�g�̐e
    [SerializeField] Transform m_spawnParent;

    //�������J�n����v���C���[�Ƃ̋���
    [SerializeField] float m_startSpawnDistance = 40.0f;
    //�ĂїL�����ɂȂ�v���C���[�Ƃ̋���
    [SerializeField] float m_reActivePlayerDistance = 100.0f;

    // �A�C�e���̃C���X�^���X
    List<GameObject> m_items = new List<GameObject>();

    //�v���C���[�I�u�W�F�N�g
    private GameObject m_playerObj;
    //�f�o�b�O�p�V�[���ł̕\���؂�ւ�
    private MeshRenderer m_meshRenderer;

    //�X�|�i�[�̗L����
    private bool m_isActive = true;

    private void Awake()
    {
        //�v���C���[�擾
        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        //�f�o�b�O�p
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    //�v���C���[���߂Â����琶���J�n
    private void Update()
    {

        //�v���C���[�Ƃ̋����v�Z
        float distance = Vector3.Distance(transform.position, m_playerObj.transform.position);

        //�v���C���[�Ɨ��ꂷ������L����
        if (!m_isActive &&
            distance > m_reActivePlayerDistance)
        {
            //�ēx�X�|�[���\��
            m_isActive = true;
            //�f�o�b�O�p�ɉ���
            m_meshRenderer.enabled = true;
        }
        else if (m_isActive &&
                 distance < m_startSpawnDistance)//�v���C���[�������J�n�͈͂ɓ�������
        {         
            //����
            StartSpawn();

            //���x�������Ȃ��悤��
            m_isActive = false;
            //�f�o�b�O�p�ɔ�\��
            m_meshRenderer.enabled = false;
        }
    }

    /// <summary>
    /// <para>�����J�n</para>
    /// �C���X�y�N�^�Ŏw�肵�������ŁA�d�Ȃ�Ȃ��悤�ɃI�u�W�F�N�g�𐶐�����
    /// </summary>
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

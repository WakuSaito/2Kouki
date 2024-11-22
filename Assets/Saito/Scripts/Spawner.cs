using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]//����������v���n�u
    GameObject[] spawnObjPrefab;
    [SerializeField]//�v���n�u���������m���i�d�݁j
    int[] spawnObjProbability;

    [SerializeField]//��������I�u�W�F�N�g��y���W
    float spawnPosY = 1.0f;

    [SerializeField]//��������I�u�W�F�N�g�̓����蔻��̃T�C�Y�̔���(�~�ɂ��Ă�����)
    Vector3 halfColliderSize = new Vector3(0.5f,0.5f,0.5f);

    [SerializeField]//1�x�ɐ��������
    int spawnQuantityMin = 3;
    [SerializeField]//1�x�ɐ��������
    int spawnQuantityMax = 5;

    [SerializeField]//��������͈�(���a)
    float spawnAreaRadius = 5.0f;

    [SerializeField]//��������I�u�W�F�N�g�̐e
    Transform spawnParent;

    [SerializeField]//�������J�n����v���C���[�Ƃ̋���
    float startSpawnDistance = 100.0f;

    // �A�C�e���̃C���X�^���X
    List<GameObject> items = new List<GameObject>();

    private GameObject playerObj;//�v���C���[�I�u�W�F�N�g

    private void Awake()
    {
        //�v���C���[�擾
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        //�v���C���[�Ƃ̋����v�Z
        float distance = Vector3.Distance(transform.position, playerObj.transform.position);

        //�v���C���[�������J�n�͈͂ɓ�������
        if(distance < startSpawnDistance)
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
        if (spawnObjPrefab == null) return;
        //�I�u�W�F�N�g�̐����m���̐������Ȃ�
        if (spawnObjPrefab.Length > spawnObjProbability.Length) return;

        items.Clear();//�z�񃊃Z�b�g

        //�������鐔�����߂�
        int quantity = Random.Range(spawnQuantityMin, spawnQuantityMax + 1);

        //�m���֘A
        int probMax = 0;
        for(int i=0;i< spawnObjPrefab.Length;i++)
        {
            probMax += spawnObjProbability[i];
        }

        //������������
        for (int i = 0; i < quantity; i++) 
        {
            //�������[�v���N���Ȃ��悤��10�񂾂�����
            for (int n = 0; n < 10; n++) 
            {
                //�����ʒu�������_���Ɍ��߂�
                Vector3 spawn_pos = Random.insideUnitCircle * spawnAreaRadius;
                spawn_pos.z = spawn_pos.y;//���ʏ�ɐ������邽�ߓ���ւ�
                spawn_pos.y = spawnPosY;//y�����͈ꗥ�ɂ���
                //�A�^�b�`�����I�u�W�F�N�g����ɂ���
                spawn_pos.x += transform.position.x;
                spawn_pos.z += transform.position.z;

                //�����蔻�肪���̂��̂Əd�Ȃ�Ȃ��Ƃ�
                //��4�����Ŕ��肵�Ȃ����C���[��ݒ��
                if (!Physics.CheckBox(spawn_pos, halfColliderSize))
                {
                    //�m�����琶������I�u�W�F�N�g����
                    int randNum = Random.Range(0, probMax) + 1;
                    int tmp = 0;
                    int num = 0;
                    for (int j = 0; j < spawnObjPrefab.Length; j++)
                    {
                        tmp += spawnObjProbability[j];
                        if(randNum <= tmp)
                        {
                            num = j;
                            break;
                        }
                    }


                    if (spawnParent == null)
                    {
                        //�A�C�e�����C���X�^���X��
                        items.Add(Instantiate(
                            spawnObjPrefab[num],
                            spawn_pos,
                            Quaternion.identity
                            ));

                        break;
                    }
                    else//�e���w��
                    {
                        //�A�C�e�����C���X�^���X��
                        items.Add(Instantiate(
                            spawnObjPrefab[num],
                            spawn_pos,
                            Quaternion.identity,
                            spawnParent
                            ));

                        break;
                    }
                }
            }
        }
    }
}

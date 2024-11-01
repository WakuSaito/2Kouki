using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField]//����������v���n�u
    GameObject[] spawnItemPrefab;
    [SerializeField]//�v���n�u���������m���i�d�݁j
    int[] spawnItemProbability;

    [SerializeField]//��������I�u�W�F�N�g��y���W
    float spawnPosY = 1.0f;

    [SerializeField]//�����蔻��̃T�C�Y�̔���(�~�ɂ��Ă�����)
    Vector3 half_collider_size = new Vector3(0.5f,0.5f,0.5f);

    [SerializeField]//1�x�ɐ��������
    int spawn_quantity_min = 3;
    [SerializeField]//1�x�ɐ��������
    int spawn_quantity_max = 5;

    [SerializeField]//��������͈�(���a)
    float spawn_area_radius = 5.0f;

    [SerializeField]//��������I�u�W�F�N�g�̐e
    Transform spawnParent;

    // �A�C�e���̃C���X�^���X
    List<GameObject> items = new List<GameObject>();

    private void Start()
    {
        StartSpawn();
    }

    //����ނ��������_���Ő����ł���悤�ɂ�����
    //�����J�n
    public void StartSpawn()
    {
        if (spawnItemPrefab == null) return;
        //�I�u�W�F�N�g�̐����m���̐������Ȃ�
        if (spawnItemPrefab.Length > spawnItemProbability.Length) return;

        items.Clear();//�z�񃊃Z�b�g

        //�������鐔�����߂�
        int quantity = Random.Range(spawn_quantity_min, spawn_quantity_max+1);

        //�m���֘A
        int probMax = 0;
        for(int i=0;i< spawnItemPrefab.Length;i++)
        {
            probMax += spawnItemProbability[i];
        }

        //������������
        for (int i = 0; i < quantity; i++) 
        {
            //�������[�v���N���Ȃ��悤��10�񂾂�����
            for (int n = 0; n < 10; n++) 
            {
                //�����ʒu�������_���Ɍ��߂�
                Vector3 spawn_pos = Random.insideUnitCircle * spawn_area_radius;
                spawn_pos.z = spawn_pos.y;//���ʏ�ɐ������邽�ߓ���ւ�
                spawn_pos.y = spawnPosY;//y�����͈ꗥ�ɂ���
                //�A�^�b�`�����I�u�W�F�N�g����ɂ���
                spawn_pos.x += transform.position.x;
                spawn_pos.z += transform.position.z;

                //�����蔻�肪���̂��̂Əd�Ȃ�Ȃ��Ƃ�
                //��4�����Ŕ��肵�Ȃ����C���[��ݒ��
                if (!Physics.CheckBox(spawn_pos, half_collider_size))
                {
                    //�m�����琶������I�u�W�F�N�g����
                    int randNum = Random.Range(0, probMax) + 1;
                    int tmp = 0;
                    int num = 0;
                    for (int j = 0; j < spawnItemPrefab.Length; j++)
                    {
                        tmp += spawnItemProbability[j];
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
                            spawnItemPrefab[num],
                            spawn_pos,
                            Quaternion.identity
                            ));

                        break;
                    }
                    else//�e���w��
                    {
                        //�A�C�e�����C���X�^���X��
                        items.Add(Instantiate(
                            spawnItemPrefab[num],
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField]//�]���r�̃v���n�u
    GameObject spawn_item_prefab;
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

    //�����J�n
    public void StartSpawn()
    {
        items.Clear();//�z�񃊃Z�b�g

        //�������鐔�����߂�
        int quantity = Random.Range(spawn_quantity_min, spawn_quantity_max+1);

        //������������
        for (int i = 0; i < quantity; i++) 
        {
            //�������[�v���N���Ȃ��悤��10�񂾂�����
            for (int n = 0; n < 10; n++) 
            {
                //�����ʒu�������_���Ɍ��߂�
                Vector3 spawn_pos = Random.insideUnitCircle * spawn_area_radius;
                spawn_pos.z = spawn_pos.y;//���ʏ�ɐ������邽�ߓ���ւ�
                spawn_pos.y = 0.5f;//y�����͈ꗥ�ɂ���
                //�A�^�b�`�����I�u�W�F�N�g����ɂ���
                spawn_pos.x += transform.position.x;
                spawn_pos.z += transform.position.z;

                //�����蔻�肪���̂��̂Əd�Ȃ�Ȃ��Ƃ�
                //��4�����Ŕ��肵�Ȃ����C���[��ݒ��
                if (!Physics.CheckBox(spawn_pos, half_collider_size))
                {
                    if (spawnParent == null)
                    {
                        //�A�C�e�����C���X�^���X��
                        items.Add(Instantiate(
                            spawn_item_prefab,
                            spawn_pos,
                            Quaternion.identity
                            ));

                        break;
                    }
                    else//�e���w��
                    {
                        //�A�C�e�����C���X�^���X��
                        items.Add(Instantiate(
                            spawn_item_prefab,
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

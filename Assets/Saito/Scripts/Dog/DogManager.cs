using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
��قǃN���X�𕪊�������
�E�ړ��N���X
�E�A�j���[�V�����N���X
�E���̑��A�N�V�������Ƃɕ���
*/

/// <summary>
/// ���̃}�l�[�W���[�N���X
/// </summary>
public class DogManager : MonoBehaviour
{
    //�U���ΏۃI�u�W�F�N�g
    private GameObject attackTargetObj;

    //�U���Ώۂɓːi��
    private bool isChargeTarget = false;
    //�s����~
    private bool isStopAction = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isChargeTarget)//�ːi
        {
            //�U���Ώۂ����݂��Ȃ��Ȃ�return
            //�U���Ώۂ��r����Destroy���ꂽ�ꍇ�̋�������
            if (attackTargetObj == null)
            {
                isChargeTarget = false;//�ːi���f
                return;
            }

            //attackTargetObj�̈ʒu�Ɍ������Ĉړ�����

            //�U���ΏۂɌ���Ȃ��߂Â�����
            //���݂�
        }
        else//�ʏ펞�̈ړ�
        {
            //if(�v���C���[�Ƃ̋���������Ă���)
            //�v���C���[�̕��Ɉړ�����
            //��Q���̔���ɒ���

            //else if(�߂��Ȃ�)
            //�Ƃ肠�����͉������Ȃ�
            //�v���C���[�̋߂��ŃE���E�����������@�͈͓��̃����_���ʒu��ڕW�Ƃ��Ĉړ��݂�����
        }
    }

    /// <summary>
    /// �U���w�����󂯂��Ƃ�
    /// </summary>
    public void OrderAttack(GameObject _obj)
    {
        isChargeTarget = true;

        attackTargetObj = _obj;//�U���Ώۂ��擾
    }

    /// <summary>
    /// �]���r�Ɋ��݂�
    /// </summary>
    private void BiteZombie(GameObject _zombieObj)
    {
        //attackTargetObj����ZombieManager���擾���AFreezePosition���Ăяo��

        isStopAction = true;
    }
}

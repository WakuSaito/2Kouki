using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

/*
��قǃN���X�𕪊�������
�E�ړ��N���X
�E�A�j���[�V�����N���X
�E���̑��A�N�V�������Ƃɕ���
*/

//�����ŃA�^�b�`�����X�N���v�g
[RequireComponent(typeof(DogMove))]
[RequireComponent(typeof(DogAnimation))]

/// <summary>
/// ���̃}�l�[�W���[�N���X
/// </summary>
public class DogManager : MonoBehaviour
{
    /// <summary>
    /// ���삷��N���X
    /// </summary>
    [SerializeField]
    private DogBase[] dogBases;

    private DogMove dogMove;
    private DogAnimation dogAnimation;

    [SerializeField]//���݂��Ă��鎞��
    private double biteStaySec = 4.0;

    [SerializeField]//�ҋ@��ԂɂȂ�v���C���[�Ƃ̋���
    private float stayPlayerDistance = 5.0f;


    //�U���ΏۃI�u�W�F�N�g
    private GameObject attackTargetObj;
    //�v���C���[
    private GameObject playerObj;

    //�U���Ώۂɓːi��
    private bool isChargeTarget = false;
    //�s����~
    private bool isStopAction = false;

    private void Awake()
    {
        //�v���C���[�擾
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var dog in dogBases)
        {
            //�e�N���X�ŃI�[�o�[���C�h���������ݒ���s
            dog.SetUpDog();

            //���L�N���X�ɊY�����邩�m�F�����
            //�ǂ����Ⴄ�N���X��TryGetComponent�����null������������ۂ��̂�null�`�F�b�N
            if (dogMove == null) dog.TryGetComponent(out dogMove);
            if (dogAnimation == null) dog.TryGetComponent(out dogAnimation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopAction) {
            dogMove.StopMove();
            return; 
        }

        if(Input.GetKeyDown(KeyCode.O))//�f�o�b�O�p
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Zombie");
            OrderAttack(obj);
        }

        if (isChargeTarget)//�ːi
        {
            //�U���Ώۂ����݂��Ȃ��Ȃ�return
            //�U���Ώۂ��r����Destroy���ꂽ�ꍇ�̋�������
            if (attackTargetObj == null){
                isChargeTarget = false;//�ːi���f
                return;
            }

            //attackTargetObj�̈ʒu�Ɍ������Ĉړ�����
            dogMove.LookAtObject(attackTargetObj);
            dogMove.RunFront();//�ړ�

            dogAnimation.Run();//�A�j���[�V����

            //�U���ΏۂɌ���Ȃ��߂Â�����
            if (GetObjectDistance(attackTargetObj) < 0.5f)
            {
                //���݂�
                BiteZombie(attackTargetObj);
            }
        }
        else//�ʏ펞�̈ړ�
        {

            //if(�v���C���[�Ƃ̋���������Ă���)
            if (GetObjectDistance(playerObj) > stayPlayerDistance)
            {
                //�v���C���[�̕��Ɉړ�����
                //��Q���̔���ɒ���
                dogMove.LookAtObject(playerObj);
                dogMove.RunFront();

                dogAnimation.Run();//�A�j���[�V����

            }
            else
            {
                dogAnimation.Idle();
                dogMove.StopMove();
            }
            //else if(�߂��Ȃ�)
            //�Ƃ肠�����͉������Ȃ�
            //�v���C���[�̋߂��ŃE���E�����������@�͈͓��̃����_���ʒu��ڕW�Ƃ��Ĉړ��݂�����
        }
    }

    /// <summary>
    /// �Ώۂ̃I�u�W�F�N�g�Ƃ̋��������߂�
    /// </summary>
    private float GetObjectDistance(GameObject _target)
    {
        Vector3 pos = transform.position;
        Vector3 targetPos = _target.transform.position;
        pos.y = 0; targetPos.y = 0;//y�������v�Z���Ȃ�
        return Vector3.Distance(targetPos, pos);
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
        ZombieManager zombieManager;
        //attackTargetObj����ZombieManager���擾���AFreezePosition���Ăяo��
        attackTargetObj.TryGetComponent(out zombieManager);
        if (zombieManager == null) return;

        zombieManager.FreezePosition(biteStaySec);//�]���r���~

        dogAnimation.Attack();
        isStopAction = true;
        isChargeTarget = false;

        DelayRunAsync(
            biteStaySec,
            () => {
                isStopAction = false; 
            });
    }

    /// <summary>
    /// �x�点��Action�����s����async
    /// </summary>
    private async ValueTask DelayRunAsync(double wait_sec, Action action)
    {
        // �f�B���C����
        await Task.Delay(TimeSpan.FromSeconds(wait_sec));
        action();
    }
}

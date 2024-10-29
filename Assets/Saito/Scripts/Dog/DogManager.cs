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

    //�ړ��ڕW���W
    private Vector3 targetPos;

    //�ړ���~�t���O
    private bool onFreezeMove = false;

    //�U���Ώۂɓːi��
    private bool isChargeTarget = false;
    //�s����~
    private bool isStopAction = false;

    //�ړ����@����s�ɂ���
    private bool isMoveTypeWalk = false;


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

        RandomTargetPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopAction) {
            dogMove.StopMove();//�ړ���~
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
            dogMove.LookAtPosition(attackTargetObj.transform.position);
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
            NomalUpdate();
        }
    }

    /// <summary>
    /// �Ώۂ̃I�u�W�F�N�g�Ƃ̋��������߂�
    /// </summary>
    private float GetObjectDistance(GameObject _target)
    {
        if (_target == null) return 0.0f;

        Vector3 pos = transform.position;
        Vector3 targetPos = _target.transform.position;
        pos.y = 0; targetPos.y = 0;//y�������v�Z���Ȃ�
        return Vector3.Distance(targetPos, pos);
    }

    /// <summary>
    /// �U���w�����󂯂��Ƃ�
    /// </summary>
    public void OrderAttack(GameObject _obj)//zombie�̎q�̃p�[�c���n���ꂽ�Ƃ������Ȃ��\���A��
    {
        Debug.Log("�U���w�����󂯕t����");

        isChargeTarget = true;

        attackTargetObj = _obj;//�U���Ώۂ��擾
    }

    /// <summary>
    /// �]���r�Ɋ��݂�
    /// </summary>
    private void BiteZombie(GameObject _zombieObj)
    {
        Debug.Log("�]���r�Ɋ��݂�");

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

    //�ʏ�s���p�̉��֐�
    private void NomalUpdate()
    {
        //�ړ�����W���v���C���[���痣��Ă���Ȃ猈�߂Ȃ���
        float playerTargetDistance = Vector3.Distance(playerObj.transform.position, targetPos);
        if (playerTargetDistance > stayPlayerDistance) { 
            RandomTargetPos();
            onFreezeMove = false;//��~���ł�����
            isMoveTypeWalk = false;//����悤�ɂ���
        }

        if (onFreezeMove) return;

        //�ڕW���W�܂ł̈ʒu�����߂�
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        //�v���C���[�Ǝ��g�̋���
        float playerDistance = Vector3.Distance(playerObj.transform.position, pos);

        //�����ňړ�
        dogMove.LookAtPosition(targetPos);//�����ύX
        //�v���C���[�Ƃ̋����ɂ���đ��x�ύX
        if (isMoveTypeWalk)
        {
            dogMove.WalkFront();
            dogAnimation.Walk();
        }
        else
        {
            dogMove.RunFront();
            dogAnimation.Run();
        }

        float distance = Vector3.Distance(pos, targetPos);
        //����������
        if (distance < 0.1f)
        {
            //��~
            dogMove.StopMove();
            dogAnimation.Idle();

            //��~���Ԃ������_���Ɍ��߂�
            //�ϐ��͌�ŃN���X�ϐ��ɂ���
            double freezeSec = UnityEngine.Random.Range(2.0f, 5.0f);

            onFreezeMove = true;
            DelayRunAsync(
                        freezeSec,
                        () => { 
                            onFreezeMove = false;
                            RandomTargetPos();
                            isMoveTypeWalk = true;}
                        );
        }
    }

    //�v���C���[���͈͂̃����_���ʒu��ڕW���W�ɐݒ肷��
    private void RandomTargetPos()
    {
        //�v���C���[�̎��͂̃����_���ʒu�����߂�
        Vector3 pPos = playerObj.transform.position;
        //�ړ���ʒu�������_���Ɍ��߂�
        targetPos = UnityEngine.Random.insideUnitCircle * stayPlayerDistance;
        targetPos.z = targetPos.y;//���ʏ�ɐ������邽�ߓ���ւ�
        targetPos.y = 0.5f;//y�����͈ꗥ�ɂ���
        //�A�^�b�`�����I�u�W�F�N�g����ɂ���
        targetPos.x += pPos.x;
        targetPos.z += pPos.z;
    }


}

/*
�E�ҋ@���̒ʏ�s��
�v���C���[���͈�苗���̃����_���ȑҋ@�G���A���ʒu�����߂�
��
�����܂ŕ���
��
�����_���Ȏ��Ԓ�~�@���@��֖߂�

�E�ҋ@���Ƀv���C���[���痣�ꂽ�Ƃ��̍s��
�ҋ@���̒ʏ�s�����L�����Z��
��
�v���C���[���͈�苗���̃����_���ȑҋ@�G���A���ʒu�����߂�
��
�����܂ő���
��
�����_���Ȏ��Ԓ�~�@���@�ʏ�s���֖߂�

�E�U�����̍s��
�U���w�����󂯂�
��
�Ώۂɓːi
��
���݂�
��
�b�������痣���āA������ƃX�^���@���@���ꂽ���̍s����

�ʏ�s�����̂ݑ��̎w�����󂯂�

�w���͈�̊֐��̈����Ŏw�肷��̂������̂ł�
���݂̍s���𑼂���Q�Ƃł���悤�ɂ�����

 */
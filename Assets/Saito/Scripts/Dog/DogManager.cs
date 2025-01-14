using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//�����ŃA�^�b�`�����X�N���v�g
[RequireComponent(typeof(DogMove))]
[RequireComponent(typeof(DogAnimation))]
[RequireComponent(typeof(DogSound))]

/// <summary>
/// ���}�l�[�W���[�N���X
/// </summary>
public class DogManager : MonoBehaviour, IStopObject
{
    /// <summary>
    /// ���삷��N���X
    /// </summary>
    [SerializeField]
    private DogBase[] mDogBases;

    private DogMove mDogMove;
    private DogAnimation mDogAnimation;
    private DogSound mDogSound;

    private TargetMark mTargetMark;

    [SerializeField]//���݂��Ă��鎞��
    private float mBiteStaySec = 4.0f;

    [SerializeField]//�ҋ@��ԂɂȂ�v���C���[�Ƃ̋���
    private float mStayPlayerDistance = 5.0f;

    [SerializeField]//�T�m�̃N�[���^�C��
    private float mDetectCooldownSec = 60.0f;


    //�U���ΏۃI�u�W�F�N�g
    private GameObject mAttackTargetObj;
    //�v���C���[
    private GameObject mPlayerObj;

    //�ړ��ڕW���W
    private Vector3 mTargetPos;

    //�ړ���~�t���O
    private bool mOnFreezeMove = false;

    //�s����~
    [SerializeField]
    private bool mIsStopAction = false;
    //�w�����󂯂Ȃ��t���O
    private bool mIsIgnoreOrder = false;
    //�T�m�̃N�[���^�C����
    private bool mIsDetectCooldown = false;

    //�U���Ώۂɓːi��
    private bool mIsChargeTarget = false;
    //�ړ����@����s�ɂ���
    private bool mIsMoveTypeWalk = false;

    //�|�[�Y�p��~�t���O
    private bool mIsPause = false;

    //���쒆�̒x������
    List<IEnumerator> mInActionDelays = new List<IEnumerator>();


    private void Awake()
    {
        //�v���C���[�擾
        mPlayerObj = GameObject.FindGameObjectWithTag("Player");

        mTargetMark = gameObject.GetComponent<TargetMark>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var dog in mDogBases)
        {
            //�e�N���X�ŃI�[�o�[���C�h���������ݒ���s
            dog.SetUpDog();

            //���L�N���X�ɊY�����邩�m�F�����
            //�ǂ����Ⴄ�N���X��TryGetComponent�����null������������ۂ��̂�null�`�F�b�N
            if (mDogMove == null) dog.TryGetComponent(out mDogMove);
            if (mDogAnimation == null) dog.TryGetComponent(out mDogAnimation);
            if (mDogSound == null) dog.TryGetComponent(out mDogSound);
        }

        RandomTargetPos();
    }


    // Update is called once per frame
    void Update()
    {
        if (mIsStopAction || mIsPause) {
            mDogMove.StopMove();//�ړ���~
            return; 
        }

        if (mIsChargeTarget)//�ːi
        {
            //�U���Ώۂ����݂��Ȃ��Ȃ�return
            //�U���Ώۂ��r����Destroy���ꂽ�ꍇ�̋�������
            if (mAttackTargetObj == null){
                mIsChargeTarget = false;//�ːi���f
                return;
            }

            //attackTargetObj�̈ʒu�Ɍ������Ĉړ�����
            mDogMove.LookAtPosition(mAttackTargetObj.transform.position);
            mDogMove.RunFront();//�ړ�

            mDogAnimation.Run();//�A�j���[�V����

            //�U���ΏۂɌ���Ȃ��߂Â�����
            if (GetObjectDistance(mAttackTargetObj) < 0.5f)
            {
                //���݂�
                BiteZombie(mAttackTargetObj);
            }
        }
        else//�ʏ펞�̈ړ�
        {
            NomalUpdate();
        }
    }

    //�ʏ�s���p�̉��֐�
    private void NomalUpdate()
    {
        //�ړ�����W���v���C���[���痣��Ă���Ȃ猈�߂Ȃ���
        float player_target_distance = Vector3.Distance(mPlayerObj.transform.position, mTargetPos);
        if (player_target_distance > mStayPlayerDistance)
        {
            RandomTargetPos();
            mOnFreezeMove = false;//��~���ł�����
            mIsMoveTypeWalk = false;//����悤�ɂ���
        }

        if (mOnFreezeMove) return;

        //�ڕW���W�܂ł̈ʒu�����߂�
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        //�v���C���[�Ǝ��g�̋���
        float player_distance = Vector3.Distance(mPlayerObj.transform.position, pos);
        //�����������ꍇ�͎w�����󂯕t���Ȃ�
        if (player_distance <= mStayPlayerDistance)
        {
            mIsIgnoreOrder = false;
        }
        else
        {
            mIsIgnoreOrder = true;
        }

        //�����ňړ�
        mDogMove.LookAtPosition(mTargetPos);//�����ύX
        //�v���C���[�Ƃ̋����ɂ���đ��x�ύX
        if (mIsMoveTypeWalk)
        {
            mDogMove.WalkFront();
            mDogAnimation.Walk();
        }
        else
        {
            mDogMove.RunFront();
            mDogAnimation.Run();
        }

        float distance = Vector3.Distance(pos, mTargetPos);
        //����������
        if (distance < 0.1f)
        {
            //��~
            mDogMove.StopMove();
            mDogAnimation.Idle();

            //��~���Ԃ������_���Ɍ��߂�
            //�ϐ��͌�ŃN���X�ϐ��ɂ���
            float freeze_sec = UnityEngine.Random.Range(2.0f, 5.0f);

            mOnFreezeMove = true;
            //��莞�Ԓ�~
            mInActionDelays.Add(
                        DelayRunCoroutine(
                        freeze_sec,
                        () => {
                            mOnFreezeMove = false;
                            RandomTargetPos();
                            mIsMoveTypeWalk = true;
                        }
                        ));
            StartCoroutine(mInActionDelays[mInActionDelays.Count - 1]);
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
        if (!CanOrderAttack()) return;
        Debug.Log("�U���w�����󂯕t����");

        mIsChargeTarget = true;

        mAttackTargetObj = _obj;//�U���Ώۂ��擾

        mDogSound.PlayAttackBark();//����
    }
    /// <summary>
    /// �w���F���͂̒T�m
    /// </summary>
    public void OrderDetection()
    {
        if (!CanOrderDetection()) return;
        Debug.Log("�T�m�J�n");

        //���͈͂̑Ώۂ̃I�u�W�F�N�g���}�[�N
        mTargetMark.RangeMark();

        mDogSound.PlayDetectBark();//����

        mIsChargeTarget = false;//�U���̓L�����Z��

        //�N�[���^�C��
        mIsDetectCooldown = true;

        mInActionDelays.Add(
                        DelayRunCoroutine(
                        mDetectCooldownSec,
                        () => {
                            mIsDetectCooldown = false;
                        }
                        ));
        StartCoroutine(mInActionDelays[mInActionDelays.Count - 1]);
    }
    //�U���w���\��
    public bool CanOrderAttack()
    {
        if (mIsStopAction) return false;
        if (mIsIgnoreOrder) return false;

        return true;
    }
    //�U���w���\��
    public bool CanOrderDetection()
    {
        if (mIsStopAction) return false;
        if (mIsIgnoreOrder) return false;
        if (mIsDetectCooldown) return false;

        return true;
    }

    //�`���[�g���A���p�̃N�[���^�C���Ď��p
    public bool UsedOrderDetection()
    {
        return mIsDetectCooldown;
    }

    /// <summary>
    /// �]���r�Ɋ��݂�
    /// </summary>
    private void BiteZombie(GameObject _zombie_obj)
    {
        Debug.Log("�]���r�Ɋ��݂�");

        ZombieManager zombie_manager;
        //attackTargetObj����ZombieManager���擾���AFreezePosition���Ăяo��
        mAttackTargetObj.TryGetComponent(out zombie_manager);
        if (zombie_manager == null) return;

        zombie_manager.FreezePosition((float)mBiteStaySec);//�]���r���~

        mDogAnimation.Attack();
        mIsStopAction = true;
        mIsChargeTarget = false;

        //��莞�Ԓ�~
        mInActionDelays.Add(
                        DelayRunCoroutine(
                        mBiteStaySec,
                        () => {
                            mIsStopAction = false;
                        }
                        ));
        StartCoroutine(mInActionDelays[mInActionDelays.Count - 1]);
    }


    /// <summary>
    /// �x�点��Action�����s����R���[�`��
    /// </summary>
    private IEnumerator DelayRunCoroutine(float _wait_sec, Action _action)
    {
        //���̃R���[�`���̏��擾 �o����΃��X�g�ǉ��������ł�肽��
        IEnumerator this_cor = mInActionDelays[mInActionDelays.Count - 1];

        //�R���[�`�����ĊJ���Ă��ҋ@���ԏ�񂪏����Ȃ��悤�ɂ���
        for (float i = 0; i < _wait_sec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);

        _action();
        //�I�����ɂ��̃R���[�`�������폜
        mInActionDelays.Remove(this_cor);
    }



    //�v���C���[���͈͂̃����_���ʒu��ڕW���W�ɐݒ肷��
    private void RandomTargetPos()
    {
        //�v���C���[�̎��͂̃����_���ʒu�����߂�
        Vector3 p_pos = mPlayerObj.transform.position;
        //�ړ���ʒu�������_���Ɍ��߂�
        mTargetPos = UnityEngine.Random.insideUnitCircle * mStayPlayerDistance;
        mTargetPos.z = mTargetPos.y;//���ʏ�ɐ������邽�ߓ���ւ�
        mTargetPos.y = 0.5f;//y�����͈ꗥ�ɂ���
        //�A�^�b�`�����I�u�W�F�N�g����ɂ���
        mTargetPos.x += p_pos.x;
        mTargetPos.z += p_pos.z;
    }

    //�O������s���̒�~��ς���p
    public void OnStopAction(bool _flag)
    {
        mIsStopAction = _flag;
    }


    //�C���^�[�t�F�[�X�ł̒�~�����p
    //�ꎞ��~
    public void Pause()
    {
        mIsPause = true;

        //���[�v���ɗv�f���ς��Ȃ��悤�ɃN�b�V���������܂�
        List<IEnumerator> tmp_list = new List<IEnumerator>(mInActionDelays);
        foreach (var cor in tmp_list)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
    }
    //�ĊJ
    public void Resume()
    {
        mIsPause = false;

        List<IEnumerator> tmp_list = new List<IEnumerator>(mInActionDelays);
        foreach (var cor in tmp_list)
        {
            if (cor == null) continue;

            StartCoroutine(cor);
        }
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
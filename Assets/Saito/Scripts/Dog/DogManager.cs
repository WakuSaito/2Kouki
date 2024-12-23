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
[RequireComponent(typeof(DogSound))]

/// <summary>
/// ���̃}�l�[�W���[�N���X
/// </summary>
public class DogManager : MonoBehaviour, IStopObject
{
    /// <summary>
    /// ���삷��N���X
    /// </summary>
    [SerializeField]
    private DogBase[] dogBases;

    private DogMove dogMove;
    private DogAnimation dogAnimation;
    private DogSound dogSound;

    private TargetMark targetMark;

    [SerializeField]//���݂��Ă��鎞��
    private float biteStaySec = 4.0f;

    [SerializeField]//�ҋ@��ԂɂȂ�v���C���[�Ƃ̋���
    private float stayPlayerDistance = 5.0f;

    [SerializeField]//�T�m����
    private float detectRange = 30.0f;

    [SerializeField]//�T�m�̃N�[���^�C��
    private float detectCooldownSec = 60.0f;


    //�U���ΏۃI�u�W�F�N�g
    private GameObject attackTargetObj;
    //�v���C���[
    private GameObject playerObj;

    //�ړ��ڕW���W
    private Vector3 targetPos;

    //�ړ���~�t���O
    private bool onFreezeMove = false;

    //�s����~
    [SerializeField]
    private bool isStopAction = false;
    //�w�����󂯂Ȃ��t���O
    private bool isIgnoreOrder = false;
    //�T�m�̃N�[���^�C����
    private bool isDetectCooldown = false;

    //�U���Ώۂɓːi��
    private bool isChargeTarget = false;
    //�ړ����@����s�ɂ���
    private bool isMoveTypeWalk = false;

    //�|�[�Y�p��~�t���O
    private bool isPause = false;

    //���쒆�̒x������
    List<IEnumerator> inActionDelays = new List<IEnumerator>();


    private void Awake()
    {
        //�v���C���[�擾
        playerObj = GameObject.FindGameObjectWithTag("Player");

        targetMark = gameObject.GetComponent<TargetMark>();
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
            if (dogSound == null) dog.TryGetComponent(out dogSound);
        }

        RandomTargetPos();
    }


    // Update is called once per frame
    void Update()
    {
        if (isStopAction || isPause) {
            dogMove.StopMove();//�ړ���~
            return; 
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

    //�ʏ�s���p�̉��֐�
    private void NomalUpdate()
    {
        //�ړ�����W���v���C���[���痣��Ă���Ȃ猈�߂Ȃ���
        float playerTargetDistance = Vector3.Distance(playerObj.transform.position, targetPos);
        if (playerTargetDistance > stayPlayerDistance)
        {
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
        //�����������ꍇ�͎w�����󂯕t���Ȃ�
        if (playerDistance <= stayPlayerDistance)
        {
            isIgnoreOrder = false;
        }
        else
        {
            isIgnoreOrder = true;
        }

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
            float freezeSec = UnityEngine.Random.Range(2.0f, 5.0f);

            onFreezeMove = true;
            //��莞�Ԓ�~
            inActionDelays.Add(
                        DelayRunCoroutine(
                        freezeSec,
                        () => {
                            onFreezeMove = false;
                            RandomTargetPos();
                            isMoveTypeWalk = true;
                        }
                        ));
            StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
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

        isChargeTarget = true;

        attackTargetObj = _obj;//�U���Ώۂ��擾

        dogSound.PlayAttackBark();//����
    }
    /// <summary>
    /// �w���F���͂̒T�m
    /// </summary>
    public void OrderDetection()
    {
        if (!CanOrderDetection()) return;
        Debug.Log("�T�m�J�n");

        //���͈͂̑Ώۂ̃I�u�W�F�N�g���}�[�N
        targetMark.RangeMark();

        dogSound.PlayDetectBark();//����

        isChargeTarget = false;//�U���̓L�����Z��

        //�N�[���^�C��
        isDetectCooldown = true;

        inActionDelays.Add(
                        DelayRunCoroutine(
                        detectCooldownSec,
                        () => {
                            isDetectCooldown = false;
                        }
                        ));
        StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
    }
    //�U���w���\��
    public bool CanOrderAttack()
    {
        if (isStopAction) return false;
        if (isIgnoreOrder) return false;

        return true;
    }
    //�U���w���\��
    public bool CanOrderDetection()
    {
        if (isStopAction) return false;
        if (isIgnoreOrder) return false;
        if (isDetectCooldown) return false;

        return true;
    }

    //�`���[�g���A���p�̃N�[���^�C���Ď��p
    public bool UsedOrderDetection()
    {
        return isDetectCooldown;
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

        zombieManager.FreezePosition((float)biteStaySec);//�]���r���~

        dogAnimation.Attack();
        isStopAction = true;
        isChargeTarget = false;

        //��莞�Ԓ�~
        inActionDelays.Add(
                        DelayRunCoroutine(
                        biteStaySec,
                        () => {
                            isStopAction = false;
                        }
                        ));
        StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
    }


    /// <summary>
    /// �x�点��Action�����s����R���[�`��
    /// </summary>
    private IEnumerator DelayRunCoroutine(float _wait_sec, Action _action)
    {
        //���̃R���[�`���̏��擾 �o����΃��X�g�ǉ��������ł�肽��
        IEnumerator thisCor = inActionDelays[inActionDelays.Count - 1];

        //�R���[�`�����ĊJ���Ă��ҋ@���ԏ�񂪏����Ȃ��悤�ɂ���
        for (float i = 0; i < _wait_sec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);

        _action();
        //�I�����ɂ��̃R���[�`�������폜
        inActionDelays.Remove(thisCor);
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

    //�O������s���̒�~��ς���p
    public void OnStopAction(bool _flag)
    {
        isStopAction = _flag;
    }


    //�C���^�[�t�F�[�X�ł̒�~�����p
    //�ꎞ��~
    public void Pause()
    {
        isPause = true;

        //���[�v���ɗv�f���ς��Ȃ��悤�ɃN�b�V���������܂�
        List<IEnumerator> tmpList = new List<IEnumerator>(inActionDelays);
        foreach (var cor in tmpList)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
    }
    //�ĊJ
    public void Resume()
    {
        isPause = false;

        List<IEnumerator> tmpList = new List<IEnumerator>(inActionDelays);
        foreach (var cor in tmpList)
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
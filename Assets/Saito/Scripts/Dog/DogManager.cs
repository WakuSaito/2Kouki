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
    private DogBase[] m_dogBases;

    private DogMove m_dogMove;
    private DogAnimation m_dogAnimation;
    private DogSound m_dogSound;

    private TargetMark m_targetMark;

    [SerializeField]//���݂��Ă��鎞��
    private float m_biteStaySec = 4.0f;

    [SerializeField]//�ҋ@��ԂɂȂ�v���C���[�Ƃ̋���
    private float m_stayPlayerDistance = 5.0f;

    [SerializeField]//�T�m�̃N�[���^�C��
    private float m_detectCooldownSec = 60.0f;


    //�U���ΏۃI�u�W�F�N�g
    private GameObject m_attackTargetObj;
    //�v���C���[
    private GameObject m_playerObj;

    //�ړ��ڕW���W
    private Vector3 m_targetPos;

    //�ړ���~�t���O
    private bool m_onFreezeMove = false;

    //�s����~
    [SerializeField]
    private bool m_isStopAction = false;
    //�w�����󂯂Ȃ��t���O
    private bool m_isIgnoreOrder = false;
    //�T�m�̃N�[���^�C����
    private bool m_isDetectCooldown = false;

    //�U���Ώۂɓːi��
    private bool m_isChargeTarget = false;
    //�ړ����@����s�ɂ���
    private bool m_isMoveTypeWalk = false;

    //�|�[�Y�p��~�t���O
    private bool m_isPause = false;

    //���쒆�̒x������
    List<IEnumerator> m_inActionDelays = new List<IEnumerator>();


    private void Awake()
    {
        //�v���C���[�擾
        m_playerObj = GameObject.FindGameObjectWithTag("Player");

        m_targetMark = gameObject.GetComponent<TargetMark>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var dog in m_dogBases)
        {
            //�e�N���X�ŃI�[�o�[���C�h���������ݒ���s
            dog.SetUpDog();

            //���L�N���X�ɊY�����邩�m�F�����
            //�ǂ����Ⴄ�N���X��TryGetComponent�����null������������ۂ��̂�null�`�F�b�N
            if (m_dogMove == null) dog.TryGetComponent(out m_dogMove);
            if (m_dogAnimation == null) dog.TryGetComponent(out m_dogAnimation);
            if (m_dogSound == null) dog.TryGetComponent(out m_dogSound);
        }

        RandomTargetPos();
    }


    // Update is called once per frame
    void Update()
    {
        if (m_isStopAction || m_isPause) {
            m_dogMove.StopMove();//�ړ���~
            return; 
        }

        if (m_isChargeTarget)//�ːi
        {
            //�U���Ώۂ����݂��Ȃ��Ȃ�return
            //�U���Ώۂ��r����Destroy���ꂽ�ꍇ�̋�������
            if (m_attackTargetObj == null){
                m_isChargeTarget = false;//�ːi���f
                return;
            }

            //attackTargetObj�̈ʒu�Ɍ������Ĉړ�����
            m_dogMove.LookAtPosition(m_attackTargetObj.transform.position);
            m_dogMove.RunFront();//�ړ�

            m_dogAnimation.Run();//�A�j���[�V����

            //�U���ΏۂɌ���Ȃ��߂Â�����
            if (GetObjectDistance(m_attackTargetObj) < 0.5f)
            {
                //���݂�
                BiteZombie(m_attackTargetObj);
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
        float player_target_distance = Vector3.Distance(m_playerObj.transform.position, m_targetPos);
        if (player_target_distance > m_stayPlayerDistance)
        {
            RandomTargetPos();
            m_onFreezeMove = false;//��~���ł�����
            m_isMoveTypeWalk = false;//����悤�ɂ���
        }

        if (m_onFreezeMove) return;

        //�ڕW���W�܂ł̈ʒu�����߂�
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        //�v���C���[�Ǝ��g�̋���
        float player_distance = Vector3.Distance(m_playerObj.transform.position, pos);
        //�����������ꍇ�͎w�����󂯕t���Ȃ�
        if (player_distance <= m_stayPlayerDistance)
        {
            m_isIgnoreOrder = false;
        }
        else
        {
            m_isIgnoreOrder = true;
        }

        //�����ňړ�
        m_dogMove.LookAtPosition(m_targetPos);//�����ύX
        //�v���C���[�Ƃ̋����ɂ���đ��x�ύX
        if (m_isMoveTypeWalk)
        {
            m_dogMove.WalkFront();
            m_dogAnimation.Walk();
        }
        else
        {
            m_dogMove.RunFront();
            m_dogAnimation.Run();
        }

        float distance = Vector3.Distance(pos, m_targetPos);
        //����������
        if (distance < 0.1f)
        {
            //��~
            m_dogMove.StopMove();
            m_dogAnimation.Idle();

            //��~���Ԃ������_���Ɍ��߂�
            //�ϐ��͌�ŃN���X�ϐ��ɂ���
            float freeze_sec = UnityEngine.Random.Range(2.0f, 5.0f);

            m_onFreezeMove = true;
            //��莞�Ԓ�~
            m_inActionDelays.Add(
                        DelayRunCoroutine(
                        freeze_sec,
                        () => {
                            m_onFreezeMove = false;
                            RandomTargetPos();
                            m_isMoveTypeWalk = true;
                        }
                        ));
            StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
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

        m_isChargeTarget = true;

        m_attackTargetObj = _obj;//�U���Ώۂ��擾

        m_dogSound.PlayAttackBark();//����
    }
    /// <summary>
    /// �w���F���͂̒T�m
    /// </summary>
    public void OrderDetection()
    {
        if (!CanOrderDetection()) return;
        Debug.Log("�T�m�J�n");

        //���͈͂̑Ώۂ̃I�u�W�F�N�g���}�[�N
        m_targetMark.RangeMark();

        m_dogSound.PlayDetectBark();//����

        m_isChargeTarget = false;//�U���̓L�����Z��

        //�N�[���^�C��
        m_isDetectCooldown = true;

        m_inActionDelays.Add(
                        DelayRunCoroutine(
                        m_detectCooldownSec,
                        () => {
                            m_isDetectCooldown = false;
                        }
                        ));
        StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
    }
    //�U���w���\��
    public bool CanOrderAttack()
    {
        if (m_isStopAction) return false;
        if (m_isIgnoreOrder) return false;

        return true;
    }
    //�U���w���\��
    public bool CanOrderDetection()
    {
        if (m_isStopAction) return false;
        if (m_isIgnoreOrder) return false;
        if (m_isDetectCooldown) return false;

        return true;
    }

    //�`���[�g���A���p�̃N�[���^�C���Ď��p
    public bool UsedOrderDetection()
    {
        return m_isDetectCooldown;
    }

    /// <summary>
    /// �]���r�Ɋ��݂�
    /// </summary>
    private void BiteZombie(GameObject _zombie_obj)
    {
        Debug.Log("�]���r�Ɋ��݂�");

        ZombieManager zombie_manager;
        //attackTargetObj����ZombieManager���擾���AFreezePosition���Ăяo��
        m_attackTargetObj.TryGetComponent(out zombie_manager);
        if (zombie_manager == null) return;

        zombie_manager.FreezePosition((float)m_biteStaySec);//�]���r���~

        m_dogAnimation.Attack();
        m_isStopAction = true;
        m_isChargeTarget = false;

        //��莞�Ԓ�~
        m_inActionDelays.Add(
                        DelayRunCoroutine(
                        m_biteStaySec,
                        () => {
                            m_isStopAction = false;
                        }
                        ));
        StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
    }


    /// <summary>
    /// �x�点��Action�����s����R���[�`��
    /// </summary>
    private IEnumerator DelayRunCoroutine(float _wait_sec, Action _action)
    {
        //���̃R���[�`���̏��擾 �o����΃��X�g�ǉ��������ł�肽��
        IEnumerator this_cor = m_inActionDelays[m_inActionDelays.Count - 1];

        //�R���[�`�����ĊJ���Ă��ҋ@���ԏ�񂪏����Ȃ��悤�ɂ���
        for (float i = 0; i < _wait_sec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);

        _action();
        //�I�����ɂ��̃R���[�`�������폜
        m_inActionDelays.Remove(this_cor);
    }



    //�v���C���[���͈͂̃����_���ʒu��ڕW���W�ɐݒ肷��
    private void RandomTargetPos()
    {
        //�v���C���[�̎��͂̃����_���ʒu�����߂�
        Vector3 p_pos = m_playerObj.transform.position;
        //�ړ���ʒu�������_���Ɍ��߂�
        m_targetPos = UnityEngine.Random.insideUnitCircle * m_stayPlayerDistance;
        m_targetPos.z = m_targetPos.y;//���ʏ�ɐ������邽�ߓ���ւ�
        m_targetPos.y = 0.5f;//y�����͈ꗥ�ɂ���
        //�A�^�b�`�����I�u�W�F�N�g����ɂ���
        m_targetPos.x += p_pos.x;
        m_targetPos.z += p_pos.z;
    }

    //�O������s���̒�~��ς���p
    public void OnStopAction(bool _flag)
    {
        m_isStopAction = _flag;
    }


    //�C���^�[�t�F�[�X�ł̒�~�����p
    //�ꎞ��~
    public void Pause()
    {
        m_isPause = true;

        //���[�v���ɗv�f���ς��Ȃ��悤�ɃN�b�V���������܂�
        List<IEnumerator> tmp_list = new List<IEnumerator>(m_inActionDelays);
        foreach (var cor in tmp_list)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
    }
    //�ĊJ
    public void Resume()
    {
        m_isPause = false;

        List<IEnumerator> tmp_list = new List<IEnumerator>(m_inActionDelays);
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
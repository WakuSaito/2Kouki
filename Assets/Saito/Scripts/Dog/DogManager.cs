using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//�����ŃA�^�b�`�����X�N���v�g
[RequireComponent(typeof(DogMove))]
[RequireComponent(typeof(DogAnimation))]
[RequireComponent(typeof(DogSound))]

/// <summary>
/// <para>���}�l�[�W���[�N���X</para>
/// ���x�[�X�N���X���p�����Ă���X�N���v�g�̊Ǘ�
/// �|�[�Y���ɒ�~���邽�߁AIStopObject���p��
/// </summary>
public class DogManager : MonoBehaviour, IStopObject
{
    /// <summary>
    /// ���삷��N���X
    /// </summary>
    [SerializeField] private DogBase[] m_dogBases;

    private DogMove m_dogMove;
    private DogAnimation m_dogAnimation;
    private DogSound m_dogSound;

    private TargetMark m_targetMark;

    //���݂��Ă��鎞��
    [SerializeField] private float m_biteStaySec = 4.0f;
    //�T�m�̃N�[���^�C��
    [SerializeField] private float m_detectCooldownSec = 60.0f;

    //�ҋ@��ԂɂȂ�v���C���[�Ƃ̋���
    [SerializeField] private float m_stayPlayerDistance = 5.0f;
    //�ǐՂłǂ��܂ŋ߂Â���
    [SerializeField] private float m_chasePlayerDistanceMin = 3.0f;
    //���[�v���J�n����v���C���[�Ƃ̋���
    [SerializeField] private float m_startWarpPlayerDistance = 20.0f;

    //�U���ΏۃI�u�W�F�N�g
    private GameObject m_attackTargetObj;
    //�v���C���[
    private GameObject m_playerObj;

    //�ړ��ڕW���W
    private Vector3 m_targetPos;

    //�ړ���~�t���O
    private bool m_onFreezeMove = false;

    //�s����~ ������Ԑݒ��
    [SerializeField] private bool m_isStopAction = false;
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

    //���쒆�̒x������(��~�A�ĊJ����p)
    List<IEnumerator> m_inActionDelays = new List<IEnumerator>();
    //���w���ŃR���[�`�����L�����Z������p
    IEnumerator m_stayActionDelay;
    IEnumerator m_safeAreaActionDelay;

    //�Z�[�t�G���A�őҋ@����ꏊ
    [SerializeField] private Transform m_safeAreaStayTransform;

    /// <summary>
    /// �ړ����[�v�̎��
    /// </summary>
    enum MOVE_UPDATE_TYPE
    {
        STAY,
        CHASE,
        SAFE_AREA,
        CHARGE,
        NULL
    }
    //�O����s���ꂽ�ړ��A�b�v�f�[�g
    MOVE_UPDATE_TYPE m_prevMoveUpdateType = MOVE_UPDATE_TYPE.NULL;

    //�R���|�[�l���g�̎擾
    private void Awake()
    {
        //�v���C���[�擾
        m_playerObj = GameObject.FindGameObjectWithTag("Player");

        m_targetMark = gameObject.GetComponent<TargetMark>();
    }

    //DogBase���p�������N���X�̎擾�ƈړ��挈��
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

        RandomTargetPos();//�ړ���ݒ�
    }


    // ��Ԃɂ���ăA�N�V���������߂郁�C�����[�v
    void Update()
    {
        if (m_isStopAction || m_isPause)
        {
            m_dogMove.StopMove();//�ړ���~
            return;
        }

        //�v���C���[�Ƃ̋���
        float player_distance = Vector3.Distance(m_playerObj.transform.position, transform.position);
        //�ړ����[�v���ۑ��p
        MOVE_UPDATE_TYPE current_move_type;

        //�ړ��^�C�v�����߂�
        if (m_isChargeTarget)
        {
            //�ːi
            current_move_type = MOVE_UPDATE_TYPE.CHARGE;

            //�U���Ώۂ����݂��Ȃ��Ȃ�return
            //�U���Ώۂ��r����Destroy���ꂽ�ꍇ�̋�������
            if (m_attackTargetObj == null)
            {
                m_isChargeTarget = false;//�ːi���f
                return;
            }

            //attackTargetObj�̈ʒu�Ɍ������Ĉړ�����
            m_dogMove.LookAtPosition(m_attackTargetObj.transform.position);
            m_dogMove.RunFront();//�ړ�

            m_dogAnimation.Run();//�A�j���[�V����

            //�U���ΏۂɌ���Ȃ��߂Â�����
            if (GetObjectDistance(m_attackTargetObj) < 0.6f)
            {
                //���݂�
                BiteZombie(m_attackTargetObj);
            }
        }
        else if (m_playerObj.GetComponent<player>().m_inSafeAreaFlag)
        {
            current_move_type = MOVE_UPDATE_TYPE.SAFE_AREA;
            //�v���C���[���Z�[�t�G���A��
            SafeAreaUpdate();
        }
        else if ((m_prevMoveUpdateType != MOVE_UPDATE_TYPE.CHASE && player_distance >= m_stayPlayerDistance) ||
                 (m_prevMoveUpdateType == MOVE_UPDATE_TYPE.CHASE && player_distance >= m_chasePlayerDistanceMin))
        {
            current_move_type = MOVE_UPDATE_TYPE.CHASE;
            //�v���C���[�ƈ��ȏ㗣��Ă���
            ChasePlayerUpdate();
        }
        else
        {
            current_move_type = MOVE_UPDATE_TYPE.STAY;
            //�ҋ@��
            StayUpdate();
        }

        //�ړ����[�v���ς�����Ƃ����s���̃R���[�`�����L�����Z��
        if(m_prevMoveUpdateType == MOVE_UPDATE_TYPE.STAY &&
            current_move_type != MOVE_UPDATE_TYPE.STAY)
        {
            StopCoroutine(m_stayActionDelay);//��~
            m_inActionDelays.Remove(m_stayActionDelay);//�ĊJ���Ȃ��悤��Remove
            m_stayActionDelay = null;
        }
        else if(m_prevMoveUpdateType == MOVE_UPDATE_TYPE.SAFE_AREA &&
            current_move_type != MOVE_UPDATE_TYPE.SAFE_AREA)
        {
            StopCoroutine(m_safeAreaActionDelay);//��~
            m_inActionDelays.Remove(m_safeAreaActionDelay);//�ĊJ���Ȃ��悤��Remove
            m_safeAreaActionDelay = null;
        }

        m_prevMoveUpdateType = current_move_type;//�ړ��^�C�v���ۑ�
    }

    /// <summary>
    /// <para>�Z�[�t�G���A�ł̍s��</para>
    /// �v���C���[���Z�[�t�G���A���ɂ��鎞�̋���
    /// </summary>
    private void SafeAreaUpdate()
    {
        //�A�����s�łȂ�
        if(m_prevMoveUpdateType != MOVE_UPDATE_TYPE.SAFE_AREA)
        {
            //��莞�Ԍ�ɏ���̈ʒu�Ƀ��[�v
            //�Z�[�t�G���A����o���Ƃ��L�����Z�����Ȃ��Ƃ����Ȃ� lerp�ł��悳��
            m_inActionDelays.Add(
                m_safeAreaActionDelay =//�L�����Z���p�ɕۑ�
                        DelayRunCoroutine(
                        2.0f,//��
                        () => {
                            m_dogMove.Warp(m_safeAreaStayTransform.position);//���[�v������
                            m_dogMove.ChangeDirection(m_safeAreaStayTransform.rotation);//������ς���
                            m_dogMove.StopMove();
                            m_dogAnimation.Idle();
                        }
                        ));
            StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
        }
        //�ړ���Ƃ̋���
        float target_distance = Vector3.Distance(transform.position, m_safeAreaStayTransform.position);

        if (target_distance > 0.3f)
        {
            //�Z�[�t�G���A����ʒu�Ɉړ� ���̕�DogMove�ɂ܂Ƃ߂������ǂ�����
            m_dogMove.LookAtPosition(m_safeAreaStayTransform.position);
            m_dogMove.RunFront();
            m_dogAnimation.Run();
        }
        else
        {
            m_dogMove.ChangeDirection(m_safeAreaStayTransform.rotation);
            m_dogMove.StopMove();
            m_dogAnimation.Idle();
        }
    }

    /// <summary>
    /// <para>�v���C���[�ǐՃ��[�v</para>
    /// �v���C���[��ǂ��悤�Ɉړ����� ���ꂷ�����烏�[�v����
    /// </summary>
    private void ChasePlayerUpdate()
    {
        //�v���C���[�Ƃ̋���
        float player_distance = Vector3.Distance(m_playerObj.transform.position, transform.position);
        if(player_distance >= m_startWarpPlayerDistance)//�v���C���[�Ƃ̋��������ȏ�
        {
            Vector3 player_behind_pos = m_playerObj.transform.position - m_playerObj.transform.forward * -1.0f;

            m_dogMove.Warp(player_behind_pos);//���[�v������
        }
        //�ڕW�n�_�ݒ�
        m_targetPos = m_playerObj.transform.position;
      
        m_dogMove.LookAtPosition(m_targetPos);//�����ύX
        m_dogMove.RunFront();//�ړ�
        m_dogAnimation.Run();//�A�j���[�V�����ݒ�
    }

    /// <summary>
    /// <para>�ҋ@�����[�v</para>
    /// ���펞�̍s���@��{��~�ł��܂Ƀv���C���[�̎��͂������
    /// </summary>
    private void StayUpdate()
    {
        //�A�����s�łȂ�
        if(m_prevMoveUpdateType != MOVE_UPDATE_TYPE.STAY)
        {
            m_targetPos = transform.position;
            m_onFreezeMove = false;
        }

        if (m_onFreezeMove) return;

        //�ڕW�n�_�ɓ���
        if (Vector3.Distance(m_targetPos, transform.position) <= 1.0f)
        {
            //�ړ���~
            m_dogMove.StopMove();
            m_dogAnimation.Idle();
            //��~���Ԃ������_���Ɍ��߂�
            float freeze_sec = UnityEngine.Random.Range(2.0f, 5.0f);
            //��莞�Ԓ�~ �ʂ̃��[�v���L�����Z��������
            m_onFreezeMove = true;
            m_inActionDelays.Add(
                m_stayActionDelay =//�L�����Z���o����悤�ɕۑ�
                        DelayRunCoroutine(
                        freeze_sec,
                        () => {
                            m_onFreezeMove = false;
                            RandomTargetPos();//�ړ��挈��
                        }
                        ));
            StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
        }
        else
        {
            m_dogMove.LookAtPosition(m_targetPos);//�����ύX
            m_dogMove.WalkFront();//�ړ�
            m_dogAnimation.Walk();
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
    /// <para>�U���w���̎󂯕t��</para>
    /// �v���C���[���ŌĂяo���A�U�����J�n����
    /// </summary>
    /// <param name="_target_obj">�U���ΏۃI�u�W�F�N�g</param>
    public void OrderAttack(GameObject _target_obj)
    {
        //�U���\�����ׂ�
        if (!CanOrderAttack()) return;
        Debug.Log("�U���w�����󂯕t����");

        m_isChargeTarget = true;//�U����Ԃ�
        m_attackTargetObj = _target_obj;//�U���Ώۂ�ۑ�

        m_dogSound.PlayAttackBark();//����
    }
    /// <summary>
    /// <para>�T�m�w���̎󂯕t��</para>
    /// �v���C���[���ŌĂяo���A�T�m���J�n����
    /// </summary>
    public void OrderDetection()
    {
        //�T�m���\�����ׂ�
        if (!CanOrderDetection()) return;
        Debug.Log("�T�m�J�n");

        //���͈͂̑Ώۂ̃I�u�W�F�N�g���}�[�N����
        m_targetMark.RangeMark();

        m_isChargeTarget = false;//�U���̓L�����Z��      

        m_dogSound.PlayDetectBark();//����

        //��莞�ԃN�[���^�C��
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
    /// <summary>
    /// <para>�U���w���̎󂯕t�����\��</para>
    /// �󂯕t���\�ȏꍇ��������������UI�֘A�̃N���X�ł��Q��
    /// </summary>
    /// <returns>�s���\ : true</returns>
    public bool CanOrderAttack()
    {
        if (m_isStopAction) return false;
        if (m_isIgnoreOrder) return false;

        return true;
    }
    /// <summary>
    /// <para>�T�m�w���̎󂯕t�����\��</para>
    /// �󂯕t���\�ȏꍇ��������������UI�֘A�̃N���X�ł��Q��
    /// </summary>
    /// <returns>�s���\ : true</returns>
    public bool CanOrderDetection()
    {
        if (m_isStopAction) return false;
        if (m_isIgnoreOrder) return false;
        if (m_isDetectCooldown) return false;

        return true;
    }
    /// <summary>
    /// <para>�T�m���󂯕t������</para>
    /// �`���[�g���A���p�̃N�[���^�C���Ď��p
    /// </summary>
    public bool UsedOrderDetection()
    {
        return m_isDetectCooldown;
    }

    /// <summary>
    /// <para>�]���r�Ɋ��݂�</para>
    /// �U�����Ɏg�p�@�]���r���̒�~�֐����Ăяo��
    /// </summary>
    /// <param name="_zombie_obj">���݂��]���r</param>
    private void BiteZombie(GameObject _zombie_obj)
    {
        Debug.Log("�]���r�Ɋ��݂�");

        //attackTargetObj����ZombieManager���擾
        ZombieManager zombie_manager;
        m_attackTargetObj.TryGetComponent(out zombie_manager);
        if (zombie_manager == null) return;

        zombie_manager.FreezePosition((float)m_biteStaySec);//�]���r���~

        m_dogAnimation.Attack(); //�A�j���[�V�����Đ�
        m_isChargeTarget = false;//�U���I��

        //��莞�ԍs����~
        m_isStopAction = true;
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
    /// <para>�x�����s����R���[�`��</para>
    /// �����_���Ŏw�肵����������莞�Ԍ�Ɏ��s �ꎞ��~��
    /// </summary>
    /// <param name="_delay_sec">�ҋ@����</param>
    /// <param name="_action">���s���鏈��</param>
    private IEnumerator DelayRunCoroutine(float _delay_sec, Action _action)
    {
        //���̃R���[�`���̏��擾 �o����΃��X�g�ǉ��������ł�肽��
        IEnumerator this_cor = m_inActionDelays[m_inActionDelays.Count - 1];

        //�R���[�`�����ĊJ���Ă��ҋ@���ԏ�񂪏����Ȃ��悤�ɂ���
        for (float i = 0; i < _delay_sec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);

        _action();
        //�I�����ɂ��̃R���[�`�������폜
        m_inActionDelays.Remove(this_cor);
    }

    /// <summary>
    /// <para>�ړ�����W�������_���Ɍ��߂�</para>
    /// �v���C���[�̎��͈��͈͓��̃����_���ʒu��ڕW���W�ɐݒ肷��
    /// </summary>
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

    /// <summary>
    /// <para>�s����~�̐؂�ւ�</para>
    /// �O������s����~��Ԃ�ς���p
    /// </summary>
    /// <param name="_flag">��~ : true</param>
    public void OnStopAction(bool _flag)
    {
        m_isStopAction = _flag;
    }


    /// <summary>
    /// <para>�ꎞ��~</para>
    /// �C���^�[�t�F�[�X�ł̒�~�����p
    /// </summary>
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

    /// <summary>
    /// <para>�ĊJ</para>
    /// �C���^�[�t�F�[�X�ł̒�~�����p
    /// </summary>
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

/
�U������

�v���C���[���Z�[�t�G���A��
�@���b���Ŏw��ʒu�Ɍ�����
�@�オ���s���I�������w��ʒu�A�w������Ń��[�v������

�v���C���[���͈͓�
�@��{�~�܂�
�@��莞�Ԃ��̏�Ԃ���������A�͈͓������R�ɓ���

�v���C���[���͈͊O
�@�v���C���[�Ɍ�����
�@�������ꂷ������
�@�@�v���C���[�̔w��Ƀ��[�v
/
 */
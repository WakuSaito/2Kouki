using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

//�����ŃA�^�b�`�����X�N���v�g
[RequireComponent(typeof(ZombieMove))]
[RequireComponent(typeof(ZombieAnimation))]
[RequireComponent(typeof(ZombieAction))]
[RequireComponent(typeof(ZombieHP))]

//�]���r�֘A�̃N���X��nameof�ł܂Ƃ߂Ă���������


/// <summary>
/// <para>�]���r�̊Ǘ��N���X</para>
/// ZombieBase���p�������N���X������
/// </summary>
public class ZombieManager : MonoBehaviour, IStopObject
{
    /// <summary>
    /// ���삷��N���X
    /// </summary>
    [SerializeField] private ZombieBase[] m_zombieBases;

    private ZombieMove m_zombieMove;
    private ZombieAttack m_zombieAttack;
    private ZombieAnimation m_zombieAnimation;
    private ZombieAction m_zombieAction;
    private ZombieHP m_zombieHP;
    private ZombieSound m_zombieSound;

    GameObject m_playerObj;

    //�v���C���[�̒T�m�͈�
    [SerializeField] float m_detectionPlayerRangeMin = 10.0f;//�ŏ�
    [SerializeField] float m_detectionPlayerRangeMax = 30.0f;//�ő�

    //���݂̃v���C���[�T�m�͈�
    private float m_currentDetectionRange;

    //�U���J�n����
    [SerializeField] float m_attackStartRange = 2.0f;

    //���̃I�u�W�F�N�g���폜����v���C���[�Ƃ̋���
    //�X�|�i�[�̍ŗL�����Ɠ������炢�̋����ɂ���
    [SerializeField] float m_despawnPlayerDistance = 100.0f;

    //�U���Ώۂ𔭌����Ă���
    private bool m_isFoundTarget = false;
    //�����_���Ɍ�����ς���N�[���^�C����
    private bool m_isChangeDirCoolDown = false;
    //�ړ��s�t���O
    private bool m_isFreezePos = false;
    //���S�σt���O
    private bool m_isDead = false;
    //�X�^���t���O
    private bool m_isStan = false;
    //�ꎞ��~
    private bool m_isStop = false;

    //�`���[�g���A���p�I�u�W�F�N�g��
    [SerializeField] private bool m_isTutorialObj = false;

    //�X�^�������L�����Z���p�g�[�N��
    private IEnumerator m_stanCoroutine;

    //Mesh���A�^�b�`���ꂽ�I�u�W�F�N�g
    [SerializeField] GameObject m_meshObj;

    //���쒆�̒x������
    List<IEnumerator> m_inActionDelays = new List<IEnumerator>();

    //�I�u�W�F�N�g�̎擾
    private void Awake()
    {
        //�v���C���[�I�u�W�F�N�g�擾
        m_playerObj = GameObject.FindGameObjectWithTag("Player");

        m_currentDetectionRange = m_detectionPlayerRangeMin;
    }

    //�]���r�x�[�X���p�������N���X�̎擾�Ə����ݒ�
    void Start()
    {
        foreach(var zombie in m_zombieBases)
        {
            Debug.Log(zombie);

            //�e�N���X�ŃI�[�o�[���C�h���������ݒ���s
            zombie.SetUpZombie();

            //���L�N���X�ɊY�����邩�m�F�����
            //�ǂ����Ⴄ�N���X��TryGetComponent�����null������������ۂ��̂�null�`�F�b�N
            if (m_zombieMove == null) zombie.TryGetComponent(out m_zombieMove);
            if (m_zombieAttack == null) zombie.TryGetComponent(out m_zombieAttack);
            if (m_zombieAnimation == null) zombie.TryGetComponent(out m_zombieAnimation);
            if (m_zombieAction == null) zombie.TryGetComponent(out m_zombieAction);
            if (m_zombieHP == null) zombie.TryGetComponent(out m_zombieHP);
            if (m_zombieSound == null) zombie.TryGetComponent(out m_zombieSound);
        }
        
        Debug.Log("�]���r�����̗�:"+ m_zombieHP.GetCurrentHP());

        if (m_isTutorialObj)
            m_zombieAnimation.Idle();
    }


    //�s���p�^�[�������߂郁�C�����[�v
    void Update()
    {
        if (m_isStop) return;
        if (m_playerObj == null) return;

        //���S�`�F�b�N
        if (m_zombieHP.IsDead())
            Dead();

        if (m_isTutorialObj)
        {
            Attack();//�U���̂݌J��Ԃ�
            return;
        }

        if (m_isDead) return;//���S�ςȂ瓮�����Ȃ�
        if (m_isStan) return;//�X�^�����͓������Ȃ�

        //���W�擾
        Vector3 pos = transform.position;
        Vector3 player_pos = m_playerObj.transform.position;
        pos.y = 0; player_pos.y = 0;//����y���W�𖳎�
        //�v���C���[�Ƃ̋����v�Z
        float player_distance = Vector3.Distance(pos, player_pos);

        //�v���C���[���痣�ꂷ������폜
        if (player_distance > m_despawnPlayerDistance)
        {
            Destroy(gameObject);

            return;
        }


        ChangeDetectRange();//�T�m�͈͌v�Z


        //�U���Ώۂ������Ă��邩
        if (player_distance < m_currentDetectionRange)
        {
            m_isFoundTarget = true;//����
        }
        else
        {
            m_isFoundTarget = false;
        }

        //�ړ�
        {
            if (m_isFreezePos || m_zombieAttack.m_isAttack)//��~
            {
                //�v���C���[�̕�������
                m_zombieMove.LookAtPosition(player_pos);

                m_zombieMove.StopMove();
                m_zombieAnimation.Idle();//��~���[�V����
            }
            else if (m_isFoundTarget)
            {
                //�v���C���[�̕�������
                m_zombieMove.LookAtPosition(player_pos);

                if (player_distance < m_attackStartRange)
                {
                    //�Ƃ肠�����߂Â������Ȃ��悤�ɂ���
                    m_zombieMove.StopMove();
                    m_zombieAnimation.Idle();//��~���[�V����

                    Attack();//�U��
                }
                else
                {
                    //����
                    m_zombieMove.RunFront();
                    m_zombieAnimation.Run();//�ړ����[�V����
                }

            }
            else//�ʏ�̍s��
            {
                //�����ύX
                if (!m_isChangeDirCoolDown)
                {
                    m_isChangeDirCoolDown = true;//�N�[���^�C������
                    m_inActionDelays.Add(
                        DelayRunCoroutine(
                        UnityEngine.Random.Range(4.0f, 8.0f),//���Ɍ�����ς���܂ł̎��Ԃ����߂�
                        () => m_isChangeDirCoolDown = false  //�t���O�I�t
                        ));
                    StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);

                    //�����_���Ɍ�����ݒ�
                    Vector3 direction = new Vector3(0, UnityEngine.Random.Range(-180, 180), 0);
                    //������ύX
                    m_zombieMove.ChangeDirection(Quaternion.Euler(direction));
                }

                //����
                m_zombieMove.WalkFront();
                m_zombieAnimation.Walk();//�ړ����[�V����
            }

        }
    }
    /// <summary>
    /// <para>�U��</para>
    /// �t���O�`�F�b�N�Ɗ֐��Ăяo��
    /// </summary>
    private void Attack()
    {
        if (m_zombieAttack.m_isAttack) return;//�N�[���^�C���`�F�b�N
        if (m_isDead) return;

        //�U���J�n
        m_zombieAttack.StartAttack();
        //�U�����[�V�����Đ�
        m_zombieAnimation.Attack();
    }

    /// <summary>
    /// �T�m�͈͂̕ύX
    /// �v���C���[�̗̑͂��Ⴂ�قǃv���C���[�������₷���Ȃ�
    /// </summary>
    private void ChangeDetectRange()
    {
        //�v���C���[�̃X�N���v�g�擾
        player player_script = m_playerObj.GetComponent<player>();
        if (player_script == null) return;

        //�v���C���[�̗͎̑擾
        float max_hp = player_script.hp_num_max;
        float current_hp = player_script.hp_num_now;

        //���݂̗̑͂̊����擾
        float current_hp_per = current_hp / max_hp;

        //���݂̗̑͊�������T�m�͈͂��ԂŌv�Z
        m_currentDetectionRange = m_detectionPlayerRangeMin * current_hp_per +
            m_detectionPlayerRangeMax * (1.0f - current_hp_per);
    }

    /// <summary>
    /// <para>�̂Ƀ_���[�W���󂯂�</para>
    /// �_���[�W�v�Z�ƃA�j���[�V�������̍Đ�
    /// </summary>
    /// <param name="_hit_pos">��e�n�_ (�A�j���[�V�����p)</param>
    /// <param name="_damage">�_���[�W��</param>
    public void DamageBody(Vector3 _hit_pos, int _damage)
    {
        Debug.Log("Body");

        m_zombieAttack.AttackCancel();//�U�������̃L�����Z��

        Vector3 vec = _hit_pos - transform.position;

        Vector3 axis = Vector3.Cross(transform.forward, vec);

        if (axis.y < 0)
        {
            Debug.Log("����");
            //zombieAnimation.DamageHitLeft();
        }
        else
        {
            Debug.Log("�E��");
            //zombieAnimation.DamageHitRight();
        }
        //�G�t�F�N�g�\��
        m_zombieAnimation.DamagedEffect(_hit_pos);

        m_zombieHP.Damage(_damage);//�_���[�W

        Stan(0.1f);//�X�^��
    }
    /// <summary>
    /// </para>���Ƀ_���[�W���󂯂�</para>
    /// �_���[�W�v�Z�ƃA�j���[�V�������Đ�
    /// </summary>
    /// <param name="_hit_pos">��e�n�_ (�A�j���[�V�����p)</param>
    /// <param name="_damage">�_���[�W��</param>
    public void DamageHead(Vector3 _hit_pos, int _damage)
    {
        Debug.Log("Head");

        m_zombieAttack.AttackCancel();//�U�������̃L�����Z��

        m_zombieHP.Damage(_damage * 2);//�_���[�W

        //�A�j���[�V����
        m_zombieAnimation.DamageHitRight();
        //�G�t�F�N�g�\��
        m_zombieAnimation.DamagedEffect(_hit_pos);

        Stan(0.3f);//�X�^��
    }

    /// <summary>
    /// <para>�X�^��</para>
    /// ��莞�ԍs���s�ɂȂ�
    /// </summary>
    /// <param name="_sec">�X�^������b��</param>
    private void Stan(float _sec)
    {
        if (m_isDead) return;

        //���ݓ��쒆�̃X�^���R���[�`�����L�����Z��
        if (m_isStan && m_stanCoroutine != null)
        {
            StopCoroutine(m_stanCoroutine);
            m_inActionDelays.Remove(m_stanCoroutine);
            m_stanCoroutine = null;
        }

        m_zombieAttack.AttackCancel();//�U�������̃L�����Z��

        m_isStan = true;

        //�ړ��x�N�g�����[���ɂ���
        m_zombieMove.StopMove();
        m_zombieAnimation.Idle();//��~���[�V����


        m_inActionDelays.Add(
            DelayRunCoroutine(
            _sec,
            () => m_isStan = false
            ));
        m_stanCoroutine = m_inActionDelays[m_inActionDelays.Count - 1];
        StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
        
    }


    /// <summary>
    /// <para>���S����</para>
    /// �����蔻��������A�A�j���[�V�����Đ�
    /// </summary>
    private void Dead()
    {
        if (m_isDead) return;

        m_isDead = true;//�ʂ̓�����~�߂邽�߂Ƀt���O�I��
        m_isFreezePos = true;//�ړ���~

        m_zombieAnimation.Die();//�A�j���[�V����
        m_zombieSound.PlayDead();//�T�E���h
        GetComponent<Rigidbody>().velocity = Vector3.zero;//�������~�߂�

        EnableCollider();//�R���C�_�[������

        //�A�j���[�V�������I��邱��ɃI�u�W�F�N�g������
        m_inActionDelays.Add(
            DelayRunCoroutine(
                    2.5f,//��Œ萔��������
                    () => m_zombieAction.Dead()//���S
                    ));
        StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
    }
    /// <summary>
    /// �R���C�_�[������
    /// </summary>
    private void EnableCollider()
    {
        //�S�Ă̎q�I�u�W�F�N�g�̃R���C�_�[�擾
        Collider[] colliders = transform.GetComponentsInChildren<Collider>();

        foreach(var col in colliders)
        {
            col.enabled = false;//������
        }

        GetComponent<Rigidbody>().useGravity = false;
    }

    /// <summary>
    /// </para>�ړ��s��Ԃɂ���</para>
    /// �����ŌĂяo��
    /// </summary>
    /// <param name="_sec">��~����b��</param>
    public void FreezePosition(float _sec)
    {
        //�ړ���~�t���O�I��
        m_isFreezePos = true;
        //���΂炭������I�t�ɂ���
        m_inActionDelays.Add(
            DelayRunCoroutine(
                    _sec,
                    () => m_isFreezePos = false
                    ));
        StartCoroutine(m_inActionDelays[m_inActionDelays.Count - 1]);
    }


    /// <summary>
    /// �x�点��Action�����s����R���[�`��
    /// �L�����Z����
    /// </summary>
    /// <param name="_wait_sec">�x���b��</param>
    /// <param name="_action">�x������s���鏈��</param>
    private IEnumerator DelayRunCoroutine(float _wait_sec, Action _action)
    {
        //���̃R���[�`���̏��擾 �o����΃��X�g�ǉ��������ł�肽��
        IEnumerator this_coroutine = m_inActionDelays[m_inActionDelays.Count - 1];

        //�R���[�`�����ĊJ���Ă��ҋ@���ԏ�񂪏����Ȃ��悤�ɂ���
        for (float i = 0; i < _wait_sec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);

        _action();
        //�I�����ɂ��̃R���[�`�������폜
        m_inActionDelays.Remove(this_coroutine);
    }

    /// <summary>
    /// <para>�ꎞ��~</para>
    /// �C���^�[�t�F�[�X�ł̒�~�����p
    /// </summary>
    public void Pause()
    {
        m_isStop = true;

        m_zombieAttack.Pause();
        m_zombieMove.StopMove();
        m_zombieAnimation.Pause();

        //���[�v���ɗv�f���ς��Ȃ��悤�ɃN�b�V���������܂�
        List<IEnumerator> tmp_list = new List<IEnumerator>(m_inActionDelays);
        foreach(var cor in tmp_list)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
        
    }
    /// <summary>
    /// <para>�ĊJ</para>
    /// �C���^�[�t�F�[�X�ł̒�~���������p
    /// </summary>
    public void Resume()
    {
        m_isStop = false;

        m_zombieAttack.Resume();
        m_zombieAnimation.Resume();

        List<IEnumerator> tmp_list = new List<IEnumerator>(m_inActionDelays);
        foreach (var cor in tmp_list)
        {
            if (cor == null) continue;

            StartCoroutine(cor);
        }
    }

}

/*
�]���r�̍s��

�@�v���C���[�𔭌������Ƃ�
�A�v���C���[�ֈړ�
�B���ߋ����܂ł�����U��
�C�v���C���[���班������Ă���Ȃ�A�A��
�@�v���C���[����ƂĂ�����Ă���Ȃ�A�ʏ�̍s����

 */
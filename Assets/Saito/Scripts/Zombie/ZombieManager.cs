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

//�]���r�֘A�̃N���X��nameof�ł܂Ƃ߂Ă���������


/// <summary>
/// �]���r�̊Ǘ��N���X
/// ZombieBase���p�������N���X������
/// </summary>
public class ZombieManager : MonoBehaviour
{
    /// <summary>
    /// ���삷��N���X
    /// </summary>
    [SerializeField]
    private ZombieBase[] zombieBases;

    private ZombieMove zombieMove;
    private ZombieAttack zombieAttack;
    private ZombieAnimation zombieAnimation;
    private ZombieAction zombieAction;

    GameObject playerObj;

    [SerializeField]//�v���C���[�̒T�m�͈�
    float detectionPlayerRangeMin = 10.0f;
    [SerializeField]
    float detectionPlayerRangeMax = 30.0f;

    [SerializeField]//�U���J�n����
    float attackStartRange = 3.0f;

    [SerializeField]//���̃I�u�W�F�N�g���폜����v���C���[�Ƃ̋���
    float despawnPlayerDistance = 120.0f;

    //�U���Ώۂ𔭌����Ă���
    private bool isFoundTarget = false;
    //�U���̃N�[���^�C����
    private bool isAttackCoolDown = false;
    //�����_���Ɍ�����ς���N�[���^�C����
    private bool isChangeDirCoolDown = false;
    //�ړ��s�t���O
    private bool isFreezePos = false;
    //���S�σt���O
    private bool isDead = false;
    //�X�^���t���O
    private bool isStan = false;

    //�X�^�������L�����Z���p�g�[�N��
    private CancellationTokenSource stanCancellTokenSource = new CancellationTokenSource();

    //�ڕW�Ƃ������
    Quaternion targetRotation;

    [SerializeField]//Mesh���A�^�b�`���ꂽ�I�u�W�F�N�g
    GameObject meshObj;

    private void Awake()
    {
        //�v���C���[�I�u�W�F�N�g�擾
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var zombie in zombieBases){
            Debug.Log(zombie);

            //�e�N���X�ŃI�[�o�[���C�h���������ݒ���s
            zombie.SetUpZombie();

            //���L�N���X�ɊY�����邩�m�F�����
            //�ǂ����Ⴄ�N���X��TryGetComponent�����null������������ۂ��̂�null�`�F�b�N
            if (zombieMove == null) zombie.TryGetComponent(out zombieMove);
            if (zombieAttack == null) zombie.TryGetComponent(out zombieAttack);
            if (zombieAnimation == null) zombie.TryGetComponent(out zombieAnimation);
            if (zombieAction == null) zombie.TryGetComponent(out zombieAction);
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (playerObj == null) return;
        if (isDead) return;//���S�ςȂ瓮�����Ȃ�
        if (isStan) return;//�X�^�����͓������Ȃ�

        //���W�擾
        Vector3 pos = transform.position;
        Vector3 playerPos = playerObj.transform.position;
        pos.y = 0; playerPos.y = 0;//����y���W�𖳎�
        //�v���C���[�Ƃ̋����v�Z
        float playerDistance = Vector3.Distance(pos, playerPos);

        //�v���C���[���痣�ꂷ������폜����
        //if(playerDistance > despawnPlayerDistance)
        //{
        //    Destroy(gameObject);
        //}

        //�U���Ώۂ������Ă��邩
        if(playerDistance < detectionPlayerRangeMin)
        {
            isFoundTarget = true;//����
        }
        else
        {
            isFoundTarget = false;
        }

        //�ړ�
        {
            if (isFreezePos || isAttackCoolDown)//��~
            {
                //�v���C���[�̕�������
                zombieMove.LookAtPosition(playerPos);

                zombieMove.StopMove();
                zombieAnimation.Idle();//��~���[�V����
            }
            else if (isFoundTarget)
            {
                //�v���C���[�̕�������
                zombieMove.LookAtPosition(playerPos);

                if (playerDistance < attackStartRange)
                {
                    //�Ƃ肠�����߂Â������Ȃ��悤�ɂ���
                    zombieMove.StopMove();
                    zombieAnimation.Idle();//��~���[�V����

                    Attack();//�U��
                }
                else
                {
                    //����
                    zombieMove.RunFront();
                    zombieAnimation.Run();//�ړ����[�V����
                }

            }
            else//�ʏ�̍s��
            {
                //�����ύX
                if (!isChangeDirCoolDown)
                {
                    isChangeDirCoolDown = true;//�N�[���^�C������
                    _ = DelayRunAsync(
                        UnityEngine.Random.Range(4.0f, 8.0f),//���Ɍ�����ς���܂ł̎��Ԃ����߂�
                        () => isChangeDirCoolDown = false  //�t���O�I�t
                        );

                    //�����_���Ɍ�����ݒ�
                    Vector3 direction = new Vector3(0, UnityEngine.Random.Range(-180, 180), 0);
                    //������ύX
                    zombieMove.ChangeDirection(Quaternion.Euler(direction));
                }

                //����
                zombieMove.WalkFront();
                zombieAnimation.Walk();//�ړ����[�V����
            }

        }
    }
    //�U��
    private void Attack()
    {
        if (isAttackCoolDown) return;//�N�[���^�C���`�F�b�N

        //�U���J�n
        zombieAttack.StartAttack();
        //�U�����[�V�����Đ�
        zombieAnimation.Attack();

        //�U���̃N�[���^�C�����ɂ���
        isAttackCoolDown = true;
        //���b��N�[���^�C������
        _ = DelayRunAsync(
            3.0,
            () => isAttackCoolDown = false
            );
    }

    //��莞�ԃX�^��
    private void Stan(double _sec)
    {
        if(isStan)
            stanCancellTokenSource.Cancel();//���ݓ����Ă���X�^�������̃L�����Z��

        stanCancellTokenSource = new CancellationTokenSource();

        isStan = true;

        //�ړ��x�N�g�����[���ɂ���
        zombieMove.StopMove();
        zombieAnimation.Idle();//��~���[�V����

        _ = DelayRunAsync(
            _sec,
            stanCancellTokenSource.Token,
            () => isStan = false
            );
    }

    /// <summary>
    /// �̂Ƀ_���[�W���󂯂�
    /// </summary>
    public void DamageBody()
    {
        Debug.Log("Body");

        Stan(2.0);//�X�^��
    }
    //��e�n�_����A�j���[�V������ύX������p�̃I�[�o�[���[�h
    public void DamageBody(Vector3 _hitPos)
    {
        Debug.Log("Body");

        Vector3 vec = _hitPos - transform.position;

        Vector3 axis = Vector3.Cross(transform.forward, vec);

        if (axis.y < 0)
        {
            Debug.Log("����");
            zombieAnimation.DamageHitLeft();
        }
        else
        {
            Debug.Log("�E��");
            zombieAnimation.DamageHitRight();
        }

        Stan(2.0);//�X�^��
    }
    /// <summary>
    /// ���Ƀ_���[�W���󂯂�
    /// </summary>
    public void DamageHead()
    {
        Debug.Log("Head");

        if (isDead) return;

        isDead = true;

        isFreezePos = true;//�ړ���~

        zombieAnimation.Die();//�A�j���[�V����

        //�A�j���[�V�������I��邱��ɃI�u�W�F�N�g������
        _ = DelayRunAsync(
                    3.5,//��Œ萔��������
                    () => zombieAction.Dead()//���S
                    );
    }

    /// <summary>
    /// �ړ��s��Ԃɂ���
    /// (��~���鎞��)
    /// �����ŌĂяo���Ă�
    /// </summary>
    public void FreezePosition(double _sec)
    {
        //�ړ���~�t���O�I��
        isFreezePos = true;
        //���΂炭������I�t�ɂ���
        _ = DelayRunAsync(
                    _sec,
                    () => isFreezePos = false
                    );
    }

    //�F�̃A���t�@�l�ύX
    public void ChangeColorAlpha(float _alpha)
    {
        Color currentColor = meshObj.GetComponent<Renderer>().materials[1].color;
        meshObj.GetComponent<Renderer>().materials[1].color = new Color(currentColor.r,currentColor.g,currentColor.b,_alpha);
    }

    /// <summary>
    /// �x�点��Action�����s����async
    /// </summary>
    private async ValueTask DelayRunAsync(double _wait_sec, Action _action)
    {
        // �f�B���C����
        await Task.Delay(TimeSpan.FromSeconds(_wait_sec));
        _action();
    }
    //�L�����Z���p
    private async ValueTask DelayRunAsync(double _wait_sec, CancellationToken _token, Action _action)
    {
        // �f�B���C����
        await Task.Delay(TimeSpan.FromSeconds(_wait_sec), _token);
        _action();
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
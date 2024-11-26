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
    private ZombieHP zombieHP;

    GameObject playerObj;

    [SerializeField]//�v���C���[�̒T�m�͈�
    float detectionPlayerRangeMin = 10.0f;
    [SerializeField]
    float detectionPlayerRangeMax = 30.0f;

    //���݂̃v���C���[�T�m�͈�
    private float currentDetectionRange;

    [SerializeField]//�T�m�͈͉����p
    GameObject debugDetectionCirclePrefab;

    GameObject debugDetectionCircle;

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

    [SerializeField]//Mesh���A�^�b�`���ꂽ�I�u�W�F�N�g
    GameObject meshObj;
    //���݂̐F�̃A���t�@�l
    private float currentAlpha;

    private void Awake()
    {
        //�v���C���[�I�u�W�F�N�g�擾
        playerObj = GameObject.FindGameObjectWithTag("Player");

        //�J���[�̃A���t�@�l�擾
        currentAlpha = meshObj.GetComponent<Renderer>().materials[1].color.a;

        currentDetectionRange = detectionPlayerRangeMin;

        //�f�o�b�O�p
        if (debugDetectionCirclePrefab != null)
            debugDetectionCircle = Instantiate(debugDetectionCirclePrefab,
                transform.position + transform.up * 0.2f, 
                Quaternion.AngleAxis(-90.0f,Vector3.left), 
                transform
                );
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
            if (zombieHP == null) zombie.TryGetComponent(out zombieHP);
        }

        Debug.Log(zombieHP.GetCurrentHP());
    }


    // Update is called once per frame
    void Update()
    {
        if (playerObj == null) return;

        //���S�`�F�b�N
        if (zombieHP.IsDead())
            Dead();

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


        ChangeDetectRange();//�T�m�͈͌v�Z

        //�f�o�b�O�p �͈͉���
        if(debugDetectionCircle != null)
        {
            float scale =  currentDetectionRange;
            debugDetectionCircle.transform.localScale = new Vector3(scale, scale, 1);
        }

        //�U���Ώۂ������Ă��邩
        if (playerDistance < currentDetectionRange)
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
        if (isDead) return;

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

    //�T�m�͈͕ύX
    private void ChangeDetectRange()
    {
        //�v���C���[�̃X�N���v�g�擾
        player playerScript = playerObj.GetComponent<player>();
        if (playerScript == null) return;

        //�v���C���[�̗͎̑擾
        float maxHP = playerScript.hp_num_max;
        float currentHP = playerScript.hp_num_now;

        //���݂̗̑͂̊����擾
        float currentHPPer = currentHP / maxHP;

        //���݂̗̑͊�������T�m�͈͂��ԂŌv�Z
        currentDetectionRange = detectionPlayerRangeMin * currentHPPer + 
            detectionPlayerRangeMax * (1.0f - currentHPPer);
    }

    /// <summary>
    /// �̂Ƀ_���[�W���󂯂�
    /// </summary>
    public void DamageBody(int _damage)
    {
        Debug.Log("Body");

        zombieHP.Damage(_damage);
        zombieAnimation.DamageHitLeft();

        Stan(2.0);//�X�^��
    }
    //��e�n�_����A�j���[�V������ύX������p
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

        zombieHP.Damage(1);

        Stan(2.0);//�X�^��
    }
    public void DamageBody(Vector3 _hitPos, int _damage)
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

        zombieHP.Damage(_damage);//�_���[�W

        Stan(2.0);//�X�^��
    }
    /// <summary>
    /// ���Ƀ_���[�W���󂯂�
    /// </summary>
    public void DamageHead()
    {
        Debug.Log("Head");

        zombieAttack.AttackCancel();//�U�������̃L�����Z��

        zombieHP.Damage(1000);
    }
    public void DamageHead(int _damage)
    {
        Debug.Log("Head");

        zombieAttack.AttackCancel();//�U�������̃L�����Z��

        zombieHP.Damage(_damage * 2);//�_���[�W
        
        Stan(2.5);//�X�^��
    }

    //��莞�ԃX�^��
    private void Stan(double _sec)
    {
        if (isDead) return;

        if (isStan)
            stanCancellTokenSource.Cancel();//���ݓ����Ă���X�^�������̃L�����Z��

        zombieAttack.AttackCancel();//�U�������̃L�����Z��

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
    /// ���S����
    /// </summary>
    private void Dead()
    {
        if (isDead) return;

        isDead = true;//�ʂ̓�����~�߂邽�߂Ƀt���O�I��
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
        //�F���ς��Ȃ��ꍇ�������s��Ȃ��悤�ɂ���
        if (currentAlpha == _alpha) return;
        currentAlpha = _alpha;

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
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
public class ZombieManager : MonoBehaviour, IStopObject
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
    private ZombieSound zombieSound;

    GameObject playerObj;

    [SerializeField]//�v���C���[�̒T�m�͈�
    float detectionPlayerRangeMin = 10.0f;
    [SerializeField]
    float detectionPlayerRangeMax = 30.0f;

    //���݂̃v���C���[�T�m�͈�
    private float currentDetectionRange;

    [SerializeField]//�U���J�n����
    float attackStartRange = 3.0f;

    [SerializeField]//���̃I�u�W�F�N�g���폜����v���C���[�Ƃ̋���
    float despawnPlayerDistance = 120.0f;

    //�U���Ώۂ𔭌����Ă���
    private bool isFoundTarget = false;
    //�����_���Ɍ�����ς���N�[���^�C����
    private bool isChangeDirCoolDown = false;
    //�ړ��s�t���O
    private bool isFreezePos = false;
    //���S�σt���O
    private bool isDead = false;
    //�X�^���t���O
    private bool isStan = false;
    //�ꎞ��~
    private bool isStop = false;

    [SerializeField] //�`���[�g���A���p��
    private bool isTutorialObj = false;

    //�X�^�������L�����Z���p�g�[�N��
    private IEnumerator stanCoroutine;

    [SerializeField]//Mesh���A�^�b�`���ꂽ�I�u�W�F�N�g
    GameObject meshObj;
    //���݂̐F�̃A���t�@�l
    private float currentAlpha;

    //���쒆�̒x������
    List<IEnumerator> inActionDelays = new List<IEnumerator>();

    private void Awake()
    {
        //�v���C���[�I�u�W�F�N�g�擾
        playerObj = GameObject.FindGameObjectWithTag("Player");

        //�J���[�̃A���t�@�l�擾
        currentAlpha = meshObj.GetComponent<Renderer>().materials[1].color.a;

        currentDetectionRange = detectionPlayerRangeMin;

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
            if (zombieSound == null) zombie.TryGetComponent(out zombieSound);
        }
        
        Debug.Log("�]���r�����̗�:"+zombieHP.GetCurrentHP());

        if (isTutorialObj)
            zombieAnimation.Idle();
    }


    // Update is called once per frame
    void Update()
    {
        if (isStop) return;
        if (playerObj == null) return;

        //���S�`�F�b�N
        if (zombieHP.IsDead())
            Dead();

        if (isTutorialObj)
        {
            Attack();//�U���̂݌J��Ԃ�
            return;
        }

        if (isDead) return;//���S�ςȂ瓮�����Ȃ�
        if (isStan) return;//�X�^�����͓������Ȃ�

        //���W�擾
        Vector3 pos = transform.position;
        Vector3 playerPos = playerObj.transform.position;
        pos.y = 0; playerPos.y = 0;//����y���W�𖳎�
        //�v���C���[�Ƃ̋����v�Z
        float playerDistance = Vector3.Distance(pos, playerPos);

        //�v���C���[���痣�ꂷ�����瓮�����Ȃ�
        if (playerDistance > despawnPlayerDistance)
        {
            zombieMove.StopMove();
            zombieAnimation.Idle();//��~���[�V����
            return;
        }


        ChangeDetectRange();//�T�m�͈͌v�Z


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
            if (isFreezePos || zombieAttack.IsAttack)//��~
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
                    inActionDelays.Add(
                        DelayRunCoroutine(
                        UnityEngine.Random.Range(4.0f, 8.0f),//���Ɍ�����ς���܂ł̎��Ԃ����߂�
                        () => isChangeDirCoolDown = false  //�t���O�I�t
                        ));
                    StartCoroutine(inActionDelays[inActionDelays.Count - 1]);

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
        if (zombieAttack.IsAttack) return;//�N�[���^�C���`�F�b�N
        if (isDead) return;

        //�U���J�n
        zombieAttack.StartAttack();
        //�U�����[�V�����Đ�
        zombieAnimation.Attack();
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
    //��e�n�_����A�j���[�V������ύX������p
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
        //�G�t�F�N�g�\��
        zombieAnimation.DamagedEffect(_hitPos);

        zombieHP.Damage(_damage);//�_���[�W

        Stan(2.0f);//�X�^��
    }
    /// <summary>
    /// ���Ƀ_���[�W���󂯂�
    /// </summary>
    public void DamageHead(int _damage)
    {
        Debug.Log("Head");

        zombieAttack.AttackCancel();//�U�������̃L�����Z��

        zombieHP.Damage(_damage * 2);//�_���[�W

        zombieAnimation.DamageHitRight();

        Stan(2.5f);//�X�^��
    }
    public void DamageHead(Vector3 _hitPos, int _damage)
    {
        Debug.Log("Head");
        
        zombieAttack.AttackCancel();//�U�������̃L�����Z��

        zombieHP.Damage(_damage * 2);//�_���[�W

        //�A�j���[�V����
        zombieAnimation.DamageHitRight();
        //�G�t�F�N�g�\��
        zombieAnimation.DamagedEffect(_hitPos);

        Stan(2.5f);//�X�^��
    }

    //��莞�ԃX�^��
    private void Stan(float _sec)
    {
        if (isDead) return;

        if (isStan && stanCoroutine != null)
        {
            StopCoroutine(stanCoroutine);
            inActionDelays.Remove(stanCoroutine);
            stanCoroutine = null;
        }
           // stanCancellTokenSource.Cancel();//���ݓ����Ă���X�^�������̃L�����Z��

        zombieAttack.AttackCancel();//�U�������̃L�����Z��

        //stanCancellTokenSource = new CancellationTokenSource();

        isStan = true;

        //�ړ��x�N�g�����[���ɂ���
        zombieMove.StopMove();
        zombieAnimation.Idle();//��~���[�V����


        inActionDelays.Add(
            DelayRunCoroutine(
            _sec,
            () => isStan = false
            ));
        stanCoroutine = inActionDelays[inActionDelays.Count - 1];
        StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
        
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
        zombieSound.PlayDead();//�T�E���h

        EnableCollider();//�R���C�_�[������

        //�A�j���[�V�������I��邱��ɃI�u�W�F�N�g������
        inActionDelays.Add(
            DelayRunCoroutine(
                    3.5f,//��Œ萔��������
                    () => zombieAction.Dead()//���S
                    ));
        StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
    }
    //�R���C�_�[������
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
    /// �ړ��s��Ԃɂ���
    /// (��~���鎞��)
    /// �����ŌĂяo���Ă�
    /// </summary>
    public void FreezePosition(float _sec)
    {
        //�ړ���~�t���O�I��
        isFreezePos = true;
        //���΂炭������I�t�ɂ���
        inActionDelays.Add(
            DelayRunCoroutine(
                    _sec,
                    () => isFreezePos = false
                    ));
        StartCoroutine(inActionDelays[inActionDelays.Count - 1]);
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
    /// �x�点��Action�����s����R���[�`��
    /// </summary>
    private IEnumerator DelayRunCoroutine(float _wait_sec, Action _action)
    {
        //���̃R���[�`���̏��擾 �o����΃��X�g�ǉ��������ł�肽��
        IEnumerator thisCor = inActionDelays[inActionDelays.Count - 1];
        yield return new WaitForSeconds(_wait_sec);

        _action();
        //�I�����ɂ��̃R���[�`�������폜
        inActionDelays.Remove(thisCor);
    }

    //�ꎞ��~
    public void Pause()
    {
        isStop = true;

        zombieAttack.Pause();

        foreach(var cor in inActionDelays)
        {
            if (cor == null) continue;

            StopCoroutine(cor);
        }
        
    }
    //�ĊJ
    public void Resume()
    {
        isStop = false;

        zombieAttack.Resume();

        foreach (var cor in inActionDelays)
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
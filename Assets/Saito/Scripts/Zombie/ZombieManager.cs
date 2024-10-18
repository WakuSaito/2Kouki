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
    float detectionPlayerRange = 10.0f;

    [SerializeField]//�U���J�n����
    float attackStartRange = 1.0f;

    //�U���̃N�[���^�C����
    private bool isAttackCoolDown = false;
    //�����_���Ɍ�����ς���N�[���^�C����
    private bool isChangeDirCoolDown = false;
    //�ړ��s�t���O
    private bool isFreezePos = false;

    //�ڕW�Ƃ������
    Quaternion targetRotation;

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
        //���W�擾
        Vector3 pos = transform.position;
        Vector3 playerPos = playerObj.transform.position;
        pos.y = 0; playerPos.y = 0;//����y���W�𖳎�
        //�v���C���[�Ƃ̋����v�Z
        float playerDistance = Vector3.Distance(pos, playerPos);

        //�ړ�
        {
            //��~
            if (playerDistance < 0.5f|| isFreezePos)
            {
                //�Ƃ肠�����߂Â������Ȃ��悤�ɂ���
                zombieMove.StopMove();
                zombieAnimation.Idle();//��~���[�V����
            }
            else if (playerDistance < detectionPlayerRange)
            {
                //�v���C���[�̕��̌��������߂�
                Vector3 direction = playerPos - pos;
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                //�p�x����
                //�����Ƒ���œ����悤�ȃX�N���v�g�Ȃ̂ŉ��P�̗]�n���� ��ԕ��@����l�̗]�n����
                var qua = Quaternion.RotateTowards(transform.rotation, targetRotation, 500 * Time.deltaTime);
                //������ύX
                zombieMove.ChangeDirection(qua);

                //����
                zombieMove.RunFront();
                zombieAnimation.Run();//�ړ����[�V����
            }
            else
            {
                //�����ύX
                if (!isChangeDirCoolDown)
                {
                    isChangeDirCoolDown = true;//�N�[���^�C������
                    DelayRunAsync(
                        UnityEngine.Random.Range(4.0f, 8.0f),//���Ɍ�����ς���܂ł̎��Ԃ����߂�
                        () => isChangeDirCoolDown = false  //�t���O�I�t
                        );

                    //�����_���Ɍ�����ݒ�
                    Vector3 direction = new Vector3(0, UnityEngine.Random.Range(-180, 180), 0);
                    targetRotation = Quaternion.Euler(direction);
                }

                //�p�x����
                var qua = Quaternion.RotateTowards(transform.rotation, targetRotation, 100 * Time.deltaTime);
                //������ύX
                zombieMove.ChangeDirection(qua);

                //����
                zombieMove.WalkFront();
                zombieAnimation.Walk();//�ړ����[�V����
            }
        }
        

        //�U��
        {
            if (isAttackCoolDown) return;//�N�[���^�C���`�F�b�N

            if (playerDistance < attackStartRange) 
            {
                //�U���J�n
                zombieAttack.StartAttack();
                //�U�����[�V�����Đ�
                zombieAnimation.Attack();

                //�U���̃N�[���^�C�����ɂ���
                isAttackCoolDown = true;
                //���b��N�[���^�C������
                DelayRunAsync(
                    3.0,
                    () => isAttackCoolDown = false
                    );
            }
        }
    }

    /// <summary>
    /// �̂Ƀ_���[�W���󂯂�
    /// </summary>
    public void DamageBody()
    {
        Debug.Log("Body");

        Vector3 playerPos = playerObj.transform.position;
        Vector3 pos = transform.position;
        //�v���C���[�Ƃ͋t�����̃x�N�g�������߂�
        Vector3 vec = (playerPos - pos) * -1.0f;

        //�̂����点��
        zombieAction.KnockBack(vec);
    }
    /// <summary>
    /// ���Ƀ_���[�W���󂯂�
    /// </summary>
    public void DamageHead()
    {
        Debug.Log("Head");
        zombieAction.Dead();//���S
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
        DelayRunAsync(
                    _sec,
                    () => isFreezePos = false
                    );
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

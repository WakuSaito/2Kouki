using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

//�����ŃA�^�b�`�����X�N���v�g
[RequireComponent(typeof(ZombieMove))]
[RequireComponent(typeof(ZombieAnimation))]

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

    GameObject playerObj;

    [SerializeField]//�v���C���[�̒T�m�͈�
    float detectionPlayerRange = 10.0f;

    [SerializeField]//�U���J�n����
    float attackStartRange = 1.0f;

    //await�Ńt���O�̃I���I�t�������ق����ǂ�����
    float randomWalkTime = 0.0f;//�����_���E�H�[�N�̖ڕW���ԗp
    float randomWalkCount = 0.0f;//�����_���E�H�[�N�̎��Ԍv���p

    //�U���̃N�[���^�C����
    private bool isAttackCoolDown = false;

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
            //�ǂ����Ⴄ�N���X��TryGetComponent�����null������������ۂ�
            if (zombieMove == null) zombie.TryGetComponent(out zombieMove);
            if (zombieAttack == null) zombie.TryGetComponent(out zombieAttack);
            if (zombieAnimation == null) zombie.TryGetComponent(out zombieAnimation);
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
            if(playerDistance < 0.5f)
            {
                //�Ƃ肠�����߂Â������Ȃ��悤�ɂ���
                zombieMove.StopMove();
            }
            else if (playerDistance < detectionPlayerRange)
            {
                //�v���C���[�̕��̌��������߂�
                Vector3 direction = playerPos - pos;
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
                //�����̕ύX
                zombieMove.ChangeDirection(lookRotation);

                //����
                zombieMove.RunFront();
            }
            else
            {
                if (randomWalkCount >= randomWalkTime)
                {
                    randomWalkCount = 0.0f;//���Z�b�g
                                           //���Ɍ�����ς���܂ł̎��Ԃ����߂�
                    randomWalkTime = UnityEngine.Random.Range(4.0f, 8.0f);

                    //�����_���Ɍ�����ݒ�
                    Vector3 direction = new Vector3(0, UnityEngine.Random.Range(0, 180), 0);
                    //������ύX
                    zombieMove.ChangeDirection(Quaternion.Euler(direction));
                }
                else
                {
                    //���ԃJ�E���g
                    randomWalkCount += Time.deltaTime;
                }

                //����
                zombieMove.WalkFront();
            }
        }

        //�U��
        {
            if (isAttackCoolDown) return;//�N�[���^�C���`�F�b�N

            if (playerDistance < attackStartRange) 
            {
                //�U���J�n
                zombieAttack.StartAttack();

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


    private async ValueTask DelayRunAsync(double wait_sec, Action action)
    {
        // �f�B���C����
        await Task.Delay(TimeSpan.FromSeconds(wait_sec));
        action();
    }

}

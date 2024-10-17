using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

//�폜�\��̃N���X
public class Zombie : MonoBehaviour
{
    [SerializeField]//�T�m�͈�
    float detection_range = 20.0f;
    [SerializeField]//�͂ދ���
    float grap_range = 1.0f;
    [SerializeField]//���鑬�x
    float run_speed = 6.0f;
    [SerializeField]//�������x
    float walk_speed = 1.0f;
    [SerializeField]//�U���̃f�B���C
    double attack_delay_sec = 3.0;

    float random_walk_time = 0.0f;//�����_���E�H�[�N�̖ڕW���ԗp
    float random_walk_count = 0.0f;//�����_���E�H�[�N�̎��Ԍv���p

    Rigidbody rb;

    GameObject PlayerObj;//�v���C���[

    [SerializeField]
    bool on_move_stop = false;//�ړ��s�t���O

    //async�p�g�[�N���i�x�����s�̃L�����Z���p�j
    private CancellationTokenSource _cancellationTokenSource =
            new CancellationTokenSource();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //�v���C���[�̈ʒu�擾
        PlayerObj = GameObject.FindGameObjectWithTag("Player");

        //on_move_stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) DamageBody();
        if (Input.GetKeyDown(KeyCode.B)) DamageHead();

        //�ړ��s�Ȃ珈�����Ȃ�
        if (on_move_stop) {
            rb.velocity = Vector3.zero;
            return;
        }

        //���W�擾
        Vector3 pos = transform.position;
        Vector3 player_pos = PlayerObj.transform.position;
        //�v���C���[�Ƃ̋����v�Z
        float player_distance = Vector3.Distance(pos, player_pos);

        float current_speed;


        if (player_distance <= detection_range)//�v���C���[�Ƃ̋��������ȉ�
        {
            //�v���C���[�̕�������
            var direction = player_pos - pos;
            direction.y = 0;

            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);

            //transform.LookAt(PlayerObj.transform, transform.up);

            current_speed = run_speed;//���x�ύX
        }
        else//�ʏ�̓���
        {
            if (random_walk_count >= random_walk_time)
            {
                random_walk_count = 0.0f;//���Z�b�g
                random_walk_time = UnityEngine.Random.Range(4.0f, 8.0f);//���Ɍ�����ς���܂ł̎���
                //�����_���Ɍ�����ύX
                Vector3 course = new Vector3(0, UnityEngine.Random.Range(0, 180), 0);
                transform.localRotation = Quaternion.Euler(course);
            }
            else
            {
                //���ԃJ�E���g
                random_walk_count += Time.deltaTime;
            }

            current_speed = walk_speed;//���x�ύX
        }

        //y���𖳎�����
        Vector3 vec = transform.forward;
        vec.y = 0.0f;
        Vector3.Normalize(vec);

        rb.velocity = vec * current_speed;

        //�����Ă�������Ɉړ�
        //transform.Translate(Vector3.forward * walk_speed * Time.deltaTime);

        if (player_distance <= grap_range)//�݂͂����鋗��
        {
            GrapPlayer();
        }
    }
    //�v���C���[������
    private void GrapPlayer()
    {
        Debug.Log("Grap");
        on_move_stop = true;//�ړ���~

        //�����Ńv���C���[���̂��܂��֐����Ăяo������
        //���̍ۂ���GameObject�̏���n������

        // CancellationToken�𐶐�
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        //���b��
        //���̊ԂɃ_���[�W���󂯂�ȂǂŃL�����Z��
        DelayRunAsync(token, attack_delay_sec,
            () => BitePlayer() //���݂�
            );    
    }
    //�v���C���[�Ɋ��݂�
    private void BitePlayer()
    {
        //�v���C���[���Q�[���I�[�o�[�ɂ���
        Debug.Log("Attack");

        on_move_stop = false;//�ړ��ĊJ�i����e�X�g�p�j
    }

    //�̂Ƀ_���[�W���󂯂�
    public void DamageBody()
    {
        Debug.Log("Body");
        //�X�^��
        Stan();
    }
    //���Ƀ_���[�W���󂯂�
    public void DamageHead()
    {
        Debug.Log("Head");
        Dead();//���S
    }
    //�X�^��
    private void Stan()
    {
        on_move_stop = true;
        //�̂�����or��莞�Ԓ�~
        _cancellationTokenSource.Cancel();//�x�����s����async��~

        //�̂�����
        rb.AddForce(transform.forward * -1.0f * 20.0f, ForceMode.Impulse);

        DelayRunAsync(1.5,
        () => on_move_stop = false//�ړ��ĊJ
        );
    }
    //���S����
    private void Dead()
    {
        _cancellationTokenSource.Cancel();//�x�����s����async��~

        Destroy(gameObject);
    }
    //�x�点�Ď��s����
    private async ValueTask DelayRunAsync(CancellationToken token, double wait_sec, Action action)
    {
        // �f�B���C����
        await Task.Delay(TimeSpan.FromSeconds(wait_sec), token);
        action();
    }
    private async ValueTask DelayRunAsync(double wait_sec, Action action)
    {
        // �f�B���C����
        await Task.Delay(TimeSpan.FromSeconds(wait_sec));
        action();
    }
}

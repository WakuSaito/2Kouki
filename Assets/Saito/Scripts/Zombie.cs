using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

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

    float random_walk_time = 0.0f;//�����_���E�H�[�N�̖ڕW���ԗp
    float random_walk_count = 0.0f;//�����_���E�H�[�N�̎��Ԍv���p

    GameObject PlayerObj;//�v���C���[

    bool on_move_stop = false;//�ړ��s�t���O

    private readonly CancellationTokenSource _cancellationTokenSource =
            new CancellationTokenSource();

    private void Awake()
    {
        //�v���C���[�̈ʒu�擾
        PlayerObj = GameObject.FindGameObjectWithTag("Player");

        on_move_stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (on_move_stop) return;//�ړ��s�Ȃ珈�����Ȃ�

        //���W�擾
        Vector3 pos = transform.position;
        Vector3 player_pos = PlayerObj.transform.position;
        //�v���C���[�Ƃ̋����v�Z
        float player_distance = Vector3.Distance(pos, player_pos);

        
        if (player_distance <= detection_range)//�v���C���[�Ƃ̋��������ȉ�
        {
            //�v���C���[�̕�������
            transform.LookAt(PlayerObj.transform, transform.up);
            //�����Ă�������Ɉړ�
            transform.Translate(Vector3.forward * run_speed * Time.deltaTime);
        }        
        else//�ʏ�̓���
        {
            if(random_walk_count>=random_walk_time)
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
            //�����Ă�������Ɉړ�
            transform.Translate(Vector3.forward * walk_speed * Time.deltaTime);
        }

        if (player_distance <= grap_range)//�݂͂����鋗��
        {
            GrapPlayer();
        }
    }
    //�v���C���[������
    private void GrapPlayer()
    {
        on_move_stop = true;//�ړ���~


        // CancellationToken�𐶐�
        var token = _cancellationTokenSource.Token;


        //���b��
        //���̊ԂɃ_���[�W���󂯂�ȂǂŃL�����Z��
        DelayRunAsync(token, 3.0,
            () => BitePlayer() //���݂�             
            );    
    }
    //�v���C���[�Ɋ��݂�
    private void BitePlayer()
    {
        //�v���C���[���Q�[���I�[�o�[�ɂ���
        on_move_stop = false;
    }

    //�̂Ƀ_���[�W���󂯂�
    private void DamageBody()
    {
        //�X�^��
    }
    //���Ƀ_���[�W���󂯂�
    private void DamageHead()
    {
        Dead();//���S
    }
    //�X�^��
    private void Stan()
    {
        //�̂�����or��莞�Ԓ�~
        _cancellationTokenSource.Dispose();
    }
    //���S����
    private void Dead()
    {

    }

    private async ValueTask DelayRunAsync(CancellationToken token, double wait_sec, Action action)
    {
        // �f�B���C����
        await Task.Delay(TimeSpan.FromSeconds(3), token);
        action();
    }
}

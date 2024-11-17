using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFunction : MonoBehaviour
{
    //�L�[����
    int key_push_cnt = 0;   //��������
    float push_timer = 0.0f;//�_�u�����͂�Timer

    //�ړ�
    public bool Move(float _speed,Rigidbody _rb)
    {
        bool idle_flag = true;

        //���W�b�g�{�f�B�[�ł̈ړ�
        Vector3 vec = Vector3.zero;

        // W�L�[�i�O���ړ��j
        if (Input.GetKey(KeyCode.W))
        {
            idle_flag = false;
            vec += transform.forward;
        }

        // S�L�[�i����ړ��j
        if (Input.GetKey(KeyCode.S))
        {
            idle_flag = false;
            vec += -transform.forward;
        }

        // D�L�[�i�E�ړ��j
        if (Input.GetKey(KeyCode.D))
        {
            idle_flag = false;
            vec += transform.right;
        }

        // A�L�[�i���ړ��j
        if (Input.GetKey(KeyCode.A))
        {
            idle_flag = false;
            vec += -transform.right;
        }

        //�΂߈ړ��̑��x�����ɂ��邽�ߐ��K��
        vec.Normalize();

        //y�͂��̂܂܁i�������Əd�͂ɉe�������邽�߁j
        _rb.velocity = new Vector3(vec.x * _speed, _rb.velocity.y, vec.z * _speed);

        return idle_flag;
    }
    //����
    public bool RunFlag()
    {
        bool run_flag = false;

        //�ړ��L�[�����͂���Ă��Ȃ����_�b�V���R�}���h���͂P��ڂ���Ȃ���Έړ�������
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && key_push_cnt != 1)
        {
            key_push_cnt = 0;
            push_timer = 0.0f;
            run_flag = false;
        }

        //W�L�[���Q����͂��ꂽ��_�b�V��
        if (Input.GetKeyDown(KeyCode.W))
        {
            key_push_cnt++;
        }

        //�_�b�V���R�}���h�P��ڂ̏ꍇ
        if (key_push_cnt == 1)
        {
            run_flag = false;

            //�_�u�����͂���Ȃ���Ε����i�Z�����Ԉȓ��ɂQ����́j
            push_timer += Time.deltaTime;
            if (push_timer >= 1)
            {
                key_push_cnt = 0;
            }
        }
        //�_�u�����͂����΃_�b�V��
        else if (key_push_cnt >= 2)
        {
            run_flag = true;
        }


        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            run_flag = true;
        }

        return run_flag;
    }

    //�}�E�X�J�[�\���\����\��
    public void MouseCursorVisibility()
    {
        if (Input.GetKeyDown(KeyCode.End))
        {
            if (Screen.lockCursor == false)
            {
                Screen.lockCursor = true;
            }
            else
            {
                Screen.lockCursor = false;
            }
        }
    }
}

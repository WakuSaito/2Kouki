using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : CharacterBase
{
    const float Walk_Speed = 5.0f;
    const float Run_Speed = 10.0f;
    const float Max_Y_angle = 60.0f;
    const float Max_X_angle = 90.0f;

    //Pistol
    [SerializeField] GameObject bullet;
    [SerializeField] Transform pistol;
    [SerializeField] GameObject[] angle_change_obj;


    //�ړ�
    bool run_flag = false;  //�����Ă��邩�ǂ����t���O
    int key_push_cnt = 0;   //�L�[���͂��ꂽ��
    float push_timer = 0.0f;//�_�u�����̓J�E���g�p
    //���_�ړ�
    Vector3 mouse_pos;                      //�}�E�X�̈ʒu
    Vector3 angle = new Vector3(0, 0, 0);�@ //�p�x
    [SerializeField] GameObject rot_obj;�@  //�e�ې����ʒu�p
    [SerializeField] GameObject dir_obj;    //�����𐧌䂵����Object

    // Start is called before the first frame update
    void Start()
    {
        mouse_pos = Input.mousePosition;
        angle = this.transform.localEulerAngles;

    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�����
        {
            //�_�b�V������
            {
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
            }

            //����ړ�
            if (run_flag)
            {
                Run();
            }
            //�����ړ�
            else
            {
                Walk();
            }
        }

        //���_�ړ�
        {
            //Y������
            if (angle.y <= Max_Y_angle && angle.y >= -Max_Y_angle)
            {
                angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
            }
            else
            {
                if (angle.y > Max_Y_angle) 
                {
                    //angle.y = Max_Y_angle;
                    angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
                    dir_obj.transform.localEulerAngles = new Vector3(dir_obj.transform.localEulerAngles.x, angle.y);
                }
                else if(angle.y<-Max_Y_angle)
                {
                    //angle.y = -Max_Y_angle;
                    angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
                    dir_obj.transform.localEulerAngles = new Vector3(dir_obj.transform.localEulerAngles.x, angle.y);

                }
            }

            //X������
            if (angle.x <= Max_X_angle && angle.x >= -Max_X_angle)
            {
                angle.x -= (Input.mousePosition.y - mouse_pos.y) * 0.2f;
            }
            else
            {
                if (angle.x > Max_X_angle)
                {
                    angle.x = Max_X_angle;
                }
                else if (angle.x < -Max_X_angle)
                {
                    angle.x = -Max_X_angle;
                }
            }

            //�A���O�����䂵����Object�ɑ��
            for (int i = 0; i < angle_change_obj.Length; i++)
            {
                angle_change_obj[i].transform.localEulerAngles = angle;
            }


            mouse_pos = Input.mousePosition;
        }

        //�U��
        {
            //Pistol
            if (Input.GetMouseButtonDown(0))
            {
                //�ʒu
                Vector3 pos = transform.position;
                //����
                Quaternion rot = rot_obj.transform.rotation;
                //�e�ې���
                Instantiate(bullet, pistol.position, rot);
            }
        }
    }


    void Walk()
    {
        // W�L�[�i�O���ړ��j
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Walk_Speed * transform.forward * Time.deltaTime;
        }

        // S�L�[�i����ړ��j
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Walk_Speed * transform.forward * Time.deltaTime;
        }

        // D�L�[�i�E�ړ��j
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Walk_Speed * transform.right * Time.deltaTime;
        }

        // A�L�[�i���ړ��j
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Walk_Speed * transform.right * Time.deltaTime;
        }
    }
    void Run()
    {
        // W�L�[�i�O���ړ��j
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Run_Speed * transform.forward * Time.deltaTime;
        }

        // S�L�[�i����ړ��j
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Run_Speed * transform.forward * Time.deltaTime;
        }

        // D�L�[�i�E�ړ��j
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Run_Speed * transform.right * Time.deltaTime;
        }

        // A�L�[�i���ړ��j
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Run_Speed * transform.right * Time.deltaTime;
        }
    }

}

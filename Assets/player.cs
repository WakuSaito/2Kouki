using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : CharacterBase
{
    const float Attacked_Speed = 1.5f;
    const float Walk_Speed = 5.0f;
    const float Run_Speed = 10.0f;
    const float Max_Y_angle = 60.0f;
    const float Max_X_angle = 60.0f;
    Vector3 Pistol_angle { get { return new Vector3(0, -15, 0); } }

    //�ړ�
    bool run_flag = false;  //�����Ă��邩�ǂ����t���O
    int key_push_cnt = 0;   //�L�[���͂��ꂽ��
    float push_timer = 0.0f;//�_�u�����̓J�E���g�p

    //�}�b�v�ړ�
    public float moving_distance_X = 0.0f;//�ړ������ۑ�
    public float moving_distance_Z = 0.0f;//�ړ������ۑ�

    //���_�ړ�
    Vector3 mouse_pos;                      //�}�E�X�̈ʒu
    Vector3 angle = new Vector3(0, 0, 0);�@ //�p�x
    [SerializeField] GameObject rot_obj;�@  //�e�ې����ʒu�p
    [SerializeField] GameObject dir_obj;    //�����𐧌䂵����Object
    [SerializeField] GameObject camera_obj;

    Vector3 mouse_start;

    //�A�C�e�����E��
    [SerializeField] GameObject hand;
    [SerializeField] GameObject hand_item;
    GameObject item;

    //�_���[�W����
    public bool attacked_zonbi_flag = false;
    public bool bitten_zonbi_flag = false;
    [SerializeField] GameObject gameover_ui;

    //�U��
    //Pistol
    [SerializeField] GameObject bullet;



    // Start is called before the first frame update
    void Start()
    {
        mouse_pos = Input.mousePosition;
        mouse_start = Input.mousePosition;
        //angle = this.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bitten_zonbi_flag)
        {
            //�ړ�����
            {
                //�_�b�V�����菈��
                if (!attacked_zonbi_flag)
                {
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
                        Move(Run_Speed);
                    }
                    //�����ړ�
                    else
                    {
                        Move(Walk_Speed);
                    }
                }
                else
                {
                    Move(Attacked_Speed);
                }
            }

            //���_�ړ�
            {
                //Y������
                //angle.y += (Input.mousePosition.x - mouse_pos.x) * 0.2f;
                //dir_obj.transform.localEulerAngles = new Vector3(dir_obj.transform.localEulerAngles.x, angle.y);

                //if (mouse_pos.y <= Max_X_angle)
                //{
                //    mouse_pos.y = Max_X_angle;
                //}
                //else if (mouse_pos.y >= -Max_X_angle)
                //{
                //    //mouse_pos.y = -Max_X_angle;
                //}

                //������

                float rot_character = mouse_pos.x;
                rot_character += 2.0f * Time.deltaTime;


                dir_obj.transform.localRotation = Quaternion.Euler(0.0f, rot_character, 0.0f);

                //�c��������
                float rot = mouse_start.y - mouse_pos.y;
                if (Mathf.Abs(rot) <= Max_X_angle)
                {
                    rot += 2.0f * Time.deltaTime;
                    Quaternion rotation = Quaternion.Euler(rot, 0.0f, 0.0f);

                    camera_obj.transform.localRotation = rotation;
                }
                //camera_obj.transform.localRotation = rotation;
                //�A���O�����䂵����Object�ɑ��
                //for (int i = 0; i < angle_change_obj.Length; i++)
                //{
                //    angle_change_obj[i].transform.localEulerAngles = angle;
                //}


                mouse_pos = Input.mousePosition;
            }

            //�A�C�e�����E��
            {
                if (Input.GetMouseButtonDown(1))
                {
                    //�r���[�|�[�g���W�̃��C���΂�
                    Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                    RaycastHit hit = new RaycastHit();

                    if (Physics.Raycast(ray, out hit))
                    {
                        //�A�C�e���܂ł̋����𒲂ׂ�
                        float distance = Vector3.Distance(hit.transform.position, transform.position);

                        //�����߂���ΏE��
                        if (distance <= 5.0f)
                        {
                            item = hit.collider.gameObject;

                            switch (item.tag)
                            {
                                case "pistol":
                                    if (hand_item == null)
                                    {
                                        hand_item = item;
                                        ParentChildren(hand, hand_item);                                //��̎q�ɂ���
                                        hand_item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //�X�P�[���ύX
                                        hand_item.transform.localEulerAngles = Pistol_angle;            //�s�X�g���p�̃A���O���֕ύX
                                    }
                                    break;
                            }

                            Debug.Log(item);
                        }
                    }
                }
            }

            //�U��
            {
                if (Input.GetMouseButtonDown(0) && hand_item != null)
                {
                    switch (hand_item.tag)
                    {
                        case "pistol":
                            //�ʒu
                            Vector3 pos = transform.position;
                            //����
                            Quaternion rot = rot_obj.transform.rotation;
                            //�e�ې���
                            Instantiate(bullet, hand_item.transform.position, rot);
                            break;
                    }
                }
            }

        }
        else//�Q�[���I�[�o�[
        {
            //�]���r�̌����Ă�������ɂ���ē|��������ς���i�]���r�̌����Ă�������̋t�����֓|���i���j�j
            //���A��ʃt�F�[�h�A�E�g

            gameover_ui.SetActive(true);
        }
    }

    private void Move(float _speed)
    {
        Vector3 moving_distance = transform.position;

        // W�L�[�i�O���ړ��j
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += _speed * transform.forward * Time.deltaTime;
        }

        // S�L�[�i����ړ��j
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= _speed * transform.forward * Time.deltaTime;
        }

        // D�L�[�i�E�ړ��j
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += _speed * transform.right * Time.deltaTime;
        }

        // A�L�[�i���ړ��j
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= _speed * transform.right * Time.deltaTime;
        }

        moving_distance_X += transform.position.x - moving_distance.x;
        moving_distance_Z += transform.position.z - moving_distance.z;
    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

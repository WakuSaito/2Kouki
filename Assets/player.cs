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
    const int MAX_AMMO = 60;
    const int GET_AMMO_NUM = 10;

    Vector3 Pistol_angle { get { return new Vector3(0, -15, 0); } }

    //�ړ�
    bool run_flag = false;  //�����Ă��邩�ǂ����t���O
    int key_push_cnt = 0;   //�L�[���͂��ꂽ��
    float push_timer = 0.0f;//�_�u�����̓J�E���g�p
    //���_�ړ�
    Vector3 mouse_pos;                      //�}�E�X�̈ʒu
    Vector3 angle = new Vector3(0, 0, 0);�@ //�p�x
    [SerializeField] GameObject rot_obj;�@  //�e�ې����ʒu�p
    [SerializeField] GameObject dir_obj;    //�����𐧌䂵����Object
    [SerializeField] GameObject camera_obj;

    Vector3 mouse_start;

    //�A�C�e�����E��
    [SerializeField] GameObject hand_parent;
    [SerializeField] GameObject hand_item;
    GameObject ranged_weapon = null;//����������
    HAND_INVENTORY hand = HAND_INVENTORY.NON;

    enum HAND_INVENTORY
    {
        NON,
        LONG_WEAPON,
        SHORT_WEAPON,
    }

    //�A�C�e��
    int pistol_ammo = 10;

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
                float character_rot = mouse_pos.x;
                character_rot += 2.0f * Time.deltaTime;
                dir_obj.transform.localRotation = Quaternion.Euler(0.0f, character_rot, 0.0f);

                //�c��������
                float camera_rot=mouse_start.y-mouse_pos.y;
                if (Mathf.Abs(camera_rot) <= Max_X_angle)
                {
                    camera_rot += 2.0f * Time.deltaTime;
                    Quaternion rotation = Quaternion.Euler(camera_rot, 0.0f, 0.0f);
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
                            //��Ō������Q�[���I�u�W�F�N�g��ۑ�
                            GameObject get_item = hit.collider.gameObject;

                            switch (get_item.tag)
                            {
                                case "pistol":
                                    //����������ɓ����Ă��Ȃ������ꍇ����
                                    if (ranged_weapon == null)
                                    {
                                        ranged_weapon = get_item;

                                        //���肵�A��ɉ����Ȃ���Ύ����I�Ɏ���
                                        if (hand_item == null)
                                        {
                                            hand = HAND_INVENTORY.LONG_WEAPON;
                                            hand_item = ranged_weapon;
                                            ParentChildren(hand_parent, hand_item);                         //��̎q�ɂ���
                                            hand_item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //�X�P�[���ύX
                                            hand_item.transform.localEulerAngles = Pistol_angle;            //�s�X�g���p�̃A���O���֕ύX
                                        }
                                    }
                                    else//���łɏ������Ă����ꍇ
                                    {
                                        Destroy(get_item);

                                        //�e����l��
                                        for (int i = 0; i < GET_AMMO_NUM; i++)
                                        {
                                            //����ɒB���Ă��Ȃ���Γ���
                                            if (pistol_ammo <= MAX_AMMO)
                                            {
                                                pistol_ammo++;
                                            }
                                        }                                        
                                    }
                                    break;
                            }

                            Debug.Log(get_item);
                            Debug.Log(pistol_ammo);
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

                            if (pistol_ammo > 0)
                            {
                                //����
                                Quaternion rot = rot_obj.transform.rotation;
                                //�e�ې���
                                Instantiate(bullet, hand_item.transform.position, rot);
                                pistol_ammo--;
                            }
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

    }

    void ParentChildren(GameObject _parent,GameObject _child)
    {
        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

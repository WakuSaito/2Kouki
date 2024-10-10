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

    //アイテムを拾う
    [SerializeField] GameObject hand_parent;
    [SerializeField] GameObject hand_item;
    GameObject item;

    enum HAND_INVENTORY
    {
        NON,
        LONG_WEAPON,
        SHORT_WEAPON,
    }

    //アイテム
    int pistol_ammo = 10;

    //ダメージ判定
    public bool attacked_zonbi_flag = false;
    public bool bitten_zonbi_flag = false;
    [SerializeField] GameObject gameover_ui;

    //攻撃
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
            //移動処理
            {
                //ダッシュ判定処理
                if (!attacked_zonbi_flag)
                {
                    {
                        //移動キーが入力されていないかつダッシュコマンド入力１回目じゃなければ移動初期化
                        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && key_push_cnt != 1)
                        {
                            key_push_cnt = 0;
                            push_timer = 0.0f;
                            run_flag = false;
                        }

                        //Wキーが２回入力されたらダッシュ
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            key_push_cnt++;
                        }

                        //ダッシュコマンド１回目の場合
                        if (key_push_cnt == 1)
                        {
                            run_flag = false;

                            //ダブル入力されなければ歩き（短い時間以内に２回入力）
                            push_timer += Time.deltaTime;
                            if (push_timer >= 1)
                            {
                                key_push_cnt = 0;
                            }
                        }
                        //ダブル入力されればダッシュ
                        else if (key_push_cnt >= 2)
                        {
                            run_flag = true;
                        }
                    }

                    //走り移動
                    if (run_flag)
                    {
                        Move(Run_Speed);
                    }
                    //歩き移動
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

            //視点移動
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

            //アイテムを拾う
            {
                if (Input.GetMouseButtonDown(1))
                {
                    //ビューポート座標のレイを飛ばす
                    Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                    RaycastHit hit = new RaycastHit();

                    if (Physics.Raycast(ray, out hit))
                    {
                        //アイテムまでの距離を調べる
                        float distance = Vector3.Distance(hit.transform.position, transform.position);

                        //距離近ければ拾う
                        if (distance <= 5.0f)
                        {
                            //例で見つけたゲームオブジェクトを保存
                            GameObject get_item = hit.collider.gameObject;

                            switch (get_item.tag)
                            {
                                case "pistol":
                                    //遠距離武器に入っていなかった場合入手
                                    if (ranged_weapon == null)
                                    {
                                        ranged_weapon = get_item;

                                        //入手し、手に何もなければ自動的に持つ
                                        if (hand_item == null)
                                        {
                                            hand = HAND_INVENTORY.LONG_WEAPON;
                                            hand_item = ranged_weapon;
                                            ParentChildren(hand_parent, hand_item);                         //手の子にする
                                            hand_item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //スケール変更
                                            hand_item.transform.localEulerAngles = Pistol_angle;            //ピストル用のアングルへ変更
                                        }
                                    }
                                    else//すでに所持していた場合
                                    {
                                        Destroy(get_item);

                                        //弾薬を獲得
                                        for (int i = 0; i < GET_AMMO_NUM; i++)
                                        {
                                            //上限に達していなければ入手
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

            //攻撃
            {
                if (Input.GetMouseButtonDown(0) && hand_item != null)
                {
                    switch (hand_item.tag)
                    {
                        case "pistol":

                            if (pistol_ammo > 0)
                            {
                                //向き
                                Quaternion rot = rot_obj.transform.rotation;
                                //弾丸生成
                                Instantiate(bullet, hand_item.transform.position, rot);
                                pistol_ammo--;
                            }
                            break;
                    }
                }
            }

        }
        else//ゲームオーバー
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

        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= _speed * transform.forward * Time.deltaTime;
        }

        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += _speed * transform.right * Time.deltaTime;
        }

        // Aキー（左移動）
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

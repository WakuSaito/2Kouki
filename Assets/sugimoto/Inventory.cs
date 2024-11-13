using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : ID
{
    //�萔
    public const int INVENTORY_MAX = 10;        //�A�C�e���C���x���g���̍ő�g
    public const int WEAPON_INVENTORY_MAX = 4;  //����C���x���g���̍ő�g
    const int ITEM_MAX = 30;                    //�X�^�b�N�ł���ő吔

    //����C���x���g��
    public GameObject[] weapon_hand_obj = new GameObject[WEAPON_INVENTORY_MAX] { null, null, null, null };  //����z��
    int weapon_cnt = 0;
    /*UI�֘A*/
    [SerializeField] GameObject[] weapon_sprite_obj = new GameObject[WEAPON_INVENTORY_MAX]; //�A�C�e���̃X�v���C�g������I�u�W�F
    [SerializeField] Sprite[] weapon_sprite;                                                //��ޕʕ���X�v���C�g
    [SerializeField] Transform frame_pos;
    [SerializeField] GameObject bullet_text_obj;
    [SerializeField] Text bullet_text;
    float color = 0.0f;
    float display_timer = 0.0f;
    bool display_flag = true;

    public enum WEAPON_ID
    {
        HAND,
        KNIFE,
        PISTOL,
        DOG,
    }

    public WEAPON_ID hand_weapon = WEAPON_ID.HAND;


    //�A�C�e���C���x���g�� 
    public int[] item_num = new int[INVENTORY_MAX] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };                    //�A�C�e���̐��ۑ�
    public int[] item_type_id = new int[INVENTORY_MAX] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };      //�A�C�e���̎�ޕۑ�
    [SerializeField] Transform hand_pos;        //��̈ʒu
    /*UI�֘A*/
    [SerializeField] GameObject item_inventory;                                         //�C���x���g��UI
    bool item_inventory_flag = false;                                                   //�C���x���g���J���Ă邩���Ă邩  
    [SerializeField] GameObject[] item_sprite_obj = new GameObject[INVENTORY_MAX];      //�A�C�e���̃X�v���C�g������I�u�W�F
    [SerializeField] Sprite[] item_sprite;                                              //��ޕʃA�C�e���X�v���C�g
    [SerializeField] Text[] item_num_text;                                              //�A�C�e���̌��\��


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BulletText();
    }



    public void ItemInventory()
    {
        //�C���x���g���J��
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!item_inventory_flag)
            {
                Screen.lockCursor = false;
                item_inventory_flag = true;
                item_inventory.SetActive(true);

            }
            else
            {
                Screen.lockCursor = true;
                item_inventory_flag = false;
                item_inventory.SetActive(false);
            }
        }

        if(item_inventory_flag)
        {
            //����C���x���g���\��
            display_flag = true;
            display_timer = 0.0f;
        }
    }

    public int PistolBulletNum()
    {
        //�s�X�g���ɓ����Ă���e�����擾
        if (weapon_hand_obj[(int)WEAPON_ID.PISTOL] != null)
        {
            int num= weapon_hand_obj[(int)WEAPON_ID.PISTOL].GetComponent<Pistol>().pistol_bullet_num;
            return num;
        }
        else
        {
            return 0;
        }
    }

    public int InventoryBulletNum()
    {
        //�C���x���g���ɓ����Ă��邷�ׂĂ̒e�����擾
        int bullet_num = 0;
        for (int i = 0; i < INVENTORY_MAX; i++)
        {
            if (item_type_id[i] == (int)ITEM_ID.BULLET)
            {
                bullet_num += item_num[i];
            }
        }

        return bullet_num;
    }

    public void ReduceInventory(int _i)
    {
        //�C���x���g���̒��g�����炷
        item_num_text[_i].text = item_num[_i] + "";

        //�A�C�e�����Ȃ��Ȃ��UI������
        if (item_num[_i] == 0)
        {
            item_sprite_obj[_i].GetComponent<Image>().sprite = null;
            item_sprite_obj[_i].SetActive(false);
        }
    }

    public void ItemGet(GameObject _item)
    {
        //�A�C�e���擾����(�����A�C�e���͍ő�X�^�b�N������Ȃ���D��I�ɃX�^�b�N������)

        //�A�C�e������ID�擾
        int item_id = (int)_item.GetComponent<ItemSet_ID>().id;

        //�A�C�e�����s�X�g���������ꍇ�̂ݎ擾����A�C�e���ύX
        switch(item_id)
        {
            case (int)ITEM_ID.PISTOL:
                item_id = (int)ITEM_ID.BULLET;
                break;
        }

        //�擾�\�ȃA�C�e���̐�
        int get_num = _item.GetComponent<ItemSet_ID>().get_num;

        //�擾�\����0�ɂȂ�܂Ń��[�v
        while (get_num != 0)
        {
            bool input_flag = false;    //�C���x���g���ɓ���邩���ׂ�
            int input_pos = -1;         //�����ʒu��ۑ�

            //�ŏ��ɃC���x���g���ɓ����A�C�e�������邩���ׂ�
            for (int i = 0; i < INVENTORY_MAX; i++)
            {
                //�C���x���g���̃A�C�e���Ɠ���ID��������
                if (item_type_id[i] == item_id && item_num[i] != ITEM_MAX)
                {
                    //���łɃA�C�e�����ɂ���A�X�^�b�N�������Ȃ����
                    input_flag = true;
                    input_pos = i;
                    break;
                }
            }

            //���ɓ����A�C�e�����Ȃ������ꍇ
            if (!input_flag)
            {
                for (int i = 0; i < INVENTORY_MAX; i++)
                { 
                    //��(-1)�������ID�ۑ����ăA�C�e��������
                    if (item_type_id[i] == -1)
                    {
                        item_type_id[i] = item_id;
                        input_flag = true;
                        input_pos = i;
                        break;
                    }
                }
            }

            Debug.Log(input_pos);


            if (input_flag)
            {
                //�擾�\�ő吔��ۑ�
                int get_max = get_num;

                for (int cnt = 1; cnt <= get_max; cnt++)
                {
                    //�A�C�e������Max����Ȃ����
                    if (item_num[input_pos] != ITEM_MAX)
                    {
                        item_num[input_pos]++;
                        get_num--;
                    }
                }

                //�A�C�e���C���x���g��UI�ύX����
                item_sprite_obj[input_pos].SetActive(true);
                item_sprite_obj[input_pos].GetComponent<Image>().sprite = item_sprite[item_id];
                item_num_text[input_pos].text = item_num[input_pos] + "";
            }
            else
            {
                //�����ꏊ��������Ȃ���ΏI��             
                Debug.Log("�A�C�e����MAX�ł�");
                break;
            }

        }

        

        //�m�F�p
        //for (int i = 0; i <INVENTORY_MAX;i++)
        //{
        //    Debug.Log(item_num[i]);
        //    Debug.Log(item_type_id[i]);
        //}
    }

    public void WeaponGet(GameObject _item)
    {
        //�A�C�e������ID�擾
        ITEM_ID item_id = _item.GetComponent<ItemSet_ID>().id;

        //ID���Ƃɏ���
        switch(item_id)
        {
            //�s�X�g��
            case ITEM_ID.PISTOL:

                //�����蔻���OFF�ɂ���
                _item.GetComponent<BoxCollider>().enabled = false;

                //����C���x���g���ɂȂ������ꍇ
                if (weapon_hand_obj[(int)WEAPON_ID.PISTOL] == null)
                {
                    //����C���x���g���ɓ����
                    weapon_hand_obj[(int)WEAPON_ID.PISTOL] = _item;

                    //��ɉ��������Ă��Ȃ���Ύ����I�Ɏ���
                    if (hand_weapon == WEAPON_ID.HAND)
                    {
                        //�������ւ�
                        HandWeapon(WEAPON_ID.PISTOL);
                        //�v���C���[�̌��݂̕�����s�X�g���ɕύX
                        GetComponent<player>().hand_weapon = weapon_hand_obj[(int)WEAPON_ID.PISTOL];
                    }
                    else
                    {
                        _item.SetActive(false);
                    }
                }
                else
                {
                    //�e��(�A�C�e��)���擾
                    ItemGet(_item);
                    Destroy(_item);
                }
                break;
        }
    }

    public void BulletText()
    {
        bullet_text.text = PistolBulletNum() + "�^" + InventoryBulletNum();
    }

    public void ChangeWeapon()
    {
        //�z�C�[������
        {
            //��]�̎擾
            float mouse_wheel = Input.GetAxis("Mouse ScrollWheel");

            //�}�E�X�z�C�[���ɓ�������������ύX
            if (Mathf.Abs(mouse_wheel) != 0)
            {
                //���݂̕����\��
                weapon_hand_obj[weapon_cnt].SetActive(false);
                weapon_sprite_obj[weapon_cnt].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                //�C���x���g���\��
                display_timer = 0.0f;
                display_flag = true;
            }

            //�}�E�X�z�C�[������
            if (mouse_wheel < 0)
            {
                //���̕���C���x���g����
                weapon_cnt++;
                //����C���x���g���̗̈�𒴂�����ŏ��ɖ߂�
                if (weapon_cnt >= WEAPON_INVENTORY_MAX)
                {
                    weapon_cnt = 0;
                }

                //����C���x���g���̒��g�������Ȃ���Β��g�̂��镐���
                while (weapon_hand_obj[weapon_cnt] == null)
                {
                    if (weapon_hand_obj[weapon_cnt] == null)
                    {
                        //���̕���C���x���g����
                        weapon_cnt++;
                        //����C���x���g���̗̈�𒴂�����ŏ��ɖ߂�
                        if (weapon_cnt >= WEAPON_INVENTORY_MAX)
                        {
                            weapon_cnt = 0;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            //���
            if (mouse_wheel > 0)
            {
                //�O�̕���C���x���g��
                weapon_cnt--;
                if (weapon_cnt < 0)
                {
                    //����C���x���g���̗̈�𒴂�����Ō�ɂ���
                    weapon_cnt = WEAPON_INVENTORY_MAX - 1;
                }

                //����C���x���g���̒��g�������Ȃ���Β��g�̂��镐���
                while (weapon_hand_obj[weapon_cnt] == null)
                {
                    if (weapon_hand_obj[weapon_cnt] == null)
                    {
                        //�O�̕���C���x���g��
                        weapon_cnt--;
                        if (weapon_cnt < 0)
                        {
                            //����C���x���g���̗̈�𒴂�����Ō�ɂ���
                            weapon_cnt = WEAPON_INVENTORY_MAX - 1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            //�}�E�X�z�C�[���ɓ�������������ύX
            if (Mathf.Abs(mouse_wheel) != 0)
            {
                //�����Ă��镐���ύX
                hand_weapon = (WEAPON_ID)weapon_cnt;
                weapon_hand_obj[weapon_cnt].SetActive(true);
                weapon_sprite_obj[weapon_cnt].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                frame_pos.position = weapon_sprite_obj[weapon_cnt].transform.position;
            }

        }

        //����C���x���g���\����\��
        {
            display_timer += Time.deltaTime;

            if (display_timer >= 5.0f)
            {
                display_flag = false;
                color = 0.0f;
            }

            if (!display_flag)
            {
                color += Time.deltaTime;

                for (int i = 0; i < weapon_sprite_obj.Length; i++)
                {
                    //�e�I�u�W�F
                    weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color.a - color);
                    //�I�u�W�F�{��
                    weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, weapon_sprite_obj[i].GetComponent<Image>().color.a - color);
                    //�g�I�u�W�F
                    frame_pos.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, frame_pos.GetComponent<Image>().color.a - color);
                }
            }
            else
            {
                for (int i = 0; i < weapon_sprite_obj.Length; i++)
                {
                    //�e�I�u�W�F
                    weapon_sprite_obj[i].transform.parent.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

                    //�I�u�W�F�{��
                    if (hand_weapon == (WEAPON_ID)i)
                    {
                        weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }
                    else if (weapon_hand_obj[i] != null)
                    {
                        weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    }
                    else
                    {
                        weapon_sprite_obj[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    }

                    //�g�I�u�W�F
                    frame_pos.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                }
            }
        }

        //�z�u�AUI�ݒ�
        bullet_text_obj.SetActive(false);
        switch (hand_weapon)
        {
            case WEAPON_ID.PISTOL:
                bullet_text_obj.SetActive(true);
                //transform�ݒ�
                ParentChildren(hand_pos.gameObject, weapon_hand_obj[weapon_cnt]);
                weapon_hand_obj[weapon_cnt].transform.position = hand_pos.position;
                weapon_hand_obj[weapon_cnt].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //�X�P�[���ύX
                break;
        }

    }

    public void HandWeapon(WEAPON_ID _weapon_id)
    {
        //���݂̕����\��
        weapon_hand_obj[weapon_cnt].SetActive(false);
        weapon_sprite_obj[weapon_cnt].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        //���݂̕����ID��ύX���������ID�ɕύX
        hand_weapon = _weapon_id;
        weapon_cnt = (int)_weapon_id;
        weapon_hand_obj[weapon_cnt].SetActive(true);
        weapon_sprite_obj[weapon_cnt].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        frame_pos.position = weapon_sprite_obj[weapon_cnt].transform.position;
        //����C���x���g���\��
        display_flag = true;
        display_timer = 0.0f;
    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        //�e�q�֌W�ɐݒ�

        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

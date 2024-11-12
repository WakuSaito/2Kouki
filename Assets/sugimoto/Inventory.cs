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
    public GameObject[] weapon_hand_obj = new GameObject[WEAPON_INVENTORY_MAX] { null, null, null, null };
    int weapon_cnt = 0;


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

    public enum WEAPON_ID
    {
        HAND,
        KNIFE,
        PISTOL,
        DOG,
    }

    public WEAPON_ID hand_weapon = WEAPON_ID.HAND;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    }

    public int PistolBulletNum()
    {
        //�s�X�g���ɓ����Ă���e�����擾
        if (weapon_hand_obj[(int)WEAPON_ID.PISTOL] != null)
        {
            return weapon_hand_obj[(int)WEAPON_ID.PISTOL].GetComponent<Pistol>().pistol_bullet_num;
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

        Debug.Log(get_num);


        //�擾�\����0�ɂȂ�܂Ń��[�v
        while (get_num != 0)
        {
            bool input_flag = false;    //�C���x���g���ɓ���邩���ׂ�
            int input_pos = -1;         //�����ʒu��ۑ�

            //�C���x���g���ɓ����A�C�e�������邩���ׂ�
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

            //�����A�C�e�����Ȃ������ꍇ
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
                //�󔒂�������Ȃ���ΏI��
                Debug.Log("�A�C�e����MAX�ł�");
                break;
            }

            if(input_flag)
            {
                //�擾�\�ő吔��ۑ�
                int get_max = get_num;

                for (int cnt = 1; cnt <= get_max; cnt++)
                {
                    //�A�C�e������Max����Ȃ����
                    if (item_num[input_pos] == ITEM_MAX)
                    {

                    }
                    else
                    {
                        item_num[input_pos]++;
                        get_num--;
                    }
                }

                //�擾�\�ȃA�C�e�������Ȃ��Ȃ�ΏI��
                if (get_num <= 0)
                {
                    //�A�C�e���C���x���g��UI�ύX����
                    item_sprite_obj[input_pos].SetActive(true);
                    item_sprite_obj[input_pos].GetComponent<Image>().sprite = item_sprite[input_pos];
                    item_num_text[input_pos].text = item_num[input_pos] + "";
                }
            }

            //�C���x���g�����Ō�܂Ō�����l���\�ȃA�C�e����0�ɂ���
            if(input_pos==INVENTORY_MAX)
                get_num = 0;
        }

        

        //�m�F�p
        //for (int i = 0; i <INVENTORY_MAX;i++)
        //{
        //    Debug.Log(item_num[i]);
        //    Debug.Log(item_type_id[i]);
        //}
    }


    public void HandWeapon()
    {
        //����̐؂�ւ�

        weapon_hand_obj[weapon_cnt].SetActive(false);

        //��]�̎擾
        float mouse_wheel = Input.GetAxis("Mouse ScrollWheel");

        //�}�E�X�z�C�[�����
        if (mouse_wheel > 0)
        {
            //����weapon�C���x���g����
            weapon_cnt++;
            //����C���x���g���̗̈�𒴂�����ŏ��ɖ߂�
            if (weapon_cnt >= WEAPON_INVENTORY_MAX)
            {
                weapon_cnt = 0;
            }

            //�C���x���g���̒��g�������Ȃ���Β��g�̂���C���x���g����
            while (weapon_hand_obj[weapon_cnt] == null)
            {
                if (weapon_hand_obj[weapon_cnt] == null)
                {
                    weapon_cnt++;

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
        if (mouse_wheel < 0)
        {
            //����weapon�C���x���g����
            weapon_cnt--;
            if (weapon_cnt < 0)
            {
                weapon_cnt = WEAPON_INVENTORY_MAX - 1;
            }

            //�C���x���g���̒��g�������Ȃ���Β��g�̂���C���x���g����
            while (weapon_hand_obj[weapon_cnt] == null)
            {
                if (weapon_hand_obj[weapon_cnt] == null)
                {
                    weapon_cnt--;
                    if (weapon_cnt < 0)
                    {
                        weapon_cnt = WEAPON_INVENTORY_MAX - 1;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        //�����Ă��镐���ύX
        hand_weapon = (WEAPON_ID)weapon_cnt;
        weapon_hand_obj[weapon_cnt].SetActive(true);

        switch (hand_weapon)
        {
            case WEAPON_ID.PISTOL:
                //transform�ݒ�
                ParentChildren(hand_pos.gameObject, weapon_hand_obj[weapon_cnt]);
                weapon_hand_obj[weapon_cnt].transform.position = hand_pos.position;
                weapon_hand_obj[weapon_cnt].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //�X�P�[���ύX
                break;
        }

    }

    public void HandWeapon(WEAPON_ID _weapon_id)
    {
        weapon_hand_obj[weapon_cnt].SetActive(false);
        hand_weapon = _weapon_id;
        weapon_cnt = (int)_weapon_id;
        weapon_hand_obj[weapon_cnt].SetActive(true);
    }

    void ParentChildren(GameObject _parent, GameObject _child)
    {
        //�e�q�֌W�ɐݒ�

        _child.transform.parent = _parent.transform;
        _child.transform.position = _parent.transform.position;
    }
}

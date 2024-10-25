using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int INVENTORY_MAX = 10;
    public const int WEAPON_INVENTORY_MAX = 4;
    const int ITEM_MAX = 30;
    
    //�A�C�e���̐��ۑ�
    public int[] item_num = new int[INVENTORY_MAX] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //�A�C�e���̎�ޕۑ�
    public int[] item_type_id = new int[INVENTORY_MAX] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

    public enum WEAPON_ID
    {
        HAND,
        KNIFE,
        PISTOL,
        DOG,
    }

    //����̃C���x���g��
    public GameObject[] weapon_hand_obj = new GameObject[WEAPON_INVENTORY_MAX] { null, null, null, null };
    public int weapon_cnt = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int PistolBulletNum()
    {
        if (weapon_hand_obj[(int)WEAPON_ID.PISTOL] != null)
        {
            return weapon_hand_obj[(int)WEAPON_ID.PISTOL].GetComponent<Pistol>().bullet_num;
        }
        else
        {
            return 0;
        }
    }

    public int InventoryBulletNum()
    {
        int bullet_num = 0;

        for (int i = 0; i < INVENTORY_MAX; i++)
        {
            if (item_type_id[i] == (int)Item.ITEM_ID.BULLET)
            {
                bullet_num += item_num[i];
            }
        }

        return bullet_num;
    }

    public void HandWeapon()
    {
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
                weapon_cnt = WEAPON_INVENTORY_MAX-1;
            }

            //�C���x���g���̒��g�������Ȃ���Β��g�̂���C���x���g����
            while (weapon_hand_obj[weapon_cnt] == null)
            {
                if (weapon_hand_obj[weapon_cnt] == null)
                {
                    weapon_cnt--;
                    if (weapon_cnt < 0)
                    {
                        weapon_cnt = WEAPON_INVENTORY_MAX-1;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        weapon_hand_obj[weapon_cnt].SetActive(true);
    }

    public void ItemGet(GameObject _item)
    {
        //�A�C�e���X�N���v�g����A�C�e����ID�擾
        int item_id = (int)_item.GetComponent<Item>().id;

        //�A�C�e�����s�X�g���������ꍇ�̂ݎ擾����A�C�e���ύX
        if (item_id == (int)Item.ITEM_ID.PISTOL)
        {
            item_id = (int)Item.ITEM_ID.BULLET;
        }

        //�擾�\�ȃA�C�e���̐�
        int get_num = _item.GetComponent<Item>().get_num[item_id];

        while (get_num != 0)
        {

            for (int i = 0; i < INVENTORY_MAX; i++)
            {
                //�C���x���g���̃A�C�e��������(-1)�܂��͓���ID��������
                if (item_type_id[i] == -1 || item_type_id[i] == item_id)
                {
                    if (item_type_id[i] == -1)
                    {
                        item_type_id[i] = item_id;
                    }

                    int get_max = get_num;
                    for (int cnt = 1; cnt <= get_max; cnt++)
                    {
                        //�A�C�e������Max����Ȃ����
                        if (item_num[i] == ITEM_MAX)
                        {
                            break;
                        }
                        else
                        {
                            item_num[i]++;
                            get_num--;
                        }
                    }
                }

                //�擾�\�ȃA�C�e�������Ȃ��Ȃ�ΏI��
                if (get_num <= 0)
                    break;
            }

            //�C���x���g�����Ō�܂Ō�����l���\�ȃA�C�e����0�ɂ���
            get_num = 0;
        }

        //�m�F�p
        //for (int i = 0; i <INVENTORY_MAX;i++)
        //{
        //    Debug.Log(item_num[i]);
        //    Debug.Log(item_type_id[i]);
        //}
    }


}

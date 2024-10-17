using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Item Item;

    public const int INVENTORY_MAX = 10;
    const int ITEM_MAX = 30;
    
    //�A�C�e���̐��ۑ�
    public int[] item_num = new int[INVENTORY_MAX] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //�A�C�e���̎�ޕۑ�
    public int[] item_type_id = new int[INVENTORY_MAX] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

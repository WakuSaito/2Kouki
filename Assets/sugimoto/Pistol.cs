using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    const int BULLET_MAX = 10;

    Inventory Inventory;



    [SerializeField] GameObject player_obj;
    [SerializeField] GameObject bullet_obj;

    //���Ƃ��Ƃ̒e��
    public int bullet_num = 10;

    // Start is called before the first frame update
    void Start()
    {
        Inventory = player_obj.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reload()
    {
        if (bullet_num <= BULLET_MAX)
        {
            for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
            {
                //�C���x���g���ɒe�ۂ����邩
                if (Inventory.item_type_id[i] == (int)Item.ITEM_ID.BULLET)
                {
                    //�s�X�g���ɓ���e�ې��𒲂ׂ�
                    int reload_num = BULLET_MAX - bullet_num;
                    Debug.Log(reload_num);

                    int max_reload = reload_num;

                    for (int cnt = 0; cnt < max_reload; cnt++)
                    {
                        if (Inventory.item_num[i] == 0)
                        {
                            break;
                        }
                        else
                        {
                            Inventory.item_num[i]--;
                            bullet_num++;
                            reload_num--;
                        }
                    }

                    //�C���x���g���ɂ������e�ۂ̎c�肪0�ɂȂ�����id������������
                    if (Inventory.item_num[i] == 0)
                    {
                        Inventory.item_type_id[i] = -1;
                    }
                    break;
                }
            }
        }
    }

    public void Attack(GameObject _rot_obj,GameObject _hand_obj)
    {
        if (bullet_num > 0)
        {
            //�������˂�������
            Quaternion rot = _rot_obj.transform.rotation;
            //�e�ې���
            Instantiate(bullet_obj, _hand_obj.transform.position, rot);

            //Pistol���̒e�ۂ����炷
            bullet_num--;
        }
    }
}

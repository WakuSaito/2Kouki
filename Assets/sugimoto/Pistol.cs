using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    const int BULLET_MAX = 10;

    [SerializeField] GameObject bullet_obj;

    //���Ƃ��Ƃ̒e��
    public int pistol_bullet_num = 10;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reload(Inventory inventory)
    {
        //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
        if (pistol_bullet_num < BULLET_MAX)
        {
            for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
            {
                //�C���x���g���ɒe�ۂ����邩
                if (inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
                {
                    //�s�X�g���ɓ���e�ې��𒲂ׂ�
                    int reload_num = BULLET_MAX - pistol_bullet_num;
                    //reload�ł���ő吔��ۑ�
                    int max_reload = reload_num;

                    for (int cnt = 0; cnt < max_reload; cnt++)
                    {
                        if (inventory.item_num[i] == 0)
                        {
                            //�C���x���g���ɂ������e�ۂ̎c�肪0�ɂȂ�����id������������
                            inventory.item_type_id[i] = -1;
                            break;
                        }
                        else
                        {
                            inventory.item_num[i]--;
                            pistol_bullet_num++;
                            reload_num--;
                        }
                        //�C���x���g���̒��g�����炷
                        inventory.ReduceInventory(i);
                    }
                }
            }
        }
    }

    public void Attack(GameObject _rot_obj,GameObject _hand_obj)
    {
        if (pistol_bullet_num > 0)
        {
            //�r���[�|�[�g���W�̃��C���΂�
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                //�A�C�e���܂ł̋����𒲂ׂ�
                float distance = Vector3.Distance(hit.transform.position, transform.position);

                //�������͈͓��Ȃ�
                if (distance <= 30.0f)
                {
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);

                    GameObject hit_obj = hit.collider.gameObject;
                    Debug.Log(hit_obj);
                    if (hit_obj.tag == "Body")
                    {
                        hit_obj.GetComponentInParent<ZombieManager>().DamageBody();
                    }
                    if (hit_obj.tag == "Head")
                    {
                        hit_obj.GetComponentInParent<ZombieManager>().DamageHead();
                    }
                }
            }
            //�������˂�������
            Quaternion rot = _rot_obj.transform.rotation;
            //�e�ې���
            Instantiate(bullet_obj, _hand_obj.transform.position, rot);

            //Pistol���̒e�ۂ����炷
            pistol_bullet_num--;
            
        }
    }
}

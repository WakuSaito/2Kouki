using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    const int BULLET_MAX = 10;

    [SerializeField] GameObject bullet_obj;

    //���Ƃ��Ƃ̒e��
    public int bullet_num = 10;

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
        if (bullet_num < BULLET_MAX)
        {

            for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
            {
                Debug.Log(inventory.item_type_id[i]);


                //�C���x���g���ɒe�ۂ����邩
                if (inventory.item_type_id[i] == (int)Item.ITEM_ID.BULLET)
                {

                    //�s�X�g���ɓ���e�ې��𒲂ׂ�
                    int reload_num = BULLET_MAX - bullet_num;
                    Debug.Log(reload_num);

                    int max_reload = reload_num;

                    for (int cnt = 0; cnt < max_reload; cnt++)
                    {
                        if (inventory.item_num[i] == 0)
                        {
                            break;
                        }
                        else
                        {
                            inventory.item_num[i]--;
                            bullet_num++;
                            reload_num--;
                        }
                    }

                    //�C���x���g���ɂ������e�ۂ̎c�肪0�ɂȂ�����id������������
                    if (inventory.item_num[i] == 0)
                    {
                        inventory.item_type_id[i] = -1;
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
            bullet_num--;
            
        }
    }
}

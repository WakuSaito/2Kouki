using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    const int BULLET_MAX = 10;

    [SerializeField] GameObject bullet_obj;
    
    //�v���C���[�����������ɑ��
    public GameObject hand_player_obj = null;
    bool set_player_flag = false;

    Animator anim;
    Inventory inventory;

    //���Ƃ��Ƃ̒e��
    public int pistol_bullet_num = 10;

    [SerializeField] private Transform muzzleTransform;//�e���ʒu
    [SerializeField] private GameObject bulletLine;//�e��
    [SerializeField] private float bulletDistance = 20.0f;//�e�̔򋗗�
    [SerializeField] private float bulletSpread = 0.03f;//�e�u��

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hand_player_obj != null && !set_player_flag) 
        {
            inventory = hand_player_obj.GetComponent<Inventory>();
            set_player_flag = true;
        }
    }
        
    public void Reload()
    {
        //�����[�h����
        if (Input.GetKeyDown(KeyCode.R))
        {
            //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
            if (pistol_bullet_num < BULLET_MAX)
            {
                for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
                {
                    //�C���x���g���ɒe�ۂ����邩
                    if (inventory.item_type_id[i] == (int)ITEM_ID.BULLET)
                    {
                        anim.SetBool("Reload", true);  //reload
                        Invoke("ReroadFin", 2.8f);
                    }
                }
            }
        }
    }

    public void Reload(Inventory inventory)
    {
        //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
        if (pistol_bullet_num < BULLET_MAX)
        {
            for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
            {
                //�C���x���g���ɒe�ۂ����邩
                if (inventory.item_type_id[i] == (int)ITEM_ID.BULLET)
                {
                    //�s�X�g���ɓ���e�ې��𒲂ׂ�
                    int reload_num = BULLET_MAX - pistol_bullet_num;
                    //reload�ł���ő吔��ۑ�
                    int max_reload = reload_num;

                    //animation
                    anim.SetBool("Reload", true);  //reload
                    Invoke("ReroadFin", 2.8f);

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
            //�΂���������_���Ɍ��߂�
            float x = Random.Range(-bulletSpread, bulletSpread);
            float y = Random.Range(-bulletSpread, bulletSpread);

            //���_�x�N�g���ɂ΂�������Z
            Vector3 gunVec = _rot_obj.transform.forward + new Vector3(x, y, 0);


            //�e���p��LineRenderer���擾�i�����ڗp�j
            LineRenderer lineRend = Instantiate(bulletLine,
                Vector3.zero,
                Quaternion.identity).GetComponent<LineRenderer>();

            //�_�̐�
            lineRend.positionCount = 2;
            //�n�_�̍��W�w��
            lineRend.SetPosition(0, muzzleTransform.position);


            //�r���[�|�[�g���W�̃��C���΂�
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            RaycastHit hit = new RaycastHit();


            if (Physics.Raycast(ray, out hit))
            {
                //���������ꏊ����̏I�_�ɂ���
                lineRend.SetPosition(1, hit.point);

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
                        hit_obj.GetComponentInParent<ZombieManager>().DamageBody(hit.point,2);
                    }
                    if (hit_obj.tag == "Head")
                    {
                        //hit_obj.GetComponentInParent<ZombieManager>().DamageHead(2);
                    }
                }
            }
            //���C�����ɂ�������Ȃ������Ƃ�
            else
            {
                //�e�ۂ̃x�N�g���̏I�_����̏I�_�ɂ���
                lineRend.SetPosition(1, _rot_obj.transform.position + (gunVec * bulletDistance));
            }

            ////�������˂�������
            //Quaternion rot = _rot_obj.transform.rotation;
            ////�e�ې���
            //Instantiate(bullet_obj, _hand_obj.transform.position, rot);

            //Pistol���̒e�ۂ����炷
            pistol_bullet_num--;
            
        }
    }

    void ReroadFin()
    {
        anim.SetBool("Reload", false);  //reload

        //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
        if (pistol_bullet_num < BULLET_MAX)
        {
            for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
            {
                //�C���x���g���ɒe�ۂ����邩
                if (inventory.item_type_id[i] == (int)ITEM_ID.BULLET)
                {
                    //�s�X�g���ɓ���e�ې��𒲂ׂ�
                    int reload_num = BULLET_MAX - pistol_bullet_num;
                    //reload�ł���ő吔��ۑ�
                    int max_reload = reload_num;

                    //animation

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
}

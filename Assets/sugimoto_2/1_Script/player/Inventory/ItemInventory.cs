using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*  �ł��邱��
 �E�C���x���g���J��
 �E�C���x���g���𒲂ׂ�
 �E�e�Q�[�W�񕜏���
 �E�A�C�e���ړ�
 �E�X���b�g�̊Ǘ�
 �E�����A�C�e�����m�A�X�^�b�N�������Ȃ���΃X�^�b�N����
 */


public class ItemInventory : MonoBehaviour
{
    public InventoryTest Inventory;
    [SerializeField] int sloat_size = 10;
    [SerializeField] Transform[] sprite_pos;
    [SerializeField] Text[] text;
    [SerializeField] GameObject[] sloat_box;

    GameObject catch_obj;
    GameObject select_item;
    int select_sloat_num;
    GameObject hit_box;
    bool drag_flag = false;

    public bool item_inventory_flag = false;

    //�Q�[�W
    [SerializeField] GameObject food_gauge_obj; //�H��
    [SerializeField] GameObject hp_gauge_obj;   //�̗�


    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryTest(sloat_size, sprite_pos, text);
    }

    // Update is called once per frame
    void Update()
    {
        // �f�o�b�O�p: �C���x���g���̓��e��\��
        if (Input.GetKeyDown(KeyCode.D)) { Inventory.PrintInventory(); }

        if (item_inventory_flag)
        {
            CheckInventoryItem();
            ItemCatch();
        }
    }
    public void InventoryOpenOrClose()
    {
        //�C���x���g���J��

        if(item_inventory_flag)
        {
            Screen.lockCursor = true;
            item_inventory_flag = false;
            gameObject.SetActive(item_inventory_flag);
        }
        else
        {
            Screen.lockCursor = false;
            item_inventory_flag = true;
            gameObject.SetActive(item_inventory_flag);
        }

    }

    public void CheckInventoryItem()    //�J�[�\���̂����Ă���A�C�e���𒲂ׂ�
    {
        select_item = null;

        //�}�E�X�̈ʒu����UI���擾����
        //RaycastAll�̈����iPointerEventData�j�쐬
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        //RaycastAll�̌��ʊi�[�pList
        List<RaycastResult> RayResult = new List<RaycastResult>();

        //PointerEventData�Ƀ}�E�X�̈ʒu���Z�b�g
        pointData.position = Input.mousePosition;

        //RayCast�i�X�N���[�����W�j
        EventSystem.current.RaycastAll(pointData, RayResult);

        foreach (RaycastResult result in RayResult)
        {
            int sloat_num = 0;

            //�J�[�\���������Ă���A�C�e��
            foreach (var sloat in Inventory.Sloats)
            {
                if (result.gameObject == sloat.Set_Pos.gameObject)
                {
                    select_item = result.gameObject;
                    select_sloat_num = sloat_num;
                    break;
                }
                sloat_num++;
            }

            //�A�C�e��������ł��Ȃ��ꍇ�A�X���b�g�A�C�e��
            //if (!drag_flag)
            //{
            //    foreach (var sloat in Inventory.Sloats)
            //    {
            //        if (result.gameObject == sloat.Set_Pos.gameObject)
            //        {
            //            catch_obj = result.gameObject;
            //            break;
            //        }
            //    }
            //}

            foreach (var box in sloat_box)
            {
                if (result.gameObject == box)
                {
                    hit_box = box;
                    break;
                }
            }


        }
    }

    public void Recovery_Gage()
    {
        if (select_item != null)
        {
            ITEM_ID _id = Inventory.Sloats[select_sloat_num].ItemInfo.id;

            //�H��
            {
                if (_id >= ITEM_ID.FOOD_1 && _id <= ITEM_ID.FOOD_4)
                {
                    food_gauge_obj.GetComponent<Gauge>().Increase_Gauge(10);
                    //playerSound.PlayEat();//SE
                }
                if (_id >= ITEM_ID.DRINK_1 && _id <= ITEM_ID.DRINK_2)
                {
                    food_gauge_obj.GetComponent<Gauge>().Increase_Gauge(5);
                    //playerSound.PlayDrink();//SE
                }
            }

            //�̗�
            {
                if (_id >= ITEM_ID.EMERGENCY_PACK)
                {
                    hp_gauge_obj.GetComponent<Gauge>().Increase_Gauge(3);
                    //playerSound.PlayHeal();//SE
                }
            }
        }

        Inventory.UseItem(select_sloat_num);
    }

    void ItemCatch()
    {
        //���񂾃I�u�W�F�N�g��ۑ�
        if(Input.GetMouseButtonDown(0)&& select_item != null)
        {
            catch_obj = select_item;
        }

        if (Input.GetMouseButton(0) && catch_obj != null) 
        {
            catch_obj.transform.position = Input.mousePosition;
            drag_flag = true;
        }
        else
        {
            //�X���b�g�̏ꏊ�ɂ������Ă��邩
            if (hit_box != null)
            {
                if (catch_obj != null)
                {
                    //���̏ꏊ�𒲂ׂ�
                    int chatch_num = -1;

                    for (int sloat = 0; sloat < sloat_size; sloat++)
                    {
                        if (catch_obj == Inventory.Sloats[sloat].Set_Pos.gameObject)
                        {
                            chatch_num = sloat;
                            break;
                        }
                    }
                    //�ړ���𒲂ׂ� //�o�O�F�A�C�e����񂪂���ꍇ�͈ړ��ł��Ȃ��悤�ɂ���
                    for (int sloat = 0; sloat < sloat_size; sloat++)
                    {
                        if (hit_box == sloat_box[sloat])
                        {
                            //���g�����ւ���
                            Inventory.ItemSloatChange(chatch_num, sloat);
                            //�ݒu
                            catch_obj.transform.position = hit_box.transform.position;
                            catch_obj = null;
                            drag_flag = false;
                            break;
                        }
                    }

                }
            }
            else
            {
                //���̈ʒu�ɖ߂�
                for (int sloat = 0; sloat < sloat_size; sloat++)
                {
                    if (catch_obj == Inventory.Sloats[sloat].Set_Pos.gameObject)
                    {
                        catch_obj.transform.position = Inventory.Sloats[sloat].Start_Pos;
                        catch_obj = null;
                        drag_flag = false;
                        break;
                    }
                }
            }
        }

    }
}

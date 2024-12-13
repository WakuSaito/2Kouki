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
    [SerializeField] Transform[] sloat_box;

    GameObject catch_obj;
    int catch_sloat_num;
    GameObject select_item;
    int select_sloat_num;
    GameObject hit_box;
    int hit_box_num;
    bool drag_flag = false;

    public bool item_inventory_flag = false;

    //�Q�[�W
    [SerializeField] GameObject food_gauge_obj; //�H��
    [SerializeField] GameObject hp_gauge_obj;   //�̗�


    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryTest(sloat_size, sloat_box);
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
            Inventory.SetUI();
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
        hit_box = null;

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
            //�J�[�\���������Ă���A�C�e��
            for (int cnt = 0; cnt < sloat_box.Length; cnt++)
            {
                if (result.gameObject == sloat_box[cnt].GetChild(0).gameObject)
                {
                    select_item = result.gameObject;
                    select_sloat_num = cnt;
                    break;
                }
            }

            //�J�[�\���������Ă���X���b�g��box
            for (int cnt = 0; cnt < sloat_box.Length; cnt++)
            {
                if (result.gameObject == sloat_box[cnt].gameObject)
                {
                    hit_box = sloat_box[cnt].gameObject;
                    hit_box_num = cnt;
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
            int recovery_num = Inventory.Sloats[select_sloat_num].ItemInfo.recoveryitem_info.recovery_num;

            Debug.Log(recovery_num);

            //�H��
            {
                if (_id >= ITEM_ID.FOOD_1 && _id <= ITEM_ID.FOOD_4)
                {
                    food_gauge_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);
                    //playerSound.PlayEat();//SE
                }
                if (_id >= ITEM_ID.DRINK_1 && _id <= ITEM_ID.DRINK_2)
                {
                    food_gauge_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);
                    //playerSound.PlayDrink();//SE
                }
            }

            //�̗�
            {
                if (_id >= ITEM_ID.EMERGENCY_PACK)
                {
                    hp_gauge_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);
                    //playerSound.PlayHeal();//SE
                }
            }
            Inventory.Sloats[select_sloat_num].UseItem();
        }

    }

    void ItemCatch()
    {
        //���񂾃I�u�W�F�N�g��ۑ�
        if (Input.GetMouseButtonDown(0) && select_item != null)
        {
            catch_obj = select_item;
            catch_sloat_num = select_sloat_num;
        }

        if (catch_obj == null) return;

        if (Input.GetMouseButton(0)) 
        {
            catch_obj.transform.position = Input.mousePosition;
            drag_flag = true;
        }
        else
        {
            if (hit_box != null)
            {
                //�ړ���𒲂ׂ�
                //���ꂽ���ꏊ�̃X���b�g����
                if (Inventory.Sloats[hit_box_num].ItemInfo == null)
                {
                    Debug.Log("a1");
                    //���g�����ւ���
                    Inventory.ItemSloatChange(catch_sloat_num, hit_box_num);
                    //�ݒu
                    catch_obj.transform.position = sloat_box[catch_sloat_num].transform.position;
                    //�ړ��O�̃X���b�g������
                    Inventory.Sloats[catch_sloat_num].CrearSloat();
                }
                else
                {
                    //�X���b�g�A�C�e���{�X���b�g�A�C�e��
                    if (Inventory.AddSloatSloatInventory(catch_sloat_num, hit_box_num))
                    {
                        Debug.Log("a2");

                        //�ݒu
                        catch_obj.transform.position = sloat_box[hit_box_num].transform.position;
                    }
                    else
                    {
                        catch_obj.transform.position = Inventory.Sloat_Box[catch_sloat_num].position;
                    }
                }
                hit_box = null;
                catch_obj = null;
                drag_flag = false;
            }

            if (catch_obj != null) 
            {
                //���̈ʒu�ɖ߂�
                catch_obj.transform.position = sloat_box[catch_sloat_num].transform.position;
                catch_obj = null;
                drag_flag = false;
            }
        }
    }
}

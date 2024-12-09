using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemInventory : MonoBehaviour
{
    public InventoryTest Inventory;
    [SerializeField] int sloat_size = 10;
    [SerializeField] Transform[] sprite_pos;
    [SerializeField] Text[] text;

    public bool item_inventory_flag = false;

    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryTest(sloat_size, sprite_pos, text);
    }

    // Update is called once per frame
    void Update()
    {
        // �f�o�b�O�p: �C���x���g���̓��e��\���i�Ⴆ�΁AD�L�[���������Ƃ��j
        if (Input.GetKeyDown(KeyCode.D)) { Inventory.PrintInventory(); }

        if (item_inventory_flag)
        {
            CheckInventoryItem();
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
        //�}�E�X�̈ʒu����UI���擾����
        //RaycastAll�̈����iPointerEventData�j�쐬
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        //RaycastAll�̌��ʊi�[�pList
        List<RaycastResult> RayResult = new List<RaycastResult>();
        Debug.Log(pointData);

        //PointerEventData�Ƀ}�E�X�̈ʒu���Z�b�g
        pointData.position = Input.mousePosition;

        //RayCast�i�X�N���[�����W�j
        EventSystem.current.RaycastAll(pointData, RayResult);

        foreach (RaycastResult result in RayResult)
        {
            Debug.Log(result.gameObject);

            foreach(var sloat in Inventory.Sloats)
            {
                Debug.Log("t");

                sloat.InMouseSloat(result.gameObject);
            }
        }
    }

}

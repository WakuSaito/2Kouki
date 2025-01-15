using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWeapon : MonoBehaviour
{
    //�C���x���g���}�l�[�W���[
    inventoryManager mInventoryManager;

    //�C���x���g���̗v�f
    public InventoryClass Inventory;
    public int slot_size = 4;
    public Transform[] sprite;
    public Transform[] slot_box;
    public Text[] text;

    //�I�u�W�F�N�g
    [SerializeField] GameObject mInventoryManagerObj;
    [SerializeField] GameObject mWeapon_inventory_UI_obj; //�C���x���g��UI
    [SerializeField] GameObject[] mStartItemObj;

    public enum SLOT_ORDER
    {
        HAND,
        KNIFE,
        GUN,
        DOG,
    }

    //public Sloat_Order select_weapon = Sloat_Order.HAND;
    Color mColorAlphaHalf = new Color(1.0f, 1.0f, 1.0f, 0.5f);//������
    Color mColorAlphaFull = new Color(1.0f, 1.0f, 1.0f, 1.0f);//�s����

    // Start is called before the first frame update
    void Start()
    {
        //�C���x���g���̃C���X�g���N�^�쐬
        Inventory = new InventoryClass(slot_size, slot_box);
        //�C���x���g���}�l�[�W���[�擾
        mInventoryManager = mInventoryManagerObj.GetComponent<inventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartItemSlotSet()
    {
        for (int i = 0; i < mStartItemObj.Length; i++)
        {
            ITEM_ID id = mStartItemObj[i].GetComponent<ItemSetting>().iteminfo.id;
            

            //switch(id)
            //{
            //    case ITEM_ID.PISTOL:
            //        Inventory.Slots[(int)SLOT_ORDER.GUN].ItemInfo=
            //    case ITEM_ID.ASSAULT:
            //    case ITEM_ID.SHOTGUN:
            //        break;
            //    case ITEM_ID.HAND:
            //        break;
            //    case ITEM_ID.KNIFE:
            //        break;
            //    case ITEM_ID.DOG_DIRECTION:
            //        break;
            //}
        }
    }
}

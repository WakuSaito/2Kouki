using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWeapon : MonoBehaviour
{
    //インベントリマネージャー
    inventoryManager mInventoryManager;

    //インベントリの要素
    public InventoryClass Inventory;
    public int slot_size = 4;
    public Transform[] sprite;
    public Transform[] slot_box;
    public Text[] text;

    //オブジェクト
    [SerializeField] GameObject mInventoryManagerObj;
    [SerializeField] GameObject mWeapon_inventory_UI_obj; //インベントリUI
    [SerializeField] GameObject[] mStartItemObj;

    public enum SLOT_ORDER
    {
        HAND,
        KNIFE,
        GUN,
        DOG,
    }

    //public Sloat_Order select_weapon = Sloat_Order.HAND;
    Color mColorAlphaHalf = new Color(1.0f, 1.0f, 1.0f, 0.5f);//半透明
    Color mColorAlphaFull = new Color(1.0f, 1.0f, 1.0f, 1.0f);//不透明

    // Start is called before the first frame update
    void Start()
    {
        //インベントリのインストラクタ作成
        Inventory = new InventoryClass(slot_size, slot_box);
        //インベントリマネージャー取得
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

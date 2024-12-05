using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    public ItemInformation iteminfo;
    [SerializeField] ITEM_ID id;
    [SerializeField] Sprite sprite;

    ItemInventory Inventory;

    //Start is called before the first frame update
    void Start()
    {
        switch (id)
        {
            //�R���X�g���N�^(��ށAID�A�擾�\���A�X�^�b�N�\���A�摜�A�񕜐�)
            case ITEM_ID.FOOD_1:
            case ITEM_ID.FOOD_2:
            case ITEM_ID.FOOD_3:
            case ITEM_ID.FOOD_4:
                iteminfo = new ItemInformation(ITEM_TYPE.FOOD, id, 1, 1, sprite, 10);
                break;
            case ITEM_ID.DRINK_1:
            case ITEM_ID.DRINK_2:
                iteminfo = new ItemInformation(ITEM_TYPE.FOOD, id, 1, 1, sprite, 5);
                break;
            case ITEM_ID.EMERGENCY_PACK:
                iteminfo = new ItemInformation(ITEM_TYPE.RECOVERY, id, 1, 1, sprite, 10);
                break;
            //�R���X�g���N�^(��ށAID�A�擾�\���A�X�^�b�N�\���A�摜)
            case ITEM_ID.BULLET:
                iteminfo = new ItemInformation(ITEM_TYPE.WEAPON, id, 10, 30, sprite);
                break;
            case ITEM_ID.PISTOL:
            case ITEM_ID.ASSAULT:
            case ITEM_ID.SHOTGUN:
            case ITEM_ID.HAND:
            case ITEM_ID.KNIFE:
            case ITEM_ID.DOG_DIRECTION:
                iteminfo = new ItemInformation(ITEM_TYPE.WEAPON, id, 1, 1, sprite);
                break;
        }

        Inventory = new ItemInventory();
    }

    //Update is called once per frame
    void Update()
    {
        //Inventory.iteminfo[0].DebugLog();
        //iteminfo.DebugLog();
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ITEM_TYPE
{
    NON,        //èâä˙íl
    FOOD,
    RECOVERY,
    WEAPON,
}
public class ItemInformation/* : MonoBehaviour*/
{
    public enum ITEM_TYPE
    {
        NON,        //èâä˙íl
        FOOD,
        RECOVERY,
        WEAPON,
    }

    public enum ITEM_ID
    {
        NON,        //èâä˙íl
        FOOD_1,
        FOOD_2,
        FOOD_3,
        FOOD_4,
        DRINK_1,
        DRINK_2,
        EMERGENCY_PACK,
        BULLET,
        PISTOL,
        ASSAULT,
        SHOTGUN,
        HAND,
        KNIFE,
        DOG_DIRECTION,
    }

    public ITEM_TYPE type   = ITEM_TYPE.NON;
    public ITEM_ID id       = ITEM_ID.NON;
    public int get_num      = 0;
    public int stack_max    = 0;
    public Sprite sprite    = null;

    public RecoveryItemInformation recoveryitem_info;

    private void Start()
    {
        switch(type)
        {
            case ITEM_TYPE.FOOD:
            case ITEM_TYPE.RECOVERY:
                recoveryitem_info = new RecoveryItemInformation();
            break;
        }
    }
}

public class RecoveryItemInformation
{
    public int recovery_num;
}
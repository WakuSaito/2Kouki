using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInformation : MonoBehaviour
{
    public enum ITEM_TYPE
    {
        FOOD,
        RECOVERY,
        WEAPON,
    }

    public enum ITEM_ID
    {
        FOOD_1,
        FOOD_2,
        FOOD_3,
        FOOD_4,
        DRINK_1,
        DRINK_2,
        EMERGENCY_PACK,
        PISTOL,
        BULLET,
    }

    public struct ItemInfo
    {
        public ITEM_TYPE type;
        public ITEM_ID id;
        public int get_num;
        public int stack_max;
    }
}

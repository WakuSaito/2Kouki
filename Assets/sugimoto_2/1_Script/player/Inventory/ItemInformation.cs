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
        BULLET,
        PISTOL,
        ASSAULT,
        SHOTGUN,
        HAND,
        KNIFE,
        DOG_DIRECTION,
    }

    public ITEM_TYPE type;
    public ITEM_ID id;
    public int get_num;
    public int stack_max;

    //コンストラクタ
    public ItemInformation(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max)
    {
        //自分のクラスのメンバーに引数の値を代入する
        type = _type;
        id = _id;
        get_num = _get_num;
        stack_max = _stack_max;
    }
}

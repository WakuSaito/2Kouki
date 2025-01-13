using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM_TYPE
{
    NON,
    FOOD,
    RECOVERY,
    WEAPON,
}

public enum ITEM_ID
{
    NON,
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
    CHEST,
}

[System.Serializable]
public class ItemInformation
{
    public ITEM_TYPE type;
    public ITEM_ID id;
    public int get_num;
    public int stack_max;
    public Sprite sprite;

    public RecoveryItemInformation recoveryitem_info;
    public WeaponItemInformation weaponitem_info;

    public ItemInformation() { }

    public ItemInformation(ItemInformation _item)
    {
        type = _item.type;
        id = _item.id;
        get_num = _item.get_num;
        stack_max = _item.stack_max;
        sprite = _item.sprite;

        switch(type)
        {
            case ITEM_TYPE.FOOD:
            case ITEM_TYPE.RECOVERY:
                recoveryitem_info = new RecoveryItemInformation(_item.recoveryitem_info.recovery_num);
                break;
            case ITEM_TYPE.WEAPON:
                weaponitem_info = new WeaponItemInformation(_item.weaponitem_info.weapon_obj, _item.weaponitem_info.bullet_sprite);
                break;
            default:
                break;
        }
    }

    public ItemInformation(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max, Sprite _sprite)
    {
        type = _type;
        id = _id;
        get_num = _get_num;
        stack_max = _stack_max;
        sprite = _sprite;
    }

    public ItemInformation(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max, Sprite _sprite, int _num)
    {
        type = _type;
        id = _id;
        get_num = _get_num;
        stack_max = _stack_max;
        sprite = _sprite;

        recoveryitem_info = new RecoveryItemInformation(_num);
    }

    public ItemInformation(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max, Sprite _sprite, GameObject _weapon_obj,Sprite _bullet_sprite)
    {
        type = _type;
        id = _id;
        get_num = _get_num;
        stack_max = _stack_max;
        sprite = _sprite;

        weaponitem_info = new WeaponItemInformation(_weapon_obj, _bullet_sprite);
    }

    public int AddGetItem(int _get_num,int _stack_max)
    {
        int add_num = 0;//ë´ÇµÇΩêî

        while (add_num != _stack_max)
        {
            _get_num--;
            add_num++;

            if (_get_num == 0) return 0;
        }

        //écÇ¡ÇΩêîÇï‘Ç∑
        return get_num = _get_num;
    }

    public void BulletInfo()
    {
        type = ITEM_TYPE.WEAPON;
        id = ITEM_ID.BULLET;
        get_num = 10;
        stack_max = 30;
        sprite = weaponitem_info.bullet_sprite;
        weaponitem_info = null;
    }

    public void DebugLog()
    {
        Debug.Log("type : " + type);
        Debug.Log("id : "+id);
        Debug.Log("get_num : " + get_num);
        Debug.Log("stack_max : " + stack_max);
        Debug.Log("sprite : " + sprite);
        switch (type)
        {
            case ITEM_TYPE.FOOD:
            case ITEM_TYPE.RECOVERY:
                Debug.Log(recoveryitem_info.recovery_num);
                break;
            case ITEM_TYPE.WEAPON:
                Debug.Log(weaponitem_info.weapon_obj);
                break;
        }

    }
}

[System.Serializable]
public class RecoveryItemInformation
{
    public int recovery_num = -1;
    public RecoveryItemInformation(int _recovery_num)
    {
        recovery_num = _recovery_num;
    }
}

[System.Serializable]
public class WeaponItemInformation
{
    public GameObject weapon_obj;
    public Sprite bullet_sprite;

    public WeaponItemInformation(GameObject _weapon_obj,Sprite _bullet_sprite)
    {
        weapon_obj = _weapon_obj;
        bullet_sprite = _bullet_sprite;
    }
}

public class ItemPickup : MonoBehaviour 
{ 
    public ItemInformation ItemInfo; 
}
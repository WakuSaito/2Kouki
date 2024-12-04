using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM_TYPE
{
    NON,        //�����l
    FOOD,
    RECOVERY,
    WEAPON,
}

public enum ITEM_ID
{
    NON,        //�����l
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

public class ItemInformation
{
    public ITEM_TYPE type;
    public ITEM_ID id;
    public int get_num;
    public int stack_max;
    public Sprite sprite;

    //�񕜃A�C�e���i���H�A�̗́j
    public RecoveryItemInformation recoveryitem_info;
    //����A�C�e��
    public WeaponItemInformation weaponitem_info;


    //�R���X�g���N�^(�����Ȃ�)
    public ItemInformation()
    {
        type        = ITEM_TYPE.NON;
        id          = ITEM_ID.NON;
        get_num     = 0;
        stack_max   = 0;
        sprite      = null;
    }

    //�R���X�g���N�^(��ށAID�A�擾�\���A�X�^�b�N�\���A�摜)
    public ItemInformation(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max, Sprite _sprite)
    {
        type        = _type;
        id          = _id;
        get_num     = _get_num;
        stack_max   = _stack_max;
        sprite      = _sprite;
    }

    //�R���X�g���N�^(��ށAID�A�擾�\���A�X�^�b�N�\���A�摜�A��ޕʈ����i�񕜐�or�e���j)
    public ItemInformation(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max, Sprite _sprite, int _num)
    {
        type        = _type;
        id          = _id;
        get_num     = _get_num;
        stack_max   = _stack_max;
        sprite      = _sprite;

        switch(type)
        {
            case ITEM_TYPE.FOOD:
            case ITEM_TYPE.RECOVERY:
                recoveryitem_info = new RecoveryItemInformation(_num);
                break;
            case ITEM_TYPE.WEAPON:
                weaponitem_info = new WeaponItemInformation(_num);
                break;
        }
    }
}

public class RecoveryItemInformation
{
    public int recovery_num = -1;
    public RecoveryItemInformation(int _recovery_num)
    {
        recovery_num = _recovery_num;
    }
}

public class WeaponItemInformation
{
    public int bullet_num = -1;

    public WeaponItemInformation(int _bullet_num)
    {
        bullet_num = _bullet_num;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryInformation
{
    public int stack_num = 0;

    //public ItemInventoryInformation(int _stack_num)
    //{
    //    stack_num = _stack_num;
    //}
}

public class ItemInventory : MonoBehaviour
{
    const int SLOAT_MAX = 10;

    //�A�C�e������ۑ�
    ItemInformation[] iteminfo = new ItemInformation[SLOAT_MAX];
    //�A�C�e���̏��������擾
    ItemInventoryInformation[] iteminventoryinfo = new ItemInventoryInformation[SLOAT_MAX];

    //private void Start()
    //{
    //    for (int sloat = 0; sloat < SLOAT_MAX; sloat++) 
    //    {
    //        iteminventoryinfo[sloat] = new ItemInventoryInformation();
    //        iteminfo[sloat] = new ItemInformation();
    //    }
    //}

    public bool ItemGet(ItemInformation _item)
    {
        return ItemCheck(_item);
    }

    bool ItemCheck(ItemInformation _item)
    {
        if (_item.type == ITEM_TYPE.WEAPON) 
        {
            switch(_item.id)
            {
                case ITEM_ID.PISTOL:
                    //�������Ă��Ȃ��ꍇ
                    /*����C���x���g���ɓ����*/

                    //�������Ă���ꍇ
                    _item.id = ITEM_ID.BULLET;
                    break;
            }
        }

        return InSloat(_item,SloatCheck(_item));
    }

    int SloatCheck(ItemInformation _item)
    {
        for (int sloat = 0; sloat < SLOAT_MAX; sloat++)
        {
            if (iteminfo[sloat] == null)
            {
                iteminventoryinfo[sloat] = new ItemInventoryInformation();
                return sloat;
            }

            if (iteminfo[sloat].id == _item.id)
            {
                if (_item.stack_max != iteminventoryinfo[sloat].stack_num)
                {
                    return sloat;
                }
            }
        }

        return -1;
    }

    bool InSloat(ItemInformation _item ,int _sloat)
    {
        Debug.Log(_sloat);
        //�X���b�g�ɓ���Ȃ�
        if (_sloat == -1) return false;

        //�X�^�b�N����܂��͎擾�ł��镨���Ȃ��Ȃ�܂�
        while (iteminventoryinfo[_sloat].stack_num != _item.stack_max)
        {
            //�擾�ł�����̂��Ȃ��Ȃ�ΏI��
            if (_item.get_num == 0) return true;

            iteminventoryinfo[_sloat].stack_num++;
            _item.get_num--;
        }

        //�擾�ł�����̂��c���Ă���ꍇ�󂢂Ă���X���b�g�𒲂ׂ�
        if (_item.get_num != 0)
        {
            InSloat(_item, SloatCheck(_item));
        }
        else
        {
            return true;
        }

        return false;
    }


    //bool ItemGet(ItemInformation _item ,int _sloat_num)
    //{
    //    //�X���b�g�ɓ����Ȃ���΂��̂܂ܕԂ�
    //    if (_sloat_num == -1) return false;

    //    //�A�C�e����񂪂Ȃ���Γ����
    //    if (iteminfo[_sloat_num] == null) 
    //    {
    //        iteminfo[_sloat_num] = _item;
    //        iteminventoryinfo[_sloat_num] = new ItemInventoryInformation();
    //    }

    //    //�X�^�b�N����܂��͎擾�ł��镨���Ȃ��Ȃ�܂�
    //    while (iteminventoryinfo[_sloat_num].stack_num != _item.stack_max)
    //    {
    //        //�擾�ł�����̂��Ȃ��Ȃ�ΏI��
    //        if (_item.get_num == 0) break;

    //        iteminventoryinfo[_sloat_num].stack_num++;
    //        _item.get_num--;
    //    }

    //    //�X���b�g�ɃA�C�e����������
    //    if (iteminfo[_sloat_num].type == ITEM_TYPE.NON)
    //    {
    //        Debug.Log("aa");

    //        iteminfo[_sloat_num] = _item;

    //        //�擾�ł�����̂��c���Ă���ꍇ�󂢂Ă���X���b�g�𒲂ׂ�
    //        if (_item.get_num != 0)
    //        {
    //            ItemGet(_item, SloatCheck(_item));
    //        }
    //        else
    //        {
    //            Debug.Log("aa");
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public bool ItemCheck(ItemInformation _item)
    //{

    //    Debug.Log(_item);
    //    Debug.Log(_item.type);
    //    Debug.Log(_item.id);
    //    Debug.Log(_item.get_num);
    //    Debug.Log(_item.stack_max);

    //    //�A�C�e���𒲂ׂ�
    //    if (_item.type == ITEM_TYPE.WEAPON) 
    //    {
    //        switch(_item.id)
    //        {
    //            case ITEM_ID.PISTOL:
    //                //���łɏ������Ă���

    //                //�������Ă��Ȃ�
    //                _item.id = ITEM_ID.BULLET;
    //                return ItemGet(_item, SloatCheck(_item));
    //            default:
    //                return ItemGet(_item, SloatCheck(_item));
    //        }
    //    }
    //    else
    //    {
    //        return ItemGet(_item, SloatCheck(_item));
    //    }
    //}

    //int SloatCheck(ItemInformation _item)
    //{
    //    //�A�C�e��������ꏊ�𒲂ׂ�
    //    for (int sloat = 0; sloat < SLOAT_MAX; sloat++) 
    //    {
    //        //�A�C�e������
    //        if (iteminfo[sloat] == null)                                    
    //        {
    //            return sloat;
    //        }

    //        //�A�C�e��ID������ 
    //        if (_item.id == iteminfo[sloat].id)                                           
    //        {
    //            //�X�^�b�N����ł͂Ȃ��ꍇ�̂�
    //            if (_item.stack_max != iteminventoryinfo[sloat].stack_num)
    //            {
    //                return sloat;
    //            }          
    //        }
    //    }

    //    Debug.Log("aaaa");
    //    return -1;  //����
    //}

    // Update is called once per frame
    void Update()
    {

    }
}

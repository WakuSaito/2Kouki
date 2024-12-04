using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryInformation
{
    public int stack_num = 0;
}

public class ItemInventory : MonoBehaviour
{
    const int SLOAT_MAX = 10;

    //�A�C�e������ۑ�
    ItemInformation[] iteminfo = new ItemInformation[SLOAT_MAX];
    //�A�C�e���̏��������擾
    ItemInventoryInformation[] iteminventoryinfo = new ItemInventoryInformation[SLOAT_MAX];

    ItemInformation ItemGet(ItemInformation _item ,int _sloat_num)
    {
        //�X���b�g�ɓ����Ȃ���΂��̂܂ܕԂ�
        if (_sloat_num == -1) return _item;

        //�A�C�e����񂪂Ȃ���Γ����
        if (iteminfo[_sloat_num] == null) 
        {
            iteminfo[_sloat_num] = _item;
            iteminventoryinfo[_sloat_num] = new ItemInventoryInformation();
        }

        //�X�^�b�N����܂��͎擾�ł��镨���Ȃ��Ȃ�܂�
        while (iteminventoryinfo[_sloat_num].stack_num != _item.stack_max)
        {
            //�擾�ł�����̂��Ȃ��Ȃ�ΏI��
            if (_item.get_num == 0) break;

            iteminventoryinfo[_sloat_num].stack_num++;
            _item.get_num--;
        }

        //�X���b�g�ɃA�C�e����������
        if (iteminfo[_sloat_num].type == ItemInformation.ITEM_TYPE.NON)
        {
            iteminfo[_sloat_num] = _item;

            //�擾�ł�����̂��c���Ă���ꍇ�󂢂Ă���X���b�g�𒲂ׂ�
            if (iteminfo[_sloat_num].get_num != 0)
            {
                ItemGet(_item, SloatCheck(_item));
            }
        }

        //�X�V�������̂�Ԃ�
        return iteminfo[_sloat_num];
    }

    public ItemInformation ItemCheck(ItemInformation _item)
    {

        Debug.Log(_item);
        Debug.Log(_item.type);
        Debug.Log(_item.id);
        Debug.Log(_item.get_num);
        Debug.Log(_item.stack_max);

        //�A�C�e���𒲂ׂ�
        if (_item.type == ItemInformation.ITEM_TYPE.WEAPON) 
        {
            switch(_item.id)
            {
                case ItemInformation.ITEM_ID.PISTOL:
                    //���łɏ������Ă���

                    //�������Ă��Ȃ�
                    _item.id = ItemInformation.ITEM_ID.BULLET;
                    return ItemGet(_item, SloatCheck(_item));
                default:
                    return ItemGet(_item, SloatCheck(_item));
            }
        }
        else
        {
            return ItemGet(_item, SloatCheck(_item));
        }
    }

    int SloatCheck(ItemInformation _item)
    {
        //�A�C�e��������ꏊ�𒲂ׂ�
        for (int sloat = 0; sloat < SLOAT_MAX; sloat++) 
        {
            //�A�C�e������
            if (iteminfo[sloat] == null)                                    
            {
                return sloat;
            }

            //�A�C�e��ID������ 
            if (_item.id == iteminfo[sloat].id)                                           
            {
                //�X�^�b�N����ł͂Ȃ��ꍇ�̂�
                if (_item.stack_max != iteminventoryinfo[sloat].stack_num)
                {
                    return sloat;
                }          
            }
        }

        Debug.Log("aaaa");
        return -1;  //����
    }

    // Update is called once per frame
    void Update()
    {

    }
}

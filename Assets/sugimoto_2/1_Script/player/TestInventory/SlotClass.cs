using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 �A�C�e�����ۑ�
 �A�C�e��������邩���ׂ�
�A�C�e����������i�c���Ԃ��j
 */

public class SlotClass
{
    public ItemInformation ItemInfo;
    public int SlotNo;
    public Transform SlotBox;

    //�󂩒��ׂ�
    public bool CheckEmpty()
    {
        //���g���Ȃ�
        if (ItemInfo == null) return true;
        //�l���\�����[��
        if (ItemInfo.get_num == 0) return true;

        return false;
    }

    //�A�C�e�����
    public bool Can_Add_Slot(ItemInformation _item)
    {
        //���g���Ȃ�
        if (ItemInfo == null) return true;
        //ID������&&�A�C�e�����������菭�Ȃ�
        if (ItemInfo.id == _item.id && ItemInfo.get_num < ItemInfo.stack_max) return true;

        return false;
    }

    public int Add_PickUPItem(ItemInformation _item)
    {
        //�A�C�e����񂪂Ȃ���Γ����
        if (ItemInfo == null)
        {
            ItemInfo = new ItemInformation(_item);
            ItemInfo.get_num = 0;//�A�C�e��������Ő����ē���Ȃ�������
        }

        //�A�C�e���������Ă��āAID���Ⴄ�ꍇ�͓����Ȃ�
        if (ItemInfo.id != _item.id)
        {
            return ItemInfo.get_num;
        }

        //�X���b�g�̋󂫗e�ʂ𒲂ׂ�
        int stack_space = ItemInfo.stack_max - ItemInfo.get_num;
        //�ǉ��ł���A�C�e�����𒲂ׂ�
        int add_num = Mathf.Min(_item.get_num, stack_space);//�擾�\�����͂��邩�A�󂫗e�ʂ̐���������Ȃ���

        //�X���b�g�̃A�C�e�������X�V
        ItemInfo.get_num += add_num;
        _item.get_num -= add_num;

        return _item.get_num;
    }

}

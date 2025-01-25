using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 �A�C�e�����ۑ�
 �A�C�e��������邩���ׂ�
�A�C�e����������i�c���Ԃ��j
 */

//[System.Serializable]
public class SlotClass
{
    public ItemInformation ItemInfo { get; set; }
    public int SlotNo { get; set; }
    public Transform SlotBox { get; set; }

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

    /// <summary>
    /// �A�C�e�����d�˂邱�Ƃ��ł��邩
    /// �ړ���̃X���b�g�̒��g�ƒ͂�ł���X���b�g�̒��g(����)�Ɣ�ׂ�
    /// ���g����̏ꍇ�A�قȂ��ނ̃A�C�e�����m�̏ꍇ�͒��g�̓���ւ��݂̂Ȃ̂ŃX�^�b�N�����͂��Ȃ�
    /// ���g������A������ނ̃A�C�e��&&�����菭�Ȃ���΃X�^�b�N����
    /// </summary>
    /// <param name="_slot">�͂�ł���X���b�g</param>
    /// <returns></returns>
    public bool CanAddStackItem(SlotClass _slot)
    {
        //���g���Ȃ�
        if (ItemInfo == null) return false;
        //ID������&&�A�C�e�����������菭�Ȃ�
        if (ItemInfo.id == _slot.ItemInfo.id && ItemInfo.get_num < ItemInfo.stack_max) return true;

        return false;
    }

    /// <summary>
    /// �A�C�e�����X�^�b�N
    /// �͂�ł���X���b�g�̒��g���ړ���̃X���b�g�̒��g�ɁA����܂œ����
    /// �ǉ��ł���A�C�e�����𒲂ׁA�����̃X���b�g�̃A�C�e�������X�V
    /// �͂�ł���X���b�g�̒��g����ɂȂ����珉�������Ă���
    /// </summary>
    /// <param name="_slot">�͂�ł���X���b�g</param>
    public void AddStackItem(ref SlotClass _slot)
    {
        //�X���b�g�̋󂫗e�ʂ𒲂ׂ�
        int stack_space = ItemInfo.stack_max - ItemInfo.get_num;
        //�ǉ��ł���A�C�e�����𒲂ׂ�
        int add_num = Mathf.Min(_slot.ItemInfo.get_num, stack_space);//�擾�\�����͂��邩�A�󂫗e�ʂ̐���������Ȃ���

        //�X���b�g�̃A�C�e�������X�V
        ItemInfo.get_num += add_num;
        _slot.ItemInfo.get_num -= add_num;

        //���g����ɂȂ����ꍇ������
        if (_slot.CheckEmpty())
        {
            _slot.initializationSlot();
        }
    }

    public void initializationSlot()
    {
        if(CheckEmpty())
        {
            ItemInfo = null;
        }
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

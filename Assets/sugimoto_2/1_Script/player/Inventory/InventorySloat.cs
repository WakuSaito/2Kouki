using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  �ł��邱��
 �E�A�C�e�����Ǘ�
 �E�X���b�g�A�C�e���̈ʒu
 */

public class InventorySloat
{
    public ItemInformation ItemInfo { get; set; } // �X���b�g�Ɋi�[�����A�C�e��
    //public int Item_Num { get; set; }   // �X���b�g���̃A�C�e����
    public int Sloat_No { get; set; }   //�X���b�g�̔ԍ�
    public Vector3 Start_Pos { get; set; }

    //public Transform Set_Pos { get; set; }

    //public Text Text { get; set; }

    //�X���b�g���󂩒��ׂ�
    public bool IsEmpty()
    {
        return ItemInfo.get_num == 0;
    }
    
    /*�v���C���[���E�����A�C�e��*/
    //�A�C�e����ǉ��ł��邩���ׂ�
    public bool CanAdd_PickUPItem(ItemInformation _iteminfro)
    {
        //����̏ꍇ�̏�����ǉ�
        if (ItemInfo == null) return true;
        if (ItemInfo.id == _iteminfro.id && ItemInfo.get_num < ItemInfo.stack_max) return true;

        return false;
    }

    public bool CanAdd_SloatItem(InventorySloat _sloat)
    {
        if (ItemInfo == null) return false;
        if (Sloat_No == _sloat.Sloat_No) return false;
        if (ItemInfo.id != _sloat.ItemInfo.id) return false;
        if (ItemInfo.get_num == ItemInfo.stack_max) return false;

        return true;
    }

    //�A�C�e����ǉ��A�X���b�g�ɓ���Ȃ������A�C�e������Ԃ�
    //public int Add_PickUPItem(ItemInformation _iteminfo)
    //{
    //    //�A�C�e����񂪂Ȃ���Γ����
    //    if(ItemInfo == null)
    //    {
    //        ItemInfo = _iteminfo;
    //        Item_Num = 0;           //�A�C�e��������Ő����ē���Ȃ�������
    //    }

        

    //    //�A�C�e���������Ă��āAID���Ⴄ�ꍇ�͓����Ȃ�
    //    if (ItemInfo.id != _iteminfo.id)
    //    {
    //        return ItemInfo.get_num;
    //    }


    //    //�X���b�g�̋󂫗e�ʂ𒲂ׂ�
    //    int stack_space = ItemInfo.stack_max - Item_Num;
    //    //�ǉ��ł���A�C�e�����𒲂ׂ�
    //    int add_num = Mathf.Min(_iteminfo.get_num, stack_space);//�擾�\�����͂��邩�A�󂫗e�ʂ̐���������Ȃ���

    //    //�X���b�g�̃A�C�e�������X�V
    //    Item_Num += add_num;
    //    _iteminfo.get_num -= add_num;

    //    return _iteminfo.get_num;
    //}

    public int Add_PickUPItem(ItemInformation _iteminfo)
    {
        //�A�C�e����񂪂Ȃ���Γ����
        if(ItemInfo == null)
        {
            ItemInfo = new ItemInformation(_iteminfo.type, _iteminfo.id, _iteminfo.get_num, _iteminfo.stack_max, _iteminfo.sprite);
            ItemInfo.get_num = 0;           //�A�C�e��������Ő����ē���Ȃ�������
        }

        

        //�A�C�e���������Ă��āAID���Ⴄ�ꍇ�͓����Ȃ�
        if (ItemInfo.id != _iteminfo.id)
        {
            return ItemInfo.get_num;
        }


        //�X���b�g�̋󂫗e�ʂ𒲂ׂ�
        int stack_space = ItemInfo.stack_max - ItemInfo.get_num;
        //�ǉ��ł���A�C�e�����𒲂ׂ�
        int add_num = Mathf.Min(_iteminfo.get_num, stack_space);//�擾�\�����͂��邩�A�󂫗e�ʂ̐���������Ȃ���

        //�X���b�g�̃A�C�e�������X�V
        ItemInfo.get_num += add_num;
        _iteminfo.get_num -= add_num;

        return _iteminfo.get_num;
    }


    /*�X���b�g�A�C�e���𑫂����킹��*/

    public int Add_SloatItem(InventorySloat _sloat)
    {
        //�X���b�g�̋󂫗e�ʂ𒲂ׂ�
        int stack_space = ItemInfo.stack_max - ItemInfo.get_num;
        //�ǉ��ł���A�C�e�����𒲂ׂ�
        int add_num = Mathf.Min(_sloat.ItemInfo.get_num, stack_space);//�擾�\�����͂��邩�A�󂫗e�ʂ̐���������Ȃ���

        //�X���b�g�̃A�C�e�������X�V
        ItemInfo.get_num += add_num;
        _sloat.ItemInfo.get_num -= add_num;

        return _sloat.ItemInfo.get_num;
    }

    /*�ݒ�֌W*/

    //public void SetSloatItemInfo()
    //{
    //    if (ItemInfo != null)
    //    {
    //        //�l���\���͏������Ă��鐔
    //        Debug.Log(ItemInfo.get_num);
    //        Debug.Log(ItemInfo.get_num);
    //        ItemInfo.get_num = ItemInfo.get_num;
    //    }
    //}

    public void UseItem()
    {
        ItemInfo.get_num--;
        //SetSloatItemInfo();

        //��ɂȂ����珉����
        if (IsEmpty())
        {
            ItemInfo = null;
        }
    }

    public void CrearSloat()
    {
        ItemInfo = null;
    }
}

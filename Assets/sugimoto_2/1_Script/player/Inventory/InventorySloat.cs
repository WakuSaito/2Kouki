using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySloat
{
    public ItemInformation ItemInfo { get; set; } // �X���b�g�Ɋi�[�����A�C�e��
    public int Item_Num { get; set; } // �X���b�g���̃A�C�e����

    //�X���b�g���󂩒��ׂ�
    public bool CheckSloatEmpty()
    {
        return Item_Num == 0;
    }

    //�A�C�e����ǉ��ł��邩���ׂ�
    public bool CanAddItem(ItemInformation _iteminfro)
    {
        //����̏ꍇ�̏�����ǉ�


        return ItemInfo == null || (ItemInfo.id == _iteminfro.id && Item_Num < ItemInfo.stack_max);
    }

    //�A�C�e����ǉ��A�X���b�g�ɓ���Ȃ������A�C�e������Ԃ�
    public int AddItem(ItemInformation _iteminfo)
    {

        if(ItemInfo == null)
        {
            ItemInfo = _iteminfo;
            Debug.Log(ItemInfo);
            Item_Num = 0;           //�A�C�e��������Ő����ē���Ȃ�������
        }

        if(ItemInfo.id != _iteminfo.id)
        {
            return ItemInfo.get_num;
        }

        //�X���b�g�̋󂫗e�ʂ𒲂ׂ�
        int stack_space = ItemInfo.stack_max - Item_Num;
        //�ǉ��ł���A�C�e�����𒲂ׂ�
        int add_num = Mathf.Min(_iteminfo.get_num, stack_space);//�擾�\���͂��邩�A�󂫗e�ʂ̐���������Ȃ���
        //�X���b�g�̃A�C�e�������X�V
        Item_Num += add_num;

        return ItemInfo.get_num - add_num;
    }

}

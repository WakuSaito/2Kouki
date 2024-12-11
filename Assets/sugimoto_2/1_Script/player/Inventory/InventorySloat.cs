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
    public int Item_Num { get; set; }   // �X���b�g���̃A�C�e����
    public int Sloat_No { get; set; }   //�X���b�g�̔ԍ�
    public Vector3 Start_Pos { get; set; }

    public Transform Set_Pos { get; set; }

    public Text Text { get; set; }

    //�X���b�g���󂩒��ׂ�
    public bool IsEmpty()
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
        //�A�C�e����񂪂Ȃ���Γ����
        if(ItemInfo == null)
        {
            ItemInfo = _iteminfo;
            Item_Num = 0;           //�A�C�e��������Ő����ē���Ȃ�������
        }

        //�A�C�e���������Ă��āAID���Ⴄ�ꍇ�͓����Ȃ�
        if(ItemInfo.id != _iteminfo.id)
        {
            return ItemInfo.get_num;
        }


        //�X���b�g�̋󂫗e�ʂ𒲂ׂ�
        int stack_space = ItemInfo.stack_max - Item_Num;
        //�ǉ��ł���A�C�e�����𒲂ׂ�
        int add_num = Mathf.Min(_iteminfo.get_num, stack_space);//�擾�\�����͂��邩�A�󂫗e�ʂ̐���������Ȃ���

        //�X���b�g�̃A�C�e�������X�V
        Item_Num += add_num;
        _iteminfo.get_num -= add_num;

        return _iteminfo.get_num;
    }

    public void SetSloatUI()
    {
        //UI�\��
        Set_Pos.gameObject.SetActive(true);
        Set_Pos.GetComponent<Image>().sprite = ItemInfo.sprite;
        Text.GetComponent<Text>().text = Item_Num + "";
    }

    public void DeleteSloatUI()
    {       
        //UI�\��
        Set_Pos.GetComponent<Image>().sprite = null;
        Text.GetComponent<Text>().text = Item_Num + "";
        Set_Pos.gameObject.SetActive(false);
    }

    public void SetSloatItemInfo()
    {
        //�l���\���͏������Ă��鐔
        ItemInfo.get_num = Item_Num;
    }

    public void UseItem()
    {
        Item_Num--;
        SetSloatItemInfo();

        //��ɂȂ����珉����
        if (IsEmpty())
        {
            ItemInfo = null;
            DeleteSloatUI();
        }
    }

    public void CrearSloat()
    {
        ItemInfo = null;
        DeleteSloatUI();
    }
}

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

    //�R���X�g���N�^
    public InventorySloat() { }
    //����C���x���g���p
    public InventorySloat(ITEM_TYPE _type, ITEM_ID _id, int _get_num, int _stack_max, Sprite _sprite, GameObject _weapon_obj)
    {
        ItemInfo = new ItemInformation(_type, _id, _get_num, _stack_max, _sprite, _weapon_obj);
    }

    //�X���b�g���󂩒��ׂ�
    public bool IsEmpty()
    {
        return ItemInfo.get_num == 0;
    }
    
    /*�v���C���[���E�����A�C�e��*/
    //�A�C�e����ǉ��ł��邩���ׂ�
    public bool CanAdd_PickUPItem(ItemInformation _iteminfo)
    {
        if (ItemInfo == null) return true;
        if (ItemInfo.id == _iteminfo.id && ItemInfo.get_num < ItemInfo.stack_max) return true;

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

    public int Add_PickUPItem(ItemInformation _iteminfo)
    {
        //�A�C�e����񂪂Ȃ���Γ����
        if(ItemInfo == null)
        {
            switch(_iteminfo.type)
            {
                case ITEM_TYPE.FOOD:
                case ITEM_TYPE.RECOVERY:
                    ItemInfo = new ItemInformation(_iteminfo.type, _iteminfo.id, _iteminfo.get_num, _iteminfo.stack_max, _iteminfo.sprite, _iteminfo.recoveryitem_info.recovery_num);
                    break;
                case ITEM_TYPE.WEAPON:
                    if (_iteminfo.id >= ITEM_ID.PISTOL && _iteminfo.id <= ITEM_ID.SHOTGUN)
                    {
                        ItemInfo = new ItemInformation(_iteminfo.type, _iteminfo.id, _iteminfo.get_num, _iteminfo.stack_max, _iteminfo.sprite, _iteminfo.weaponitem_info.weapon_obj);
                    }
                    else
                    {
                        ItemInfo = new ItemInformation(_iteminfo.type, _iteminfo.id, _iteminfo.get_num, _iteminfo.stack_max, _iteminfo.sprite);
                    }
                    break;
                default:
                    ItemInfo = new ItemInformation(_iteminfo.type, _iteminfo.id, _iteminfo.get_num, _iteminfo.stack_max, _iteminfo.sprite);
                    break;
            }
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

    public void UseItem()
    {
        ItemInfo.get_num--;

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

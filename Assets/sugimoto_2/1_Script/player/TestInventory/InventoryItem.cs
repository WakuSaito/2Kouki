using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    const int GUN_SLOT = 3;

    //�C���x���g���̗v�f
    public InventoryClass Inventory;
    public int slot_size = 10;
    public Transform[] sprite;
    public Transform[] slot_box;
    public Text[] text;

    //�I�u�W�F�N�g
    [SerializeField] GameObject item_inventory_obj; //�C���x���g��
    [SerializeField] GameObject player_obj; 

    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryClass(slot_size, slot_box);
    }

    // Update is called once per frame
    void Update()
    {
        Inventory.SetUI(sprite, text);
    }

    public bool AddInventory_PickUP_Item(ItemInformation _item, ref WeaponInventory _weapon)//�A�C�e���擾
    {
        //����̏ꍇ�̏�����ǉ�
        /*
        �������Ă��镐��I�u�W�F�N�g�̒��ɁA�擾�����A�C�e���Ɠ�����ނ���Ȃ���Ύ擾
        ��������ꏊ�̓X���b�g�R
        �I�u�W�F�N�g�͏������ɁA����C���x���g���ɕۑ�����i�̂Ă邱�Ƃ����Ȃ��ȊO�͕\����\���őΉ��j
        ������ނ̏ꍇ�A�C�e������e�ۂɕύX

        �ŏ��ɕ���C���x���g���𒲂ׂ�
        ����C���x���g���ɓ����Ă����ꍇ�A�C�e���C���x���g���𒲂ׂ�
         */

        //��ނ�����̏ꍇ
        if (_item.type == ITEM_TYPE.WEAPON)
        {
            //����C���x���g���ɏe���Ȃ��ꍇ
            if (_weapon.Inventory.Sloats[GUN_SLOT].ItemInfo == null)
            {
                //�e���C���x���g���ɓ����
                _weapon.Inventory.Sloats[GUN_SLOT].ItemInfo = new ItemInformation(_item);

                //������v���C���[�̎q�ɂ��Ă���
                _weapon.WeaponGet(_item.weaponitem_info.weapon_obj);

                return false;
            }
            //�A�C�e���C���x���g���ɂ��邩���ׂ�
            else
            {
                //�C���x���g���ɓ����邩���ׂ�
                bool in_flag = true;

                //����C���x���g���Ɠ���ID�����ׂ�
                if (_weapon.Inventory.Sloats[GUN_SLOT].ItemInfo.id == _item.id)
                {
                    in_flag = false;
                }

                //�C���x���g���̒��g�Ɣ�ׂ�
                for (int sloat = 0; sloat < Inventory.Slots.Length; sloat++)
                {
                    if (Inventory.Slots[sloat].ItemInfo != null && Inventory.Slots[sloat].ItemInfo.id == _item.id)
                    {
                        in_flag = false;
                        break;
                    }
                }

                //�C���x���g���ɓ����Ȃ���Βe�ۂɕύX
                if (!in_flag)
                {
                    //�A�C�e������e�ۂɕύX
                    _item.BulletInfo();
                }
                else
                {
                    //������v���C���[�̎q�ɂ��Ă���
                    _weapon.WeaponGet(_item.weaponitem_info.weapon_obj);
                }
            }
        }

        //�A�C�e�����C���x���g����
        for (int sloat = 0; sloat < Inventory.Slots.Length; sloat++)
        {
            if (Inventory.Slots[sloat].Can_Add_Slot(_item))
            {
                int remaining_num = Inventory.Slots[sloat].Add_PickUPItem(_item);

                //���ׂĒǉ��ł����ꍇ
                if (remaining_num <= 0)
                {
                    return true;
                }
                else
                {
                    //�ǉ��ł��Ȃ����������X�V
                    _item.get_num = remaining_num;
                }
            }
        }
        return false;
    }

}

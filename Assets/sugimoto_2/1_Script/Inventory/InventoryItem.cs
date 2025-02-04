using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// �A�C�e���C���x���g��
/// �v���C���[����������A�C�e���̃C���x���g��
/// </summary>
public class InventoryItem : MonoBehaviour
{
    const int GUN_SLOT = 3;

    //�C���x���g���}�l�[�W���[
    InventoryManager m_inventoryManager;

    //�C���x���g���̗v�f
    public InventoryClass m_inventory;
    public int m_slotSize = 10;
    public Transform[] m_BoxTrans;
    public Transform[] m_spriteTrans;
    public Transform[] m_noUseMarkTrans;
    public Text[] m_Text;

    //�I�u�W�F�N�g
    [SerializeField] GameObject m_inventoryManagerObj;
    public GameObject m_uiObj; //�C���x���g��UI

    [SerializeField]//�T�E���h�p
    private PlayerSound m_playerSound;

    /// <summary>
    /// �X�^�[�g�֐�
    /// �C���x���g���N���X�쐬
    /// �C���x���g���}�l�[�W���[�擾
    /// </summary>
    void Start()
    {
        //�C���x���g���̃C���X�g���N�^�쐬
        m_inventory = new InventoryClass(m_slotSize, m_BoxTrans);
        //�C���x���g���}�l�[�W���[�擾
        m_inventoryManager = m_inventoryManagerObj.GetComponent<InventoryManager>();
    }

    public bool AddInventory_PickUP_Item(ItemInformation _item, ref InventoryWeapon _weapon)//�A�C�e���擾
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
            //�A�C�e����񂩂�X���b�g�̈ʒu���擾
            int slot_num = _weapon.CanWeaponGet(_item.weaponitem_info.weapon_obj);

            if (_weapon.m_weaponSlotObj[slot_num] == null)
            {
                //����C���x���g���ɏe���Ȃ��ꍇ
                
                _weapon.WeaponGet(_item.weaponitem_info.weapon_obj);//����擾
                return false;
            }
            else
            {
                //�C���x���g���ɓ����邩���ׂ�
                bool in_flag = true;

                //����C���x���g���Ɠ���ID�����ׂ�
                if (_weapon.m_Inventory.Slots[slot_num].ItemInfo.id == _item.id)
                {
                    in_flag = false;
                }

                //�A�C�e���C���x���g���̒��g�Ɣ�ׂ�
                for (int sloat = 0; sloat < m_inventory.Slots.Length; sloat++)
                {
                    if (m_inventory.Slots[sloat].ItemInfo != null && m_inventory.Slots[sloat].ItemInfo.id == _item.id)
                    {
                        in_flag = false;
                        break;
                    }
                }

                //�e�ۂɂ��ē�����邩���ׂ�

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
        for (int sloat = 0; sloat < m_inventory.Slots.Length; sloat++)
        {
            if (m_inventory.Slots[sloat].Can_Add_Slot(_item))
            {
                int remaining_num = m_inventory.Slots[sloat].Add_PickUPItem(_item);

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

    /// <summary>
    /// �Q�[�W��
    /// �H���Q�[�W�A�̗̓Q�[�W���񕜂�����
    /// </summary>
    /// <param name="_food_gage_obj">�H���Q�[�W��script���������I�u�W�F�N�g</param>
    /// <param name="_hp_gage_obj">�̗̓Q�[�W��script���������I�u�W�F�N�g</param>
    public void Recovery_Gage(GameObject _food_gage_obj,GameObject _hp_gage_obj,bool _area_flag)
    {
        bool recovery_flag = false;

        //�J�[�\���̂����Ă���I�u�W�F�N�g���擾
        foreach (RaycastResult result in m_inventoryManager.HitResult())
        {
            //�J�[�\���̓������Ă���X���b�g���擾
            for (int slot = 0; slot < m_slotSize; slot++)
            {
                if (result.gameObject == m_spriteTrans[slot].gameObject)
                {
                    //ID���擾
                    ITEM_ID id = m_inventory.Slots[slot].ItemInfo.id;

                    if (id >= ITEM_ID.FOOD_1 && id <= ITEM_ID.EMERGENCY_PACK)
                    {
                        //�񕜂���l���擾
                        int recovery_num = m_inventory.Slots[slot].ItemInfo.recoveryitem_info.recovery_num;

                        //�H��
                        {
                            if (_area_flag)
                            {
                                if (id >= ITEM_ID.FOOD_1 && id <= ITEM_ID.DRINK_2)
                                {
                                    _food_gage_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);//�Q�[�W�𑝂₷
                                    recovery_flag = true;
                                    m_playerSound.PlayEat();//SE
                                }
                            }
                            else
                            {
                                if (id >= ITEM_ID.DRINK_1 && id <= ITEM_ID.DRINK_2)
                                {
                                    _food_gage_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);//�Q�[�W�𑝂₷
                                    recovery_flag = true;
                                    m_playerSound.PlayEat();//SE
                                }
                            }
                            
                        }

                        //�̗�
                        {
                            if (id >= ITEM_ID.EMERGENCY_PACK)
                            {
                                _hp_gage_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);//�Q�[�W�𑝂₷
                                recovery_flag = true;
                                m_playerSound.PlayHeal();//SE
                            }
                        }
                        //�A�C�e������
                        if (recovery_flag) UseItem(slot);
                    }
                    return;
                }
            }
        }
    }

    /// <summary>
    /// �A�C�e������
    /// �X���b�g�̏ꏊ���w�肵�A���̃X���b�g�̃A�C�e�����P���炷
    /// �A�C�e�������ׂĂȂ��Ȃ����炻�̃X���b�g��������
    /// </summary>
    /// <param name="_slot">�X���b�g�̏ꏊ</param>
    void UseItem(int _slot)
    {
        //�X���b�g�̃A�C�e���̐������炷
        m_inventory.Slots[_slot].ItemInfo.get_num--;
        
        //��ɂȂ��������ׂ�
        if (m_inventory.Slots[_slot].CheckEmpty())
        {
            m_inventory.Slots[_slot].initializationSlot();//������
        }
    }

    /// <summary>
    /// �e�ۂ������
    /// �e�̃����[�h�̍ۂɎg�p
    /// �e�ۂ������Ă���X���b�g�𒲂ׁA��������������
    /// </summary>
    /// <param name="_amount">�e�ɓ���e��</param>
    /// <returns>�e�ɓ��ꂽ�e��</returns>
    public int UseBullet(int _amount)
    {
        //�e�ɓ���e�����擾
        int addAmount = _amount;
        //���₵���e����ۑ�
        int add_num = 0;

        //�C���x���g���̒��g�𒲂ׂ�
        for (int slot = 0; slot < m_slotSize; slot++)
        {
            //�X���b�g�̒��g����Ȃ玟�̃X���b�g��
            if (m_inventory.Slots[slot].CheckEmpty()) continue;

            //���g���e��
            if (m_inventory.Slots[slot].ItemInfo.id == ITEM_ID.BULLET)
            {
                //�e�ۂ̐����擾
                int bullet_num = m_inventory.Slots[slot].ItemInfo.get_num;

                //�e�X���b�g�ɂ���e�ɓ���e�������X���b�g�̒e���̂ق�������
                //�e�ɓ���e�����ꂽ�瑦�I��
                if (bullet_num > addAmount)
                {
                    //�e�X���b�g�ɂ���e�ɓ���e���������X���b�g�̒e�ۂ����炷
                    m_inventory.Slots[slot].ItemInfo.get_num -= addAmount;
                    return addAmount + add_num;//���₵���e���i+add_num����Ȃ������j
                }
                else
                {
                    //�X���b�g�̒e���̂ق������Ȃ�
                    //�X���b�g�̒��g���ׂďe�ɓ��ꂽ�玟�̃X���b�g�𒲂ׂ�

                    //�e�X���b�g�ɂ���e�ɓ���e�����X���b�g�̒e���̌��炷
                    addAmount -= m_inventory.Slots[slot].ItemInfo.get_num;
                    //���₵�����͒e�����X���b�g�̒e��
                    add_num += m_inventory.Slots[slot].ItemInfo.get_num;
                    //�X���b�g�̒��g���O�ɂ���
                    m_inventory.Slots[slot].ItemInfo.get_num = 0;

                    //��ɂȂ�����
                    if (m_inventory.Slots[slot].CheckEmpty())
                    {
                        m_inventory.Slots[slot].initializationSlot();//������
                    }
                }
            }

        }

        return _amount - addAmount;//���₵���e��
    }



    /// <summary>
    /// �C���x���g���ɒe�ۂ����邩���ׂ�
    /// �����[�h�ł��邩�ǂ����Ɏg�p
    /// </summary>
    /// <returns>����true�A�Ȃ�false</returns>
    public bool CheckBullet()
    {
        for (int slot = 0; slot < m_slotSize; slot++)
        {
            if (m_inventory.Slots[slot].ItemInfo == null) continue;

            if (m_inventory.Slots[slot].ItemInfo.id == ITEM_ID.BULLET)
            {
                return true;
            }
        }
        return false;
    }

}

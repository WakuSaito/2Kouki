using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�`�F�X�g�g�p�t�F�C�Y</para>
/// �`�F�X�g�̎g������m���Ă��炤
/// �ŏ�����`�F�X�g�ɓ����Ă���A�C�e������ɓ����`�ɂ�����
/// </summary>
public class PhaseUseChest : TutorialBase
{
    //�ڕW�̍��W
    [SerializeField] private Vector3 m_targetPos;

    //�C���x���g���̏�ԊĎ��p
    [SerializeField] private InventoryManager m_inventoryManager;
    //�`�F�X�g�̃A�C�e���Ď��p
    [SerializeField] private InventoryChest m_inventoryChest;
    //�A�C�e���ړ���擾�p
    [SerializeField] private InventoryItem m_inventoryItem;

    //�A�C�e���ړ��𑣂�UI
    [SerializeField] private GameObject m_plzMoveItemUI;

    //�O�t���[���̃`�F�X�g���ۑ��p
    private int m_prev_chest_used_slot;

    public override void SetUpPhase()
    {
        //�C���x���g���̋󂫈ʒu�擾
        Vector2 inventory_slot_pos = GetInventoryEmptySlotPos();

        //�J�[�\���̊J�n�ʒu�ύX
        m_plzMoveItemUI.GetComponent<CursorAdvisorUI>().SetStartPos(m_inventoryChest.m_slotBoxTrans[0].position);
        //�J�[�\���̏I���ʒu�ύX
        m_plzMoveItemUI.GetComponent<CursorAdvisorUI>().SetEndPos(inventory_slot_pos);

        m_tutorialManager.SetText("�`�F�X�g�ŃA�C�e���𐮗����悤");
        m_tutorialManager.CreateMarker(m_targetPos);
    }

    public override void UpdatePhase()
    {
        //�`�F�X�g���J����Ă���Ȃ�
        if (m_inventoryManager.m_inventoryState == INVENTORY.CHEST)
        {
            m_plzMoveItemUI.SetActive(true);
            m_plzMoveItemUI.GetComponent<CursorAdvisorUI>().StartMove(CursorAdvisorUI.ANIM_TYPE.DRAG);//UI�𓮂���

            //�`�F�X�g����A�C�e���������Ă�����
            int used_count = GetChestUsedSlotCount();
            if (used_count < m_prev_chest_used_slot)
            {
                //���̃t�F�[�Y�ɐi�߂�
                m_tutorialManager.NextPhase();
            }
            else
            {
                m_prev_chest_used_slot = used_count;//���ۑ�
            }
        }
        else
        {
            m_plzMoveItemUI.SetActive(false);
            m_plzMoveItemUI.GetComponent<CursorAdvisorUI>().StopMove();//UI���~�߂�
        }     

    }

    /// <summary>
    /// �`�F�X�g�̎g���Ă���X���b�g���擾
    /// </summary>
    /// <returns>�g���Ă���X���b�g��</returns>
    private int GetChestUsedSlotCount()
    {
        int used_count = 0;
        //�`�F�X�g�̃X���b�g��S�Ē��ׂ�
        for (int i = 0; i < m_inventoryChest.m_inventory.Slots.Length; i++)
        {
            if (m_inventoryChest.m_inventory.Slots[i].ItemInfo == null) continue;

            //�����A�C�e��������΃J�E���g
            ITEM_ID id = m_inventoryChest.m_inventory.Slots[i].ItemInfo.id;
            if (id != ITEM_ID.NON)
            {
                used_count++;
            }
        }

        return used_count;
    }
    /// <summary>
    /// �C���x���g���̎g���Ă��Ȃ��X���b�g�̈ʒu�擾
    /// </summary>
    private Vector2 GetInventoryEmptySlotPos()
    {
        //�A�C�e���C���x���g���̃X���b�g��S�Ē��ׂ�
        for (int i = 0; i < m_inventoryItem.m_inventory.Slots.Length; i++)
        {
            if (m_inventoryItem.m_inventory.Slots[i].ItemInfo == null) continue;

            //�󂫂�����΂����Ԃ�
            ITEM_ID id = m_inventoryItem.m_inventory.Slots[i].ItemInfo.id;
            if (id == ITEM_ID.NON)
            {
                return m_inventoryItem.m_BoxTrans[i].position;
            }
        }

        //�󂫂��������0�Ԗڂ�Ԃ�
        return m_inventoryItem.m_BoxTrans[0].position;
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();
    }
}

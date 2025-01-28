using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�H���g�p�t�F�C�Y</para>
/// ����A�C�e���̎g������������
/// </summary>
//�H�����ꏊ�ɂ���Ďg�������Ȃ����\���A��
public class PhaseEatFood : TutorialBase
{
    //�C���x���g���Ǘ��X�N���v�g
    [SerializeField] private InventoryManager m_inventoryManager;
    //�v���C���[�̃C���x���g���X�N���v�g
    [SerializeField] private InventoryItem m_inventoryItem;
    //�H���Q�[�W�X�N���v�g
    [SerializeField] private Gauge m_foodGauge;

    //�O�t���[���̐H���Q�[�W�̒l
    private float m_prevFoodAmount;

    //�o�b�O���J����悤�w������UI
    [SerializeField] private GameObject m_plzOpenBagUI;
    //�A�C�e�����g���悤�w������UI
    [SerializeField] private GameObject m_plzUseItemUI;

    //�H���A�C�R���̈ʒu
    private Vector2 m_foodIconPos;

    public override void SetUpPhase()
    {
        //��\��
        m_plzOpenBagUI.SetActive(false);
        m_plzUseItemUI.SetActive(false);

        //�A�C�R���ʒu�擾
        m_foodIconPos = SerchFoodIcon();

        Debug.Log("pos:" + m_foodIconPos);

        //�J�[�\���̏I���ʒu�ύX
        m_plzUseItemUI.GetComponent<CursorAdvisorUI>().SetEndPos(m_foodIconPos);

        //�H���Q�[�W�̒l�擾
        m_prevFoodAmount = m_foodGauge.GetCurrentAmount();

        m_tutorialManager.SetText("�H����H�ׂ悤");
    }

    public override void UpdatePhase()
    {
        if (m_inventoryManager == null) return;


        //UI null�`�F�b�N
        if (m_plzOpenBagUI != null && m_plzUseItemUI != null)
        {
            //�o�b�O���J���Ă��Ȃ��Ȃ�@�J���悤�ɑ���
            if(m_inventoryManager.m_inventoryState != INVENTORY.ITEM)
            {
                m_plzOpenBagUI.SetActive(true);
                m_plzUseItemUI.SetActive(false);
                m_plzUseItemUI.GetComponent<CursorAdvisorUI>().StopMove();
            }
            //else �J���Ă���Ȃ�@�A�C�e�����g���悤����
            else
            {
                m_plzOpenBagUI.SetActive(false);
                m_plzUseItemUI.SetActive(true);
                m_plzUseItemUI.GetComponent<CursorAdvisorUI>().StartMove(CursorAdvisorUI.ANIM_TYPE.CLICK);
            }
        }
        //���݂̐H���Q�[�W�̒l�擾
        float food_amount = m_foodGauge.GetCurrentAmount();

        //�H���Q�[�W�̒l�������Ă�����A�H�ׂ��Ƃ݂Ȃ��@�̂Ă�or�����o����悤�ɂȂ�����o�O��
        if (m_prevFoodAmount < food_amount)
        {
            m_tutorialManager.NextPhase();
        }
        else
        {
            m_prevFoodAmount = food_amount;//�H���Q�[�W�̒l�L��
        }

    }

    //�H���A�C�R����T���i1�Ԏn�߂Ɍ����������̂̂݁j
    private Vector2 SerchFoodIcon()
    {
        if (m_inventoryManager == null) return Vector2.zero;     

        for (int i = 0; i < m_inventoryItem.m_inventory.Slots.Length; i++)
        {
            if (m_inventoryItem.m_inventory.Slots[i].ItemInfo == null) continue;

            ITEM_ID id = m_inventoryItem.m_inventory.Slots[i].ItemInfo.id;
            if ((int)id >= (int)ITEM_ID.FOOD_1 &&
                (int)id <= (int)ITEM_ID.DRINK_2)
            {
                return m_inventoryItem.m_BoxTrans[i].position;
            }
        }

        Debug.Log("�H���A�C�R����������܂���");
        return m_inventoryItem.m_BoxTrans[0].position;
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();

        //UI��\��
        m_plzOpenBagUI.SetActive(false);
        m_plzUseItemUI.SetActive(false);
    }
}

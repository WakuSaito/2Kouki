using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEatFood : TutorialBase
{
    //�C���x���g���X�N���v�g
    private Inventory m_inventory;

    //�O�t���[���̏������Ă���H���̐�
    private int m_prevHaveFoodSum = 0;

    [SerializeField]//�o�b�O���J����悤�w������UI
    private GameObject m_plzOpenBagUI;

    [SerializeField]//�A�C�e�����g���悤�w������UI
    private GameObject m_plzUseItemUI;

    //�H���A�C�R���̈ʒu
    private Vector2 m_foodIconPos;

    public override void SetUpPhase()
    {
        //�v���C���[����Inventory�擾
        m_inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        //��\��
        m_plzOpenBagUI.SetActive(false);
        m_plzUseItemUI.SetActive(false);

        //�A�C�R���ʒu�擾
        m_foodIconPos = SerchFoodIcon();

        Debug.Log("pos:" + m_foodIconPos);

        //�J�[�\���̏I���ʒu�ύX
        m_plzUseItemUI.GetComponent<CursorAdvisorUI>().SetEndPos(m_foodIconPos);

        m_tutorialManager.SetText("�H����H�ׂ悤");
    }

    public override void UpdatePhase()
    {
        if (m_inventory == null) return;


        //UI null�`�F�b�N
        if (m_plzOpenBagUI != null && m_plzUseItemUI != null)
        {
            //�o�b�O���J���Ă��Ȃ��Ȃ�@�J���悤�ɑ���
            if(m_inventory.item_inventory_flag == false)
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
                m_plzUseItemUI.GetComponent<CursorAdvisorUI>().StartMove();
            }
        }
        int food_sum = m_inventory.GetFoodItemSum();

        //�H���̐��������Ă���΁A�H�ׂ��Ƃ݂Ȃ��@�̂Ă�or�����o����悤�ɂȂ�����o�O��
        if (food_sum < m_prevHaveFoodSum)
        {
            m_tutorialManager.NextPhase();
        }
        else
        {
            m_prevHaveFoodSum = food_sum;//�X���b�g���L��
        }
    }

    //�H���A�C�R����T���i1�Ԏn�߂Ɍ����������̂̂݁j
    private Vector2 SerchFoodIcon()
    {
        if (m_inventory == null) return Vector2.zero;

        for (int i = 0; i < m_inventory.item_type_id.Length; i++)
        {
            int id = m_inventory.item_type_id[i];
            if (id >= (int)ID.ITEM_ID.FOOD_1 &&
                id <= (int)ID.ITEM_ID.DRINK_2)
            {
                return m_inventory.GetItemIconPos(i);
            }
        }

        Debug.Log("�H���A�C�R����������܂���");
        return Vector2.zero;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEatFood : TutorialBase
{
    //�C���x���g���X�N���v�g
    private Inventory inventory;

    //�O�t���[���̏������Ă���H���̐�
    private int prevHaveFoodSum = 0;

    [SerializeField]//�o�b�O���J����悤�w������UI
    private GameObject plzOpenBagUI;

    [SerializeField]//�A�C�e�����g���悤�w������UI
    private GameObject plzUseItemUI;

    //�H���A�C�R���̈ʒu
    private Vector2 foodIconPos;

    //debug�p
    private bool bagOpen = false;

    public override void SetUpPhase()
    {
        //�v���C���[����Inventory�擾
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        //��\��
        plzOpenBagUI.SetActive(false);
        plzUseItemUI.SetActive(false);

        //�A�C�R���ʒu�擾
        foodIconPos = SerchFoodIcon();

        Debug.Log("pos:" + foodIconPos);

        //�J�[�\���̏I���ʒu�ύX
        plzUseItemUI.GetComponent<CursorAdvisorUI>().SetEndPos(foodIconPos);

        tutorialManager.SetText("�H����H�ׂ悤");
    }

    public override void UpdatePhase()
    {
        if (inventory == null) return;


        //UI null�`�F�b�N
        if (plzOpenBagUI != null && plzUseItemUI != null)
        {
            //�o�b�O���J���Ă��Ȃ��Ȃ�@�J���悤�ɑ���
            if(inventory.item_inventory_flag == false)
            {
                plzOpenBagUI.SetActive(true);
                plzUseItemUI.SetActive(false);
                plzUseItemUI.GetComponent<CursorAdvisorUI>().StopMove();
            }
            //else �J���Ă���Ȃ�@�A�C�e�����g���悤����
            else
            {
                plzOpenBagUI.SetActive(false);
                plzUseItemUI.SetActive(true);
                plzUseItemUI.GetComponent<CursorAdvisorUI>().StartMove();
            }
        }
        int foodSum = inventory.GetFoodItemSum();

        //�H���̐��������Ă���΁A�H�ׂ��Ƃ݂Ȃ��@�̂Ă�or�����o����悤�ɂȂ�����o�O��
        if (foodSum < prevHaveFoodSum)
        {
            tutorialManager.NextPhase();
        }
        else
        {
            prevHaveFoodSum = foodSum;//�X���b�g���L��
        }
    }

    //�H���A�C�R����T���i1�Ԏn�߂Ɍ����������̂̂݁j
    private Vector2 SerchFoodIcon()
    {
        if (inventory == null) return Vector2.zero;

        for (int i = 0; i < inventory.item_type_id.Length; i++)
        {
            int id = inventory.item_type_id[i];
            if (id >= (int)ID.ITEM_ID.FOOD_1 &&
                id <= (int)ID.ITEM_ID.DRINK_2)
            {
                return inventory.GetItemIconPos(i);
            }
        }

        Debug.Log("�H���A�C�R����������܂���");
        return Vector2.zero;
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();

        //UI��\��
        plzOpenBagUI.SetActive(false);
        plzUseItemUI.SetActive(false);
    }
}

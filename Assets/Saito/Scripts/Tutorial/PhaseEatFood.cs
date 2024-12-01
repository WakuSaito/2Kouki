using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEatFood : TutorialBase
{
    //ui�p
    private Transform canvas;

    //�C���x���g���X�N���v�g
    private Inventory inventory;

    //�O�t���[���̏������Ă���H���̐�
    private int prevHaveFoodSum = 0;

    [SerializeField]//�o�b�O���J����悤�w������UI
    private GameObject plzOpenBagUIPrefab;
    private GameObject plzOpenBagUI;

    [SerializeField]//�A�C�e�����g���悤�w������UI
    private GameObject plzUseItemUIPrefab;
    private GameObject plzUseItemUI;
    
    public override void SetUpPhase()
    {
        //�v���C���[����Inventory�擾
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        //�L�����o�X�擾
        canvas = GameObject.Find("Canvas").transform;
        //UI����
        plzOpenBagUI = Instantiate(plzOpenBagUIPrefab, canvas);
        plzUseItemUI = Instantiate(plzUseItemUIPrefab, canvas);
        //��\��
        plzOpenBagUI.SetActive(false);
        plzUseItemUI.SetActive(false);

        tutorialManager.SetText("�H����H�ׂ悤");
    }

    public override void UpdatePhase()
    {
        if (inventory == null) return;

        //UI null�`�F�b�N
        if (plzOpenBagUI != null && plzUseItemUI != null)
        {
            //�o�b�O���J���Ă��Ȃ��Ȃ�@�J���悤�ɑ���
            if (inventory.item_inventory_flag == false)
            {
                plzOpenBagUI.SetActive(true);
                plzUseItemUI.SetActive(false);
            }
            //else �J���Ă���Ȃ�@�A�C�e�����g���悤����
            else
            {
                plzOpenBagUI.SetActive(false);
                plzUseItemUI.SetActive(true);
            }
        }


        //�H���̐��������Ă���΁A�H�ׂ��Ƃ݂Ȃ��@�̂Ă�or�����o����悤�ɂȂ�����o�O��
        if(inventory.GetFoodItemSum() < prevHaveFoodSum)
        {
            tutorialManager.NextPhase();
        }
        else
        {
            prevHaveFoodSum = inventory.GetFoodItemSum();//�X���b�g���L��
        }  
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();

        //UI�폜
        Destroy(plzOpenBagUI);
        Destroy(plzUseItemUI);
    }
}

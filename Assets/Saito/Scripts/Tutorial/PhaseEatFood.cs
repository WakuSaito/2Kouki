using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEatFood : TutorialBase
{

    public override void SetUpPhase()
    {
        tutorialManager.SetText("�H����H�ׂ悤");
    }

    public override void UpdatePhase()
    {
        //if �o�b�O���J���Ă��Ȃ��Ȃ�@�J���悤�ɑ���
        //Inventory.item_inventory_flag == false

        //else �J���Ă���Ȃ�@�A�C�e�����g���悤����

        //if �g������
        //�C���x���g���T�C�Y�Ď�

        tutorialManager.NextPhase();
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }
}

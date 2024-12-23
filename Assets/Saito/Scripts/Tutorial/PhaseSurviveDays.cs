using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSurviveDays : TutorialBase
{
    TimeController timeController;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("����������3���ڂ܂Ő����c�낤");
    }

    public override void UpdatePhase()
    {
        //���݂̓����擾
        int dayCount = timeController.GetDayCount();

        //3���ڂȂ�����𖞂���
        if(dayCount >= 3)
        {
            //���̃t�F�[�Y�ɐi�߂�
            tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
    }
}

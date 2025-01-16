using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSurviveDays : TutorialBase
{
    TimeController m_timeController;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("����������3���ڂ܂Ő����c�낤");
    }

    public override void UpdatePhase()
    {
        //���݂̓����擾
        int dayCount = m_timeController.GetDayCount();

        //3���ڂȂ�����𖞂���
        if(dayCount >= 3)
        {
            //���̃t�F�[�Y�ɐi�߂�
            m_tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
    }
}

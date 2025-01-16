using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSurviveDays : TutorialBase
{
    TimeController m_timeController;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("助けが来る3日目まで生き残ろう");
    }

    public override void UpdatePhase()
    {
        //現在の日数取得
        int dayCount = m_timeController.GetDayCount();

        //3日目なら条件を満たす
        if(dayCount >= 3)
        {
            //次のフェーズに進める
            m_tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
    }
}

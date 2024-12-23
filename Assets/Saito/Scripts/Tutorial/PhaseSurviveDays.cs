using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSurviveDays : TutorialBase
{
    TimeController timeController;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("助けが来る3日目まで生き残ろう");
    }

    public override void UpdatePhase()
    {
        //現在の日数取得
        int dayCount = timeController.GetDayCount();

        //3日目なら条件を満たす
        if(dayCount >= 3)
        {
            //次のフェーズに進める
            tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
    }
}

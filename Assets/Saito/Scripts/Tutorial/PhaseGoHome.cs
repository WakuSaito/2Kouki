using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseGoHome : TutorialBase
{
    [SerializeField]//自宅の座標
    private Vector3 HomePos;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("自宅へ向かえ");
        tutorialManager.CreateMarker(HomePos);
    }

    public override void UpdatePhase()
    {
        //プレイヤーと目標座標の距離が一定以下なら
        float distance = Vector3.Distance(HomePos, PlayerPos());
        if(distance < 4.0f)
        {
            //次のフェーズに進める
            tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }
    
}

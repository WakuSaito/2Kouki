using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseGoTown : TutorialBase
{
    [SerializeField]//目標の座標
    private Vector3 TargetPos;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("街へ向かえ");
        tutorialManager.CreateMarker(TargetPos);
    }

    public override void UpdatePhase()
    {
        //プレイヤーと目標座標の距離が一定以下なら
        float distance = Vector3.Distance(TargetPos, PlayerPos());
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

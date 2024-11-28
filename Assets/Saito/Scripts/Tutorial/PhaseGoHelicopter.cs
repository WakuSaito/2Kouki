using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseGoHelicopter : TutorialBase
{
    [SerializeField]//目標の座標
    private Vector3 TargetPos;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("基地屋上のヘリコプターで脱出しよう");
        tutorialManager.CreateMarker(TargetPos);
    }

    public override void UpdatePhase()
    {
        //プレイヤーと目標座標の距離が一定以下なら
        float distance = Vector3.Distance(TargetPos, PlayerPos());
        if (distance < 4.0f)
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

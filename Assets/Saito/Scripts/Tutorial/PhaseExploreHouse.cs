using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseExploreHouse : TutorialBase
{
    [SerializeField]//目標の座標
    private Vector3 TargetPos;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("犬に指示を出し\n辺りを調べよう");
        tutorialManager.CreateMarker(TargetPos);
    }

    public override void UpdatePhase()
    {
        //UI強調表示

        //if(指示を出したら)

        tutorialManager.SetText("家の中を探索しよう");
        tutorialManager.CreateMarker(TargetPos);
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }

}

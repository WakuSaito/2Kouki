using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseUseKnife : TutorialBase
{
    [SerializeField]//自宅の座標
    private GameObject targetZombieObj;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("ナイフでゾンビを倒そう");
        tutorialManager.CreateMarker(targetZombieObj.transform.position);
    }

    public override void UpdatePhase()
    {
        //ゾンビを倒したら
        //フェーズ移行
        if(targetZombieObj == null)
            tutorialManager.NextPhase();
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }
}

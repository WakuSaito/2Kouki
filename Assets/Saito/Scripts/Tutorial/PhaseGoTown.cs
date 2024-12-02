using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseGoTown : TutorialBase
{
    [SerializeField]//目標の座標
    private Vector3 TargetPos;

    [SerializeField]
    private GameObject fadeUI;

    public override void SetUpPhase()
    {

        tutorialManager.SetText("街で装備を整え\nヘリコプターで脱出しよう");
        tutorialManager.CreateMarker(TargetPos);
    }

    public override void UpdatePhase()
    {
        //プレイヤーと目標座標の距離が一定以下なら
        float distance = Vector3.Distance(TargetPos, PlayerPos());
        if(distance < 1.0f)
        {
            fadeUI.SetActive(true);
            fadeUI.GetComponent<FadeImage>().StartFade();


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

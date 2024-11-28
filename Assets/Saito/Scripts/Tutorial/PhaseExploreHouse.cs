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
    }

    public override void UpdatePhase()
    {
        //if 笛を持っていないなら　持たせるよう促す
        //if(player.hand_weapon.tag != "Whistle")

        //else 持っているなら　使うよう促す

        //if(指示を出したら)
        //DogManagerのクールダウンを監視

        tutorialManager.SetText("家の中を探索しよう");
        tutorialManager.CreateMarker(TargetPos);
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

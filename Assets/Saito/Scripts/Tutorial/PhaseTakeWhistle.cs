using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTakeWhistle : TutorialBase
{
    [SerializeField]//犬笛オブジェクト
    private GameObject whistleObj;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("犬笛を取れ");
        tutorialManager.CreateMarker(whistleObj.transform.position);
    }

    public override void UpdatePhase()
    {
        //ナイフを取得したら
        //フェーズ移行
        tutorialManager.NextPhase();
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }
}

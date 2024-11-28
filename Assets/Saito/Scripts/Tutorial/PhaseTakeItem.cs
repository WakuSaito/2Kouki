using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTakeItem : TutorialBase
{
    [SerializeField]//犬笛オブジェクト
    private GameObject itemObj;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("食料を手に入れよう");
        tutorialManager.CreateMarker(itemObj.transform.position);
    }

    public override void UpdatePhase()
    {
        //アイテムを取得したら        
        if(itemObj == null)
            tutorialManager.NextPhase();//フェーズ移行
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }
}

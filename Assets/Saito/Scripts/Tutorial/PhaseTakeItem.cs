using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTakeItem : TutorialBase
{
    [SerializeField]//食料オブジェクト
    private GameObject m_itemObj;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("食料を手に入れよう");
        m_tutorialManager.CreateMarker(m_itemObj.transform.position);
    }

    public override void UpdatePhase()
    {
        //アイテムを取得したら        
        if(m_itemObj == null)
            m_tutorialManager.NextPhase();//フェーズ移行
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseGoHelicopter : TutorialBase
{
    [SerializeField]//目標の座標
    private Vector3 m_targetPos;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("基地屋上のヘリコプターで脱出しよう");
        m_tutorialManager.CreateMarker(m_targetPos);
    }

    public override void UpdatePhase()
    {
        //プレイヤーと目標座標の距離が一定以下なら
        float distance = Vector3.Distance(m_targetPos, PlayerPos());
        if (distance < 4.0f)
        {
            //次のフェーズに進める
            m_tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();
    }
}

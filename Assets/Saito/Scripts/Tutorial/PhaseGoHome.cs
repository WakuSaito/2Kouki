using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>帰宅フェイズ</para>
/// 自宅（セーフエリア）の存在を知ってもらう
/// </summary>
public class PhaseGoHome : TutorialBase
{
    //目標の座標
    [SerializeField] private Vector3 m_targetPos;

    [SerializeField] private player m_player;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("食事をするために\n一度自宅に向かおう");
        m_tutorialManager.CreateMarker(m_targetPos);
    }

    public override void UpdatePhase()
    {
        //プレイヤーがセーフエリアに入ったら
        if (m_player.m_inSafeAreaFlag == true)
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

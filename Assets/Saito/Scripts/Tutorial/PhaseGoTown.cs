using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 使ってなさそう
/// </summary>
public class PhaseGoTown : TutorialBase
{
    [SerializeField]//目標の座標
    private Vector3 m_targetPos;

    [SerializeField]//フェードアウト用
    private GameObject m_fadeUI;

    [SerializeField]//見た目用ヘリコプターオブジェクト
    private GameObject m_helicoptorObj;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("街で装備を整え\nヘリコプターで脱出しよう");
        m_tutorialManager.CreateMarker(m_targetPos);
        //ヘリを表示
        if(m_helicoptorObj != null)
            m_helicoptorObj.SetActive(true);
    }

    public override void UpdatePhase()
    {
        //プレイヤーと目標座標の距離が一定以下なら
        float distance = Vector3.Distance(m_targetPos, PlayerPos());
        if(distance < 1.0f)
        {
            m_fadeUI.SetActive(true);
            m_fadeUI.GetComponent<FadeImage>().StartFade();


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

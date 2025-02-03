using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// ゲームオーバー用UIクラス
/// ゲームオーバー時に外部から有効、無効化を切り替え
/// </summary>
public class GameOverUI : MonoBehaviour
{
    //アニメーションさせるテキスト
    [SerializeField] private Text m_gameOverText;
    [SerializeField] private Text m_pressRText;

    //テキストのアルファ値変更用
    private CanvasGroup m_gameOverCanvasGroup;
    private CanvasGroup m_pressRCanvasGroup;

    //dotweenアニメーション用
    private Sequence m_sequence;

    /// <summary>
    /// アニメーション有効化
    /// UIをフェードインさせる
    /// </summary>
    /// <param name="_delay">遅延秒数</param>
    public void OnActive(float _delay_sec = 0.0f)
    {
        m_gameOverCanvasGroup = m_gameOverText.GetComponent<CanvasGroup>();
        m_pressRCanvasGroup = m_pressRText.GetComponent<CanvasGroup>();
        m_gameOverCanvasGroup.alpha = 0.0f;
        m_pressRCanvasGroup.alpha = 0.0f;

        m_sequence = DOTween.Sequence();

        m_sequence.AppendInterval(_delay_sec);
        //GAMEOVERがフェードイン
        m_sequence.Append(m_gameOverCanvasGroup.DOFade(endValue: 1.0f, duration: 1.0f));
        m_sequence.AppendInterval(0.2f);
        //PressRがフェードイン
        m_sequence.Append(m_pressRCanvasGroup.DOFade(endValue: 1.0f, duration: 1.0f));
        m_sequence.AppendInterval(1.0f);
        //ゆっくり点滅
        m_sequence.Append(m_pressRCanvasGroup.DOFade(endValue: 0.0f, duration: 1.0f)
            .SetLoops(-1, LoopType.Yoyo));

        m_sequence.Play();

    }

    /// <summary>
    /// アニメーション無効化
    /// テキストをフェードアウトする
    /// </summary>
    public void OffActive()
    {
        m_sequence.Kill();

        //同時にフェードアウト
        m_gameOverCanvasGroup.DOFade(endValue: 0.0f, duration: 0.5f);
        m_pressRCanvasGroup.DOFade(endValue: 0.0f, duration: 0.5f);
    }
}

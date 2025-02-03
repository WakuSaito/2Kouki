using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// <para>フェード画像クラス</para>
/// 画像のフェードイン、アウトを行う
/// </summary>
public class FadeImage : MonoBehaviour
{
    enum ANIM_TYPE
    {
        FADE_IN,
        FADE_OUT,
    }

    //どのアニメーションを再生するか
    [SerializeField] ANIM_TYPE m_animType = ANIM_TYPE.FADE_IN;

    //アニメーション秒数
    [SerializeField] float m_animSec = 1.0f;

    //透過させるためのコンポーネント
    CanvasGroup m_canvasGroup;

    //起動時に自動で動くか
    [SerializeField] private bool m_onAwake = false;

    private void Awake()
    {
        m_canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }
    
    private void Start()
    {
        if (m_onAwake)
            StartFade();
    }

    /// <summary>
    /// フェードの開始
    /// </summary>
    public void StartFade()
    {
        switch (m_animType)
        {
            case ANIM_TYPE.FADE_IN:
                FadeIn();
                break;

            case ANIM_TYPE.FADE_OUT:
                FadeOut();
                break;
        }
    }

    /// <summary>
    /// 画像のフェードイン
    /// </summary>
    /// <returns>アニメーション秒数</returns>
    public float FadeIn()
    {
        m_canvasGroup.alpha = 0f;

        m_canvasGroup.DOFade(endValue: 1f, duration: m_animSec);

        return m_animSec;
    }

    /// <summary>
    /// 画像のフェードアウト
    /// </summary>
    public void FadeOut()
    {
        m_canvasGroup.alpha = 1f;

        m_canvasGroup.DOFade(endValue: 0f, duration: m_animSec)
            .OnKill(() => m_canvasGroup.alpha = 0f);
    }
}

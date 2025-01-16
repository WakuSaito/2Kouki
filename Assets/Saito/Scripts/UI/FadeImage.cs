using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeImage : MonoBehaviour
{
    enum ANIM_TYPE
    {
        FADE_IN,
        FADE_OUT,
    }

    [SerializeField]//どのアニメーションを再生するか
    ANIM_TYPE m_animType = ANIM_TYPE.FADE_IN;

    [SerializeField]//アニメーション秒数
    float m_animSec = 1.0f;

    //透過させるためのコンポーネント
    CanvasGroup m_canvasGroup;

    [SerializeField]//起動時に自動で動くか
    private bool m_onAwake = false;

    private void Awake()
    {
        m_canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (m_onAwake)
            StartFade();
    }

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

    //画像のフェードイン
    private void FadeIn()
    {
        m_canvasGroup.alpha = 0f;

        m_canvasGroup.DOFade(endValue: 1f, duration: m_animSec);
    }

    //画像のフェードアウト
    private void FadeOut()
    {
        m_canvasGroup.alpha = 1f;

        m_canvasGroup.DOFade(endValue: 0f, duration: m_animSec)
            .OnKill(() => Destroy(gameObject));
    }
}

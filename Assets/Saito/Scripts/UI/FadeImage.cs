using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeImage : MonoBehaviour
{
    enum AnimType
    {
        FADE_IN,
        FADE_OUT,
    }

    [SerializeField]//どのアニメーションを再生するか
    AnimType animType = AnimType.FADE_IN;

    [SerializeField]//アニメーション秒数
    float animSec = 1.0f;

    //透過させるためのコンポーネント
    CanvasGroup canvasGroup;

    [SerializeField]//起動時に自動で動くか
    private bool onAwake = false;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (onAwake)
            StartFade();
    }

    public void StartFade()
    {
        switch (animType)
        {
            case AnimType.FADE_IN:
                FadeIn();
                break;

            case AnimType.FADE_OUT:
                FadeOut();
                break;
        }
    }

    //画像のフェードイン
    private void FadeIn()
    {
        canvasGroup.alpha = 0f;

        canvasGroup.DOFade(endValue: 1f, duration: animSec);
    }

    //画像のフェードアウト
    private void FadeOut()
    {
        canvasGroup.alpha = 1f;

        canvasGroup.DOFade(endValue: 0f, duration: animSec)
            .OnKill(() => Destroy(gameObject));
    }
}

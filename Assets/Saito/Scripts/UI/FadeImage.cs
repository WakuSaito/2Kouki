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

    [SerializeField]//�ǂ̃A�j���[�V�������Đ����邩
    AnimType animType = AnimType.FADE_IN;

    [SerializeField]//�A�j���[�V�����b��
    float animSec = 1.0f;

    //���߂����邽�߂̃R���|�[�l���g
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    private void Start()
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

    //�摜�̃t�F�[�h�C��
    private void FadeIn()
    {
        canvasGroup.alpha = 0f;

        canvasGroup.DOFade(endValue: 1f, duration: animSec);
    }

    //�摜�̃t�F�[�h�A�E�g
    private void FadeOut()
    {
        canvasGroup.alpha = 1f;

        canvasGroup.DOFade(endValue: 0f, duration: animSec)
            .OnKill(() => Destroy(gameObject));
    }
}

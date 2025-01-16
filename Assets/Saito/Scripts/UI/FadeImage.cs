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

    [SerializeField]//�ǂ̃A�j���[�V�������Đ����邩
    ANIM_TYPE m_animType = ANIM_TYPE.FADE_IN;

    [SerializeField]//�A�j���[�V�����b��
    float m_animSec = 1.0f;

    //���߂����邽�߂̃R���|�[�l���g
    CanvasGroup m_canvasGroup;

    [SerializeField]//�N�����Ɏ����œ�����
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

    //�摜�̃t�F�[�h�C��
    private void FadeIn()
    {
        m_canvasGroup.alpha = 0f;

        m_canvasGroup.DOFade(endValue: 1f, duration: m_animSec);
    }

    //�摜�̃t�F�[�h�A�E�g
    private void FadeOut()
    {
        m_canvasGroup.alpha = 1f;

        m_canvasGroup.DOFade(endValue: 0f, duration: m_animSec)
            .OnKill(() => Destroy(gameObject));
    }
}

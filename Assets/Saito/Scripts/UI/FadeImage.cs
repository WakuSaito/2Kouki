using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// <para>�t�F�[�h�摜�N���X</para>
/// �摜�̃t�F�[�h�C���A�A�E�g���s��
/// </summary>
public class FadeImage : MonoBehaviour
{
    enum ANIM_TYPE
    {
        FADE_IN,
        FADE_OUT,
    }

    //�ǂ̃A�j���[�V�������Đ����邩
    [SerializeField] ANIM_TYPE m_animType = ANIM_TYPE.FADE_IN;

    //�A�j���[�V�����b��
    [SerializeField] float m_animSec = 1.0f;

    //���߂����邽�߂̃R���|�[�l���g
    CanvasGroup m_canvasGroup;

    //�N�����Ɏ����œ�����
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
    /// �t�F�[�h�̊J�n
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
    /// �摜�̃t�F�[�h�C��
    /// </summary>
    /// <returns>�A�j���[�V�����b��</returns>
    public float FadeIn()
    {
        m_canvasGroup.alpha = 0f;

        m_canvasGroup.DOFade(endValue: 1f, duration: m_animSec);

        return m_animSec;
    }

    /// <summary>
    /// �摜�̃t�F�[�h�A�E�g
    /// </summary>
    public void FadeOut()
    {
        m_canvasGroup.alpha = 1f;

        m_canvasGroup.DOFade(endValue: 0f, duration: m_animSec)
            .OnKill(() => m_canvasGroup.alpha = 0f);
    }
}

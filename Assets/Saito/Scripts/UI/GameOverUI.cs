using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �Q�[���I�[�o�[�pUI�N���X
/// �Q�[���I�[�o�[���ɊO������L���A��������؂�ւ�
/// </summary>
public class GameOverUI : MonoBehaviour
{
    //�A�j���[�V����������e�L�X�g
    [SerializeField] private Text m_gameOverText;
    [SerializeField] private Text m_pressRText;

    //�e�L�X�g�̃A���t�@�l�ύX�p
    private CanvasGroup m_gameOverCanvasGroup;
    private CanvasGroup m_pressRCanvasGroup;

    //dotween�A�j���[�V�����p
    private Sequence m_sequence;

    /// <summary>
    /// �A�j���[�V�����L����
    /// UI���t�F�[�h�C��������
    /// </summary>
    /// <param name="_delay">�x���b��</param>
    public void OnActive(float _delay_sec = 0.0f)
    {
        m_gameOverCanvasGroup = m_gameOverText.GetComponent<CanvasGroup>();
        m_pressRCanvasGroup = m_pressRText.GetComponent<CanvasGroup>();
        m_gameOverCanvasGroup.alpha = 0.0f;
        m_pressRCanvasGroup.alpha = 0.0f;

        m_sequence = DOTween.Sequence();

        m_sequence.AppendInterval(_delay_sec);
        //GAMEOVER���t�F�[�h�C��
        m_sequence.Append(m_gameOverCanvasGroup.DOFade(endValue: 1.0f, duration: 1.0f));
        m_sequence.AppendInterval(0.2f);
        //PressR���t�F�[�h�C��
        m_sequence.Append(m_pressRCanvasGroup.DOFade(endValue: 1.0f, duration: 1.0f));
        m_sequence.AppendInterval(1.0f);
        //�������_��
        m_sequence.Append(m_pressRCanvasGroup.DOFade(endValue: 0.0f, duration: 1.0f)
            .SetLoops(-1, LoopType.Yoyo));

        m_sequence.Play();

    }

    /// <summary>
    /// �A�j���[�V����������
    /// �e�L�X�g���t�F�[�h�A�E�g����
    /// </summary>
    public void OffActive()
    {
        m_sequence.Kill();

        //�����Ƀt�F�[�h�A�E�g
        m_gameOverCanvasGroup.DOFade(endValue: 0.0f, duration: 0.5f);
        m_pressRCanvasGroup.DOFade(endValue: 0.0f, duration: 0.5f);
    }
}

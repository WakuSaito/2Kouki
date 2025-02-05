using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// <para>�����\��UI</para>
/// ���݂̓�����\�����A�ύX���ɃA�j���[�V����������
/// </summary>
public class DayCountUI : MonoBehaviour
{
    Text m_dayText;//�����e�L�X�g

    //�ύX���ꂽ�Ƃ��\������ʒu
    [SerializeField] Vector2 m_changePos;
    //�A�j���[�V��������ۂ̈ړ����x
    float m_moveSec = 0.8f;

    Vector3 m_defaultScale;//��{�T�C�Y

    private void Awake()
    {
        m_dayText = GetComponent<Text>();

        m_defaultScale = transform.localScale;
    }

    /// <summary>
    /// <para>�����ύX</para>
    /// ���l���A�j���[�V���������ĕύX
    /// </summary>
    /// <param name="_day_count">�ύX��̓���</param>
    public void ChangeDay(int _day_count)
    {
        //Sequence�̃C���X�^���X���쐬
        Sequence sequence = DOTween.Sequence();

        RectTransform rect_transform = GetComponent<RectTransform>();

        sequence.Append(rect_transform.DOAnchorPos(m_changePos, m_moveSec).SetEase(Ease.InOutQuad));
        sequence.Join(transform.DOScale(m_defaultScale * 3f, m_moveSec));
        sequence.AppendInterval(0.4f);
        sequence.AppendCallback(()=> m_dayText.text = _day_count + "����");//�e�L�X�g�X�V

        sequence.Play().SetLoops(2, LoopType.Yoyo);

    }
}

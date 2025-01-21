using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// <para>�J�[�\���⏕UI�N���X</para>
/// �}�E�X�J�[�\���̈ړ��A�N���b�N�𑣂�UI
/// </summary>
public class CursorAdvisorUI : MonoBehaviour
{
    //�}�E�X�摜�R���|�[�l���g
    [SerializeField] private Image m_mouseImage;

    //�}�E�X����w���p�摜
    [SerializeField] private Sprite m_idleMouseSprite;
    [SerializeField] private Sprite m_clickMouseSprite;

    //�J�[�\���̈ړ��J�n�A�I�����W
    private Vector2 m_startPos;
    private Vector2 m_endPos;

    [SerializeField]//�J�[�\������������(���b�œ������邩)
    private float m_cursorMoveSec = 1f;

    //�A�j���[�V��������
    private bool m_isMove = false;

    //dotween�A�j���[�V�����p
    private Sequence m_sequence;

    private void Awake()
    {
        m_mouseImage.sprite = m_idleMouseSprite;

        //���W�ۑ�
        m_startPos = transform.position;
    }

    /// <summary>
    /// <para>�ړ��J�n</para>
    /// DOTween�Ń��[�v���čĐ������
    /// </summary>
    public void StartMove()
    {
        //������Ă΂�Ȃ��悤��
        if (m_isMove) return;
        m_isMove = true;

        //Sequence�̃C���X�^���X���쐬
        m_sequence = DOTween.Sequence();

        //�J�[�\���ړ�
        m_sequence.Append(transform.DOMove(m_endPos, m_cursorMoveSec).SetEase(Ease.InOutQuad));
        m_sequence.AppendInterval(0.5f);
        //�N���b�N
        m_sequence.AppendCallback(() => m_mouseImage.sprite = m_clickMouseSprite);//�N���b�N�摜�ɕς���
        m_sequence.AppendInterval(0.5f);
        m_sequence.AppendCallback(() => m_mouseImage.sprite = m_idleMouseSprite);//�摜��߂�
        m_sequence.AppendInterval(0.5f);

        //���[�v�����Ď��s
        m_sequence.Play().SetLoops(-1, LoopType.Restart)
            .OnKill(()=> {
                m_mouseImage.sprite = m_idleMouseSprite;
                transform.position = m_startPos;
            });
    }


    /// <summary>
    /// �ړ���~
    /// </summary>
    public void StopMove()
    {
        if (!m_isMove) return;
        m_isMove = false;

        m_sequence.Kill();
    }

    /// <summary>
    /// �I���n�_�ݒ�
    /// </summary>
    /// <param name="_pos"></param>
    public void SetEndPos(Vector2 _pos)
    {
        m_endPos = _pos;
    }

}

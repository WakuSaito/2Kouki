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
    [SerializeField] private Sprite m_clickMouseLSprite;
    [SerializeField] private Sprite m_clickMouseRSprite;

    //�J�[�\���̈ړ��J�n�A�I�����W
    private Vector2 m_startPos;
    private Vector2 m_endPos;

    [SerializeField]//�J�[�\������������(���b�œ������邩)
    private float m_cursorMoveSec = 1f;

    //�A�j���[�V��������
    private bool m_isMove = false;

    //dotween�A�j���[�V�����p
    private Sequence m_sequence;

    //�A�j���[�V�����̎��
    public enum ANIM_TYPE
    {
        CLICK,
        DRAG,
    }

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
    /// <param name="_anim_type">�A�j���[�V�����^�C�v</param>
    public void StartMove(ANIM_TYPE _anim_type)
    {
        //������Ă΂�Ȃ��悤��
        if (m_isMove) return;
        m_isMove = true;

        //Sequence�̃C���X�^���X���쐬
        m_sequence = DOTween.Sequence();

        if (_anim_type == ANIM_TYPE.CLICK)
        {
            //�J�[�\���ړ�
            m_sequence.Append(transform.DOMove(m_endPos, m_cursorMoveSec).SetEase(Ease.InOutQuad));
            m_sequence.AppendInterval(0.5f);
            //�N���b�N
            m_sequence.AppendCallback(() => m_mouseImage.sprite = m_clickMouseRSprite);//�N���b�N�摜�ɕς���
            m_sequence.AppendInterval(0.5f);
            m_sequence.AppendCallback(() => m_mouseImage.sprite = m_idleMouseSprite);//�摜��߂�
            m_sequence.AppendInterval(0.5f);
        }
        else if(_anim_type == ANIM_TYPE.DRAG)
        {
            //�N���b�N
            m_sequence.AppendInterval(0.5f);
            m_sequence.AppendCallback(() => m_mouseImage.sprite = m_clickMouseLSprite);//�N���b�N�摜�ɕς���
            m_sequence.AppendInterval(0.5f);
            //�h���b�O
            m_sequence.Append(transform.DOMove(m_endPos, m_cursorMoveSec).SetEase(Ease.InOutQuad));
            m_sequence.AppendInterval(0.5f);
            //�N���b�N����
            m_sequence.AppendCallback(() => m_mouseImage.sprite = m_idleMouseSprite);//�摜��߂�
            m_sequence.AppendInterval(0.5f);
        }

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
    /// <param name="_pos">�I���ʒu</param>
    public void SetEndPos(Vector2 _pos)
    {
        m_endPos = _pos;
    }

    /// <summary>
    /// �J�n�n�_�ݒ�
    /// </summary>
    /// <param name="_pos">�J�n�ʒu</param>
    public void SetStartPos(Vector2 _pos)
    {
        m_startPos = _pos;
        transform.position = _pos;
    }
}

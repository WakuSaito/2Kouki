using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CursorAdvisorUI : MonoBehaviour
{
    //�}�E�X�摜�R���|�[�l���g
    [SerializeField]
    private Image mouseImage;

    //�}�E�X����w���p�摜
    [SerializeField]
    private Sprite idleMouseSprite;
    [SerializeField]
    private Sprite clickMouseSprite;

    //�J�[�\���̈ړ��J�n�A�I�����W
    private Vector2 startPos;
    private Vector2 endPos;

    [SerializeField]//�J�[�\������������(���b�œ������邩)
    private float cursorMoveSec = 1f;

    //�A�j���[�V��������
    private bool isMove = false;

    //dotween�A�j���[�V�����p
    private Sequence sequence;

    private void Awake()
    {
        mouseImage.sprite = idleMouseSprite;

        //���W�ۑ�
        startPos = transform.position;
    }


    public void StartMove()
    {
        //������Ă΂�Ȃ��悤��
        if (isMove) return;
        isMove = true;

        //Sequence�̃C���X�^���X���쐬
        sequence = DOTween.Sequence();

        //�J�[�\���ړ�
        sequence.Append(transform.DOMove(endPos, cursorMoveSec).SetEase(Ease.InOutQuad));
        sequence.AppendInterval(0.5f);
        //�N���b�N
        sequence.AppendCallback(() => mouseImage.sprite = clickMouseSprite);//�N���b�N�摜�ɕς���
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() => mouseImage.sprite = idleMouseSprite);//�摜��߂�
        sequence.AppendInterval(0.5f);

        //���[�v�����Ď��s
        sequence.Play().SetLoops(-1, LoopType.Restart)
            .OnKill(()=> { 
                mouseImage.sprite = idleMouseSprite;
                transform.position = startPos;
            });
    }


    public void StopMove()
    {
        if (!isMove) return;
        isMove = false;

        sequence.Kill();
    }

    public void SetEndPos(Vector2 _pos)
    {
        endPos = _pos;
    }

}

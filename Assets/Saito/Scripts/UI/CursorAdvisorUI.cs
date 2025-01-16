using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CursorAdvisorUI : MonoBehaviour
{
    //マウス画像コンポーネント
    [SerializeField]
    private Image m_mouseImage;

    //マウス操作指示用画像
    [SerializeField]
    private Sprite m_idleMouseSprite;
    [SerializeField]
    private Sprite m_clickMouseSprite;

    //カーソルの移動開始、終了座標
    private Vector2 m_startPos;
    private Vector2 m_endPos;

    [SerializeField]//カーソルが動く時間(何秒で到着するか)
    private float m_cursorMoveSec = 1f;

    //アニメーション中か
    private bool m_isMove = false;

    //dotweenアニメーション用
    private Sequence m_sequence;

    private void Awake()
    {
        m_mouseImage.sprite = m_idleMouseSprite;

        //座標保存
        m_startPos = transform.position;
    }


    public void StartMove()
    {
        //複数回呼ばれないように
        if (m_isMove) return;
        m_isMove = true;

        //Sequenceのインスタンスを作成
        m_sequence = DOTween.Sequence();

        //カーソル移動
        m_sequence.Append(transform.DOMove(m_endPos, m_cursorMoveSec).SetEase(Ease.InOutQuad));
        m_sequence.AppendInterval(0.5f);
        //クリック
        m_sequence.AppendCallback(() => m_mouseImage.sprite = m_clickMouseSprite);//クリック画像に変える
        m_sequence.AppendInterval(0.5f);
        m_sequence.AppendCallback(() => m_mouseImage.sprite = m_idleMouseSprite);//画像を戻す
        m_sequence.AppendInterval(0.5f);

        //ループさせて実行
        m_sequence.Play().SetLoops(-1, LoopType.Restart)
            .OnKill(()=> {
                m_mouseImage.sprite = m_idleMouseSprite;
                transform.position = m_startPos;
            });
    }


    public void StopMove()
    {
        if (!m_isMove) return;
        m_isMove = false;

        m_sequence.Kill();
    }

    public void SetEndPos(Vector2 _pos)
    {
        m_endPos = _pos;
    }

}

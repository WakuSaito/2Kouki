using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// <para>カーソル補助UIクラス</para>
/// マウスカーソルの移動、クリックを促すUI
/// </summary>
public class CursorAdvisorUI : MonoBehaviour
{
    //マウス画像コンポーネント
    [SerializeField] private Image m_mouseImage;

    //マウス操作指示用画像
    [SerializeField] private Sprite m_idleMouseSprite;
    [SerializeField] private Sprite m_clickMouseLSprite;
    [SerializeField] private Sprite m_clickMouseRSprite;

    //カーソルの移動開始、終了座標
    private Vector2 m_startPos;
    private Vector2 m_endPos;

    [SerializeField]//カーソルが動く時間(何秒で到着するか)
    private float m_cursorMoveSec = 1f;

    //アニメーション中か
    private bool m_isMove = false;

    //dotweenアニメーション用
    private Sequence m_sequence;

    //アニメーションの種類
    public enum ANIM_TYPE
    {
        CLICK,
        DRAG,
    }

    private void Awake()
    {
        m_mouseImage.sprite = m_idleMouseSprite;

        //座標保存
        m_startPos = transform.position;
    }

    /// <summary>
    /// <para>移動開始</para>
    /// DOTweenでループして再生される
    /// </summary>
    /// <param name="_anim_type">アニメーションタイプ</param>
    public void StartMove(ANIM_TYPE _anim_type)
    {
        //複数回呼ばれないように
        if (m_isMove) return;
        m_isMove = true;

        //Sequenceのインスタンスを作成
        m_sequence = DOTween.Sequence();

        if (_anim_type == ANIM_TYPE.CLICK)
        {
            //カーソル移動
            m_sequence.Append(transform.DOMove(m_endPos, m_cursorMoveSec).SetEase(Ease.InOutQuad));
            m_sequence.AppendInterval(0.5f);
            //クリック
            m_sequence.AppendCallback(() => m_mouseImage.sprite = m_clickMouseRSprite);//クリック画像に変える
            m_sequence.AppendInterval(0.5f);
            m_sequence.AppendCallback(() => m_mouseImage.sprite = m_idleMouseSprite);//画像を戻す
            m_sequence.AppendInterval(0.5f);
        }
        else if(_anim_type == ANIM_TYPE.DRAG)
        {
            //クリック
            m_sequence.AppendInterval(0.5f);
            m_sequence.AppendCallback(() => m_mouseImage.sprite = m_clickMouseLSprite);//クリック画像に変える
            m_sequence.AppendInterval(0.5f);
            //ドラッグ
            m_sequence.Append(transform.DOMove(m_endPos, m_cursorMoveSec).SetEase(Ease.InOutQuad));
            m_sequence.AppendInterval(0.5f);
            //クリック離す
            m_sequence.AppendCallback(() => m_mouseImage.sprite = m_idleMouseSprite);//画像を戻す
            m_sequence.AppendInterval(0.5f);
        }

        //ループさせて実行
        m_sequence.Play().SetLoops(-1, LoopType.Restart)
            .OnKill(()=> {
                m_mouseImage.sprite = m_idleMouseSprite;
                transform.position = m_startPos;
            });
    }


    /// <summary>
    /// 移動停止
    /// </summary>
    public void StopMove()
    {
        if (!m_isMove) return;
        m_isMove = false;

        m_sequence.Kill();
    }

    /// <summary>
    /// 終了地点設定
    /// </summary>
    /// <param name="_pos">終了位置</param>
    public void SetEndPos(Vector2 _pos)
    {
        m_endPos = _pos;
    }

    /// <summary>
    /// 開始地点設定
    /// </summary>
    /// <param name="_pos">開始位置</param>
    public void SetStartPos(Vector2 _pos)
    {
        m_startPos = _pos;
        transform.position = _pos;
    }
}

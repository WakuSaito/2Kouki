using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// <para>日数表示UI</para>
/// 現在の日数を表示し、変更時にアニメーションさせる
/// </summary>
public class DayCountUI : MonoBehaviour
{
    Text m_dayText;//日数テキスト

    //変更されたとき表示する位置
    [SerializeField] Vector2 m_changePos;
    //アニメーションする際の移動速度
    float m_moveSec = 0.8f;

    Vector3 m_defaultScale;//基本サイズ

    private void Awake()
    {
        m_dayText = GetComponent<Text>();

        m_defaultScale = transform.localScale;
    }

    /// <summary>
    /// <para>日数変更</para>
    /// 数値をアニメーションさせて変更
    /// </summary>
    /// <param name="_day_count">変更後の日数</param>
    public void ChangeDay(int _day_count)
    {
        //Sequenceのインスタンスを作成
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(m_changePos, m_moveSec).SetEase(Ease.InOutQuad));
        sequence.Join(transform.DOScale(m_defaultScale * 3f, m_moveSec));
        sequence.AppendInterval(0.4f);
        sequence.AppendCallback(()=> m_dayText.text = _day_count + "日目");//テキスト更新

        sequence.Play().SetLoops(2, LoopType.Yoyo);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DayCountUI : MonoBehaviour
{
    Text m_dayText;

    [SerializeField]
    Vector2 m_changePos;

    Vector3 m_defaultScale;

    float m_moveSec = 0.8f;

    private void Awake()
    {
        m_dayText = GetComponent<Text>();

        m_defaultScale = transform.localScale;
    }

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

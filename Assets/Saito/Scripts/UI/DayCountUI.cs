using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DayCountUI : MonoBehaviour
{
    Text dayText;

    [SerializeField]
    Vector2 changePos;

    Vector3 defaultScale;

    float moveSec = 0.8f;

    private void Awake()
    {
        dayText = GetComponent<Text>();

        defaultScale = transform.localScale;
    }

    public void ChangeDay(int _dayCount)
    {
        //Sequenceのインスタンスを作成
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(changePos, moveSec).SetEase(Ease.InOutQuad));
        sequence.Join(transform.DOScale(defaultScale * 3f, moveSec));
        sequence.AppendInterval(0.4f);
        sequence.AppendCallback(()=> dayText.text = _dayCount + "日目");//テキスト更新

        sequence.Play().SetLoops(2, LoopType.Yoyo);

    }
}

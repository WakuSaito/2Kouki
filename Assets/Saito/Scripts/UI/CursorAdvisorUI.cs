using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CursorAdvisorUI : MonoBehaviour
{
    //マウス画像コンポーネント
    [SerializeField]
    private Image mouseImage;

    //マウス操作指示用画像
    [SerializeField]
    private Sprite idleMouseSprite;
    [SerializeField]
    private Sprite clickMouseSprite;

    //カーソルの移動開始、終了座標
    private Vector2 startPos;
    private Vector2 endPos;

    [SerializeField]//移動終了地点のTransformObj
    private Transform moveEndTransform;

    [SerializeField]//カーソルが動く時間(何秒で到着するか)
    private float cursorMoveSec = 1f;

    //アニメーション中か
    private bool isMove = false;

    //dotweenアニメーション用
    private Sequence sequence;

    private void Awake()
    {
        mouseImage.sprite = idleMouseSprite;

        //座標保存
        startPos = transform.position;
        endPos = moveEndTransform.position;
        Destroy(moveEndTransform.gameObject);
    }


    public void StartMove()
    {
        //複数回呼ばれないように
        if (isMove) return;
        isMove = true;

        //Sequenceのインスタンスを作成
        sequence = DOTween.Sequence();

        //カーソル移動
        sequence.Append(transform.DOMove(endPos, cursorMoveSec).SetEase(Ease.InOutQuad));
        sequence.AppendInterval(0.5f);
        //クリック
        sequence.AppendCallback(() => mouseImage.sprite = clickMouseSprite);//クリック画像に変える
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() => mouseImage.sprite = idleMouseSprite);//画像を戻す
        sequence.AppendInterval(0.5f);

        //ループさせて実行
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

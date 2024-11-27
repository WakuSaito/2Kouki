using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TutorialBase : MonoBehaviour
{
    //マネージャクラスを所持しておく
    protected TutorialManager tutorialManager;

    //位置を取得するために所持
    private GameObject playerObj;

    private void Awake()
    {
        //マネージャクラス取得
        tutorialManager = gameObject.GetComponent<TutorialManager>();

        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    //プレイヤーの位置取得
    protected Vector3 PlayerPos()
    {
        return playerObj.transform.position;
    }


    //初期設定
    public abstract void SetUpPhase();

    //処理
    public abstract void UpdatePhase();

    //終了処理
    public abstract void EndPhase();

}

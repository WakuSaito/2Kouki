using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//自動でアタッチされるスクリプト
//[RequireComponent(typeof(PhaseGoHome))]
//[RequireComponent(typeof(PhaseTakeItem))]
//[RequireComponent(typeof(PhaseUseKnife))]
//[RequireComponent(typeof(PhaseExploreHouse))]

public class TutorialManager : MonoBehaviour
{
    //現在の進行度（チュートリアルの）
    private int currentPhase = 0;

    [SerializeField]//指示用テキスト
    private Text tutorialText;

    [SerializeField]//位置指示用マーカー
    private GameObject markerPrefab;

    private GameObject markerObj;

    //チュートリアルをフェーズごとに分けたスクリプト
    [SerializeField]
    private TutorialBase[] tutorialBases;

    private void Start()
    {
        //開始時のスクリプト呼び出し
        tutorialBases[currentPhase].SetUpPhase();
    }

    private void Update()
    {
        if (currentPhase >= tutorialBases.Length) return;

        //現在のフェーズのスクリプト処理呼び出し
        tutorialBases[currentPhase].UpdatePhase();

    }

    public void NextPhase()
    {
        Debug.Log("チュートリアルのフェーズ移行");
        //終了処理呼び出し
        tutorialBases[currentPhase].EndPhase();

        currentPhase++;
        if (currentPhase >= tutorialBases.Length) return;

        //開始時のスクリプト呼び出し
        tutorialBases[currentPhase].SetUpPhase();
    }

    /// <summary>
    /// 指定したテキストの表示
    /// </summary>
    public void SetText(string _str)
    {
        if (tutorialText == null) return;
        tutorialText.text = _str;

        //普段見えないようにしたい
    }
    /// <summary>
    /// テキストを非表示にする
    /// </summary>
    public void HideText()
    {
        if (tutorialText == null) return;
        tutorialText.text = "";//とりあえずテキストの中身を上書き
    }

    /// <summary>
    /// 指定した座標にマーカー設置
    /// </summary>
    public void CreateMarker(Vector3 _pos)
    {
        if (markerPrefab == null) return;

        //既に設置してあるマーカー削除
        DeleteMarker();

        markerObj = Instantiate(markerPrefab, _pos, Quaternion.identity);
    }
    /// <summary>
    /// マーカー削除
    /// </summary>
    public void DeleteMarker()
    {
        if (markerObj == null) return;
        //マーカー削除
        markerObj.GetComponent<Marker>().StartDelete();
        markerObj = null;
    }

    //カメラを特定の場所にスライド移動できるようにしたい
}

/*
ゲーム開始（フェードイン）

犬を助けるため外のゾンビを倒す（ナイフを使う）（簡単に倒せるようにしたい）

近くの民家を犬の探知を活用し探索

食料をゲット

インベントリから食料を食べさせる

犬に攻撃指示をし、道中のゾンビを撃破

駐屯基地を攻略しよう（ゲームの目的説明）
 */
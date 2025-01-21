using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//自動でアタッチされるスクリプト
//[RequireComponent(typeof(PhaseGoHome))]
//[RequireComponent(typeof(PhaseTakeItem))]
//[RequireComponent(typeof(PhaseUseKnife))]
//[RequireComponent(typeof(PhaseExploreHouse))]

/// <summary>
/// チュートリアル管理クラス
/// TutorialBaseを継承したクラスを順番に実行する
/// </summary>
public class TutorialManager : MonoBehaviour
{
    //現在の進行度（チュートリアルの）
    private int m_currentPhase = 0;

    //指示用テキスト
    [SerializeField] private Text m_tutorialText;
    //位置指示用マーカー
    [SerializeField] private GameObject m_markerPrefab;

    private GameObject m_markerObj;

    //チュートリアルをフェーズごとに分けたスクリプト
    [SerializeField] private TutorialBase[] m_tutorialBases;

    private void Start()
    {
        //開始時のスクリプト呼び出し
        m_tutorialBases[m_currentPhase].SetUpPhase();
    }

    private void Update()
    {
        if (m_currentPhase >= m_tutorialBases.Length) return;

        //現在のフェーズのスクリプト処理呼び出し
        m_tutorialBases[m_currentPhase].UpdatePhase();
    }

    /// <summary>
    /// フェーズ移行
    /// 順に並んだチュートリアル処理を次に進める
    /// </summary>
    public void NextPhase()
    {
        Debug.Log("チュートリアルのフェーズ移行");
        //終了処理呼び出し
        m_tutorialBases[m_currentPhase].EndPhase();

        m_currentPhase++;
        if (m_currentPhase >= m_tutorialBases.Length) return;

        //開始時のスクリプト呼び出し
        m_tutorialBases[m_currentPhase].SetUpPhase();
    }

    /// <summary>
    /// テキストの表示
    /// チュートリアル用テキストを表示する
    /// </summary>
    public void SetText(string _str)
    {
        if (m_tutorialText == null) return;
        m_tutorialText.text = _str;

        //普段見えないようにしたい
    }
    /// <summary>
    /// テキストの非表示
    /// </summary>
    public void HideText()
    {
        if (m_tutorialText == null) return;
        m_tutorialText.text = "";//とりあえずテキストの中身を上書き
    }

    /// <summary>
    /// マーカー作成
    /// 指定した座標にマーカー設置
    /// </summary>
    public void CreateMarker(Vector3 _pos)
    {
        if (m_markerPrefab == null) return;

        //既に設置してあるマーカー削除
        DeleteMarker();

        m_markerObj = Instantiate(m_markerPrefab, _pos, Quaternion.identity);
    }
    /// <summary>
    /// マーカー削除
    /// </summary>
    public void DeleteMarker()
    {
        if (m_markerObj == null) return;
        //マーカー削除
        m_markerObj.GetComponent<Marker>().StartDelete();
        m_markerObj = null;
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
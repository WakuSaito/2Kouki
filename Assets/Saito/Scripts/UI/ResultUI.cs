using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>リザルトUIクラス</para>
/// リザルトシーンのUI表示 
/// </summary>
public class ResultUI : MonoBehaviour
{
    //表示テキスト
    [SerializeField] private Text m_dayText;

    // Static変数を参照し、生存日数を設定
    void Start()
    {
        m_dayText.text = "生き残った日数　" + StaticVariables.liveingDayCount + "日";
    }

}

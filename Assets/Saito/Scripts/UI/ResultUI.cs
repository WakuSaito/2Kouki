using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    private Text m_dayText;

    // Start is called before the first frame update
    void Start()
    {
        m_dayText.text = "生き残った日数　" + StaticVariables.liveingDayCount + "日";
    }

}

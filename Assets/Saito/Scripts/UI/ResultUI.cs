using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    private Text dayText;

    // Start is called before the first frame update
    void Start()
    {
        dayText.text = "�����c���������@" + StaticVariables.liveingDayCount + "��";
    }

}

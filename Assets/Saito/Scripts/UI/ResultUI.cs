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
        dayText.text = "¶‚«c‚Á‚½“ú”@" + StaticVariables.liveingDayCount + "“ú";
    }

}

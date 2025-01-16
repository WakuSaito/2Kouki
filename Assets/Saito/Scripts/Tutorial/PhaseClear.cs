using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseClear : TutorialBase
{
    [SerializeField]
    private TimeController m_timeController;

    private SceneChanger m_sceneChanger;

    private SoundManager m_soundManager;


    public override void SetUpPhase()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_sceneChanger = GetComponent<SceneChanger>();

        m_soundManager.Play2DSE(m_soundManager.escapeMap);//se

        //日数保存
        StaticVariables.liveingDayCount = m_timeController.GetDayCount();

        //遅らせてシーン移動
        StartCoroutine(SceneChange(m_soundManager.escapeMap.length));
    }

    private IEnumerator SceneChange(float _delay)
    {
        Debug.Log("クリア");
        yield return new WaitForSeconds(_delay);

        m_sceneChanger.LoadResultScene();
    }

    public override void UpdatePhase()
    {
    }

    public override void EndPhase()
    {
    }

}

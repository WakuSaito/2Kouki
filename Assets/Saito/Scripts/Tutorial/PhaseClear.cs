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

        //�����ۑ�
        StaticVariables.liveingDayCount = m_timeController.GetDayCount();

        //�x�点�ăV�[���ړ�
        StartCoroutine(SceneChange(m_soundManager.escapeMap.length));
    }

    private IEnumerator SceneChange(float _delay)
    {
        Debug.Log("�N���A");
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

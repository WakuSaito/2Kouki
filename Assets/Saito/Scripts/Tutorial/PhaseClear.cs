using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseClear : TutorialBase
{
    [SerializeField]
    private TimeController timeController;

    private SceneChanger sceneChanger;

    private SoundManager soundManager;


    public override void SetUpPhase()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        sceneChanger = GetComponent<SceneChanger>();

        soundManager.Play2DSE(soundManager.escapeMap);//se

        //“ú”•Û‘¶
        StaticVariables.liveingDayCount = timeController.GetDayCount();

        //’x‚ç‚¹‚ÄƒV[ƒ“ˆÚ“®
        StartCoroutine(SceneChange(soundManager.escapeMap.length));
    }

    private IEnumerator SceneChange(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        sceneChanger.LoadResultScene();
    }

    public override void UpdatePhase()
    {
    }

    public override void EndPhase()
    {
    }

}

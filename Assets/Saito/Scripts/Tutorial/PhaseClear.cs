using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseClear : TutorialBase
{
    private SceneChanger sceneChanger;

    private SoundManager soundManager;


    public override void SetUpPhase()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        sceneChanger = GetComponent<SceneChanger>();

        soundManager.Play2DSE(soundManager.escapeMap);//se

        Invoke("SceneChange", soundManager.escapeMap.length);
    }

    private void SceneChange()
    {
        sceneChanger.LoadResultScene();
    }

    public override void UpdatePhase()
    {
    }

    public override void EndPhase()
    {
    }

}

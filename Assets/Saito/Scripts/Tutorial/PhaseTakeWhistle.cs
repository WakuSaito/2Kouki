using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTakeWhistle : TutorialBase
{
    [SerializeField]//���J�I�u�W�F�N�g
    private GameObject whistleObj;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("���J�����");
        tutorialManager.CreateMarker(whistleObj.transform.position);
    }

    public override void UpdatePhase()
    {
        //�i�C�t���擾������
        //�t�F�[�Y�ڍs
        tutorialManager.NextPhase();
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTakeItem : TutorialBase
{
    [SerializeField]//���J�I�u�W�F�N�g
    private GameObject itemObj;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("�H������ɓ���悤");
        tutorialManager.CreateMarker(itemObj.transform.position);
    }

    public override void UpdatePhase()
    {
        //�A�C�e�����擾������        
        if(itemObj == null)
            tutorialManager.NextPhase();//�t�F�[�Y�ڍs
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseExploreHouse : TutorialBase
{
    [SerializeField]//�ڕW�̍��W
    private Vector3 TargetPos;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("���Ɏw�����o��\n�ӂ�𒲂ׂ悤");
        tutorialManager.CreateMarker(TargetPos);
    }

    public override void UpdatePhase()
    {
        //UI�����\��

        //if(�w�����o������)

        tutorialManager.SetText("�Ƃ̒���T�����悤");
        tutorialManager.CreateMarker(TargetPos);
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }

}

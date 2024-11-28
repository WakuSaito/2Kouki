using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseUseKnife : TutorialBase
{
    [SerializeField]//����̍��W
    private GameObject targetZombieObj;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("�i�C�t�Ń]���r��|����");
        tutorialManager.CreateMarker(targetZombieObj.transform.position);
    }

    public override void UpdatePhase()
    {
        //if �i�C�t�������Ă��Ȃ��Ȃ� �X���b�g�؂�ւ��𑣂�
        //if(player.hand_weapon.tag != "Knife")

        //else �����Ă���Ȃ� �U���𑣂�

        //�]���r��|������
        //�t�F�[�Y�ڍs
        if(targetZombieObj == null)
            tutorialManager.NextPhase();
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }
}

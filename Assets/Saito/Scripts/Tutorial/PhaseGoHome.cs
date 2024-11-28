using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseGoHome : TutorialBase
{
    [SerializeField]//����̍��W
    private Vector3 HomePos;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("����֌�����");
        tutorialManager.CreateMarker(HomePos);
    }

    public override void UpdatePhase()
    {
        //�v���C���[�ƖڕW���W�̋��������ȉ��Ȃ�
        float distance = Vector3.Distance(HomePos, PlayerPos());
        if(distance < 4.0f)
        {
            //���̃t�F�[�Y�ɐi�߂�
            tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }
    
}

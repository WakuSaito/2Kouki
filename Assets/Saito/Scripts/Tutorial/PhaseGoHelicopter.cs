using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseGoHelicopter : TutorialBase
{
    [SerializeField]//�ڕW�̍��W
    private Vector3 TargetPos;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("��n����̃w���R�v�^�[�ŒE�o���悤");
        tutorialManager.CreateMarker(TargetPos);
    }

    public override void UpdatePhase()
    {
        //�v���C���[�ƖڕW���W�̋��������ȉ��Ȃ�
        float distance = Vector3.Distance(TargetPos, PlayerPos());
        if (distance < 4.0f)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseExploreHouse : TutorialBase
{
    [SerializeField]//�ڕW�̍��W
    private Vector3 TargetPos;

    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject plzChangeWeaponUI;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("�Ƃ̒���T�����悤");
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

        plzChangeWeaponUI.SetActive(false);
    }

}

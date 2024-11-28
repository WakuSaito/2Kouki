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
    }

    public override void UpdatePhase()
    {
        //if �J�������Ă��Ȃ��Ȃ�@��������悤����
        //if(player.hand_weapon.tag != "Whistle")

        //else �����Ă���Ȃ�@�g���悤����

        //if(�w�����o������)
        //DogManager�̃N�[���_�E�����Ď�

        tutorialManager.SetText("�Ƃ̒���T�����悤");
        tutorialManager.CreateMarker(TargetPos);
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
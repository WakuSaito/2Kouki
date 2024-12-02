using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseGoTown : TutorialBase
{
    [SerializeField]//�ڕW�̍��W
    private Vector3 TargetPos;

    [SerializeField]
    private GameObject fadeUI;

    public override void SetUpPhase()
    {

        tutorialManager.SetText("�X�ő����𐮂�\n�w���R�v�^�[�ŒE�o���悤");
        tutorialManager.CreateMarker(TargetPos);
    }

    public override void UpdatePhase()
    {
        //�v���C���[�ƖڕW���W�̋��������ȉ��Ȃ�
        float distance = Vector3.Distance(TargetPos, PlayerPos());
        if(distance < 1.0f)
        {
            fadeUI.SetActive(true);
            fadeUI.GetComponent<FadeImage>().StartFade();


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

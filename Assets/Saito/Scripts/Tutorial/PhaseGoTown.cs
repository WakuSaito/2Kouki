using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseGoTown : TutorialBase
{
    [SerializeField]//�ڕW�̍��W
    private Vector3 TargetPos;

    [SerializeField]//�t�F�[�h�A�E�g�p
    private GameObject fadeUI;

    [SerializeField]//�����ڗp�w���R�v�^�[�I�u�W�F�N�g
    private GameObject helicoptorObj;

    public override void SetUpPhase()
    {
        tutorialManager.SetText("�X�ő����𐮂�\n�w���R�v�^�[�ŒE�o���悤");
        tutorialManager.CreateMarker(TargetPos);
        //�w����\��
        if(helicoptorObj != null)
            helicoptorObj.SetActive(true);
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

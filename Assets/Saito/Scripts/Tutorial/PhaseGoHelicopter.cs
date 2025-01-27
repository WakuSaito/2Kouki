using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�w���R�v�^�[�Ɍ������t�F�C�Y</para>
/// �E�o�̖ړI�n�Ɍ����킹��
/// </summary>
public class PhaseGoHelicopter : TutorialBase
{
    [SerializeField]//�ڕW�̍��W
    private Vector3 m_targetPos;

    [SerializeField]//�t�F�[�h�A�E�g�p
    private GameObject m_fadeUI;

    [SerializeField]//�����ڗp�w���R�v�^�[�I�u�W�F�N�g
    private GameObject m_helicoptorObj;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("�X�ő����𐮂�\n�w���R�v�^�[�ŒE�o���悤");
        m_tutorialManager.CreateMarker(m_targetPos);
        //�w����\��
        if (m_helicoptorObj != null)
            m_helicoptorObj.SetActive(true);
    }

    public override void UpdatePhase()
    {
        //�v���C���[�ƖڕW���W�̋��������ȉ��Ȃ�
        float distance = Vector3.Distance(m_targetPos, PlayerPos());
        if (distance < 1.0f)
        {
            m_fadeUI.SetActive(true);
            m_fadeUI.GetComponent<FadeImage>().StartFade();


            //���̃t�F�[�Y�ɐi�߂�
            m_tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();
    }
}

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

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("��n����̃w���R�v�^�[�ŒE�o���悤");
        m_tutorialManager.CreateMarker(m_targetPos);
    }

    public override void UpdatePhase()
    {
        //�v���C���[�ƖڕW���W�̋��������ȉ��Ȃ�
        float distance = Vector3.Distance(m_targetPos, PlayerPos());
        if (distance < 4.0f)
        {
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

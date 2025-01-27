using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�A��t�F�C�Y</para>
/// ����i�Z�[�t�G���A�j�̑��݂�m���Ă��炤
/// </summary>
public class PhaseGoHome : TutorialBase
{
    [SerializeField]//�ڕW�̍��W
    private Vector3 m_targetPos;

    [SerializeField]
    private player m_player;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("�H�������邽�߂�\n��x����Ɍ�������");
        m_tutorialManager.CreateMarker(m_targetPos);
    }

    public override void UpdatePhase()
    {
        //�v���C���[���Z�[�t�G���A�ɓ�������
        if (m_player.m_inSafeAreaFlag == true)
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

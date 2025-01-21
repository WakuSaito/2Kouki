using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�ƒT���t�F�C�Y</para>
/// �T�����o���Ă��炤
/// </summary>
public class PhaseExploreHouse : TutorialBase
{
    [SerializeField]//�ڕW�̍��W
    private Vector3 m_targetPos;

    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject m_plzChangeWeaponUI;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("�Ƃ̒���T�����悤");
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

        m_plzChangeWeaponUI.SetActive(false);
    }

}

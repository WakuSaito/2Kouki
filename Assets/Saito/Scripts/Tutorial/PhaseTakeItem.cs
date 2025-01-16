using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTakeItem : TutorialBase
{
    [SerializeField]//�H���I�u�W�F�N�g
    private GameObject m_itemObj;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("�H������ɓ���悤");
        m_tutorialManager.CreateMarker(m_itemObj.transform.position);
    }

    public override void UpdatePhase()
    {
        //�A�C�e�����擾������        
        if(m_itemObj == null)
            m_tutorialManager.NextPhase();//�t�F�[�Y�ڍs
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();
    }
}

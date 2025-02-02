using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnimation
{
    /*[SerializeField] �C���X�y�N�^�[����ݒ�*/
    /// <summary> �A�j���[�^�[�I�u�W�F�N�g </summary>
    [SerializeField] GameObject m_animObj;

    /*�v���C�x�[�g�@private*/
    /// <summary> �A�j���[�^�[ </summary>
    Animator m_anim;

    /// <summary>
    /// �A�j���[�^�[�擾
    /// </summary>
    public void SetAnim()
    {
        m_anim = m_animObj.GetComponent<Animator>();
    }

    /// <summary>
    /// �ҋ@�A�j���[�V����
    /// </summary>
    public void Idle()
    {
        m_anim.SetBool("idle", true);

        m_anim.SetBool("Run", false);
        m_anim.SetBool("HandPislol", false);
    }

    /// <summary>
    /// ����A�j���[�V����
    /// </summary>
    public void Run()
    {
        m_anim.SetBool("Run", true);

        m_anim.SetBool("idle", false);
        m_anim.SetBool("HandPislol", false);
    }

    /// <summary>
    /// �e������������Ԃ̑���A�j���[�V����
    /// </summary>
    public void GunHandRun()
    {
        m_anim.SetBool("HandPislol", true);

        m_anim.SetBool("Run", false);
        m_anim.SetBool("idle", false);
    }
}

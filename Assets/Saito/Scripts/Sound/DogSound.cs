using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���T�E���h�N���X
/// �T�E���h�̍Đ����s��
/// </summary>
public class DogSound : DogBase
{
    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    //�R���|�[�l���g�擾
    public override void SetUpDog()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// �����Đ�
    /// </summary>
    public void PlayFootStep()
    {
        m_audioSource.PlayOneShot(m_soundManager.dogFootStep);
    }
    /// <summary>
    /// �U�����̖i���Đ�
    /// </summary>
    public void PlayAttackBark()
    {
        m_audioSource.PlayOneShot(m_soundManager.dogAttackBark);
    }
    /// <summary>
    /// �T�m���̖i���Đ�
    /// </summary>
    public void PlayDetectBark()
    {
        m_audioSource.PlayOneShot(m_soundManager.dogDetectBark);
    }
}

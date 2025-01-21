using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �]���r�T�E���h�N���X
/// �]���r��SE�Đ��p
/// </summary>
public class ZombieSound : ZombieBase
{
    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    //�R���|�[�l���g�擾
    public override void SetUpZombie()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// �����Đ�
    /// </summary>
    public void PlayFootStep()
    {
        AudioClip random_se = m_soundManager.zombieFootStep[Random.Range(0, m_soundManager.zombieFootStep.Length)];
        m_audioSource.PlayOneShot(random_se);
    }
    /// <summary>
    /// ����Đ�
    /// </summary>
    public void PlayVoice()
    {
        m_audioSource.PlayOneShot(m_soundManager.zombieVoice);
    }
    /// <summary>
    /// ��_���[�W���Đ�
    /// </summary>
    public void PlayDamage()
    {
        m_audioSource.PlayOneShot(m_soundManager.zombieDamage);
    }
    /// <summary>
    /// ���S���Đ�
    /// </summary>
    public void PlayDead()
    {
        m_audioSource.PlayOneShot(m_soundManager.zombieDead);
    }

}

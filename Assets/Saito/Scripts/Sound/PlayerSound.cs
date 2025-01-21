using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�T�E���h�N���X
/// �v���C���[��SE�Đ��p
/// </summary>
public class PlayerSound : MonoBehaviour
{
    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    //�R���|�[�l���g�擾
    private void Awake()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// �����Đ�
    /// </summary>
    public void PlayFootStep()
    {
        AudioClip random_se = m_soundManager.playerFootSteps[Random.Range(0, m_soundManager.playerFootSteps.Length)];
        m_audioSource.PlayOneShot(random_se);
    }
    /// <summary>
    /// �E�����Đ�
    /// </summary>
    public void PlayPickUp()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerPickUp);
    }
    /// <summary>
    /// �H�ׂ鉹�Đ�
    /// </summary>
    public void PlayEat()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerEat);
    }
    /// <summary>
    /// ���މ��Đ�
    /// </summary>
    public void PlayDrink()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerDrink);
    }
    /// <summary>
    /// �񕜂̉��Đ�
    /// </summary>
    public void PlayHeal()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerHeal);
    }
    /// <summary>
    /// ��_���[�W���Đ�
    /// </summary>
    public void PlayDamage()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerDamage);
    }
    /// <summary>
    /// �U���w���̓J�̉��Đ�
    /// </summary>
    public void PlayWhistleAttack()
    {
        m_audioSource.PlayOneShot(m_soundManager.whistleAttack);
    }
    /// <summary>
    /// �T�m�w���̓J�̉��Đ�
    /// </summary>
    public void PlayWhistleDetect()
    {
        m_audioSource.PlayOneShot(m_soundManager.whistleDetect);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSound : ZombieBase
{
    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    public override void SetUpZombie()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        AudioClip random_se = m_soundManager.zombieFootStep[Random.Range(0, m_soundManager.zombieFootStep.Length)];
        m_audioSource.PlayOneShot(random_se);
    }
    public void PlayVoice()
    {
        m_audioSource.PlayOneShot(m_soundManager.zombieVoice);
    }
    public void PlayDamage()
    {
        m_audioSource.PlayOneShot(m_soundManager.zombieDamage);
    }
    public void PlayDead()
    {
        m_audioSource.PlayOneShot(m_soundManager.zombieDead);
    }

}

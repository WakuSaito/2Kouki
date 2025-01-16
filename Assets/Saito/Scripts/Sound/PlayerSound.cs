using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        AudioClip random_se = m_soundManager.playerFootSteps[Random.Range(0, m_soundManager.playerFootSteps.Length)];
        m_audioSource.PlayOneShot(random_se);
    }
    public void PlayPickUp()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerPickUp);
    }
    public void PlayEat()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerEat);
    }
    public void PlayDrink()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerDrink);
    }
    public void PlayHeal()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerHeal);
    }
    public void PlayDamage()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerDamage);
    }
    public void PlayWhistleAttack()
    {
        m_audioSource.PlayOneShot(m_soundManager.whistleAttack);
    }
    public void PlayWhistleDetect()
    {
        m_audioSource.PlayOneShot(m_soundManager.whistleDetect);
    }
}

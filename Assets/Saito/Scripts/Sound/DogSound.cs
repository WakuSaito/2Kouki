using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSound : DogBase
{
    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    public override void SetUpDog()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        m_audioSource.PlayOneShot(m_soundManager.dogFootStep);
    }
    public void PlayAttackBark()
    {
        m_audioSource.PlayOneShot(m_soundManager.dogAttackBark);
    }
    public void PlayDetectBark()
    {
        m_audioSource.PlayOneShot(m_soundManager.dogDetectBark);
    }
}

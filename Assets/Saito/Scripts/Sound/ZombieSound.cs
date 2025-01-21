using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゾンビサウンドクラス
/// ゾンビのSE再生用
/// </summary>
public class ZombieSound : ZombieBase
{
    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    //コンポーネント取得
    public override void SetUpZombie()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// 足音再生
    /// </summary>
    public void PlayFootStep()
    {
        AudioClip random_se = m_soundManager.zombieFootStep[Random.Range(0, m_soundManager.zombieFootStep.Length)];
        m_audioSource.PlayOneShot(random_se);
    }
    /// <summary>
    /// 呻き声再生
    /// </summary>
    public void PlayVoice()
    {
        m_audioSource.PlayOneShot(m_soundManager.zombieVoice);
    }
    /// <summary>
    /// 被ダメージ音再生
    /// </summary>
    public void PlayDamage()
    {
        m_audioSource.PlayOneShot(m_soundManager.zombieDamage);
    }
    /// <summary>
    /// 死亡音再生
    /// </summary>
    public void PlayDead()
    {
        m_audioSource.PlayOneShot(m_soundManager.zombieDead);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーサウンドクラス
/// プレイヤーのSE再生用
/// </summary>
public class PlayerSound : MonoBehaviour
{
    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    //コンポーネント取得
    private void Awake()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// 足音再生
    /// </summary>
    public void PlayFootStep()
    {
        AudioClip random_se = m_soundManager.playerFootSteps[Random.Range(0, m_soundManager.playerFootSteps.Length)];
        m_audioSource.PlayOneShot(random_se);
    }
    /// <summary>
    /// 拾う音再生
    /// </summary>
    public void PlayPickUp()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerPickUp);
    }
    /// <summary>
    /// 食べる音再生
    /// </summary>
    public void PlayEat()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerEat);
    }
    /// <summary>
    /// 飲む音再生
    /// </summary>
    public void PlayDrink()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerDrink);
    }
    /// <summary>
    /// 回復の音再生
    /// </summary>
    public void PlayHeal()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerHeal);
    }
    /// <summary>
    /// 被ダメージ音再生
    /// </summary>
    public void PlayDamage()
    {
        m_audioSource.PlayOneShot(m_soundManager.playerDamage);
    }
    /// <summary>
    /// 攻撃指示の笛の音再生
    /// </summary>
    public void PlayWhistleAttack()
    {
        m_audioSource.PlayOneShot(m_soundManager.whistleAttack);
    }
    /// <summary>
    /// 探知指示の笛の音再生
    /// </summary>
    public void PlayWhistleDetect()
    {
        m_audioSource.PlayOneShot(m_soundManager.whistleDetect);
    }
}

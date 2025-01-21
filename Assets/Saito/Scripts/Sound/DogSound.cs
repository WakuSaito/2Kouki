using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 犬サウンドクラス
/// サウンドの再生を行う
/// </summary>
public class DogSound : DogBase
{
    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    //コンポーネント取得
    public override void SetUpDog()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// 足音再生
    /// </summary>
    public void PlayFootStep()
    {
        m_audioSource.PlayOneShot(m_soundManager.dogFootStep);
    }
    /// <summary>
    /// 攻撃時の吠え再生
    /// </summary>
    public void PlayAttackBark()
    {
        m_audioSource.PlayOneShot(m_soundManager.dogAttackBark);
    }
    /// <summary>
    /// 探知時の吠え再生
    /// </summary>
    public void PlayDetectBark()
    {
        m_audioSource.PlayOneShot(m_soundManager.dogDetectBark);
    }
}

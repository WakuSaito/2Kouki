using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 銃サウンドクラス
/// 銃のSE再生用
/// </summary>
public class GunSound : MonoBehaviour
{
    //再生する銃の種類用定数
    private const string PISTOL_STR = "Pistol";
    private const string ASSAULT_STR = "Assault";
    private const string SHOTGUN_STR = "ShotGun";

    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    //コンポーネント取得
    private void Awake()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// 発砲音再生
    /// </summary>
    /// <param name="_gun_type">銃の種類 Pistol,Assault,ShotGun</param>
    public void PlayShot(string _gun_type)
    {
        AudioClip sound;
        switch (_gun_type)
        {
            case PISTOL_STR:
            default:
                sound = m_soundManager.pistolShot;
                break;
            case ASSAULT_STR:
                sound = m_soundManager.assaultShot;
                break;
            case SHOTGUN_STR:
                sound = m_soundManager.shotgunShot;
                break;
        }

        m_audioSource.PlayOneShot(sound);
    }
    /// <summary>
    /// 空撃ち再生
    /// </summary>
    public void PlayBlankShot()
    {
        m_audioSource.PlayOneShot(m_soundManager.gunBlankShot);
    }
    /// <summary>
    /// リロードのマガジンを取る音再生
    /// </summary>
    /// <param name="_gun_type">銃の種類 Pistol,Assault</param>
    public void PlayReloadOut(string _gun_type)
    {
        AudioClip sound;
        switch (_gun_type)
        {
            case PISTOL_STR:
            default:
                sound = m_soundManager.pistolReloadOut;
                break;
            case ASSAULT_STR:
                sound = m_soundManager.assaultReloadOut;
                break;
        }

        m_audioSource.PlayOneShot(sound);
    }
    /// <summary>
    /// リロードのマガジン(もしくは弾)を入れる音再生
    /// </summary>
    /// <param name="_gun_type">銃の種類 Pistol,Assault,ShotGun</param>
    public void PlayReloadIn(string _gun_type)
    {
        AudioClip sound;
        switch (_gun_type)
        {
            case PISTOL_STR:
            default:
                sound = m_soundManager.pistolReloadIn;
                break;
            case ASSAULT_STR:
                sound = m_soundManager.assaultReloadIn;
                break;
            case SHOTGUN_STR:
                sound = m_soundManager.shotgunBulletIn;
                break;
        }

        m_audioSource.PlayOneShot(sound);
    }
    /// <summary>
    /// チャージングハンドルを引く音再生
    /// </summary>
    public void PlayChargingHandle()
    {
        m_audioSource.PlayOneShot(m_soundManager.assaultChargingHandle);
    }

}

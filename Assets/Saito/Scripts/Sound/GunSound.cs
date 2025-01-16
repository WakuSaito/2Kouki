using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSound : MonoBehaviour
{
    private const string PISTOL_STR = "Pistol";
    private const string ASSAULT_STR = "Assault";
    private const string SHOTGUN_STR = "ShotGun";

    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

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
    public void PlayBlankShot()
    {
        m_audioSource.PlayOneShot(m_soundManager.gunBlankShot);
    }
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

    public void PlayChargingHandle()
    {
        m_audioSource.PlayOneShot(m_soundManager.assaultChargingHandle);
    }

}

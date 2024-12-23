using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSound : MonoBehaviour
{
    private const string PISTOL_STR = "Pistol";
    private const string ASSAULT_STR = "Assault";
    private const string SHOTGUN_STR = "ShotGun";

    private SoundManager soundManager;
    private AudioSource audioSource;

    private void Awake()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayShot(string _gunType)
    {
        AudioClip sound;
        switch (_gunType)
        {
            case PISTOL_STR:
            default:
                sound = soundManager.pistolShot;
                break;
            case ASSAULT_STR:
                sound = soundManager.assaultShot;
                break;
            case SHOTGUN_STR:
                sound = soundManager.shotgunShot;
                break;
        }

        audioSource.PlayOneShot(sound);
    }
    public void PlayBlankShot()
    {
        audioSource.PlayOneShot(soundManager.gunBlankShot);
    }
    public void PlayReloadOut(string _gunType)
    {
        AudioClip sound;
        switch (_gunType)
        {
            case PISTOL_STR:
            default:
                sound = soundManager.pistolReloadOut;
                break;
            case ASSAULT_STR:
                sound = soundManager.assaultReloadOut;
                break;
        }

        audioSource.PlayOneShot(sound);
    }
    public void PlayReloadIn(string _gunType)
    {
        AudioClip sound;
        switch (_gunType)
        {
            case PISTOL_STR:
            default:
                sound = soundManager.pistolReloadIn;
                break;
            case ASSAULT_STR:
                sound = soundManager.assaultReloadIn;
                break;
            case SHOTGUN_STR:
                sound = soundManager.shotgunBulletIn;
                break;
        }

        audioSource.PlayOneShot(sound);
    }

    public void PlayChargingHandle()
    {
        audioSource.PlayOneShot(soundManager.assaultChargingHandle);
    }

}

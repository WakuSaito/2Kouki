using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSound : MonoBehaviour
{
    private const string PISTOL_STR = "Pistol";
    private const string ASSAULT_STR = "Assault";
    private const string SHOTGUN_STR = "ShotGun";

    private SoundManager mSoundManager;
    private AudioSource mAudioSource;

    private void Awake()
    {
        mSoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        mAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayShot(string _gun_type)
    {
        AudioClip sound;
        switch (_gun_type)
        {
            case PISTOL_STR:
            default:
                sound = mSoundManager.pistolShot;
                break;
            case ASSAULT_STR:
                sound = mSoundManager.assaultShot;
                break;
            case SHOTGUN_STR:
                sound = mSoundManager.shotgunShot;
                break;
        }

        mAudioSource.PlayOneShot(sound);
    }
    public void PlayBlankShot()
    {
        mAudioSource.PlayOneShot(mSoundManager.gunBlankShot);
    }
    public void PlayReloadOut(string _gun_type)
    {
        AudioClip sound;
        switch (_gun_type)
        {
            case PISTOL_STR:
            default:
                sound = mSoundManager.pistolReloadOut;
                break;
            case ASSAULT_STR:
                sound = mSoundManager.assaultReloadOut;
                break;
        }

        mAudioSource.PlayOneShot(sound);
    }
    public void PlayReloadIn(string _gun_type)
    {
        AudioClip sound;
        switch (_gun_type)
        {
            case PISTOL_STR:
            default:
                sound = mSoundManager.pistolReloadIn;
                break;
            case ASSAULT_STR:
                sound = mSoundManager.assaultReloadIn;
                break;
            case SHOTGUN_STR:
                sound = mSoundManager.shotgunBulletIn;
                break;
        }

        mAudioSource.PlayOneShot(sound);
    }

    public void PlayChargingHandle()
    {
        mAudioSource.PlayOneShot(mSoundManager.assaultChargingHandle);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSound : MonoBehaviour
{
    private SoundManager soundManager;
    private AudioSource audioSource;

    private void Awake()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayShot()
    {
        audioSource.PlayOneShot(soundManager.gunShot);
    }
    public void PlayBlankShot()
    {
        audioSource.PlayOneShot(soundManager.gunBlankShot);
    }
    public void PlayReloadOut()
    {
        audioSource.PlayOneShot(soundManager.gunReloadOut);
    }
    public void PlayReloadIn()
    {
        audioSource.PlayOneShot(soundManager.gunReloadIn);
    }

}

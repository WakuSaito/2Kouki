using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private SoundManager soundManager;
    private AudioSource audioSource;

    private void Awake()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        AudioClip randomSE = soundManager.playerFootSteps[Random.Range(0, soundManager.playerFootSteps.Length)];
        audioSource.PlayOneShot(randomSE);
    }
    public void PlayPickUp()
    {
        audioSource.PlayOneShot(soundManager.playerPickUp);
    }
    public void PlayEat()
    {
        audioSource.PlayOneShot(soundManager.playerEat);
    }
    public void PlayDrink()
    {
        audioSource.PlayOneShot(soundManager.playerDrink);
    }
    public void PlayDamage()
    {
        audioSource.PlayOneShot(soundManager.playerDamage);
    }

}

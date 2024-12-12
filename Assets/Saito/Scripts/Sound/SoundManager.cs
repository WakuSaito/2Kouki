using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //�v���C���[
    [SerializeField] //����
    public AudioClip[] playerFootSteps;
    [SerializeField] //��_���[�W
    public AudioClip playerDamage;
    [SerializeField] //�i�C�t��U��
    public AudioClip playerKnifeSwing;
    [SerializeField] //�E��
    public AudioClip playerPickUp;
    [SerializeField] //�H�ׂ�
    public AudioClip playerEat;
    [SerializeField] //����
    public AudioClip playerDrink;
    [SerializeField] //��
    public AudioClip playerHeal;
    [SerializeField] //���C
    public AudioClip gunShot;
    [SerializeField] //�󌂂�
    public AudioClip gunBlankShot;
    [SerializeField] //�����[�h�i�}�K�W�����o���j
    public AudioClip gunReloadOut;
    [SerializeField] //�����[�h�i�}�K�W������j
    public AudioClip gunReloadIn;
    [SerializeField] //�U���w��
    public AudioClip whistleAttack;
    [SerializeField] //�T�m�w��
    public AudioClip whistleDetect;

    //�]���r
    [SerializeField] //����
    public AudioClip[] zombieFootStep;
    [SerializeField] //�
    public AudioClip zombieVoice;
    [SerializeField] //��_���[�W
    public AudioClip zombieDamage;
    [SerializeField] //���S
    public AudioClip zombieDead;

    //��
    [SerializeField] //����
    public AudioClip dogFootStep;
    [SerializeField] //�U�����̖i��
    public AudioClip dogAttackBark;
    [SerializeField] //�T�m���̖i��
    public AudioClip dogDetectBark;

    //�V�X�e��
    [SerializeField] //�{�^������
    public AudioClip pushButton;
    [SerializeField] //�E�o��
    public AudioClip escapeMap;
    [SerializeField] //�C���x���g���\��
    public AudioClip inventoryOpen;
    [SerializeField] //�C���x���g����\��
    public AudioClip inventoryClose;

    //BGM
    [SerializeField]
    public AudioClip titleBGM;
    [SerializeField] 
    public AudioClip nomalBGM;  

    private AudioClip nextBGM;
    private float changeBGMSpeed;
    private float maxVolume;
    private float currentVolume;

    private bool isFadeOut = false;
    private bool isFadeIn = false;

    //�f�o�b�O�p
    [SerializeField]
    private AudioClip[] debugPlaySounds;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        maxVolume = audioSource.volume;
        currentVolume = maxVolume;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            audioSource.PlayOneShot(debugPlaySounds[0]);
        }
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            audioSource.PlayOneShot(debugPlaySounds[1]);
        }
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            audioSource.PlayOneShot(debugPlaySounds[2]);
        }


        if(isFadeOut)
        {
            currentVolume -= changeBGMSpeed * Time.deltaTime;
            if (currentVolume <= 0)
            {
                isFadeOut = false;
                currentVolume = 0;

                audioSource.clip = nextBGM;
                nextBGM = null;
            }
        }
        else if(isFadeIn)
        {
            currentVolume += changeBGMSpeed * Time.deltaTime;
            if (currentVolume >= maxVolume)
            {
                isFadeIn = false;
                currentVolume = maxVolume;
            }
        }

        audioSource.volume = currentVolume;
    }

    public void ChangeBGM(AudioClip _bgm, float _speed)
    {
        nextBGM = _bgm;
        changeBGMSpeed = _speed;
        isFadeOut = true;
        isFadeIn = true;
    }

    public void Play2DSE(AudioClip _se)
    {
        audioSource.PlayOneShot(_se);
    }

}

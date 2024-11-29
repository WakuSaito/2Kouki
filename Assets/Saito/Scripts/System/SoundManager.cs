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
    public AudioClip zombieFootStep;
    [SerializeField] //�
    public AudioClip zombieVoice;
    [SerializeField] //��_���[�W
    public AudioClip zomvieDamage;
    [SerializeField] //���S
    public AudioClip zomvieDead;

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

    //�f�o�b�O�p
    [SerializeField]
    private AudioClip[] debugPlaySounds;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
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

    }

}

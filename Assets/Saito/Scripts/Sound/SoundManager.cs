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

    //�e
    [SerializeField] //�󌂂�
    public AudioClip gunBlankShot;
    [SerializeField] //���C
    public AudioClip pistolShot;
    [SerializeField] //�����[�h�i�}�K�W�����o���j
    public AudioClip pistolReloadOut;
    [SerializeField] //�����[�h�i�}�K�W������j
    public AudioClip pistolReloadIn;
    [SerializeField] //���C
    public AudioClip assaultShot;
    [SerializeField] //�����[�h�i�}�K�W�����o���j
    public AudioClip assaultReloadOut;
    [SerializeField] //�����[�h�i�}�K�W������j
    public AudioClip assaultReloadIn;
    [SerializeField] //�`���[�W���O�n���h��������
    public AudioClip assaultChargingHandle;
    [SerializeField] //���C
    public AudioClip shotgunShot;
    [SerializeField] //�e����
    public AudioClip shotgunBulletIn;

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

    private AudioClip m_nextBGM;
    private float m_changeBGMSpeed;
    private float m_maxVolume;
    private float m_currentVolume;

    private bool m_isFadeOut = false;
    private bool m_isFadeIn = false;

    AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_maxVolume = m_audioSource.volume;
        m_currentVolume = m_maxVolume;
    }
    private void Update()
    {
        if(m_isFadeOut)
        {
            m_currentVolume -= m_changeBGMSpeed * Time.deltaTime;
            if (m_currentVolume <= 0)
            {
                m_isFadeOut = false;
                m_currentVolume = 0;

                m_audioSource.clip = m_nextBGM;
                m_nextBGM = null;
            }
        }
        else if(m_isFadeIn)
        {
            m_currentVolume += m_changeBGMSpeed * Time.deltaTime;
            if (m_currentVolume >= m_maxVolume)
            {
                m_isFadeIn = false;
                m_currentVolume = m_maxVolume;
            }
        }

        m_audioSource.volume = m_currentVolume;
    }

    public void ChangeBGM(AudioClip _bgm, float _speed)
    {
        m_nextBGM = _bgm;
        m_changeBGMSpeed = _speed;
        m_isFadeOut = true;
        m_isFadeIn = true;
    }

    public void Play2DSE(AudioClip _se)
    {
        m_audioSource.PlayOneShot(_se);
    }

}

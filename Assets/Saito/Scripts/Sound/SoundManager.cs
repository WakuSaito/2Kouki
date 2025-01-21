using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�T�E���h�Ǘ��N���X</para>
/// �����t�@�C�����C���X�y�N�^�[�Ŏw�肵�A���N���X�ōĐ��o����
/// </summary>
// �Ȃ��ϐ�����ύX����ƃT�E���h���w�肵�Ȃ����Ȃ���΂����Ȃ����ߎ��t���Ă��Ȃ�
public class SoundManager : MonoBehaviour
{
    //�v���C���[
    [SerializeField] public AudioClip[] playerFootSteps;//����
    [SerializeField] public AudioClip playerDamage;     //��_���[�W
    [SerializeField] public AudioClip playerKnifeSwing; //�i�C�t��U��
    [SerializeField] public AudioClip playerPickUp;//�E��
    [SerializeField] public AudioClip playerEat;   //�H�ׂ�
    [SerializeField] public AudioClip playerDrink; //����
    [SerializeField] public AudioClip playerHeal;  //��

    //�e
    [SerializeField] public AudioClip gunBlankShot;//�󌂂�
    [SerializeField] public AudioClip pistolShot;  //���C
    [SerializeField] public AudioClip pistolReloadOut;//�����[�h�i�}�K�W�����o���j
    [SerializeField] public AudioClip pistolReloadIn; //�����[�h�i�}�K�W������j
    [SerializeField] public AudioClip assaultShot;     //���C
    [SerializeField] public AudioClip assaultReloadOut;//�����[�h�i�}�K�W�����o���j
    [SerializeField] public AudioClip assaultReloadIn; //�����[�h�i�}�K�W������j
    [SerializeField] public AudioClip assaultChargingHandle;//�`���[�W���O�n���h��������
    [SerializeField] public AudioClip shotgunShot;    //���C
    [SerializeField] public AudioClip shotgunBulletIn;//�e����

    [SerializeField] public AudioClip whistleAttack;//�U���w��
    [SerializeField] public AudioClip whistleDetect;//�T�m�w��

    //�]���r
    [SerializeField] public AudioClip[] zombieFootStep;//����
    [SerializeField] public AudioClip zombieVoice; //�
    [SerializeField] public AudioClip zombieDamage;//��_���[�W
    [SerializeField] public AudioClip zombieDead;  //���S

    //��
    [SerializeField] public AudioClip dogFootStep;  //����
    [SerializeField] public AudioClip dogAttackBark;//�U�����̖i��
    [SerializeField] public AudioClip dogDetectBark;//�T�m���̖i��

    //�V�X�e��
    [SerializeField] public AudioClip pushButton;//�{�^������
    [SerializeField] public AudioClip escapeMap; //�E�o��
    [SerializeField] public AudioClip inventoryOpen; //�C���x���g���\��
    [SerializeField] public AudioClip inventoryClose;//�C���x���g����\��

    //BGM
    [SerializeField] public AudioClip titleBGM;//�^�C�g��
    [SerializeField] public AudioClip nomalBGM;//���C��

    private AudioClip m_nextBGM;
    private float m_changeBGMSpeed;
    private float m_maxVolume;
    private float m_currentVolume;

    private bool m_isFadeOut = false;
    private bool m_isFadeIn = false;

    AudioSource m_audioSource;

    //�R���|�[�l���g�A���ʎ擾�@
    private void Awake()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_maxVolume = m_audioSource.volume;
        m_currentVolume = m_maxVolume;
    }

    //BGM�̎��R�Ȑ؂�ւ����s
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

    /// <summary>
    /// BGM�؂�ւ�
    /// ���R��BGM��؂�ւ���
    /// </summary>
    /// <param name="_bgm">�؂�ւ����BGM</param>
    /// <param name="_speed">�؂�ւ�鑬�x</param>
    public void ChangeBGM(AudioClip _bgm, float _speed)
    {
        m_nextBGM = _bgm;
        m_changeBGMSpeed = _speed;
        m_isFadeOut = true;
        m_isFadeIn = true;
    }

    /// <summary>
    /// 2DSE�̍Đ�
    /// ���̉����𖳎�����SE�Đ�
    /// </summary>
    /// <param name="_se">�Đ�����SE</param>
    public void Play2DSE(AudioClip _se)
    {
        m_audioSource.PlayOneShot(_se);
    }

}

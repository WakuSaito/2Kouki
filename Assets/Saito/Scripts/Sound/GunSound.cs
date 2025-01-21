using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�T�E���h�N���X
/// �e��SE�Đ��p
/// </summary>
public class GunSound : MonoBehaviour
{
    //�Đ�����e�̎�ޗp�萔
    private const string PISTOL_STR = "Pistol";
    private const string ASSAULT_STR = "Assault";
    private const string SHOTGUN_STR = "ShotGun";

    private SoundManager m_soundManager;
    private AudioSource m_audioSource;

    //�R���|�[�l���g�擾
    private void Awake()
    {
        m_soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// ���C���Đ�
    /// </summary>
    /// <param name="_gun_type">�e�̎�� Pistol,Assault,ShotGun</param>
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
    /// �󌂂��Đ�
    /// </summary>
    public void PlayBlankShot()
    {
        m_audioSource.PlayOneShot(m_soundManager.gunBlankShot);
    }
    /// <summary>
    /// �����[�h�̃}�K�W������鉹�Đ�
    /// </summary>
    /// <param name="_gun_type">�e�̎�� Pistol,Assault</param>
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
    /// �����[�h�̃}�K�W��(�������͒e)�����鉹�Đ�
    /// </summary>
    /// <param name="_gun_type">�e�̎�� Pistol,Assault,ShotGun</param>
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
    /// �`���[�W���O�n���h�����������Đ�
    /// </summary>
    public void PlayChargingHandle()
    {
        m_audioSource.PlayOneShot(m_soundManager.assaultChargingHandle);
    }

}

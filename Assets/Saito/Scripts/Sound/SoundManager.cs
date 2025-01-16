using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //プレイヤー
    [SerializeField] //足音
    public AudioClip[] playerFootSteps;
    [SerializeField] //被ダメージ
    public AudioClip playerDamage;
    [SerializeField] //ナイフを振る
    public AudioClip playerKnifeSwing;
    [SerializeField] //拾う
    public AudioClip playerPickUp;
    [SerializeField] //食べる
    public AudioClip playerEat;
    [SerializeField] //飲む
    public AudioClip playerDrink;
    [SerializeField] //回復
    public AudioClip playerHeal;

    //銃
    [SerializeField] //空撃ち
    public AudioClip gunBlankShot;
    [SerializeField] //発砲
    public AudioClip pistolShot;
    [SerializeField] //リロード（マガジン取り出し）
    public AudioClip pistolReloadOut;
    [SerializeField] //リロード（マガジン入れ）
    public AudioClip pistolReloadIn;
    [SerializeField] //発砲
    public AudioClip assaultShot;
    [SerializeField] //リロード（マガジン取り出し）
    public AudioClip assaultReloadOut;
    [SerializeField] //リロード（マガジン入れ）
    public AudioClip assaultReloadIn;
    [SerializeField] //チャージングハンドルを引く
    public AudioClip assaultChargingHandle;
    [SerializeField] //発砲
    public AudioClip shotgunShot;
    [SerializeField] //弾込め
    public AudioClip shotgunBulletIn;

    [SerializeField] //攻撃指示
    public AudioClip whistleAttack;
    [SerializeField] //探知指示
    public AudioClip whistleDetect;

    //ゾンビ
    [SerializeField] //足音
    public AudioClip[] zombieFootStep;
    [SerializeField] //呻き
    public AudioClip zombieVoice;
    [SerializeField] //被ダメージ
    public AudioClip zombieDamage;
    [SerializeField] //死亡
    public AudioClip zombieDead;

    //犬
    [SerializeField] //足音
    public AudioClip dogFootStep;
    [SerializeField] //攻撃時の吠え
    public AudioClip dogAttackBark;
    [SerializeField] //探知時の吠え
    public AudioClip dogDetectBark;

    //システム
    [SerializeField] //ボタン押下
    public AudioClip pushButton;
    [SerializeField] //脱出時
    public AudioClip escapeMap;
    [SerializeField] //インベントリ表示
    public AudioClip inventoryOpen;
    [SerializeField] //インベントリ非表示
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

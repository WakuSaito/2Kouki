using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>サウンド管理クラス</para>
/// 音声ファイルをインスペクターで指定し、他クラスで再生出来る
/// </summary>
// なお変数名を変更するとサウンドを指定しなおさなければいけないため手を付けていない
public class SoundManager : MonoBehaviour
{
    //プレイヤー
    [SerializeField] public AudioClip[] playerFootSteps;//足音
    [SerializeField] public AudioClip playerDamage;     //被ダメージ
    [SerializeField] public AudioClip playerKnifeSwing; //ナイフを振る
    [SerializeField] public AudioClip playerPickUp;//拾う
    [SerializeField] public AudioClip playerEat;   //食べる
    [SerializeField] public AudioClip playerDrink; //飲む
    [SerializeField] public AudioClip playerHeal;  //回復

    //銃
    [SerializeField] public AudioClip gunBlankShot;//空撃ち
    [SerializeField] public AudioClip pistolShot;  //発砲
    [SerializeField] public AudioClip pistolReloadOut;//リロード（マガジン取り出し）
    [SerializeField] public AudioClip pistolReloadIn; //リロード（マガジン入れ）
    [SerializeField] public AudioClip assaultShot;     //発砲
    [SerializeField] public AudioClip assaultReloadOut;//リロード（マガジン取り出し）
    [SerializeField] public AudioClip assaultReloadIn; //リロード（マガジン入れ）
    [SerializeField] public AudioClip assaultChargingHandle;//チャージングハンドルを引く
    [SerializeField] public AudioClip shotgunShot;    //発砲
    [SerializeField] public AudioClip shotgunBulletIn;//弾込め

    [SerializeField] public AudioClip whistleAttack;//攻撃指示
    [SerializeField] public AudioClip whistleDetect;//探知指示

    //ゾンビ
    [SerializeField] public AudioClip[] zombieFootStep;//足音
    [SerializeField] public AudioClip zombieVoice; //呻き
    [SerializeField] public AudioClip zombieDamage;//被ダメージ
    [SerializeField] public AudioClip zombieDead;  //死亡

    //犬
    [SerializeField] public AudioClip dogFootStep;  //足音
    [SerializeField] public AudioClip dogAttackBark;//攻撃時の吠え
    [SerializeField] public AudioClip dogDetectBark;//探知時の吠え

    //システム
    [SerializeField] public AudioClip pushButton;//ボタン押下
    [SerializeField] public AudioClip escapeMap; //脱出時
    [SerializeField] public AudioClip inventoryOpen; //インベントリ表示
    [SerializeField] public AudioClip inventoryClose;//インベントリ非表示

    //BGM
    [SerializeField] public AudioClip titleBGM;//タイトル
    [SerializeField] public AudioClip nomalBGM;//メイン

    private AudioClip m_nextBGM;
    private float m_changeBGMSpeed;
    private float m_maxVolume;
    private float m_currentVolume;

    private bool m_isFadeOut = false;
    private bool m_isFadeIn = false;

    AudioSource m_audioSource;

    //コンポーネント、音量取得　
    private void Awake()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_maxVolume = m_audioSource.volume;
        m_currentVolume = m_maxVolume;
    }

    //BGMの自然な切り替え実行
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
    /// BGM切り替え
    /// 自然にBGMを切り替える
    /// </summary>
    /// <param name="_bgm">切り替え後のBGM</param>
    /// <param name="_speed">切り替わる速度</param>
    public void ChangeBGM(AudioClip _bgm, float _speed)
    {
        m_nextBGM = _bgm;
        m_changeBGMSpeed = _speed;
        m_isFadeOut = true;
        m_isFadeIn = true;
    }

    /// <summary>
    /// 2DSEの再生
    /// 立体音響を無視したSE再生
    /// </summary>
    /// <param name="_se">再生するSE</param>
    public void Play2DSE(AudioClip _se)
    {
        m_audioSource.PlayOneShot(_se);
    }

}

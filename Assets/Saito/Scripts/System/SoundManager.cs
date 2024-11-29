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
    [SerializeField] //発砲
    public AudioClip gunShot;
    [SerializeField] //空撃ち
    public AudioClip gunBlankShot;
    [SerializeField] //リロード（マガジン取り出し）
    public AudioClip gunReloadOut;
    [SerializeField] //リロード（マガジン入れ）
    public AudioClip gunReloadIn;
    [SerializeField] //攻撃指示
    public AudioClip whistleAttack;
    [SerializeField] //探知指示
    public AudioClip whistleDetect;

    //ゾンビ
    [SerializeField] //足音
    public AudioClip zombieFootStep;
    [SerializeField] //呻き
    public AudioClip zombieVoice;
    [SerializeField] //被ダメージ
    public AudioClip zomvieDamage;
    [SerializeField] //死亡
    public AudioClip zomvieDead;

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

    //デバッグ用
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

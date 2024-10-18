using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpGage : MonoBehaviour
{
    // 体力ゲージ（表面の常に見える部分）
    [SerializeField]  GameObject gauge;
    // 猶予ゲージ（体力が減ったとき一瞬見える部分）
    [SerializeField]  GameObject graceGauge;

    //最大体力
    int hp;

    // HP1あたりの幅
    float hp_memory;
    // 体力ゲージが減った後裏ゲージが減るまでの待機時間
    float waitingTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponent<player>().hp;
        // スプライトの幅を最大HPで割ってHP1あたりの幅を”_HP1”に入れておく
        hp_memory = gauge.GetComponent<RectTransform>().sizeDelta.x / hp;
    }

    public void HpDamageGage(float _damege)
    {
        float damage = hp_memory * _damege;

        // 体力ゲージの幅と高さをVector2で取り出す(Width,Height)
        Vector2 nowsafes = gauge.GetComponent<RectTransform>().sizeDelta;
        // 体力ゲージの幅からダメージ分の幅を引く
        nowsafes.x -= damage;
        // 体力ゲージに計算済みのVector2を設定する
        gauge.GetComponent<RectTransform>().sizeDelta = nowsafes;

        hp--;
        if (hp <= 0)
        {
            GetComponent<player>().bitten_zonbi_flag = true;
        }

        GetComponent<player>().hp = hp;
    }
}

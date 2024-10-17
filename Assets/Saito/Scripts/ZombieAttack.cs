using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    /// <summary>
    /// 当たり判定処理済みを記録
    /// </summary>
    private Dictionary<int, bool> hitMasters { get; } = new Dictionary<int, bool>();

    [SerializeField]//当たり判定
    Collider col;

    [SerializeField]//アニメーション用
    Animator ani;

    [SerializeField]
    bool on_attack = false;//デバッグ用

    private void Start()
    {
        col.enabled = false;
    }

    private void Update()
    {
        if(on_attack)
        {
            on_attack = false;
            StartAttack();
        }
    }

    //攻撃開始
    public void StartAttack()
    {
        StartCoroutine(attack());//コルーチン開始
        ani.SetTrigger("Attack");
    }

    IEnumerator attack()
    {
        hitMasters.Clear(); // 追加
        col.enabled = true;
        yield return new WaitForSeconds(2.0f);
        col.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // 追加
        // 攻撃対象部位ならHitZoneが取得できる
        var hitZone = other.GetComponent<HitZone>();
        if (hitZone == null) return;

        // 攻撃対象部位の親のインスタンスIDで重複した攻撃を判定
        int masterId = hitZone.Master.GetInstanceID();
        if (hitMasters.ContainsKey(masterId)) return;
        hitMasters[masterId] = true;

        Debug.Log("Hit!");
        // ダメージ計算とかこのへんで実装できますねぇ
        hitZone.Master.TakeDamage();
        // ヒット箇所を計算してエフェクトを表示する（前回から特に変更なし）
        //Vector3 hitPos = other.ClosestPointOnBounds(col.bounds.center);
        //GameObject effectIstance = Instantiate(hitEffect, hitPos, Quaternion.identity);
        //Destroy(effectIstance, 1);
    }
}

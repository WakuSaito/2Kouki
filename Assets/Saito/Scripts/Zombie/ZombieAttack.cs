using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゾンビの攻撃クラス
/// 攻撃判定にアタッチする
/// </summary>
public class ZombieAttack : ZombieBase
{
    // 当たり判定処理済みを記録  
    private Dictionary<int, bool> hitMasters { get; } = new Dictionary<int, bool>();

    [SerializeField]//当たり判定
    Collider col;

    //コルーチンキャンセル用
    Coroutine attackCoroutine;

    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUpZombie()
    {
        col = gameObject.GetComponent<Collider>();
        col.enabled = false;
    }


    /// <summary>
    /// 攻撃開始
    /// </summary>
    public void StartAttack()
    {
        Debug.Log("ゾンビの攻撃");
        attackCoroutine = StartCoroutine(attack());//コルーチン開始
    }

    /// <summary>
    /// 攻撃キャンセル用
    /// </summary>
    public void AttackCancel()
    {
        //とりあえずコライダーを無効化にする
        col.enabled = false;

        if (attackCoroutine == null) return;

        //コルーチン停止
        StopCoroutine(attackCoroutine);

        attackCoroutine = null;
    }

    IEnumerator attack()
    {
        hitMasters.Clear(); // リセット
        yield return new WaitForSeconds(0.3f);
        col.enabled = true;
        yield return new WaitForSeconds(1.3f);
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
        // ダメージ計算とかこのへんで実装できます
        hitZone.Master.TakeDamage();
        // ヒット箇所を計算してエフェクトを表示する（前回から特に変更なし）
        //Vector3 hitPos = other.ClosestPointOnBounds(col.bounds.center);
        //GameObject effectIstance = Instantiate(hitEffect, hitPos, Quaternion.identity);
        //Destroy(effectIstance, 1);
    }
}

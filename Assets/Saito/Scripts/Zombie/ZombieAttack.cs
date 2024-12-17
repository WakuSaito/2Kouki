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
    IEnumerator attackCoroutine;

    [SerializeField]//発生時間
    private float setUpSec = 0.3f;
    [SerializeField]//硬直時間
    private float recoverySec = 1.0f;

    //攻撃中フラグ
    public bool IsAttack { get; private set; }

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
        IsAttack = true;

        attackCoroutine = attack();//コルーチン開始
        StartCoroutine(attackCoroutine);
    }

    /// <summary>
    /// 攻撃キャンセル用
    /// </summary>
    public void AttackCancel()
    {
        //とりあえずコライダーを無効化にする
        col.enabled = false;
        IsAttack = false;

        if (attackCoroutine == null) return;

        //コルーチン停止
        StopCoroutine(attackCoroutine);

        attackCoroutine = null;
    }
    //一時停止
    public void Pause()
    {
        if (attackCoroutine == null) return;

        col.enabled = false;

        //コルーチン停止
        StopCoroutine(attackCoroutine);
    }
    //再開
    public void Resume()
    {
        if (IsAttack == false) return;
        if (attackCoroutine == null) return;

        col.enabled = true;

        StartCoroutine(attackCoroutine);
    }

    IEnumerator attack()
    {
        hitMasters.Clear(); // リセット
        yield return new WaitForSeconds(setUpSec);
        col.enabled = true;
        yield return new WaitForSeconds(recoverySec);
        col.enabled = false;
        attackCoroutine = null;
        IsAttack = false;
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

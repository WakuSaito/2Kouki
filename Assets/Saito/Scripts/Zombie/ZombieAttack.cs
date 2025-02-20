using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>ゾンビの攻撃クラス</para>
/// 攻撃判定にアタッチする
/// </summary>
public class ZombieAttack : ZombieBase
{
    // 当たり判定処理済みを記録  
    private Dictionary<int, bool> m_hitMasters { get; } = new Dictionary<int, bool>();

    [SerializeField]//当たり判定
    Collider m_col;

    //コルーチンキャンセル用
    IEnumerator m_attackCoroutine;

    //発生時間
    [SerializeField] private float m_setUpSec = 0.3f;
    //攻撃判定時間
    [SerializeField] private float m_onAttackSec = 0.1f;
    //硬直時間
    [SerializeField] private float m_recoverySec = 1.0f;

    //攻撃中フラグ
    public bool m_isAttack { get; private set; }

    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUpZombie()
    {
        m_col = gameObject.GetComponent<Collider>();
        m_col.enabled = false;
    }

    /// <summary>
    /// 攻撃開始
    /// </summary>
    public void StartAttack()
    {
        m_isAttack = true;

        m_attackCoroutine = Attack();//コルーチン開始
        StartCoroutine(m_attackCoroutine);
    }

    /// <summary>
    /// 攻撃キャンセル用
    /// </summary>
    public void AttackCancel()
    {
        //とりあえずコライダーを無効化にする
        m_col.enabled = false;
        m_isAttack = false;

        if (m_attackCoroutine == null) return;

        //コルーチン停止
        StopCoroutine(m_attackCoroutine);

        m_attackCoroutine = null;
    }
    //一時停止
    public void Pause()
    {
        if (m_attackCoroutine == null) return;

        //コルーチン停止
        StopCoroutine(m_attackCoroutine);
    }
    //再開
    public void Resume()
    {
        if (m_isAttack == false) return;
        if (m_attackCoroutine == null) return;

        StartCoroutine(m_attackCoroutine);
    }
    /// <summary>
    /// <para>攻撃コルーチン</para>
    /// 攻撃判定の有効、無効化など
    /// </summary>
    IEnumerator Attack()
    {
        m_hitMasters.Clear(); // リセット
        m_col.enabled = false;
        //コルーチンを再開しても待機時間情報が消えないようにする
        for (float i = 0; i < m_setUpSec; i += 0.01f)
            yield return new WaitForSeconds(0.01f);
        m_col.enabled = true;
        for (float i = 0; i < m_onAttackSec; i += 0.01f)
            yield return new WaitForSeconds(0.01f);
        m_col.enabled = false;
        for (float i = 0; i < m_recoverySec; i += 0.1f)
            yield return new WaitForSeconds(0.1f);
        m_attackCoroutine = null;
        m_isAttack = false;
    }

    //攻撃判定にプレイヤーが当たった時にダメージを与える
    void OnTriggerEnter(Collider other)
    {
        // 追加
        // 攻撃対象部位ならHitZoneが取得できる
        var hit_zone = other.GetComponent<HitZone>();
        if (hit_zone == null) return;

        // 攻撃対象部位の親のインスタンスIDで重複した攻撃を判定
        int master_id = hit_zone.Master.GetInstanceID();
        if (m_hitMasters.ContainsKey(master_id)) return;
        m_hitMasters[master_id] = true;

        Debug.Log("Hit!");
        // ダメージ計算とかこのへんで実装できます
        hit_zone.Master.TakeDamage();
        // ヒット箇所を計算してエフェクトを表示する（前回から特に変更なし）
        //Vector3 hitPos = other.ClosestPointOnBounds(col.bounds.center);
        //GameObject effectIstance = Instantiate(hitEffect, hitPos, Quaternion.identity);
        //Destroy(effectIstance, 1);
    }
}

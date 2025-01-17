using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    // 当たり判定処理済みを記録  
    private Dictionary<int, bool> m_hitMasters { get; } = new Dictionary<int, bool>();
    //当たり判定
    [SerializeField] Collider m_collider;

    //与えるダメージ
    [SerializeField] private int m_attackDamage = 2;

    //コルーチンキャンセル用
    Coroutine m_attackCoroutine;

    private void Start()
    {
        m_collider = gameObject.GetComponent<Collider>();
        m_collider.enabled = false;
    }


    /// <summary>
    /// 攻撃開始
    /// </summary>
    public void StartAttack()
    {
        Debug.Log("ナイフ攻撃開始");

        if (m_attackCoroutine != null)
            AttackCancel();//再生中のコルーチンがあればキャンセル

        m_attackCoroutine = StartCoroutine(attack());//コルーチン開始
    }

    /// <summary>
    /// 攻撃キャンセル用
    /// </summary>
    public void AttackCancel()
    {
        //とりあえずコライダーを無効化にする
        m_collider.enabled = false;

        if (m_attackCoroutine == null) return;

        //コルーチン停止
        StopCoroutine(m_attackCoroutine);

        m_attackCoroutine = null;
    }

    IEnumerator attack()
    {
        m_hitMasters.Clear(); // リセット
        m_collider.enabled = true;

        yield return new WaitForSeconds(1.3f);

        m_collider.enabled = false;
        m_attackCoroutine = null;
    }

    void OnTriggerEnter(Collider other)
    {
        string hit_tag = other.tag;
        //対象のタグ以外は接触しない
        if (hit_tag != "Body" && hit_tag != "Head") return;

        // 追加
        // 攻撃対象部位ならHitZoneが取得できる
        var hit_zone = other.GetComponent<ZombieHitZone>();
        if (hit_zone == null) return;

        // 攻撃対象部位の親のインスタンスIDで重複した攻撃を判定
        int master_id = hit_zone.Master.GetInstanceID();
        if (m_hitMasters.ContainsKey(master_id)) return;
        m_hitMasters[master_id] = true;

        Vector3 hit_pos = other.ClosestPointOnBounds(transform.position);

        Debug.Log("Hit!");
        // ダメージ計算とかこのへんで実装できます
        hit_zone.Master.TakeDamage(hit_tag, m_attackDamage, hit_pos);

    }
}

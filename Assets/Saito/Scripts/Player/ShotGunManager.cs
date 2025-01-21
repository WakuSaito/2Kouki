using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>ショットガンマネージャークラス</para>
/// 銃マネージャークラスを元にリロードの方式を変更したいため継承して実装
/// </summary>
public class ShotGunManager : GunManager
{
    //弾を込める間隔
    private const float BULLET_IN_INTERVAL = 0.6f;

    //リロードキャンセルフラグ
    private bool onCancelReload = false;

    /// <summary>
    /// <para>リロード</para>
    /// 継承元を上書きし、一つずつ弾を入れるようにする
    /// </summary>
    public override void Reload()
    {
        if (m_isShotCooldown) return;
        if (m_isReload) return;
        //ピストルの弾丸が最大数じゃなければreload可能
        if (GetCurrentMagazine() >= GetMagazineSize()) return;

        onCancelReload = false;//キャンセル状態リセット

        //if (m_inventory == null)
        if (m_inventoryItem == null)
        {
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            Invoke(nameof(StartBulletIn), 0.22f);
            return;
        }

        /*たぶんいらない*/
        //for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        //{
        //    //インベントリに弾丸があるか
        //    if (m_inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
        //    {
        //        m_animator.SetBool("Reload", true);  //reload
        //        m_isReload = true;
        //        Invoke(nameof(StartBulletIn), 0.22f);
        //    }
        //}

    }

    /// <summary>
    /// <para>リロードキャンセル</para>
    /// アニメーション遷移とフラグ管理
    /// </summary>
    public override void StopReload()
    {
        onCancelReload = true;
        m_isReload = false;
        m_animator.SetBool("Reload", false);
    }

    /// <summary>
    /// <para>弾込め開始</para>
    /// Invokeで遅延実行するためのクッション
    /// </summary>
    public void StartBulletIn()
    {
        StartCoroutine(BulletIn());
    }

    /// <summary>
    /// <para>弾込めコルーチン</para>
    /// 一定間隔ごとに弾を込める
    /// </summary>
    private IEnumerator BulletIn()
    {
        //リロード可能な弾数取得
        int bulletInNum = HowManyCanLoaded();
        //最大、またはキャンセルされるまで一つずつ弾を入れる
        for (int i = 0; i < bulletInNum; i++)
        {
            m_animator.SetTrigger("BulletIn");//アニメーション再生

            yield return new WaitForSeconds(BULLET_IN_INTERVAL);

            if (onCancelReload)//キャンセル
                yield break;

            AddBullet(1);
        }

        //リロード終了
        m_animator.SetBool("Reload", false);
        m_isReload = false;
    }

}

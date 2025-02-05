using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>ショットガンマネージャークラス</para>
/// 銃マネージャークラスを元にリロードの方式を変更したいため継承して実装
/// </summary>
public class ShotGunManager : GunManager
{
    //弾を入れ始めるまでの時間
    private const float START_RELOAD_DELAY = 0.22f;
    //弾を込める間隔
    private const float BULLET_IN_INTERVAL = 0.6f;

    //リロードキャンセルフラグ
    private bool m_onCancelReload = false;

    //一時停止用
    IEnumerator m_bulletInCoroutine;

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

        m_onCancelReload = false;//キャンセル状態リセット

        //if (m_inventory == null)
        if (m_inventoryItem == null)
        {
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            m_bulletInCoroutine = BulletIn();
            StartCoroutine(m_bulletInCoroutine);
            return;
        }
        else if (m_inventoryItem.CheckBullet())
        {
            //インベントリに弾丸があるか
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            m_bulletInCoroutine = BulletIn();
            StartCoroutine(m_bulletInCoroutine);
        }

    }


    /// <summary>
    /// <para>リロードキャンセル</para>
    /// アニメーション遷移とフラグ管理
    /// </summary>
    public override void StopReload()
    {
        if(m_bulletInCoroutine != null)
        {
        m_onCancelReload = true;
        m_isReload = false;
        m_animator.SetBool("Reload", false);
            StopCoroutine(m_bulletInCoroutine);
            m_bulletInCoroutine = null;
        }
    }

    /// <summary>
    /// <para>弾込めコルーチン</para>
    /// 一定間隔ごとに弾を込める
    /// </summary>
    private IEnumerator BulletIn()
    {
        //コルーチンを再開しても待機時間情報が消えないようにする
        for (float j = 0; j < START_RELOAD_DELAY; j += 0.1f)
            yield return new WaitForSeconds(0.1f);

        //リロード可能な弾数取得
        int bulletInNum = HowManyCanLoaded();
        //最大、またはキャンセルされるまで一つずつ弾を入れる
        for (int i = 0; i < bulletInNum; i++)
        {
            //弾が無くなったら
            if (m_inventoryItem.CheckBullet() == false)
            {
                break;
            }

            m_animator.SetTrigger("BulletIn");//アニメーション再生

            //コルーチンを再開しても待機時間情報が消えないようにする
            for (float j = 0; j < BULLET_IN_INTERVAL; j += 0.1f)
                yield return new WaitForSeconds(0.1f);

            if (m_onCancelReload)//キャンセル
            {
                m_bulletInCoroutine = null;
                yield break;
            }
            AddBullet(1);
        }

        //リロード終了
        m_animator.SetBool("Reload", false);
        m_isReload = false;
        m_bulletInCoroutine = null;
    }

    public override void Pause()
    {
        base.Pause();

        if (m_bulletInCoroutine != null)
            StopCoroutine(m_bulletInCoroutine);
    }

    public override void Resume()
    {
        base.Resume();

        if (m_bulletInCoroutine != null)
            StartCoroutine(m_bulletInCoroutine);
    }

}

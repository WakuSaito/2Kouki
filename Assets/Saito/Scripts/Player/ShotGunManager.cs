using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunManager : GunManager
{
    private const float BULLET_IN_INTERVAL = 0.6f;

    private bool onCancelReload = false;


    public override void Reload()
    {
        if (m_isShotCooldown) return;
        if (m_isReload) return;
        //ピストルの弾丸が最大数じゃなければreload可能
        if (GetCurrentMagazine() >= GetMagazineSize()) return;

        onCancelReload = false;//キャンセル状態リセット

        if (m_inventory == null)
        {
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            Invoke(nameof(StartBulletIn), 0.22f);
            return;
        }

        for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        {
            //インベントリに弾丸があるか
            if (m_inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
            {
                m_animator.SetBool("Reload", true);  //reload
                m_isReload = true;
                Invoke(nameof(StartBulletIn), 0.22f);
            }
        }

    }

    //リロードキャンセル
    public override void StopReload()
    {
        onCancelReload = true;
        m_isReload = false;
        m_animator.SetBool("Reload", false);
    }

    public void StartBulletIn()
    {
        StartCoroutine(BulletIn());
    }

    private IEnumerator BulletIn()
    {
        //リロード可能な弾数取得
        int bulletInNum = HowManyCanLoaded();

        for (int i = 0; i < bulletInNum; i++)
        {
            m_animator.SetTrigger("BulletIn");

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

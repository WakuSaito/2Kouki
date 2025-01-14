using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunManager : GunManager
{
    private const float BULLET_IN_INTERVAL = 0.6f;

    private bool onCancelReload = false;


    public override void Reload()
    {
        if (mIsShotCooldown) return;
        if (mIsReload) return;
        //ピストルの弾丸が最大数じゃなければreload可能
        if (GetCurrentMagazine() >= GetMagazineSize()) return;

        onCancelReload = false;//キャンセル状態リセット

        if (mInventory == null)
        {
            mAnimator.SetBool("Reload", true);  //reload
            mIsReload = true;
            Invoke(nameof(StartBulletIn), 0.22f);
            return;
        }

        for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        {
            //インベントリに弾丸があるか
            if (mInventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
            {
                mAnimator.SetBool("Reload", true);  //reload
                mIsReload = true;
                Invoke(nameof(StartBulletIn), 0.22f);
            }
        }

    }

    //リロードキャンセル
    public override void StopReload()
    {
        onCancelReload = true;
        mIsReload = false;
        mAnimator.SetBool("Reload", false);
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
            mAnimator.SetTrigger("BulletIn");

            yield return new WaitForSeconds(BULLET_IN_INTERVAL);

            if (onCancelReload)//キャンセル
                yield break;

            AddBullet(1);
        }

        //リロード終了
        mAnimator.SetBool("Reload", false);
        mIsReload = false;
    }

}

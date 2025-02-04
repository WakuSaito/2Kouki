using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttack
{
    /*[SerializeField] インスペクターから設定*/
    /// <summary> インベントリオブジェクト </summary>
    [SerializeField] GameObject m_playerObj;
    /// <summary> インベントリオブジェクト </summary>
    [SerializeField] GameObject m_dogObj;

    /*プライベート　private*/
    /// <summary> InventoryWeaponクラス </summary>
    InventoryWeapon m_inventoryWeapon;
    /// <summary> SearchViewAreaクラス </summary>
    SearchViewArea m_searchViewArea;
    /// <summary> PlayerSoundクラス </summary
    PlayerSound m_playerSound;

    /// <summary>
    /// 武器インベントリ取得
    /// サーチエリア取得
    /// プレイヤーサウンド取得
    /// </summary>
    public void SetAttack()
    {
        //コンポーネント取得
        m_inventoryWeapon = m_playerObj.GetComponent<InventoryWeapon>();
        m_searchViewArea = m_playerObj.GetComponent<SearchViewArea>();
        m_playerSound = m_playerObj.GetComponent<PlayerSound>();
    }

    /// <summary>
    /// 手持ちのオブジェクトを調べる
    /// </summary>
    /// <returns>手持ちのオブジェクト、犬の場合は犬本体</returns>
    public GameObject HandWeapon()
    {
        if (SelectWeaponSlot() != SLOT_ORDER.DOG)
        {
            return m_inventoryWeapon.m_weaponSlotObj[(int)m_inventoryWeapon.m_selectSlot];
        }
        else
        {
            return m_dogObj;
        }
    }

    public SLOT_ORDER SelectWeaponSlot()
    {
        return m_inventoryWeapon.m_selectSlot;
    }

    /// <summary>
    /// ナイフ攻撃
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void AttackKnife(bool _phsh)
    {
        if (SelectWeaponSlot() != SLOT_ORDER.KNIFE) return;

        HandWeapon().GetComponent<knifeAttackAnimetion>().AttackAnimation(_phsh);
    }

    /// <summary>
    /// 銃のリロード
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void GunReload(bool _phsh)
    {
        if (!_phsh) return;
        if (SelectWeaponSlot() != SLOT_ORDER.GUN) return;

        HandWeapon().GetComponent<GunManager>().Reload();
    }

    /// <summary>
    /// 銃の単発攻撃
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void AttackGunSingle(bool _phsh)
    {
        if (!_phsh) return;
        if (SelectWeaponSlot() != SLOT_ORDER.GUN) return;

        HandWeapon().GetComponent<GunManager>().PullTriggerDown();
    }

    /// <summary>
    /// 銃の連射攻撃
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    public void AttackGunRapidFire(bool _phsh)
    {
        if (!_phsh) return;
        if (SelectWeaponSlot() != SLOT_ORDER.GUN) return;

        HandWeapon().GetComponent<GunManager>().PullTrigger();
    }

    /// <summary>
    /// 犬の攻撃
    /// </summary>
    /// <param name="phsh">入力されているか</param>
    /// <param name="_search">SearchViewAreaクラス</param>
    /// <param name="_se">PlayerSoundクラス</param>
    public void AttackDog(bool phsh)
    {
        //犬のスロット以外の場合ゾンビの色を元に戻す
        if (SelectWeaponSlot() != SLOT_ORDER.DOG)
        {
            m_searchViewArea.ResetColor("Zombie");
            return;
        }

        //一番近いゾンビオブジェクトを取得
        GameObject targt_zombie_obj = m_searchViewArea.GetObjUpdate("Zombie", 20f, 0.5f);

        if (!phsh || targt_zombie_obj == null) return;

        m_playerSound.PlayWhistleAttack();//se
        HandWeapon().GetComponent<DogManager>().OrderAttack(targt_zombie_obj.GetComponentInParent<ZombieManager>().gameObject);
    }

    /// <summary>
    /// 犬の探索スキル
    /// </summary>
    /// <param name="_phsh">入力されているか</param>
    /// <param name="_se">PlayerSoundクラス</param>
    public void SearchSkillDog(bool _phsh)
    {
        if (!_phsh) return;
        if (SelectWeaponSlot() != SLOT_ORDER.DOG) return;

        m_playerSound.PlayWhistleDetect();//se
        HandWeapon().GetComponent<DogManager>().OrderDetection();
    }
}


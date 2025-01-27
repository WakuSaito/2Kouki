using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>ナイフ使用フェイズ</para>
/// ナイフを使い、ゾンビを倒すフェイズ
/// 完了時に犬が仲間になる
/// </summary>
public class PhaseUseKnife : TutorialBase
{
    //自宅の座標
    [SerializeField] private GameObject m_targetZombieObj;

    //インベントリの開閉監視用
    [SerializeField] private InventoryManager m_inventoryManager;
    //武器インベントリ
    [SerializeField] private InventoryWeapon m_inventoryWeapon;

    //武器切り替えを促すUI
    [SerializeField] private GameObject m_plzChangeWeaponUI;
    //武器切り替えを促すUI
    [SerializeField] private GameObject m_plzUseKnifeUI;

    [SerializeField] private DogManager m_dogManager;
    //笛
    [SerializeField] private GameObject m_dogWhistle;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("ナイフでゾンビを倒そう");
        m_tutorialManager.CreateMarker(m_targetZombieObj.transform.position);
    }

    public override void UpdatePhase()
    {
        if (m_inventoryWeapon == null) return;

        //インベントリを開いているときは邪魔になるので消す
        if (m_inventoryManager.m_inventoryState == INVENTORY.CHEST)
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseKnifeUI.SetActive(false);
        }
        //if ナイフを持っていないなら スロット切り替えを促す
        else if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.KNIFE)
        {
            m_plzChangeWeaponUI.SetActive(true);
            m_plzUseKnifeUI.SetActive(false);
        }
        //持っているなら 攻撃を促す
        else
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseKnifeUI.SetActive(true);

        }

        //ゾンビを倒したら
        //フェーズ移行
        if (m_targetZombieObj == null)
        {
            //犬が仲間になる
            m_dogManager.OnStopAction(false);
            //笛を持たせる
            m_inventoryWeapon.WeaponGet(m_dogWhistle);

            m_tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();

        m_plzChangeWeaponUI.SetActive(false);
        m_plzUseKnifeUI.SetActive(false);
    }
}

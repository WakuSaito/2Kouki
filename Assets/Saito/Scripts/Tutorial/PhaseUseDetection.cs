using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>探知使用フェイズ</para>
/// 探知の使い方を覚えてもらう
/// </summary>
public class PhaseUseDetection : TutorialBase
{
    //武器切り替えを促すUI
    [SerializeField] private GameObject m_plzChangeWeaponUI;
    //武器切り替えを促すUI
    [SerializeField] private GameObject m_plzUseDetectionUI;

    //インベントリ関連
    [SerializeField] private InventoryManager m_inventoryManager;
    [SerializeField] private InventoryWeapon m_inventoryWeapon;

    [SerializeField] private DogManager m_dogManager;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("犬に指示を出し\n家を調べよう");
    }

    public override void UpdatePhase()
    {
        //インベントリを開いているときは邪魔になるので消す
        if (m_inventoryManager.m_inventoryState == INVENTORY.ITEM ||
            m_inventoryManager.m_inventoryState == INVENTORY.CHEST)
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseDetectionUI.SetActive(false);
        }
        else if (m_inventoryWeapon.m_selectSlot != SLOT_ORDER.DOG)
        {
            //笛を持っていないなら 持たせるよう促す
            m_plzChangeWeaponUI.SetActive(true);
            m_plzUseDetectionUI.SetActive(false);
        }    
        else
        {
            //笛を持っているなら　使うよう促す
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseDetectionUI.SetActive(true);
        }

        //探知を使ったら
        if (m_dogManager.UsedOrderDetection())
        {
            //次のフェーズに進める
            m_tutorialManager.NextPhase();

        }
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();

        m_plzChangeWeaponUI.SetActive(false);
        m_plzUseDetectionUI.SetActive(false);
    }

}

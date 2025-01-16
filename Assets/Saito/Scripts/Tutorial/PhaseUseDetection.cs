using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseUseDetection : TutorialBase
{
    [SerializeField]//武器切り替えを促すUI
    private GameObject m_plzChangeWeaponUI;
    [SerializeField]//武器切り替えを促すUI
    private GameObject m_plzUseDetectionUI;

    private Inventory m_inventory;

    [SerializeField]
    private DogManager m_dogManager;

    public override void SetUpPhase()
    {
        m_inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        m_tutorialManager.SetText("犬に指示を出し\n家を調べよう");
    }

    public override void UpdatePhase()
    {
        //インベントリを開いているときは邪魔になるので消す
        if (m_inventory.item_inventory_flag == true)
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseDetectionUI.SetActive(false);
        }
        //if 笛を持っていないなら　持たせるよう促す
        else if (m_inventory.hand_weapon != Inventory.WEAPON_ID.DOG)
        {
            m_plzChangeWeaponUI.SetActive(true);
            m_plzUseDetectionUI.SetActive(false);
        }
        //持っているなら　使うよう促す
        else
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseDetectionUI.SetActive(true);
        }

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

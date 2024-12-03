using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseUseDetection : TutorialBase
{
    [SerializeField]//目標の座標
    private Vector3 TargetPos;

    [SerializeField]//武器切り替えを促すUI
    private GameObject plzChangeWeaponUI;
    [SerializeField]//武器切り替えを促すUI
    private GameObject plzUseDetectionUI;

    private Inventory inventory;

    [SerializeField]
    private DogManager dogManager;

    public override void SetUpPhase()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        tutorialManager.SetText("犬に指示を出し\n家を調べよう");
    }

    public override void UpdatePhase()
    {
        //インベントリを開いているときは邪魔になるので消す
        if (inventory.item_inventory_flag == true)
        {
            plzChangeWeaponUI.SetActive(false);
            plzUseDetectionUI.SetActive(false);
        }
        //if 笛を持っていないなら　持たせるよう促す
        else if (inventory.hand_weapon != Inventory.WEAPON_ID.DOG)
        {
            plzChangeWeaponUI.SetActive(true);
            plzUseDetectionUI.SetActive(false);
        }
        //持っているなら　使うよう促す
        else
        {
            plzChangeWeaponUI.SetActive(false);
            plzUseDetectionUI.SetActive(true);
        }

        if (dogManager.UsedOrderDetection())
        {
            //次のフェーズに進める
            tutorialManager.NextPhase();

        }
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();

        plzChangeWeaponUI.SetActive(false);
        plzUseDetectionUI.SetActive(false);
    }

}

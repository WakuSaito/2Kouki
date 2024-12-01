using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseUseKnife : TutorialBase
{
    [SerializeField]//自宅の座標
    private GameObject targetZombieObj;

    [SerializeField]//武器切り替えを促すUI
    private GameObject plzChangeWeaponUI;

    private Inventory inventory;

    public override void SetUpPhase()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        tutorialManager.SetText("ナイフでゾンビを倒そう");
        tutorialManager.CreateMarker(targetZombieObj.transform.position);
    }

    public override void UpdatePhase()
    {
        //if ナイフを持っていないなら スロット切り替えを促す
        if (inventory.hand_weapon != Inventory.WEAPON_ID.KNIFE)
        {
            plzChangeWeaponUI.SetActive(true);
        }
        //持っているなら 攻撃を促す
        else
        {
            plzChangeWeaponUI.SetActive(false);
        }

        //ゾンビを倒したら
        //フェーズ移行
        if (targetZombieObj == null)
            tutorialManager.NextPhase();
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();

        plzChangeWeaponUI.SetActive(false);
    }
}

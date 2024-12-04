using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseUseKnife : TutorialBase
{
    [SerializeField]//自宅の座標
    private GameObject targetZombieObj;

    [SerializeField]//武器切り替えを促すUI
    private GameObject plzChangeWeaponUI;
    [SerializeField]//武器切り替えを促すUI
    private GameObject plzUseKnifeUI;

    private Inventory inventory;

    [SerializeField]
    private DogManager dogManager;

    [SerializeField]//笛
    private GameObject dogWhistle;

    public override void SetUpPhase()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        tutorialManager.SetText("ナイフでゾンビを倒そう");
        tutorialManager.CreateMarker(targetZombieObj.transform.position);
    }

    public override void UpdatePhase()
    {
        if (inventory == null) return;

        //インベントリを開いているときは邪魔になるので消す
        if (inventory.item_inventory_flag == true)
        {
            plzChangeWeaponUI.SetActive(false);
            plzUseKnifeUI.SetActive(false);
        }
        //if ナイフを持っていないなら スロット切り替えを促す
        else if (inventory.hand_weapon != Inventory.WEAPON_ID.KNIFE)
        {
            plzChangeWeaponUI.SetActive(true);
            plzUseKnifeUI.SetActive(false);
        }
        //持っているなら 攻撃を促す
        else
        {
            plzChangeWeaponUI.SetActive(false);
            plzUseKnifeUI.SetActive(true);

        }

        //ゾンビを倒したら
        //フェーズ移行
        if (targetZombieObj == null)
        {
            //犬が仲間になる
            dogManager.OnStopAction(false);
            //笛を持たせる
            inventory.weapon_hand_obj[3] = dogWhistle;

            tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();

        plzChangeWeaponUI.SetActive(false);
        plzUseKnifeUI.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseExploreHouse : TutorialBase
{
    [SerializeField]//目標の座標
    private Vector3 TargetPos;

    [SerializeField]//武器切り替えを促すUI
    private GameObject plzChangeWeaponUI;

    private Inventory inventory;

    public override void SetUpPhase()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        tutorialManager.SetText("犬に指示を出し\n辺りを調べよう");
    }

    public override void UpdatePhase()
    {
        //if 笛を持っていないなら　持たせるよう促す
        if (inventory.hand_weapon != Inventory.WEAPON_ID.DOG)
        {
            plzChangeWeaponUI.SetActive(true);
        }
        //持っているなら　使うよう促す
        else
        {
            plzChangeWeaponUI.SetActive(false);
        }

        //if(指示を出したら)
        //DogManagerのクールダウンを監視

        tutorialManager.SetText("家の中を探索しよう");
        tutorialManager.CreateMarker(TargetPos);
        //プレイヤーと目標座標の距離が一定以下なら
        float distance = Vector3.Distance(TargetPos, PlayerPos());
        if (distance < 4.0f)
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
    }

}

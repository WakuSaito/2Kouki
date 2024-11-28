using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEatFood : TutorialBase
{

    public override void SetUpPhase()
    {
        tutorialManager.SetText("食料を食べよう");
    }

    public override void UpdatePhase()
    {
        //if バッグを開いていないなら　開くように促す
        //Inventory.item_inventory_flag == false

        //else 開いているなら　アイテムを使うよう促す

        //if 使ったら
        //インベントリサイズ監視

        tutorialManager.NextPhase();
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();
    }
}

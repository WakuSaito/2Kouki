using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEatFood : TutorialBase
{
    //インベントリスクリプト
    private Inventory inventory;

    //前フレームの所持している食料の数
    private int prevHaveFoodSum = 0;

    [SerializeField]//バッグを開けるよう指示するUI
    private GameObject plzOpenBagUI;

    [SerializeField]//アイテムを使うよう指示するUI
    private GameObject plzUseItemUI;

    //食料アイコンの位置
    private Vector2 foodIconPos;

    //debug用
    private bool bagOpen = false;

    public override void SetUpPhase()
    {
        //プレイヤーからInventory取得
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        //非表示
        plzOpenBagUI.SetActive(false);
        plzUseItemUI.SetActive(false);

        //アイコン位置取得
        foodIconPos = SerchFoodIcon();

        Debug.Log("pos:" + foodIconPos);

        //カーソルの終了位置変更
        plzUseItemUI.GetComponent<CursorAdvisorUI>().SetEndPos(foodIconPos);

        tutorialManager.SetText("食料を食べよう");
    }

    public override void UpdatePhase()
    {
        if (inventory == null) return;


        //UI nullチェック
        if (plzOpenBagUI != null && plzUseItemUI != null)
        {
            //バッグを開いていないなら　開くように促す
            if(inventory.item_inventory_flag == false)
            {
                plzOpenBagUI.SetActive(true);
                plzUseItemUI.SetActive(false);
                plzUseItemUI.GetComponent<CursorAdvisorUI>().StopMove();
            }
            //else 開いているなら　アイテムを使うよう促す
            else
            {
                plzOpenBagUI.SetActive(false);
                plzUseItemUI.SetActive(true);
                plzUseItemUI.GetComponent<CursorAdvisorUI>().StartMove();
            }
        }
        int foodSum = inventory.GetFoodItemSum();

        //食料の数が減っていれば、食べたとみなす　捨てるor交換出来るようになったらバグる
        if (foodSum < prevHaveFoodSum)
        {
            tutorialManager.NextPhase();
        }
        else
        {
            prevHaveFoodSum = foodSum;//スロット数記憶
        }
    }

    //食料アイコンを探す（1番始めに見つかったもののみ）
    private Vector2 SerchFoodIcon()
    {
        if (inventory == null) return Vector2.zero;

        for (int i = 0; i < inventory.item_type_id.Length; i++)
        {
            int id = inventory.item_type_id[i];
            if (id >= (int)ID.ITEM_ID.FOOD_1 &&
                id <= (int)ID.ITEM_ID.DRINK_2)
            {
                return inventory.GetItemIconPos(i);
            }
        }

        Debug.Log("食料アイコンが見つかりません");
        return Vector2.zero;
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();

        //UI非表示
        plzOpenBagUI.SetActive(false);
        plzUseItemUI.SetActive(false);
    }
}

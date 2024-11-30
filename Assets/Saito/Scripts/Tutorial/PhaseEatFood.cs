using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEatFood : TutorialBase
{
    //ui用
    private Transform canvas;

    //インベントリスクリプト
    private Inventory inventory;

    //前フレームの所持している食料の数
    private int prevHaveFoodSum = 0;

    [SerializeField]//バッグを開けるよう指示するUI
    private GameObject plzOpenBagUIPrefab;
    private GameObject plzOpenBagUI;

    [SerializeField]//アイテムを使うよう指示するUI
    private GameObject plzUseItemUIPrefab;
    private GameObject plzUseItemUI;
    
    public override void SetUpPhase()
    {
        //プレイヤーからInventory取得
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        //キャンバス取得
        canvas = GameObject.Find("Canvas").transform;
        //UI生成
        plzOpenBagUI = Instantiate(plzOpenBagUIPrefab, canvas);
        plzUseItemUI = Instantiate(plzUseItemUIPrefab, canvas);
        //非表示
        plzOpenBagUI.SetActive(false);
        plzUseItemUI.SetActive(false);

        tutorialManager.SetText("食料を食べよう");
    }

    public override void UpdatePhase()
    {
        if (inventory == null) return;

        //UI nullチェック
        if (plzOpenBagUI != null && plzUseItemUI != null)
        {
            //バッグを開いていないなら　開くように促す
            if (inventory.item_inventory_flag == false)
            {
                plzOpenBagUI.SetActive(true);
                plzUseItemUI.SetActive(false);
            }
            //else 開いているなら　アイテムを使うよう促す
            else
            {
                plzOpenBagUI.SetActive(false);
                plzUseItemUI.SetActive(true);
            }
        }


        //食料の数が減っていれば、食べたとみなす　捨てるor交換出来るようになったらバグる
        if(inventory.GetFoodItemSum() < prevHaveFoodSum)
        {
            tutorialManager.NextPhase();
        }
        else
        {
            prevHaveFoodSum = inventory.GetFoodItemSum();//スロット数記憶
        }  
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();

        //UI削除
        Destroy(plzOpenBagUI);
        Destroy(plzUseItemUI);
    }
}

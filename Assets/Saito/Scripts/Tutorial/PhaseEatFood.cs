using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>食料使用フェイズ</para>
/// 消費アイテムの使い方を教える
/// </summary>
//食料が場所によって使え無くなった可能性アリ
public class PhaseEatFood : TutorialBase
{
    //インベントリ管理スクリプト
    [SerializeField] private InventoryManager m_inventoryManager;
    //プレイヤーのインベントリスクリプト
    [SerializeField] private InventoryItem m_inventoryItem;
    //食料ゲージスクリプト
    [SerializeField] private Gauge m_foodGauge;

    //前フレームの食料ゲージの値
    private float m_prevFoodAmount;

    //バッグを開けるよう指示するUI
    [SerializeField] private GameObject m_plzOpenBagUI;
    //アイテムを使うよう指示するUI
    [SerializeField] private GameObject m_plzUseItemUI;

    //食料アイコンの位置
    private Vector2 m_foodIconPos;

    public override void SetUpPhase()
    {
        //非表示
        m_plzOpenBagUI.SetActive(false);
        m_plzUseItemUI.SetActive(false);

        //アイコン位置取得
        m_foodIconPos = SerchFoodIcon();

        Debug.Log("pos:" + m_foodIconPos);

        //カーソルの終了位置変更
        m_plzUseItemUI.GetComponent<CursorAdvisorUI>().SetEndPos(m_foodIconPos);

        //食料ゲージの値取得
        m_prevFoodAmount = m_foodGauge.GetCurrentAmount();

        m_tutorialManager.SetText("食料を食べよう");
    }

    public override void UpdatePhase()
    {
        if (m_inventoryManager == null) return;


        //UI nullチェック
        if (m_plzOpenBagUI != null && m_plzUseItemUI != null)
        {
            //バッグを開いていないなら　開くように促す
            if(m_inventoryManager.m_inventoryState != INVENTORY.ITEM)
            {
                m_plzOpenBagUI.SetActive(true);
                m_plzUseItemUI.SetActive(false);
                m_plzUseItemUI.GetComponent<CursorAdvisorUI>().StopMove();
            }
            //else 開いているなら　アイテムを使うよう促す
            else
            {
                m_plzOpenBagUI.SetActive(false);
                m_plzUseItemUI.SetActive(true);
                m_plzUseItemUI.GetComponent<CursorAdvisorUI>().StartMove(CursorAdvisorUI.ANIM_TYPE.CLICK);
            }
        }
        //現在の食料ゲージの値取得
        float food_amount = m_foodGauge.GetCurrentAmount();

        //食料ゲージの値が増えていたら、食べたとみなす　捨てるor交換出来るようになったらバグる
        if (m_prevFoodAmount < food_amount)
        {
            m_tutorialManager.NextPhase();
        }
        else
        {
            m_prevFoodAmount = food_amount;//食料ゲージの値記憶
        }

    }

    //食料アイコンを探す（1番始めに見つかったもののみ）
    private Vector2 SerchFoodIcon()
    {
        if (m_inventoryManager == null) return Vector2.zero;     

        for (int i = 0; i < m_inventoryItem.m_inventory.Slots.Length; i++)
        {
            if (m_inventoryItem.m_inventory.Slots[i].ItemInfo == null) continue;

            ITEM_ID id = m_inventoryItem.m_inventory.Slots[i].ItemInfo.id;
            if ((int)id >= (int)ID.ITEM_ID.FOOD_1 &&
                (int)id <= (int)ID.ITEM_ID.DRINK_2)
            {
                return m_inventoryItem.m_BoxTrans[i].position;
            }
        }

        Debug.Log("食料アイコンが見つかりません");
        return m_inventoryItem.m_BoxTrans[0].position;
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();

        //UI非表示
        m_plzOpenBagUI.SetActive(false);
        m_plzUseItemUI.SetActive(false);
    }
}

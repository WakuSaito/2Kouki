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
    //インベントリスクリプト
    private Inventory m_inventory;

    //前フレームの所持している食料の数
    private int m_prevHaveFoodSum = 0;

    [SerializeField]//バッグを開けるよう指示するUI
    private GameObject m_plzOpenBagUI;

    [SerializeField]//アイテムを使うよう指示するUI
    private GameObject m_plzUseItemUI;

    //食料アイコンの位置
    private Vector2 m_foodIconPos;

    public override void SetUpPhase()
    {
        //プレイヤーからInventory取得
        m_inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        //非表示
        m_plzOpenBagUI.SetActive(false);
        m_plzUseItemUI.SetActive(false);

        //アイコン位置取得
        m_foodIconPos = SerchFoodIcon();

        Debug.Log("pos:" + m_foodIconPos);

        //カーソルの終了位置変更
        m_plzUseItemUI.GetComponent<CursorAdvisorUI>().SetEndPos(m_foodIconPos);

        m_tutorialManager.SetText("食料を食べよう");
    }

    public override void UpdatePhase()
    {
        if (m_inventory == null) return;


        //UI nullチェック
        if (m_plzOpenBagUI != null && m_plzUseItemUI != null)
        {
            //バッグを開いていないなら　開くように促す
            if(m_inventory.item_inventory_flag == false)
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
                m_plzUseItemUI.GetComponent<CursorAdvisorUI>().StartMove();
            }
        }
        int food_sum = m_inventory.GetFoodItemSum();

        //食料の数が減っていれば、食べたとみなす　捨てるor交換出来るようになったらバグる
        if (food_sum < m_prevHaveFoodSum)
        {
            m_tutorialManager.NextPhase();
        }
        else
        {
            m_prevHaveFoodSum = food_sum;//スロット数記憶
        }
    }

    //食料アイコンを探す（1番始めに見つかったもののみ）
    private Vector2 SerchFoodIcon()
    {
        if (m_inventory == null) return Vector2.zero;

        for (int i = 0; i < m_inventory.item_type_id.Length; i++)
        {
            int id = m_inventory.item_type_id[i];
            if (id >= (int)ID.ITEM_ID.FOOD_1 &&
                id <= (int)ID.ITEM_ID.DRINK_2)
            {
                return m_inventory.GetItemIconPos(i);
            }
        }

        Debug.Log("食料アイコンが見つかりません");
        return Vector2.zero;
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

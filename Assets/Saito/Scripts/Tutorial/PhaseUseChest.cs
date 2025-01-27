using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>チェスト使用フェイズ</para>
/// チェストの使い方を知ってもらう
/// 最初からチェストに入っているアイテムを手に入れる形にしたい
/// </summary>
public class PhaseUseChest : TutorialBase
{
    //目標の座標
    [SerializeField] private Vector3 m_targetPos;

    //インベントリの状態監視用
    [SerializeField] private InventoryManager m_inventoryManager;
    //チェストのアイテム監視用
    [SerializeField] private InventoryChest m_inventoryChest;
    //アイテム移動先取得用
    [SerializeField] private InventoryItem m_inventoryItem;

    //アイテム移動を促すUI
    [SerializeField] private GameObject m_plzMoveItemUI;

    //前フレームのチェスト情報保存用
    private int m_prev_chest_used_slot;

    public override void SetUpPhase()
    {
        //インベントリの空き位置取得
        Vector2 inventory_slot_pos = GetInventoryEmptySlotPos();

        //カーソルの開始位置変更
        m_plzMoveItemUI.GetComponent<CursorAdvisorUI>().SetStartPos(m_inventoryChest.m_slotBoxTrans[0].position);
        //カーソルの終了位置変更
        m_plzMoveItemUI.GetComponent<CursorAdvisorUI>().SetEndPos(inventory_slot_pos);

        m_tutorialManager.SetText("チェストでアイテムを整理しよう");
        m_tutorialManager.CreateMarker(m_targetPos);
    }

    public override void UpdatePhase()
    {
        //チェストが開かれているなら
        if (m_inventoryManager.m_inventoryState == INVENTORY.CHEST)
        {
            m_plzMoveItemUI.SetActive(true);
            m_plzMoveItemUI.GetComponent<CursorAdvisorUI>().StartMove(CursorAdvisorUI.ANIM_TYPE.DRAG);//UIを動かす

            //チェストからアイテムが減っていたら
            int used_count = GetChestUsedSlotCount();
            if (used_count < m_prev_chest_used_slot)
            {
                //次のフェーズに進める
                m_tutorialManager.NextPhase();
            }
            else
            {
                m_prev_chest_used_slot = used_count;//情報保存
            }
        }
        else
        {
            m_plzMoveItemUI.SetActive(false);
            m_plzMoveItemUI.GetComponent<CursorAdvisorUI>().StopMove();//UIを止める
        }     

    }

    /// <summary>
    /// チェストの使われているスロット数取得
    /// </summary>
    /// <returns>使われているスロット数</returns>
    private int GetChestUsedSlotCount()
    {
        int used_count = 0;
        //チェストのスロットを全て調べる
        for (int i = 0; i < m_inventoryChest.m_inventory.Slots.Length; i++)
        {
            if (m_inventoryChest.m_inventory.Slots[i].ItemInfo == null) continue;

            //何かアイテムがあればカウント
            ITEM_ID id = m_inventoryChest.m_inventory.Slots[i].ItemInfo.id;
            if (id != ITEM_ID.NON)
            {
                used_count++;
            }
        }

        return used_count;
    }
    /// <summary>
    /// インベントリの使われていないスロットの位置取得
    /// </summary>
    private Vector2 GetInventoryEmptySlotPos()
    {
        //アイテムインベントリのスロットを全て調べる
        for (int i = 0; i < m_inventoryItem.m_inventory.Slots.Length; i++)
        {
            if (m_inventoryItem.m_inventory.Slots[i].ItemInfo == null) continue;

            //空きがあればそれを返す
            ITEM_ID id = m_inventoryItem.m_inventory.Slots[i].ItemInfo.id;
            if (id == ITEM_ID.NON)
            {
                return m_inventoryItem.m_BoxTrans[i].position;
            }
        }

        //空きが無ければ0番目を返す
        return m_inventoryItem.m_BoxTrans[0].position;
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();
    }
}

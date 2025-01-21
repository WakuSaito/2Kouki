using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>操作説明UI管理クラス</para>
/// 状態によって操作説明の表示を変更する
/// </summary>
public class HowToUIManager : MonoBehaviour
{
    [SerializeField]//移動
    private GameObject m_moveUI;
    [SerializeField]//武器切り替え
    private GameObject m_slotChangeUI;
    [SerializeField]//拾う 拾えるアイテムがある時に中央に配置してもいいかも
    private GameObject m_pickUpUI;

    [SerializeField]//攻撃（ナイフ）
    private GameObject m_attackUI;

    [SerializeField]//発砲
    private GameObject m_shotUI;
    [SerializeField]//リロード
    private GameObject m_reloadUI;

    [SerializeField]//攻撃指示
    private GameObject m_attackOrderUI;
    [SerializeField]//探知指示
    private GameObject m_detectOrderUI;

    [SerializeField]//アイテム使用
    private GameObject m_useItemUI;

    private Inventory m_inventory;
    [SerializeField]//犬の指示が可能か調べる用
    private DogManager m_dogManager;

    private bool m_isOpenBag = false;
    private Inventory.WEAPON_ID m_gripWeaponID;

    private void Awake()
    {
        m_inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    //インベントリ、犬の状態によって表示を変える
    //インベントリの形が変わったので変更する必要があるかも
    private void Update()
    {
        if (m_inventory == null) return;

        //状態が変わっていなければ
        if (m_isOpenBag == m_inventory.item_inventory_flag &&
            m_gripWeaponID == m_inventory.hand_weapon) return;

        //バッグが開いているか取得
        m_isOpenBag = m_inventory.item_inventory_flag;
        m_gripWeaponID = m_inventory.hand_weapon;

        HideUI();//全て非表示

        //バッグを開いている場合
        if (m_isOpenBag)
        {
            m_useItemUI.SetActive(true);
        }
        //閉じている場合
        else
        {
            m_moveUI.SetActive(true);
            m_slotChangeUI.SetActive(true);
            m_pickUpUI.SetActive(true);

            switch (m_inventory.hand_weapon)
            {
                case Inventory.WEAPON_ID.KNIFE:
                    m_attackUI.SetActive(true);
                    break;
                case Inventory.WEAPON_ID.PISTOL:
                    m_shotUI.SetActive(true);
                    m_reloadUI.SetActive(true);
                    break;
                case Inventory.WEAPON_ID.DOG:
                    m_attackOrderUI.SetActive(true);
                    m_detectOrderUI.SetActive(true);
                    break;
            }
        }


        if (m_dogManager == null) return;

        //犬が行動可能かによってUIの透明度変更
        if(m_dogManager.CanOrderAttack())
            m_attackOrderUI.GetComponent<CanvasGroup>().alpha = 1f;
        else
            m_attackOrderUI.GetComponent<CanvasGroup>().alpha = 0.5f;

        if (m_dogManager.CanOrderDetection())
            m_detectOrderUI.GetComponent<CanvasGroup>().alpha = 1f;
        else
            m_detectOrderUI.GetComponent<CanvasGroup>().alpha = 0.5f;
    }

    /// <summary>
    /// UIの一括非表示
    /// </summary>
    private void HideUI()
    {
        m_moveUI.SetActive(false);
        m_slotChangeUI.SetActive(false);
        m_pickUpUI.SetActive(false);
        m_attackUI.SetActive(false);
        m_shotUI.SetActive(false);
        m_reloadUI.SetActive(false);
        m_attackOrderUI.SetActive(false);
        m_detectOrderUI.SetActive(false);
        m_useItemUI.SetActive(false);
    }
}

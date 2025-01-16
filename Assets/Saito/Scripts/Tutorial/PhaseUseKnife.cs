using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseUseKnife : TutorialBase
{
    [SerializeField]//自宅の座標
    private GameObject m_targetZombieObj;

    [SerializeField]//武器切り替えを促すUI
    private GameObject m_plzChangeWeaponUI;
    [SerializeField]//武器切り替えを促すUI
    private GameObject m_plzUseKnifeUI;

    private Inventory m_inventory;

    private WeaponInventory m_weaponInventory;

    [SerializeField]
    private DogManager m_dogManager;

    [SerializeField]//笛
    private GameObject m_dogWhistle;

    public override void SetUpPhase()
    {
        m_inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        //weaponInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<weaponInventory>();

        m_tutorialManager.SetText("ナイフでゾンビを倒そう");
        m_tutorialManager.CreateMarker(m_targetZombieObj.transform.position);
    }

    public override void UpdatePhase()
    {
        if (m_inventory == null) return;

        //インベントリを開いているときは邪魔になるので消す
        if (m_inventory.item_inventory_flag == true)
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseKnifeUI.SetActive(false);
        }
        //if ナイフを持っていないなら スロット切り替えを促す
        else if (m_inventory.hand_weapon != Inventory.WEAPON_ID.KNIFE)
        {
            m_plzChangeWeaponUI.SetActive(true);
            m_plzUseKnifeUI.SetActive(false);
        }
        //持っているなら 攻撃を促す
        else
        {
            m_plzChangeWeaponUI.SetActive(false);
            m_plzUseKnifeUI.SetActive(true);

        }

        //ゾンビを倒したら
        //フェーズ移行
        if (m_targetZombieObj == null)
        {
            //犬が仲間になる
            m_dogManager.OnStopAction(false);
            //笛を持たせる
            m_inventory.weapon_hand_obj[3] = m_dogWhistle;
            //weaponInventory.weapon[3] = dogWhistle;

            m_tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
        m_tutorialManager.DeleteMarker();

        m_plzChangeWeaponUI.SetActive(false);
        m_plzUseKnifeUI.SetActive(false);
    }
}

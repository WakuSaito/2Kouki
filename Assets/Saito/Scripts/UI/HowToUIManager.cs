using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToUIManager : MonoBehaviour
{
    [SerializeField]//移動
    private GameObject moveUI;
    [SerializeField]//武器切り替え
    private GameObject slotChangeUI;
    [SerializeField]//拾う 拾えるアイテムがある時に中央に配置してもいいかも
    private GameObject pickUpUI;

    [SerializeField]//攻撃（ナイフ）
    private GameObject attackUI;

    [SerializeField]//発砲
    private GameObject shotUI;
    [SerializeField]//リロード
    private GameObject reloadUI;

    [SerializeField]//攻撃指示
    private GameObject attackOrderUI;
    [SerializeField]//探知指示
    private GameObject detectOrderUI;

    [SerializeField]//アイテム使用
    private GameObject useItemUI;

    private Inventory inventory;
    [SerializeField]//犬の指示が可能か調べる用
    private DogManager dogManager;

    private bool isOpenBag = false;
    private Inventory.WEAPON_ID gripWeaponID;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void Update()
    {
        if (inventory == null) return;

        //状態が変わっていなければ
        if (isOpenBag == inventory.item_inventory_flag &&
            gripWeaponID == inventory.hand_weapon) return;

        //バッグが開いているか取得
        isOpenBag = inventory.item_inventory_flag;
        gripWeaponID = inventory.hand_weapon;

        HideUI();//全て非表示

        //バッグを開いている場合
        if (isOpenBag)
        {
            useItemUI.SetActive(true);
        }
        //閉じている場合
        else
        {
            moveUI.SetActive(true);
            slotChangeUI.SetActive(true);
            pickUpUI.SetActive(true);

            switch (inventory.hand_weapon)
            {
                case Inventory.WEAPON_ID.KNIFE:
                    attackUI.SetActive(true);
                    break;
                case Inventory.WEAPON_ID.PISTOL:
                    shotUI.SetActive(true);
                    reloadUI.SetActive(true);
                    break;
                case Inventory.WEAPON_ID.DOG:
                    attackOrderUI.SetActive(true);
                    detectOrderUI.SetActive(true);
                    break;
            }
        }


        if (dogManager == null) return;

        //犬が行動可能かによってUIの透明度変更
        if(dogManager.CanOrderAttack())
            attackOrderUI.GetComponent<CanvasGroup>().alpha = 1f;
        else
            attackOrderUI.GetComponent<CanvasGroup>().alpha = 0.5f;

        if (dogManager.CanOrderDetection())
            detectOrderUI.GetComponent<CanvasGroup>().alpha = 1f;
        else
            detectOrderUI.GetComponent<CanvasGroup>().alpha = 0.5f;


    }

    //全て非表示にする
    private void HideUI()
    {
        moveUI.SetActive(false);
        slotChangeUI.SetActive(false);
        pickUpUI.SetActive(false);
        attackUI.SetActive(false);
        shotUI.SetActive(false);
        reloadUI.SetActive(false);
        attackOrderUI.SetActive(false);
        detectOrderUI.SetActive(false);
        useItemUI.SetActive(false);
    }
}

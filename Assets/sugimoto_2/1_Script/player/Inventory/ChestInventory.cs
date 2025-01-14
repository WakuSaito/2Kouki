using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 できること
 ・アイテム保存
 */

public class ChestInventory : MonoBehaviour
{
    public InventoryClass InventoryClass;

    public InventoryTest Inventory;
    public int sloat_size = 10;
    public Transform[] sprite_pos;
    public Text[] text;
    public Transform[] slot_box;

    [SerializeField] Sprite test_sprite;

    [SerializeField] GameObject ChestUI;

    //プレイヤー
    player Player;
    [SerializeField] GameObject player_obj;
    ItemInventory ItemInventory;
    [SerializeField] GameObject ItemInventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryTest(sloat_size, slot_box);
        Player = player_obj.GetComponent<player>();
        Inventory.Sloats[0].ItemInfo = new ItemInformation(ITEM_TYPE.FOOD,ITEM_ID.FOOD_1,1,1, test_sprite);

        //ItemInventory = ItemInventoryUI.GetComponent<ItemInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.inventory_status == INVENTORY.CHEST)
        {
            Inventory.SetUI();
            if (Input.GetKeyDown(KeyCode.E)/* || Input.GetKeyDown(KeyCode.Escape)*/)
            {
                CloseUI();
            }
        }
    }

    public void OpenUI()
    {
        ChestUI.SetActive(true);
        Player.inventory_status = INVENTORY.CHEST;
    }

    void CloseUI()
    {
        ChestUI.SetActive(false);
        Player.inventory_status = INVENTORY.NON;
    }

    void Test()
    {
        Inventory.Sloats[0].ItemInfo = new ItemInformation();
        Inventory.Sloats[0].ItemInfo.id = ITEM_ID.EMERGENCY_PACK;
    }
}

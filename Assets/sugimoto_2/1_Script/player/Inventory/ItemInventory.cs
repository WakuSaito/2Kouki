using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{
    InventoryTest Inventory;
    [SerializeField] int sloat_size = 10;
    [SerializeField] Transform[] sprite_pos;
    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryTest(sloat_size);
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ用: インベントリの内容を表示（例えば、Dキーを押したとき）
        if (Input.GetKeyDown(KeyCode.D)) { Inventory.PrintInventory(); }
    }

    //インベントリに入れる
    public bool AddItemInventory(ItemInformation _iteminfo)
    {
        bool add_flag = Inventory.AddItemInventory(_iteminfo);

        return add_flag;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemInventory : MonoBehaviour
{
    public InventoryTest Inventory;
    [SerializeField] int sloat_size = 10;
    [SerializeField] Transform[] sprite_pos;
    [SerializeField] Text[] text;

    GameObject catch_obj;

    public bool item_inventory_flag = false;

    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryTest(sloat_size, sprite_pos, text);
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ用: インベントリの内容を表示（例えば、Dキーを押したとき）
        if (Input.GetKeyDown(KeyCode.D)) { Inventory.PrintInventory(); }

        if (item_inventory_flag)
        {
            CheckInventoryItem();
        }
    }
    public void InventoryOpenOrClose()
    {
        //インベントリ開閉

        if(item_inventory_flag)
        {
            Screen.lockCursor = true;
            item_inventory_flag = false;
            gameObject.SetActive(item_inventory_flag);
        }
        else
        {
            Screen.lockCursor = false;
            item_inventory_flag = true;
            gameObject.SetActive(item_inventory_flag);
        }

    }

    public void CheckInventoryItem()    //カーソルのあっているアイテムを調べる
    {
        //マウスの位置からUIを取得する
        //RaycastAllの引数（PointerEventData）作成
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        //RaycastAllの結果格納用List
        List<RaycastResult> RayResult = new List<RaycastResult>();
        Debug.Log(pointData);

        //PointerEventDataにマウスの位置をセット
        pointData.position = Input.mousePosition;

        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointData, RayResult);

        foreach (RaycastResult result in RayResult)
        {
            Debug.Log(result.gameObject);

            //スロットアイテム掴む
            for (int sloat = 0; sloat < sloat_size; sloat++)
            {
                if (result.gameObject == Inventory.Sloats[sloat].Set_Pos.gameObject)
                {
                    catch_obj = result.gameObject;
                    break;
                }
            }
            //foreach(var sloat in Inventory.Sloats)
            //{
            //    catch_obj = sloat.InMouseSloat(result.gameObject);
            //}
        }
    }

}

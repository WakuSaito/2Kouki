using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Item Item;

    public const int INVENTORY_MAX = 10;
    const int ITEM_MAX = 30;
    
    //アイテムの数保存
    public int[] item_num = new int[INVENTORY_MAX] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //アイテムの種類保存
    public int[] item_type_id = new int[INVENTORY_MAX] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemGet(GameObject _item)
    {
        //アイテムスクリプトからアイテムのID取得
        int item_id = (int)_item.GetComponent<Item>().id;

        //アイテムがピストルだった場合のみ取得するアイテム変更
        if (item_id == (int)Item.ITEM_ID.PISTOL)
        {
            item_id = (int)Item.ITEM_ID.BULLET;
        }

        //取得可能なアイテムの数
        int get_num = _item.GetComponent<Item>().get_num[item_id];

        while (get_num != 0)
        {

            for (int i = 0; i < INVENTORY_MAX; i++)
            {
                //インベントリのアイテム欄が空白(-1)または同じIDだったら
                if (item_type_id[i] == -1 || item_type_id[i] == item_id)
                {
                    if (item_type_id[i] == -1)
                    {
                        item_type_id[i] = item_id;
                    }

                    int get_max = get_num;
                    for (int cnt = 1; cnt <= get_max; cnt++)
                    {
                        //アイテム数がMaxじゃなければ
                        if (item_num[i] == ITEM_MAX)
                        {
                            break;
                        }
                        else
                        {
                            item_num[i]++;
                            get_num--;
                        }
                    }
                }

                //取得可能なアイテム数がなくなれば終了
                if (get_num <= 0)
                    break;
            }

            //インベントリを最後まで見たら獲得可能なアイテムを0にする
            get_num = 0;
        }

        //確認用
        //for (int i = 0; i <INVENTORY_MAX;i++)
        //{
        //    Debug.Log(item_num[i]);
        //    Debug.Log(item_type_id[i]);
        //}
    }


}

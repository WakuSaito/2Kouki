using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    //アイテムの種類の最大数
    const int ITEM_TYPE_MAX = 2;

    //獲得できるアイテム数ID順
    public int[] get_num = new int[ITEM_TYPE_MAX];
    //インベントリに表示するオブジェクトID順
    public GameObject[] item_obj = new GameObject[ITEM_TYPE_MAX];

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

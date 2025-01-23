using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSet_ID : ID
{
    //ID保存用
    public ITEM_ID id;
    public int get_num = 0;
    public int get_max = 0;

    // Start is called before the first frame update
    void Awake()
    {
        switch (id)
        {
            case ITEM_ID.PISTOL:
                break;
            case ITEM_ID.BULLET:
                get_num = 10;
                get_max = 30;
                break;
            //基本取得数は1個、スタック可能数1個
            default:
                get_num = 1;
                get_max = 1;
                break;
        }

    }


}

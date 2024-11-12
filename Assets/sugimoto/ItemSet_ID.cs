using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSet_ID : ID
{
    public int get_num = 0;
    //ID•Û‘¶—p
    public ITEM_ID id;

    // Start is called before the first frame update
    void Start()
    {
        switch (gameObject.tag)
        {
            case "pistol":
                id = ITEM_ID.PISTOL;
                break;
            case "bullet":
                id = ITEM_ID.BULLET;
                get_num = 10;
                break;
        }

    }
}

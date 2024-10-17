using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    const int ITEM_TYPE_MAX = 2;

    public enum ITEM_ID
    {
        PISTOL,
        BULLET,
    }

    public ITEM_ID id;
    public int[] get_num = new int[ITEM_TYPE_MAX] { 0, 0 };

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
                get_num[(int)id] = 10;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    public ItemInformation iteminfo;
    ItemInventory ItemInventory;

    //public ITEM_ID id = ITEM_ID.NON;

    // Start is called before the first frame update
    void Start()
    {
        iteminfo.stack_max = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemoveItem(ItemInformation _item)
    {
        Debug.Log(_item.type);
        Debug.Log(_item.id);
        Debug.Log(_item.get_num);
        Debug.Log(_item.stack_max);
    }

}

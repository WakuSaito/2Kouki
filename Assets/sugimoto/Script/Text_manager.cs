using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_manager : MonoBehaviour
{
    Inventory Inventory;
    [SerializeField] GameObject player_obj;
    [SerializeField] Text bullet_text;

    // Start is called before the first frame update
    void Start()
    {
        Inventory = player_obj.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Inventory.PistolBulletNum());
        bullet_text.text = Inventory.PistolBulletNum() + "Å^" + Inventory.InventoryBulletNum();
    }

    //void TextChange()
    //{
    //}

}

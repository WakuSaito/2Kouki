using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_manager : MonoBehaviour
{
    Inventory Inventory;
    [SerializeField] GameObject player_obj;
    [SerializeField] Text pistol_bullet_text;
    [SerializeField] Text inventory_bullet_text;

    // Start is called before the first frame update
    void Start()
    {
        Inventory = player_obj.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        TextChange();
    }

    void TextChange()
    {
        pistol_bullet_text.text = Inventory.PistolBulletNum() + " / Pistol ";
        inventory_bullet_text.text = Inventory.InventoryBulletNum() + " / InventoryBullet ";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    const int BULLET_MAX = 10;

    Inventory Inventory;



    [SerializeField] GameObject player_obj;

    //Ç‡Ç∆Ç‡Ç∆ÇÃíeêî
    public int bullet_num = 10;

    // Start is called before the first frame update
    void Start()
    {
        Inventory = player_obj.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player_obj.GetComponent<player>().hand_item == player.HAND.LONG_WEAPON)
        {
            if (Input.GetKeyDown(KeyCode.R) && bullet_num <= BULLET_MAX)
            {
                for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
                {
                    if (Inventory.item_type_id[i] == (int)Item.ITEM_ID.BULLET)
                    {
                        int reload_num = BULLET_MAX - bullet_num;
                        Debug.Log(reload_num);

                        //forï™Ç≈âÒÇ≥Ç»Ç¢Ç∆àÍãCÇ…å∏ÇÁÇ∑Ç∆ÉoÉOÇÈ

                        Inventory.item_num[i] -= reload_num;

                        if(Inventory.item_num[i]==0)
                        {
                            Inventory.item_type_id[i] = -1;
                        }

                        bullet_num += reload_num;
                        break;
                    }
                }
            }
        }

    }
}

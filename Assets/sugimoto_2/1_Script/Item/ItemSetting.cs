using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    public ItemInformation iteminfo;
    [SerializeField] ITEM_ID id;
    [SerializeField] Sprite sprite;

    public bool drop_flag = false;
    float delete_timer = 0.0f;
    public bool m_getFlag = false;
    public bool m_tutorialFlag = false;

    //Start is called before the first frame update
    void Awake()
    {
        ItemSet();
    }

    public void ItemSet()
    {
        switch (id)
        {
            //コンストラクタ(種類、ID、取得可能数、スタック可能数、画像、回復数)
            case ITEM_ID.FOOD_1:
            case ITEM_ID.FOOD_2:
            case ITEM_ID.FOOD_3:
            case ITEM_ID.FOOD_4:
                iteminfo = new ItemInformation(ITEM_TYPE.FOOD, id, 1, 1, sprite, 10);
                break;
            case ITEM_ID.DRINK_1:
            case ITEM_ID.DRINK_2:
                iteminfo = new ItemInformation(ITEM_TYPE.FOOD, id, 1, 1, sprite, 5);
                break;
            case ITEM_ID.EMERGENCY_PACK:
                iteminfo = new ItemInformation(ITEM_TYPE.RECOVERY, id, 1, 1, sprite, 3);
                break;
            //コンストラクタ(種類、ID、取得可能数、スタック可能数、画像)
            case ITEM_ID.BULLET:
                iteminfo = new ItemInformation(ITEM_TYPE.BULLET, id, 10, 30, sprite);
                break;
            case ITEM_ID.PISTOL:
            case ITEM_ID.ASSAULT:
            case ITEM_ID.SHOTGUN:
            case ITEM_ID.HAND:
            case ITEM_ID.KNIFE:
            case ITEM_ID.DOG_DIRECTION:
                iteminfo = new ItemInformation(ITEM_TYPE.WEAPON, id, 1, 1, sprite, gameObject);
                break;
        }
    }

    private void Update()
    {
        //アイテムドロップされたら時間経過で消す
        if (drop_flag)
        {
            delete_timer += Time.deltaTime;

            if (delete_timer >= 10)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (delete_timer > 0)
            {
                delete_timer = 0;
            }
        }
    }
}
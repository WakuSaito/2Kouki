using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*  できること
 ・インベントリ開閉
 ・インベントリを調べる
 ・各ゲージ回復処理
 ・アイテム移動
 ・スロットの管理
 ・同じアイテム同士、スタック上限じゃなければスタックする
 */


public class ItemInventory : MonoBehaviour
{
    public InventoryTest Inventory;
    [SerializeField] int sloat_size = 10;
    [SerializeField] Transform[] sprite_pos;
    [SerializeField] Text[] text;
    [SerializeField] GameObject[] sloat_box;

    GameObject catch_obj;
    int catch_sloat_num;
    GameObject select_item;
    int select_sloat_num;
    GameObject hit_box;
    bool drag_flag = false;

    public bool item_inventory_flag = false;

    //ゲージ
    [SerializeField] GameObject food_gauge_obj; //食料
    [SerializeField] GameObject hp_gauge_obj;   //体力


    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryTest(sloat_size, sprite_pos, text);
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ用: インベントリの内容を表示
        if (Input.GetKeyDown(KeyCode.D)) { Inventory.PrintInventory(); }

        if (item_inventory_flag)
        {
            CheckInventoryItem();
            ItemCatch();
        }
    }
    public void InventoryOpenOrClose()
    {
        //インベントリ開閉

        if(item_inventory_flag)
        {
            Screen.lockCursor = true;
            item_inventory_flag = false;
            gameObject.SetActive(item_inventory_flag);
        }
        else
        {
            Screen.lockCursor = false;
            item_inventory_flag = true;
            gameObject.SetActive(item_inventory_flag);
        }

    }

    public void CheckInventoryItem()    //カーソルのあっているアイテムを調べる
    {
        select_item = null;

        //マウスの位置からUIを取得する
        //RaycastAllの引数（PointerEventData）作成
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        //RaycastAllの結果格納用List
        List<RaycastResult> RayResult = new List<RaycastResult>();

        //PointerEventDataにマウスの位置をセット
        pointData.position = Input.mousePosition;

        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointData, RayResult);

        foreach (RaycastResult result in RayResult)
        {
            int sloat_num = 0;

            //カーソルがあっているアイテム
            foreach (var sloat in Inventory.Sloats)
            {
                if (result.gameObject == sloat.Set_Pos.gameObject)
                {
                    select_item = result.gameObject;
                    select_sloat_num = sloat_num;
                    break;
                }
                sloat_num++;
            }

            //カーソルがあっているスロットのbox
            foreach (var box in sloat_box)
            {
                if (result.gameObject == box)
                {
                    hit_box = box;
                    break;
                }
            }
        }
    }

    public void Recovery_Gage()
    {
        if (select_item != null)
        {
            ITEM_ID _id = Inventory.Sloats[select_sloat_num].ItemInfo.id;

            //食料
            {
                if (_id >= ITEM_ID.FOOD_1 && _id <= ITEM_ID.FOOD_4)
                {
                    food_gauge_obj.GetComponent<Gauge>().Increase_Gauge(10);
                    //playerSound.PlayEat();//SE
                }
                if (_id >= ITEM_ID.DRINK_1 && _id <= ITEM_ID.DRINK_2)
                {
                    food_gauge_obj.GetComponent<Gauge>().Increase_Gauge(5);
                    //playerSound.PlayDrink();//SE
                }
            }

            //体力
            {
                if (_id >= ITEM_ID.EMERGENCY_PACK)
                {
                    hp_gauge_obj.GetComponent<Gauge>().Increase_Gauge(3);
                    //playerSound.PlayHeal();//SE
                }
            }
            Inventory.Sloats[select_sloat_num].UseItem();
        }

    }

    void ItemCatch()
    {
        //つかんだオブジェクトを保存
        if(Input.GetMouseButtonDown(0)&& select_item != null)
        {
            catch_obj = select_item;
            catch_sloat_num = select_sloat_num;
        }

        if (catch_obj == null) return;

        if (Input.GetMouseButton(0)) 
        {
            catch_obj.transform.position = Input.mousePosition;
            drag_flag = true;
        }
        else
        {
            if (hit_box == null)
            {
                //元の位置に戻す
                catch_obj.transform.position = sloat_box[catch_sloat_num].transform.position;
                catch_obj = null;
                drag_flag = false;
            }
            else
            {
                //移動先を調べる //バグ：アイテム情報がある場合は移動できないようにする
                for (int sloat = 0; sloat < sloat_size; sloat++)
                {
                    if (hit_box == sloat_box[sloat])
                    {
                        //入れたい場所のスロットがいっぱいじゃなければ入れる
                        if (Inventory.Sloats[sloat].ItemInfo == null)
                        {
                            //中身を入れ替える
                            Inventory.ItemSloatChange(catch_sloat_num, sloat);
                            //設置
                            catch_obj.transform.position = hit_box.transform.position;

                            Inventory.Sloats[catch_sloat_num].CrearSloat();
                        }
                        else
                        {
                            if (Inventory.AddItemInventory(Inventory.Sloats[catch_sloat_num].ItemInfo))
                            {
                                //中身を入れ替える
                                Inventory.ItemSloatChange(catch_sloat_num, sloat);
                            }
                            else
                            {
                                catch_obj.transform.position = Inventory.Sloats[catch_sloat_num].Start_Pos;
                            }
                        }
                        catch_obj = null;
                        drag_flag = false;
                        break;
                    }
                }

            }
        }
    }
}

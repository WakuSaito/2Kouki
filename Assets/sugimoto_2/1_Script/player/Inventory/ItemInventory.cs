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
    [SerializeField] Transform[] sloat_box;

    GameObject catch_obj;
    int catch_sloat_num;
    GameObject select_item;
    int select_sloat_num;
    GameObject hit_box;
    int hit_box_num;
    GameObject hit_weapon_box;
    int hit_weapon_num;

    public bool item_inventory_flag = false;

    //ゲージ
    [SerializeField] GameObject food_gauge_obj; //食料
    [SerializeField] GameObject hp_gauge_obj;   //体力

    player Player;
    [SerializeField] GameObject player_obj;


    // Start is called before the first frame update
    void Start()
    {
        Inventory = new InventoryTest(sloat_size, sloat_box);
        Player = player_obj.GetComponent<player>();
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
            Inventory.SetUI();
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
        hit_box = null;
        hit_weapon_box = null;
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
            //カーソルがあっているアイテム
            for (int cnt = 0; cnt < sloat_box.Length; cnt++)
            {
                if (result.gameObject == sloat_box[cnt].GetChild(0).gameObject)
                {
                    select_item = result.gameObject;
                    select_sloat_num = cnt;
                    break;
                }
            }

            //カーソルがあっているスロットのbox
            for (int cnt = 0; cnt < sloat_box.Length; cnt++)
            {
                if (result.gameObject == sloat_box[cnt].gameObject)
                {
                    hit_box = sloat_box[cnt].gameObject;
                    hit_box_num = cnt;
                    break;
                }
            }

            //武器インベントリ
            for (int cnt = 0; cnt < Player.WeaponInventory.Inventory.Sloat_Box.Length; cnt++)
            {
                if (result.gameObject == Player.WeaponInventory.Inventory.Sloat_Box[cnt].gameObject)
                {
                    hit_weapon_box = Player.WeaponInventory.Inventory.Sloat_Box[cnt].gameObject;
                    hit_weapon_num = cnt;
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

            if (_id >= ITEM_ID.FOOD_1 && _id <= ITEM_ID.EMERGENCY_PACK)
            {
                int recovery_num = Inventory.Sloats[select_sloat_num].ItemInfo.recoveryitem_info.recovery_num;

                //食料
                {
                    if (_id >= ITEM_ID.FOOD_1 && _id <= ITEM_ID.FOOD_4)
                    {
                        food_gauge_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);
                        //playerSound.PlayEat();//SE
                    }
                    if (_id >= ITEM_ID.DRINK_1 && _id <= ITEM_ID.DRINK_2)
                    {
                        food_gauge_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);
                        //playerSound.PlayDrink();//SE
                    }
                }

                //体力
                {
                    if (_id >= ITEM_ID.EMERGENCY_PACK)
                    {
                        hp_gauge_obj.GetComponent<Gauge>().Increase_Gauge(recovery_num);
                        //playerSound.PlayHeal();//SE
                    }
                }
                Inventory.Sloats[select_sloat_num].UseItem();
            }
        }

    }

    public bool CheckInBullet()
    {
        for (int sloat = 0; sloat < sloat_size; sloat++) 
        {
            if (Inventory.Sloats[sloat].ItemInfo == null) continue;

            if (Inventory.Sloats[sloat].ItemInfo.id == ITEM_ID.BULLET)
            {
                return true;
            }
        }

        return false;
    }

    public int SubBullet(int _amount)
    {
        int addAmount = _amount;
        int add_num = 0;

        for (int sloat = 0; sloat < sloat_size; sloat++)
        {
            if (Inventory.Sloats[sloat].ItemInfo == null) continue;

            if (Inventory.Sloats[sloat].ItemInfo.id == ITEM_ID.BULLET)
            {
                int bullet_num = Inventory.Sloats[sloat].ItemInfo.get_num;

                if (bullet_num > addAmount)
                {
                    Inventory.Sloats[sloat].ItemInfo.get_num -= addAmount;
                    return addAmount + add_num;
                }
                else
                {
                    addAmount -= Inventory.Sloats[sloat].ItemInfo.get_num;
                    add_num += Inventory.Sloats[sloat].ItemInfo.get_num;
                    Debug.Log(addAmount);
                    Inventory.Sloats[sloat].ItemInfo.get_num = 0;
                    if (Inventory.Sloats[sloat].IsEmpty())
                    {
                        Inventory.Sloats[sloat].CrearSloat();
                    }
                }
            }

        }

        return _amount - addAmount;
    }

    void ItemCatch()
    {
        //つかんだオブジェクトを保存
        if (Input.GetMouseButtonDown(0) && select_item != null)
        {
            catch_obj = select_item;
            catch_sloat_num = select_sloat_num;
        }

        if (catch_obj == null) return;

        Debug.Log(catch_obj);

        if (Input.GetMouseButton(0)) 
        {
            catch_obj.transform.position = Input.mousePosition;
        }
        else
        {
            if (hit_weapon_box != null)
            {
                if (Inventory.Sloats[catch_sloat_num].ItemInfo.id >= ITEM_ID.PISTOL && Inventory.Sloats[catch_sloat_num].ItemInfo.id <= ITEM_ID.SHOTGUN)
                {
                    //現在の銃を非表示
                    Player.WeaponInventory.Inventory.Sloats[(int)WeaponInventory.Sloat_Order.GUN].ItemInfo.weaponitem_info.weapon_obj.SetActive(false);
                    //スロットの中身を入れ替える
                    Inventory.ItemSloatChange(Player.WeaponInventory, catch_sloat_num);
                    //掴んでいるオブジェクトの位置を当たった武器スロットの位置へ
                    catch_obj.transform.position = hit_weapon_box.transform.position;
                    //入れ替えた武器を表示
                    Player.WeaponInventory.Inventory.Sloats[(int)WeaponInventory.Sloat_Order.GUN].ItemInfo.weaponitem_info.weapon_obj.SetActive(true);
                    //武器インベントリの武器を入れ替えた武器に変更
                    Player.WeaponInventory.weapon[(int)WeaponInventory.Sloat_Order.GUN] = Player.WeaponInventory.Inventory.Sloats[(int)WeaponInventory.Sloat_Order.GUN].ItemInfo.weaponitem_info.weapon_obj;
                    //catch_obj = Player.WeaponInventory.Inventory.Sloat_Box[(int)WeaponInventory.Sloat_Order.GUN].GetChild(0).gameObject;
                    catch_obj.transform.position = sloat_box[catch_sloat_num].position;
                }
                else
                {
                    catch_obj.transform.position = sloat_box[catch_sloat_num].position;
                }

                hit_weapon_box = null;
            }
            if (hit_box != null)
            {
                //移動先を調べる
                //入れたい場所のスロットが空
                if (Inventory.Sloats[hit_box_num].ItemInfo == null)
                {
                    Debug.Log("a1");
                    //中身を入れ替える
                    Inventory.ItemSloatChange(catch_sloat_num, hit_box_num);
                    //設置
                    catch_obj.transform.position
                        = sloat_box[catch_sloat_num].transform.position;
                    //移動前のスロット初期化
                    Inventory.Sloats[catch_sloat_num].CrearSloat();
                }
                else
                {
                    //スロットアイテム＋スロットアイテム
                    if (Inventory.AddSloatSloatInventory(catch_sloat_num, hit_box_num))
                    {
                        Debug.Log("a2");

                        //設置
                        catch_obj.transform.position = sloat_box[hit_box_num].transform.position;
                    }
                    else
                    {
                        catch_obj.transform.position = Inventory.Sloat_Box[catch_sloat_num].position;
                    }
                }

                hit_box = null;
                catch_obj = null;
            }

            if (catch_obj != null) 
            {
                //元の位置に戻す
                catch_obj.transform.position = sloat_box[catch_sloat_num].transform.position;
                catch_obj = null;
            }
        }
    }
}

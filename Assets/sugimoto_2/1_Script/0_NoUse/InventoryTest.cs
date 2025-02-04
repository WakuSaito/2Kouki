using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*  できること
 ・アイテムをスロットに入れる
 ・アイテム移動
 ・スロットの管理
 ・同じアイテム同士、スタック上限じゃなければスタックする
 */


[System.Serializable]
public class InventoryTest
{
    public InventorySloat[] Sloats { get; set; } //スロット情報

    public Transform[] Sloat_Box { get; set; }  //スロットの位置

    public InventoryTest(int _sloat_size,Transform[] _sloat_box)//コンストラクタ
    {
        //サイズ分の配列作成
        Sloats = new InventorySloat[_sloat_size];
        Sloat_Box = new Transform[_sloat_size];

        //スロット内初期化初期化
        for (int sloat = 0; sloat < _sloat_size; sloat++)
        {
            Sloat_Box[sloat] = _sloat_box[sloat];
            Sloats[sloat] = new InventorySloat() { Start_Pos = _sloat_box[sloat].position, Sloat_No = sloat };//Set_Pos = _set_pos[sloat], Text = _text[sloat], 
        }
        Debug.Log(Sloats.Length);
        
    }

    public List<RaycastResult> HitResult()
    {
        //マウスの位置からUIを取得する
        //RaycastAllの引数（PointerEventData）作成
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        //RaycastAllの結果格納用List
        List<RaycastResult> RayResult = new List<RaycastResult>();

        //PointerEventDataにマウスの位置をセット
        pointData.position = Input.mousePosition;

        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointData, RayResult);

        return RayResult;
    }

    //アイテム取得
    public bool AddInventory_PickUP_Item(ItemInformation _iteminfo ,WeaponInventory _weaponInventory)
    {
        //武器の場合の処理を追加
        /*
        所持している武器オブジェクトの中に、取得したアイテムと同じ種類じゃなければ取得
        情報を入れる場所はスロット３
        オブジェクトは消さずに、武器インベントリに保存する（捨てることをしない以外は表示非表示で対応）
        同じ種類の場合アイテム情報を弾丸に変更

        最初に武器インベントリを調べる
        武器インベントリに入っていた場合アイテムインベントリを調べる
         */

        //種類が武器の場合
        if (_iteminfo.type == ITEM_TYPE.WEAPON)
        {
            //武器インベントリにあるか調べる
            if (_weaponInventory.Inventory.Sloats[(int)WeaponInventory.Sloat_Order.GUN].ItemInfo == null) 
            {
                //武器インベントリにいれる
                _weaponInventory.Inventory.Sloats[(int)WeaponInventory.Sloat_Order.GUN].ItemInfo 
                    = new ItemInformation(_iteminfo.type, _iteminfo.id, _iteminfo.get_num, _iteminfo.stack_max, _iteminfo.sprite, _iteminfo.weaponitem_info.weapon_obj);

                //武器をプレイヤーの子にしておく
                _weaponInventory.WeaponGet(_iteminfo.weaponitem_info.weapon_obj);

                return false;
            }
            //アイテムインベントリにあるか調べる
            else
            {
                //インベントリに入れれるか調べる
                bool in_flag = true;

                //武器インベントリと同じIDか調べる
                if (_weaponInventory.Inventory.Sloats[(int)WeaponInventory.Sloat_Order.GUN].ItemInfo.id == _iteminfo.id)
                {
                    in_flag = false;
                }

                //インベントリの中身と比べる
                for (int sloat = 0; sloat < Sloats.Length; sloat++)
                {
                    if (Sloats[sloat].ItemInfo != null && Sloats[sloat].ItemInfo.id == _iteminfo.id)
                    {
                        in_flag = false;
                        break;
                    }
                }

            }
        }

        for (int sloat = 0; sloat < Sloats.Length; sloat++)
        {
            if (Sloats[sloat].CanAdd_PickUPItem(_iteminfo))
            {
                int remaining_num = Sloats[sloat].Add_PickUPItem(_iteminfo);

                //すべて追加できた場合
                if (remaining_num <= 0)
                {
                    return true;
                }
                else
                {
                    //追加できなかった分を更新
                    _iteminfo.get_num = remaining_num;
                }
            }
        }
        return false;
    }

    //スロットアイテムからスロットアイテムへ
    public bool AddSloatSloatInventory(int _catch, int _in_sloat)
    {
        if (Sloats[_in_sloat].CanAdd_SloatItem(Sloats[_catch]))
        {
            int remaining_num = Sloats[_in_sloat].Add_SloatItem(Sloats[_catch]);
            //Sloats[_catch].SetSloatItemInfo();

            //すべて追加できた場合
            if (remaining_num <= 0)
            {
                if (Sloats[_catch].IsEmpty())
                {
                    Sloats[_catch].CrearSloat();
                }
                return true;
            }
            else
            {
                //追加できなかった分を更新
                Sloats[_catch].ItemInfo.get_num = remaining_num;
            }
        }

        return false;

    }

    public void Add_WeaponInventory(GameObject[] _sloats)
    {
        for (int sloat = 0; sloat < Sloats.Length; sloat++)
        {
            if (_sloats[sloat] != null)
            {
                Sloats[sloat].ItemInfo = _sloats[sloat].GetComponent<ItemSetting>().iteminfo;
            }
        }
    }

    public void SetUI()
    {
        for (int sloat = 0; sloat < Sloats.Length; sloat++)
        {
            if (Sloats[sloat].ItemInfo != null)
            {
                Sloat_Box[sloat].GetChild(0).gameObject.SetActive(true);
                Sloat_Box[sloat].GetChild(0).GetComponent<Image>().sprite = Sloats[sloat].ItemInfo.sprite;
                Sloat_Box[sloat].GetChild(0).GetChild(0).GetComponent<Text>().text = Sloats[sloat].ItemInfo.get_num + "";
            }
            else
            {
                Sloat_Box[sloat].GetChild(0).gameObject.SetActive(false);
                Sloat_Box[sloat].GetChild(0).GetComponent<Image>().sprite = null;
                Sloat_Box[sloat].GetChild(0).GetChild(0).GetComponent<Text>().text = 0 + "";
            }
        }
    }
    public void SetWeaponUI()
    {
        for (int sloat = 0; sloat < Sloats.Length; sloat++)
        {
            if (Sloats[sloat].ItemInfo != null)
            {
                Sloat_Box[sloat].GetChild(0).gameObject.SetActive(true);
                Sloat_Box[sloat].GetChild(0).GetComponent<Image>().sprite = Sloats[sloat].ItemInfo.sprite;
            }
            else
            {
                Sloat_Box[sloat].GetChild(0).gameObject.SetActive(false);
                Sloat_Box[sloat].GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
    }

    public void ItemSloatChange(int _cach_num ,int in_sloat_num)
    {
        InventorySloat temp = Sloats[in_sloat_num];
        Sloats[in_sloat_num] = Sloats[_cach_num];
        Sloats[_cach_num] = temp;
        Sloats[in_sloat_num].Sloat_No = in_sloat_num;
        Sloats[_cach_num].Sloat_No = _cach_num;
    }

    public void ItemSloatChange(WeaponInventory _weapon_sloat, int _cach_num)
    {
        InventorySloat temp = _weapon_sloat.Inventory.Sloats[(int)WeaponInventory.Sloat_Order.GUN];
        _weapon_sloat.Inventory.Sloats[(int)WeaponInventory.Sloat_Order.GUN] = Sloats[_cach_num];
        Sloats[_cach_num] = temp;
        Sloats[_cach_num].Sloat_No = _cach_num;
        _weapon_sloat.Inventory.Sloats[(int)WeaponInventory.Sloat_Order.GUN].Sloat_No = (int)WeaponInventory.Sloat_Order.GUN;
    }

    // デバッグ用メソッドを追加
    public void PrintInventory() 
    {
        for (int i = 0; i < Sloats.Length; i++) 
        { 
            var slot = Sloats[i]; 
            if (slot.ItemInfo != null) 
            {
                Debug.Log($"Slot {i}: {slot.Sloat_No}, Quantity: {slot.ItemInfo.id} , Get_Num ; {slot.ItemInfo.get_num}"); 
            } 
            else 
            { 
                Debug.Log($"Slot {i}: Empty"); 
            } 
        } 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スロットクラス
/// アイテムの情報を保存
/// </summary>
public class InventoryClass
{
    public SlotClass[] Slots { get; set; } //スロット情報

    public InventoryClass(int _size,Transform[] _box)
    {
        //サイズ分の配列作成
        Slots = new SlotClass[_size];

        //コンストラクタ
        for (int slot = 0; slot < _size; slot++) 
        {
            Slots[slot] = new SlotClass() { SlotBox = _box[slot] };
        }
    }

    /// <summary>
    /// UIをセットする
    /// インベントリのスロットのUIを決定
    /// </summary>
    /// <param name="_sprite">スプライトを入れるトランスフォーム</param>
    /// <param name="_text">個数表示用テキスト</param>
    public void SetUI(Transform[] _sprite,Text[] _text)
    {
        for (int slot = 0; slot < Slots.Length; slot++) 
        {
            if (Slots[slot].ItemInfo == null)
            {
                //アイテム情報がない場合

                _sprite[slot].gameObject.SetActive(false);          //非表示
                _sprite[slot].GetComponent<Image>().sprite = null;  //スプライト初期化
                _text[slot].GetComponent<Text>().text = 0 + "";     //テキスト初期化
            }
            else
            {
                //アイテム情報がある場合

                _sprite[slot].gameObject.SetActive(true);                                   //表示
                _sprite[slot].GetComponent<Image>().sprite = Slots[slot].ItemInfo.sprite;   //スロットにあるアイテム情報からスプライトを代入
                _text[slot].GetComponent<Text>().text = Slots[slot].ItemInfo.get_num + "";  //スロットにあるアイテム情報から個数表示
            }
        }
    }

    // デバッグ用メソッドを追加
    public void PrintInventory()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            var slot = Slots[i];
            if (slot.ItemInfo != null)
            {
                Debug.Log($"Slot {i}: {slot.SlotNo}, Quantity: {slot.ItemInfo.id} , Get_Num ; {slot.ItemInfo.get_num}");
            }
            else
            {
                Debug.Log($"Slot {i}: Empty");
            }
        }
    }

}

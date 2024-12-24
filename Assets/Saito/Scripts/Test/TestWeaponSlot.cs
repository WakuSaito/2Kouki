using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestWeaponSlot : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponSlotObjects;//武器スロットのオブジェクト
    private int selectWeponNum = 1;//選択中の武器スロットの場所

    //UI位置
    [SerializeField] private Image[] slotIconUI;
    //選択UI
    [SerializeField] private GameObject selectFrameUI;

    Color skeletonColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);//半透明
    Color nomalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);//不透明


    private void Start()
    {
        //画像更新
        for (int i = 0; i < weaponSlotObjects.Length; i++)
        {
            SetSlotIcon(i);
        }

        ChangeSelect(selectWeponNum);
    }

    //選択スロットを決める(現在の選択位置からの方向)
    public void SelectSlot(int _vec)
    {
        int slotSize = weaponSlotObjects.Length;

        int nextSelect = selectWeponNum + _vec;
        if (nextSelect >= slotSize)
        {
            nextSelect %= slotSize;
        }

        while (nextSelect < 0)
        {
            nextSelect = nextSelect + slotSize;
        }

        ChangeSelect(nextSelect);
    }

    //選択アイテムの変更
    void ChangeSelect(int _num)
    {
        selectWeponNum = _num;

        for (int i = 0; i < weaponSlotObjects.Length; i++)
        {
            if (weaponSlotObjects[i] == null) continue;

            //選択中のアイテム以外非表示
            if (i == selectWeponNum)
                weaponSlotObjects[i].SetActive(true);
            else
                weaponSlotObjects[i].SetActive(false);//銃などの遅延処理が途中で止まってしまう
        }

        MoveFrame();
    }

    //UIの更新
    void MoveFrame()
    {
        if (selectWeponNum >= slotIconUI.Length) return;

        //枠移動
        selectFrameUI.transform.position = slotIconUI[selectWeponNum].transform.position;

        foreach(var icon in slotIconUI)
        {
            if (icon == null) continue;

            //半透過
            icon.color = skeletonColor;
        }
        slotIconUI[selectWeponNum].color = nomalColor;
    }

    //UIのアイコン画像変更
    void SetSlotIcon(int _num)
    {
        if (_num < 0 || _num >= slotIconUI.Length) return;
        if (weaponSlotObjects[_num] == null) return;

       //アイテム情報取得
       ItemSetting itemSetting = weaponSlotObjects[_num].GetComponent<ItemSetting>();
        if (itemSetting == null) return;

        Debug.Log(_num + " id:" + itemSetting.iteminfo.id);
        //画像変更
        slotIconUI[_num].sprite = itemSetting.iteminfo.sprite;
        slotIconUI[_num].gameObject.SetActive(true);
    }

    //選択中のアイテム取得
    public GameObject GetSelectWeapon()
    {
        return weaponSlotObjects[selectWeponNum];
    }

    public void AddWeapon(int _num, GameObject _obj)
    {
        if (_num < 0 || _num >= weaponSlotObjects.Length) return;

        weaponSlotObjects[_num] = _obj;
    }
}

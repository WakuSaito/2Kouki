using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestWeaponSlot : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponSlotObjects;//����X���b�g�̃I�u�W�F�N�g
    private int selectWeponNum = 1;//�I�𒆂̕���X���b�g�̏ꏊ

    //UI�ʒu
    [SerializeField] private Image[] slotIconUI;
    //�I��UI
    [SerializeField] private GameObject selectFrameUI;

    Color skeletonColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);//������
    Color nomalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);//�s����


    private void Start()
    {
        //�摜�X�V
        for (int i = 0; i < weaponSlotObjects.Length; i++)
        {
            SetSlotIcon(i);
        }

        ChangeSelect(selectWeponNum);
    }

    //�I���X���b�g�����߂�(���݂̑I���ʒu����̕���)
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

    //�I���A�C�e���̕ύX
    void ChangeSelect(int _num)
    {
        selectWeponNum = _num;

        for (int i = 0; i < weaponSlotObjects.Length; i++)
        {
            if (weaponSlotObjects[i] == null) continue;

            //�I�𒆂̃A�C�e���ȊO��\��
            if (i == selectWeponNum)
                weaponSlotObjects[i].SetActive(true);
            else
                weaponSlotObjects[i].SetActive(false);//�e�Ȃǂ̒x���������r���Ŏ~�܂��Ă��܂�
        }

        MoveFrame();
    }

    //UI�̍X�V
    void MoveFrame()
    {
        if (selectWeponNum >= slotIconUI.Length) return;

        //�g�ړ�
        selectFrameUI.transform.position = slotIconUI[selectWeponNum].transform.position;

        foreach(var icon in slotIconUI)
        {
            if (icon == null) continue;

            //������
            icon.color = skeletonColor;
        }
        slotIconUI[selectWeponNum].color = nomalColor;
    }

    //UI�̃A�C�R���摜�ύX
    void SetSlotIcon(int _num)
    {
        if (_num < 0 || _num >= slotIconUI.Length) return;
        if (weaponSlotObjects[_num] == null) return;

       //�A�C�e�����擾
       ItemSetting itemSetting = weaponSlotObjects[_num].GetComponent<ItemSetting>();
        if (itemSetting == null) return;

        Debug.Log(_num + " id:" + itemSetting.iteminfo.id);
        //�摜�ύX
        slotIconUI[_num].sprite = itemSetting.iteminfo.sprite;
        slotIconUI[_num].gameObject.SetActive(true);
    }

    //�I�𒆂̃A�C�e���擾
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �X���b�g�N���X
/// �A�C�e���̏���ۑ�
/// </summary>
public class InventoryClass
{
    public SlotClass[] Slots { get; set; } //�X���b�g���

    public InventoryClass(int _size,Transform[] _box)
    {
        //�T�C�Y���̔z��쐬
        Slots = new SlotClass[_size];

        //�R���X�g���N�^
        for (int slot = 0; slot < _size; slot++) 
        {
            Slots[slot] = new SlotClass() { SlotBox = _box[slot] };
        }
    }

    /// <summary>
    /// UI���Z�b�g����
    /// �C���x���g���̃X���b�g��UI������
    /// </summary>
    /// <param name="_sprite">�X�v���C�g������g�����X�t�H�[��</param>
    /// <param name="_text">���\���p�e�L�X�g</param>
    public void SetUI(Transform[] _sprite,Text[] _text)
    {
        for (int slot = 0; slot < Slots.Length; slot++) 
        {
            if (Slots[slot].ItemInfo == null)
            {
                //�A�C�e����񂪂Ȃ��ꍇ

                _sprite[slot].gameObject.SetActive(false);          //��\��
                _sprite[slot].GetComponent<Image>().sprite = null;  //�X�v���C�g������
                _text[slot].GetComponent<Text>().text = 0 + "";     //�e�L�X�g������
            }
            else
            {
                //�A�C�e����񂪂���ꍇ

                _sprite[slot].gameObject.SetActive(true);                                   //�\��
                _sprite[slot].GetComponent<Image>().sprite = Slots[slot].ItemInfo.sprite;   //�X���b�g�ɂ���A�C�e����񂩂�X�v���C�g����
                _text[slot].GetComponent<Text>().text = Slots[slot].ItemInfo.get_num + "";  //�X���b�g�ɂ���A�C�e����񂩂���\��
            }
        }
    }

    // �f�o�b�O�p���\�b�h��ǉ�
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

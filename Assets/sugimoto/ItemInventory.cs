using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    //�A�C�e���̎�ނ̍ő吔
    const int ITEM_TYPE_MAX = 2;

    //�l���ł���A�C�e����ID��
    public int[] get_num = new int[ITEM_TYPE_MAX];
    //�C���x���g���ɕ\������I�u�W�F�N�gID��
    public GameObject[] item_obj = new GameObject[ITEM_TYPE_MAX];

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRespawn : MonoBehaviour
{
    //スポナーオブジェクト
    GameObject[] spawners;

    private void Awake()
    {
        //アクティブ状態の変更後に取得すると見つからないので
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ用
        if (Input.GetKey(KeyCode.B)&&
            Input.GetKeyDown(KeyCode.R))
            Respawn();
    }

    public void Respawn()
    {
        //全スポナー有効化
        foreach(var obj in spawners)
        {
            obj.SetActive(true);
        }

        //全アイテム設置スクリプト呼び出し
        GameObject[] itemSetter = GameObject.FindGameObjectsWithTag("ItemSetter");
        foreach(var obj in itemSetter)
        {
            obj.GetComponent<SetItem>().SetItemPos();
        }
    }
}

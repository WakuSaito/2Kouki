using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRespawn : MonoBehaviour
{
    //スポナーオブジェクト
    GameObject[] mSpawners;

    player mPlayerScript;

    private void Awake()
    {
        //アクティブ状態の変更後に取得すると見つからないので
        mSpawners = GameObject.FindGameObjectsWithTag("Spawner");

        //プレイヤースクリプト取得
        mPlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ用
        if (Input.GetKey(KeyCode.B)&&
            Input.GetKeyDown(KeyCode.R))
        {
            
            RestPlayer();
        }
    }

    public void Respawn()
    {
        //全スポナー有効化
        foreach(var obj in mSpawners)
        {
            obj.SetActive(true);
        }

        //全アイテム設置スクリプト呼び出し
        GameObject[] item_setter = GameObject.FindGameObjectsWithTag("ItemSetter");
        foreach(var obj in item_setter)
        {
            obj.GetComponent<SetItem>().SetItemPos();
        }
    }

    //プレイヤーを休息させる
    public void RestPlayer()
    {
        Respawn();

        Debug.Log("休息");

        if (mPlayerScript == null) return;

        //休息
        mPlayerScript.TakeRest(0.7f, 0.3f);
    }
}

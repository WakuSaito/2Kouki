using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRespawn : MonoBehaviour
{
    //スポナーオブジェクト
    GameObject[] spawners;

    player playerScript;

    private void Awake()
    {
        //アクティブ状態の変更後に取得すると見つからないので
        spawners = GameObject.FindGameObjectsWithTag("Spawner");

        //プレイヤースクリプト取得
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
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

    //プレイヤーを休息させる
    public void RestPlayer()
    {
        Respawn();

        Debug.Log("休息");

        if (playerScript == null) return;

        //休息
        playerScript.TakeRest(0.7f, 0.3f);
    }
}

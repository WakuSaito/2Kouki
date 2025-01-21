using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトの再生成
/// ベッドでの処理　クラス名は良くないかも
/// </summary>
public class ObjRespawn : MonoBehaviour
{
    //スポナーオブジェクト
    GameObject[] m_spawners;

    player m_playerScript;

    //オブジェクトの取得
    private void Awake()
    {
        //アクティブ状態の変更後に取得すると見つからないので
        m_spawners = GameObject.FindGameObjectsWithTag("Spawner");

        //プレイヤースクリプト取得
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
    }

    // デバッグ用
    void Update()
    {
        //デバッグ用
        if (Input.GetKey(KeyCode.B)&&
            Input.GetKeyDown(KeyCode.R))
        {
            
            RestPlayer();
        }
    }

    /// <summary>
    /// 再生成の実行
    /// </summary>
    public void Respawn()
    {
        //全スポナー有効化
        foreach(var obj in m_spawners)
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

    /// <summary>
    /// プレイヤーを休息させる
    /// </summary>
    public void RestPlayer()
    {
        Respawn();

        Debug.Log("休息");

        if (m_playerScript == null) return;

        //休息
        m_playerScript.TakeRest(0.7f, 0.3f);
    }
}

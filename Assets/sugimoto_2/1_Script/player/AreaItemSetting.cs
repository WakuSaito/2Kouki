using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaItemSetting : MonoBehaviour
{   
    //アイテムオブジェクトを保存
    List<GameObject> m_itemObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(GetItemObj), 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSphereArea(transform.position, 5.0f);
    }

    void GetItemObj()
    {
        //アイテムタグを持つオブジェクト取得
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("item");

        //リジットボディーを持っているオブジェクトのみ保存
        foreach (var obj in tmp)
        {
            if (obj.GetComponent<Rigidbody>())
            {
                m_itemObj.Add(obj);
                obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// エリア内に入っているオブジェクトを調べる
    /// </summary>
    /// <param name="_center">調べる中心</param>
    /// <param name="_radius">調べる半径</param>
    void CheckSphereArea(Vector3 _center, float _radius)
    {
        //すべてのアイテムのオブジェクトを調べる
        foreach (var obj in m_itemObj)
        {
            if (obj == null) continue;

            //範囲内のオブジェクトは表示、範囲外は非表示にする
            if (Vector3.Distance(_center, obj.transform.position) <= _radius)
            {
                if (!obj.GetComponent<ItemSetting>().m_getFlag)
                {
                    obj.SetActive(true);
                }
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }
}

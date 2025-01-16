using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//条件に合うオブジェクトにマークを付けるスクリプト
public class TargetMark : MonoBehaviour
{
    [SerializeField] //位置を保存するためのオブジェクト
    private GameObject m_markPrefab;

    [SerializeField] //判定を行う対象との距離
    private float m_targetDistance = 40.0f;

    [SerializeField] //マークするオブジェクトのタグ
    private string[] m_markTargetTags;

    [SerializeField]//ゾンビのY方向の中心（足元からの距離）
    private float m_zombieCenterY = 2.0f;


    //条件にあうオブジェクトをマークする
    public void RangeMark()
    {
        foreach(var tag_name in m_markTargetTags)
        {
            //対象のタグが付いた全オブジェクト
            GameObject[] tag_objs = GameObject.FindGameObjectsWithTag(tag_name);

            //距離が一定以下のオブジェクトのみ判定
            foreach (var obj in tag_objs)
            {
                if (Vector3.Distance(transform.position, obj.transform.position) > m_targetDistance) continue;

                Vector3 mark_pos = obj.transform.position + Vector3.up * m_zombieCenterY;
                //全ゾンビにマーカーを置く
                Instantiate(m_markPrefab, mark_pos, Quaternion.identity);
            }

        }

    }
}

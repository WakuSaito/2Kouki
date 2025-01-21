using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>ターゲットマーククラス</para>
/// インスペクタで指定した条件のオブジェクトにマーク（UI）を付ける
/// </summary>
public class TargetMark : MonoBehaviour
{
    //位置を保存するためのオブジェクト
    [SerializeField] private GameObject m_markPrefab;

    //判定を行う対象との距離
    [SerializeField] private float m_targetDistance = 40.0f;
    //マークするオブジェクトのタグ
    [SerializeField] private string[] m_markTargetTags;

    //対象オブジェクトのY方向の中心（足元からの距離）
    [SerializeField] private float m_targetCenterY = 2.0f;

    /// <summary>
    /// <para>範囲マーク</para>
    /// 一定範囲の対象タグオブジェクトにマークを付ける
    /// </summary>
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

                //Y位置調整
                Vector3 mark_pos = obj.transform.position + Vector3.up * m_targetCenterY;
                //全対象にマーカーを置く
                Instantiate(m_markPrefab, mark_pos, Quaternion.identity);
            }

        }

    }
}

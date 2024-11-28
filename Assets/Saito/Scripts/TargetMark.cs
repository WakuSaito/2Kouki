using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//条件に合うオブジェクトにマークを付けるスクリプト
public class TargetMark : MonoBehaviour
{
    [SerializeField] //位置を保存するためのオブジェクト
    private GameObject markPrefab;

    [SerializeField] //判定を行う対象との距離
    private float targetDistance = 40.0f;

    [SerializeField] //マークするオブジェクトのタグ
    private string[] markTargetTags;

    [SerializeField]//ゾンビのY方向の中心（足元からの距離）
    private float zombieCenterY = 2.0f;


    //条件にあうオブジェクトをマークする
    public void RangeMark()
    {
        foreach(var tagName in markTargetTags)
        {
            //対象のタグが付いた全オブジェクト
            GameObject[] tagObjects = GameObject.FindGameObjectsWithTag(tagName);

            //距離が一定以下のオブジェクトのみ判定
            foreach (var obj in tagObjects)
            {
                if (Vector3.Distance(transform.position, obj.transform.position) > targetDistance) continue;

                Vector3 markPos = obj.transform.position + Vector3.up * zombieCenterY;
                //全ゾンビにマーカーを置く
                Instantiate(markPrefab, markPos, Quaternion.identity);
            }

        }

    }
}

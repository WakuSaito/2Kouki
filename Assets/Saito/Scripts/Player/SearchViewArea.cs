using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchViewArea : MonoBehaviour
{
    [SerializeField]//対象となる範囲
    private float activeAngle = 20.0f;   

    private GameObject playerObj;
    private GameObject cameraObj;

    //前回の対象となったオブジェクト　タグごとに管理
    private Dictionary<string, GameObject> prevTargetObj = new Dictionary<string, GameObject>();

    private void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        cameraObj = Camera.main.gameObject;
    }

    //視点方向のオブジェクトを取得する（対象とするタグ, 対象とする距離, 中心座標Y調整用）
    public GameObject GetObjUpdate(string _targetTag, float _distance, float _objMidUp = 0f)
    {
        //全対象タグのオブジェクト
        GameObject[] itemObjects = GameObject.FindGameObjectsWithTag(_targetTag);

        Vector3 playerPos = playerObj.transform.position;
        Vector3 cameraPos = cameraObj.transform.position;
        Vector3 eyeDir = cameraObj.transform.forward;//視点方向ベクトル

        List<GameObject> targetItems = new List<GameObject>();

        ResetColor(_targetTag);//前回のオブジェクトの色を戻す

        //距離が一定以下のオブジェクトのみに絞る
        foreach (var item in itemObjects)
        {
            //距離を調べる
            Vector3 itemPos = item.transform.position;

            if (Vector3.Distance(playerPos, itemPos) > _distance) continue;

            targetItems.Add(item);//リストに追加
        }

        if (targetItems.Count != 0)
        {
            //オブジェクトの中心位置調整用
            Vector3 itemCenterAd = Vector3.up * _objMidUp;

            //対象のオブジェクトの中から視点から角度が一番近いオブジェクトを取得
            GameObject nearestEnemy =
                targetItems.OrderBy(p =>
                Vector3.Angle(((p.transform.position + itemCenterAd) - cameraPos).normalized, eyeDir)).First();


            //取得したオブジェクトまでと視点の角度が一定以下なら
            if (Vector3.Angle(((nearestEnemy.transform.position + itemCenterAd) - cameraPos).normalized, eyeDir) <= activeAngle)
            {
                //対象の色を変更
                nearestEnemy.GetComponent<ColorChanger>().ChangeColorAlpha(0.25f);

                SelectColor(nearestEnemy);//色変更
                SaveTarget(_targetTag, nearestEnemy);//情報保存

                return nearestEnemy;
            }

        }

        SaveTarget(_targetTag, null);//情報保存

        return null;

    }

    //色の変更
    private void SelectColor(GameObject _target)
    {
        ColorChanger colorChanger = _target.GetComponent<ColorChanger>();
        if (colorChanger == null) return;

        colorChanger.ChangeColorAlpha(0.25f);
    }

    //変更されているカラーを戻す
    private void ResetColor(string _tag)
    {
        if (!prevTargetObj.ContainsKey(_tag)) return;
        if (prevTargetObj[_tag] == null) return;

        ColorChanger colorChanger = prevTargetObj[_tag].GetComponent<ColorChanger>();
        if (colorChanger == null) return;


        colorChanger.ChangeColorAlpha(0f);
    }

    //対象オブジェクトの情報保存
    private void SaveTarget(string _tag, GameObject _target)
    {
        if (prevTargetObj.ContainsKey(_tag))
        {
            prevTargetObj[_tag] = _target;
        }
        else
        {
            prevTargetObj.Add(_tag, _target);
        }
    }
}

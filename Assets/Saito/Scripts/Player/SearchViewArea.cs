using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchViewArea : MonoBehaviour
{
    [SerializeField]//対象となる範囲
    private float mActiveAngle = 20.0f;   

    private GameObject mPlayerObj;
    private GameObject mCameraObj;

    //前回の対象となったオブジェクト　タグごとに管理
    private Dictionary<string, GameObject> mPrevTargetObj = new Dictionary<string, GameObject>();

    private void Awake()
    {
        mPlayerObj = GameObject.FindGameObjectWithTag("Player");
        mCameraObj = Camera.main.gameObject;
    }

    //視点方向のオブジェクトを取得する（対象とするタグ, 対象とする距離, 中心座標Y調整用）
    public GameObject GetObjUpdate(string _target_tag, float _distance, float _obj_mid_up = 0f)
    {
        //全対象タグのオブジェクト
        GameObject[] item_objects = GameObject.FindGameObjectsWithTag(_target_tag);

        Vector3 player_pos = mPlayerObj.transform.position;
        Vector3 camera_pos = mCameraObj.transform.position;
        Vector3 eye_dir = mCameraObj.transform.forward;//視点方向ベクトル

        List<GameObject> target_items = new List<GameObject>();

        ResetColor(_target_tag);//前回のオブジェクトの色を戻す

        //距離が一定以下のオブジェクトのみに絞る
        foreach (var item in item_objects)
        {
            //距離を調べる
            Vector3 item_pos = item.transform.position;

            if (Vector3.Distance(player_pos, item_pos) > _distance) continue;

            target_items.Add(item);//リストに追加
        }

        if (target_items.Count != 0)
        {
            //オブジェクトの中心位置調整用
            Vector3 item_center_add = Vector3.up * _obj_mid_up;

            //対象のオブジェクトの中から視点から角度が一番近いオブジェクトを取得
            GameObject nearest_target =
                target_items.OrderBy(p =>
                Vector3.Angle(((p.transform.position + item_center_add) - camera_pos).normalized, eye_dir)).First();

            //対象の位置
            Vector3 target_pos = nearest_target.transform.position + item_center_add;

            //取得したオブジェクトまでと視点の角度が一定以下なら
            if (Vector3.Angle((target_pos - camera_pos).normalized, eye_dir) <= mActiveAngle)
            {
                //途中に壁などが無いか調べる
                RaycastHit hit;
                Physics.Raycast(camera_pos, target_pos - camera_pos, out hit);
                //Debug.DrawRay(cameraPos, targetPos - cameraPos, Color.blue, 1f);
                //Debug.Log("hit" + hit.transform.gameObject);

                //一番上の親までに対象のタグがあるか調べる
                Transform trans = hit.transform;
                Transform root_trans = hit.transform.root;
                while(true)
                {
                    //Debug.Log("tag" + trans.tag);
                    if (trans.tag == _target_tag)
                    {
                        SelectColor(nearest_target);//色変更
                        SaveTarget(_target_tag, nearest_target);//情報保存

                        return nearest_target;
                    }

                    if (trans != root_trans)
                    {
                        trans = trans.parent;//一つ上の親に進む
                        continue;
                    }
                    else
                        break;
                }             
            }
        }

        SaveTarget(_target_tag, null);//情報保存

        return null;

    }

    //色の変更
    private void SelectColor(GameObject _target)
    {
        ColorChanger color_changer = _target.GetComponent<ColorChanger>();
        if (color_changer == null) return;

        color_changer.ChangeColorAlpha(0.25f);
    }

    //変更されているカラーを戻す
    public void ResetColor(string _tag)
    {
        if (!mPrevTargetObj.ContainsKey(_tag)) return;
        if (mPrevTargetObj[_tag] == null) return;

        ColorChanger color_changer = mPrevTargetObj[_tag].GetComponent<ColorChanger>();
        if (color_changer == null) return;


        color_changer.ChangeColorAlpha(0f);
    }

    //対象オブジェクトの情報保存
    private void SaveTarget(string _tag, GameObject _target)
    {
        if (mPrevTargetObj.ContainsKey(_tag))
        {
            mPrevTargetObj[_tag] = _target;
        }
        else
        {
            mPrevTargetObj.Add(_tag, _target);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 視点方向の範囲探索
/// 範囲にある指定したタグのオブジェクトの取得と色変更 プレイヤー側で使用
/// </summary>
public class SearchViewArea : MonoBehaviour
{
    //対象となる範囲(上下左右方向)
    [SerializeField] private float m_activeAngle = 20.0f;   

    private GameObject m_playerObj;//プレイヤー
    private GameObject m_cameraObj;//視点カメラ

    //前回の対象となったオブジェクト　タグごとに管理
    private Dictionary<string, GameObject> mPrevTargetObj = new Dictionary<string, GameObject>();

    //オブジェクト取得
    private void Awake()
    {
        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        m_cameraObj = Camera.main.gameObject;
    }

    /// <summary>
    /// 視点方向のオブジェクト取得ループ
    /// 視点方向の範囲にある対象タグオブジェクトから一つを色を変え、取得する
    /// </summary>
    /// <param name="_target_tag">対象とするタグ</param>
    /// <param name="_distance">対象とする距離</param>
    /// <param name="_obj_mid_up">中心座標Y調整用</param>
    /// <returns>範囲内の一番中心に近いオブジェクト</returns>
    public GameObject GetObjUpdate(string _target_tag, float _distance, float _obj_mid_up = 0f)
    {
        //全対象タグのオブジェクト
        GameObject[] item_objects = GameObject.FindGameObjectsWithTag(_target_tag);

        Vector3 player_pos = m_playerObj.transform.position;
        Vector3 camera_pos = m_cameraObj.transform.position;
        Vector3 eye_dir = m_cameraObj.transform.forward;//視点方向ベクトル

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

            //対象のオブジェクトから視点の中心に一番近いオブジェクトを取得
            GameObject nearest_target =
                target_items.OrderBy(p =>
                Vector3.Angle(((p.transform.position + item_center_add) - camera_pos).normalized, eye_dir)).First();

            //対象の位置
            Vector3 target_pos = nearest_target.transform.position + item_center_add;

            //取得したオブジェクトまでと視点の角度が一定以下なら
            if (Vector3.Angle((target_pos - camera_pos).normalized, eye_dir) <= m_activeAngle)
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
                    {
                        break;
                    }
                }             
            }
        }

        SaveTarget(_target_tag, null);//情報保存

        return null;

    }

    /// <summary>
    /// 選択色にする
    /// 対象のオブジェクトの色を変更する
    /// </summary>
    /// <param name="_target">対象のオブジェクト</param>
    private void SelectColor(GameObject _target)
    {
        ColorChanger color_changer = _target.GetComponent<ColorChanger>();
        if (color_changer == null) return;

        color_changer.ChangeColorAlpha(0.25f);//色変更
    }

    /// <summary>
    /// 色のリセット
    /// 前回の実行で変更されているカラーを元に戻す
    /// </summary>
    /// <param name="_tag">対象のタグ</param>
    public void ResetColor(string _tag)
    {
        if (!mPrevTargetObj.ContainsKey(_tag)) return;
        if (mPrevTargetObj[_tag] == null) return;

        ColorChanger color_changer = mPrevTargetObj[_tag].GetComponent<ColorChanger>();
        if (color_changer == null) return;

        color_changer.ChangeColorAlpha(0f);//色変更
    }

    /// <summary>
    /// 対象の情報保存
    /// 後で色を元に戻すために実行内容を保存する
    /// </summary>
    /// <param name="_tag">探索したタグ</param>
    /// <param name="_target">その結果の対象オブジェクト</param>
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

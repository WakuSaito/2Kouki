using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchViewArea : MonoBehaviour
{
    [SerializeField]//�ΏۂƂȂ�͈�
    private float m_activeAngle = 20.0f;   

    private GameObject m_playerObj;
    private GameObject m_cameraObj;

    //�O��̑ΏۂƂȂ����I�u�W�F�N�g�@�^�O���ƂɊǗ�
    private Dictionary<string, GameObject> mPrevTargetObj = new Dictionary<string, GameObject>();

    private void Awake()
    {
        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        m_cameraObj = Camera.main.gameObject;
    }

    //���_�����̃I�u�W�F�N�g���擾����i�ΏۂƂ���^�O, �ΏۂƂ��鋗��, ���S���WY�����p�j
    public GameObject GetObjUpdate(string _target_tag, float _distance, float _obj_mid_up = 0f)
    {
        //�S�Ώۃ^�O�̃I�u�W�F�N�g
        GameObject[] item_objects = GameObject.FindGameObjectsWithTag(_target_tag);

        Vector3 player_pos = m_playerObj.transform.position;
        Vector3 camera_pos = m_cameraObj.transform.position;
        Vector3 eye_dir = m_cameraObj.transform.forward;//���_�����x�N�g��

        List<GameObject> target_items = new List<GameObject>();

        ResetColor(_target_tag);//�O��̃I�u�W�F�N�g�̐F��߂�

        //���������ȉ��̃I�u�W�F�N�g�݂̂ɍi��
        foreach (var item in item_objects)
        {
            //�����𒲂ׂ�
            Vector3 item_pos = item.transform.position;

            if (Vector3.Distance(player_pos, item_pos) > _distance) continue;

            target_items.Add(item);//���X�g�ɒǉ�
        }

        if (target_items.Count != 0)
        {
            //�I�u�W�F�N�g�̒��S�ʒu�����p
            Vector3 item_center_add = Vector3.up * _obj_mid_up;

            //�Ώۂ̃I�u�W�F�N�g�̒����王�_����p�x����ԋ߂��I�u�W�F�N�g���擾
            GameObject nearest_target =
                target_items.OrderBy(p =>
                Vector3.Angle(((p.transform.position + item_center_add) - camera_pos).normalized, eye_dir)).First();

            //�Ώۂ̈ʒu
            Vector3 target_pos = nearest_target.transform.position + item_center_add;

            //�擾�����I�u�W�F�N�g�܂łƎ��_�̊p�x�����ȉ��Ȃ�
            if (Vector3.Angle((target_pos - camera_pos).normalized, eye_dir) <= m_activeAngle)
            {
                //�r���ɕǂȂǂ����������ׂ�
                RaycastHit hit;
                Physics.Raycast(camera_pos, target_pos - camera_pos, out hit);
                //Debug.DrawRay(cameraPos, targetPos - cameraPos, Color.blue, 1f);
                //Debug.Log("hit" + hit.transform.gameObject);

                //��ԏ�̐e�܂łɑΏۂ̃^�O�����邩���ׂ�
                Transform trans = hit.transform;
                Transform root_trans = hit.transform.root;
                while(true)
                {
                    //Debug.Log("tag" + trans.tag);
                    if (trans.tag == _target_tag)
                    {
                        SelectColor(nearest_target);//�F�ύX
                        SaveTarget(_target_tag, nearest_target);//���ۑ�

                        return nearest_target;
                    }

                    if (trans != root_trans)
                    {
                        trans = trans.parent;//���̐e�ɐi��
                        continue;
                    }
                    else
                        break;
                }             
            }
        }

        SaveTarget(_target_tag, null);//���ۑ�

        return null;

    }

    //�F�̕ύX
    private void SelectColor(GameObject _target)
    {
        ColorChanger color_changer = _target.GetComponent<ColorChanger>();
        if (color_changer == null) return;

        color_changer.ChangeColorAlpha(0.25f);
    }

    //�ύX����Ă���J���[��߂�
    public void ResetColor(string _tag)
    {
        if (!mPrevTargetObj.ContainsKey(_tag)) return;
        if (mPrevTargetObj[_tag] == null) return;

        ColorChanger color_changer = mPrevTargetObj[_tag].GetComponent<ColorChanger>();
        if (color_changer == null) return;


        color_changer.ChangeColorAlpha(0f);
    }

    //�ΏۃI�u�W�F�N�g�̏��ۑ�
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

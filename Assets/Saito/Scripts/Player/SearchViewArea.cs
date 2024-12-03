using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchViewArea : MonoBehaviour
{
    [SerializeField]//�ΏۂƂȂ�͈�
    private float activeAngle = 20.0f;   

    private GameObject playerObj;
    private GameObject cameraObj;

    //�O��̑ΏۂƂȂ����I�u�W�F�N�g�@�^�O���ƂɊǗ�
    private Dictionary<string, GameObject> prevTargetObj = new Dictionary<string, GameObject>();

    private void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        cameraObj = Camera.main.gameObject;
    }

    //���_�����̃I�u�W�F�N�g���擾����i�ΏۂƂ���^�O, �ΏۂƂ��鋗��, ���S���WY�����p�j
    public GameObject GetObjUpdate(string _targetTag, float _distance, float _objMidUp = 0f)
    {
        //�S�Ώۃ^�O�̃I�u�W�F�N�g
        GameObject[] itemObjects = GameObject.FindGameObjectsWithTag(_targetTag);

        Vector3 playerPos = playerObj.transform.position;
        Vector3 cameraPos = cameraObj.transform.position;
        Vector3 eyeDir = cameraObj.transform.forward;//���_�����x�N�g��

        List<GameObject> targetItems = new List<GameObject>();

        ResetColor(_targetTag);//�O��̃I�u�W�F�N�g�̐F��߂�

        //���������ȉ��̃I�u�W�F�N�g�݂̂ɍi��
        foreach (var item in itemObjects)
        {
            //�����𒲂ׂ�
            Vector3 itemPos = item.transform.position;

            if (Vector3.Distance(playerPos, itemPos) > _distance) continue;

            targetItems.Add(item);//���X�g�ɒǉ�
        }

        if (targetItems.Count != 0)
        {
            //�I�u�W�F�N�g�̒��S�ʒu�����p
            Vector3 itemCenterAd = Vector3.up * _objMidUp;

            //�Ώۂ̃I�u�W�F�N�g�̒����王�_����p�x����ԋ߂��I�u�W�F�N�g���擾
            GameObject nearestTarget =
                targetItems.OrderBy(p =>
                Vector3.Angle(((p.transform.position + itemCenterAd) - cameraPos).normalized, eyeDir)).First();

            //�Ώۂ̈ʒu
            Vector3 targetPos = nearestTarget.transform.position + itemCenterAd;

            //�擾�����I�u�W�F�N�g�܂łƎ��_�̊p�x�����ȉ��Ȃ�
            if (Vector3.Angle((targetPos - cameraPos).normalized, eyeDir) <= activeAngle)
            {
                //�r���ɕǂȂǂ����������ׂ�
                RaycastHit hit;
                Physics.Raycast(cameraPos, targetPos - cameraPos, out hit);
                Debug.DrawRay(cameraPos, targetPos - cameraPos, Color.blue, 1f);
                Debug.Log("hit" + hit.transform.gameObject);

                //��ԏ�̐e�܂łɑΏۂ̃^�O�����邩���ׂ�
                Transform trans = hit.transform;
                Transform rootTrans = hit.transform.root;
                while(true)
                {
                    Debug.Log("tag" + trans.tag);
                    if (trans.tag == _targetTag)
                    {
                        SelectColor(nearestTarget);//�F�ύX
                        SaveTarget(_targetTag, nearestTarget);//���ۑ�

                        return nearestTarget;
                    }

                    if (trans != rootTrans)
                    {
                        trans = trans.parent;//���̐e�ɐi��
                        continue;
                    }
                    else
                        break;
                }             
            }
        }

        SaveTarget(_targetTag, null);//���ۑ�

        return null;

    }

    //�F�̕ύX
    private void SelectColor(GameObject _target)
    {
        ColorChanger colorChanger = _target.GetComponent<ColorChanger>();
        if (colorChanger == null) return;

        colorChanger.ChangeColorAlpha(0.25f);
    }

    //�ύX����Ă���J���[��߂�
    public void ResetColor(string _tag)
    {
        if (!prevTargetObj.ContainsKey(_tag)) return;
        if (prevTargetObj[_tag] == null) return;

        ColorChanger colorChanger = prevTargetObj[_tag].GetComponent<ColorChanger>();
        if (colorChanger == null) return;


        colorChanger.ChangeColorAlpha(0f);
    }

    //�ΏۃI�u�W�F�N�g�̏��ۑ�
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

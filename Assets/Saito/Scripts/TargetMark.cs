using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ɍ����I�u�W�F�N�g�Ƀ}�[�N��t����X�N���v�g
public class TargetMark : MonoBehaviour
{
    [SerializeField] //�ʒu��ۑ����邽�߂̃I�u�W�F�N�g
    private GameObject markPrefab;

    [SerializeField] //������s���ΏۂƂ̋���
    private float targetDistance = 40.0f;

    [SerializeField] //�}�[�N����I�u�W�F�N�g�̃^�O
    private string[] markTargetTags;

    [SerializeField]//�]���r��Y�����̒��S�i��������̋����j
    private float zombieCenterY = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            MarkTarget();
        }
    }
    //�����ɂ����I�u�W�F�N�g���}�[�N����
    public void MarkTarget()
    {
        foreach(var tagName in markTargetTags)
        {
            //�Ώۂ̃^�O���t�����S�I�u�W�F�N�g
            GameObject[] tagObjects = GameObject.FindGameObjectsWithTag(tagName);

            //���������ȉ��̃I�u�W�F�N�g�̂ݔ���
            foreach (var obj in tagObjects)
            {
                if (Vector3.Distance(transform.position, obj.transform.position) > targetDistance) continue;

                Vector3 markPos = obj.transform.position + Vector3.up * zombieCenterY;
                //�S�]���r�Ƀ}�[�J�[��u��
                Instantiate(markPrefab, markPos, Quaternion.identity);
            }

        }

    }
}

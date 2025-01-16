using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ɍ����I�u�W�F�N�g�Ƀ}�[�N��t����X�N���v�g
public class TargetMark : MonoBehaviour
{
    [SerializeField] //�ʒu��ۑ����邽�߂̃I�u�W�F�N�g
    private GameObject m_markPrefab;

    [SerializeField] //������s���ΏۂƂ̋���
    private float m_targetDistance = 40.0f;

    [SerializeField] //�}�[�N����I�u�W�F�N�g�̃^�O
    private string[] m_markTargetTags;

    [SerializeField]//�]���r��Y�����̒��S�i��������̋����j
    private float m_zombieCenterY = 2.0f;


    //�����ɂ����I�u�W�F�N�g���}�[�N����
    public void RangeMark()
    {
        foreach(var tag_name in m_markTargetTags)
        {
            //�Ώۂ̃^�O���t�����S�I�u�W�F�N�g
            GameObject[] tag_objs = GameObject.FindGameObjectsWithTag(tag_name);

            //���������ȉ��̃I�u�W�F�N�g�̂ݔ���
            foreach (var obj in tag_objs)
            {
                if (Vector3.Distance(transform.position, obj.transform.position) > m_targetDistance) continue;

                Vector3 mark_pos = obj.transform.position + Vector3.up * m_zombieCenterY;
                //�S�]���r�Ƀ}�[�J�[��u��
                Instantiate(m_markPrefab, mark_pos, Quaternion.identity);
            }

        }

    }
}

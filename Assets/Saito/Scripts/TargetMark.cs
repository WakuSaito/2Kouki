using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�[�Q�b�g�}�[�N�N���X
/// �C���X�y�N�^�Ŏw�肵�������̃I�u�W�F�N�g�Ƀ}�[�N�iUI�j��t����
/// </summary>
public class TargetMark : MonoBehaviour
{
    //�ʒu��ۑ����邽�߂̃I�u�W�F�N�g
    [SerializeField] private GameObject m_markPrefab;

    //������s���ΏۂƂ̋���
    [SerializeField] private float m_targetDistance = 40.0f;
    //�}�[�N����I�u�W�F�N�g�̃^�O
    [SerializeField] private string[] m_markTargetTags;

    //�ΏۃI�u�W�F�N�g��Y�����̒��S�i��������̋����j
    [SerializeField] private float m_targetCenterY = 2.0f;

    /// <summary>
    /// �͈̓}�[�N
    /// ���͈͂̑Ώۃ^�O�I�u�W�F�N�g�Ƀ}�[�N��t����
    /// </summary>
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

                //Y�ʒu����
                Vector3 mark_pos = obj.transform.position + Vector3.up * m_targetCenterY;
                //�S�ΏۂɃ}�[�J�[��u��
                Instantiate(m_markPrefab, mark_pos, Quaternion.identity);
            }

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�p�[�c�����蔻��N���X</para>
/// �R���C�_�[���ƂɃA�^�b�`���A�����I�u�W�F�N�g�ɕ�����U�����肪������Ȃ��悤�ɂ���
/// </summary>
public class HitZone : MonoBehaviour
{
    /// <summary>
    /// �e�X�N���v�g���J
    /// </summary>
    public HitMaster Master => m_master;
    HitMaster m_master;

    void Start()
    {
        m_master = GetComponentInParent<HitMaster>();
    }
}

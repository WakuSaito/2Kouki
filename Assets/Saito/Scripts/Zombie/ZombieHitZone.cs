using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�]���r�p�[�c�����蔻��N���X</para>
/// �R���C�_�[���ƂɃA�^�b�`���A�����I�u�W�F�N�g�ɕ�����U�����肪������Ȃ��悤�ɂ���
/// </summary>
public class ZombieHitZone : MonoBehaviour
{
    /// <summary>
    /// �e�X�N���v�g���J
    /// </summary>
    public ZombieHitMaster Master => m_master;
    ZombieHitMaster m_master;

    void Start()
    {
        m_master = GetComponentInParent<ZombieHitMaster>();
    }
}

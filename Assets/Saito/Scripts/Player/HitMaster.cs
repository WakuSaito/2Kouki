using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����蔻��}�X�^�[�N���X
/// �q��HitZone����Ăяo����A�v���C���[�̔�_���[�W�������Ăяo��
/// </summary>
public class HitMaster : MonoBehaviour
{
    player m_playerScript;
    TestPlayerManager m_playerManager;

    //�R���|�[�l���g�擾
    private void Awake()
    {
        m_playerScript = GetComponent<player>();
        m_playerManager = GetComponent<TestPlayerManager>();
    }

    /// <summary>
    /// <para>�_���[�W���󂯂�</para>
    /// �v���C���[�̔�_���[�W�������Ă�
    /// </summary>
    public void TakeDamage()
    {
        // �_���[�W���󂯂鏈���Ƃ�
        Debug.Log("Damage!");

        //�_���[�W�Ăяo��
        if (m_playerScript != null)
            m_playerScript.DamagePlayer();
        else if (m_playerManager != null)
            m_playerManager.Damaged();
    }
}

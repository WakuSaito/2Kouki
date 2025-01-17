using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaManager : MonoBehaviour
{
    public bool m_inSafeAreaFlag = false;

    /// <summary>
    /// �v���C���[�����S�G���A�ɓ�������t���OTRUE
    /// </summary>
    /// <param name="other">�R���C�_�[�ɓ������Ă���</param>
    private void OnTriggerEnter(Collider other)
    {
        if (m_inSafeAreaFlag) return;

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<player>().m_inSafeAreaFlag = true;
            m_inSafeAreaFlag = true;
        }
    }

    /// <summary>
    /// �v���C���[�����S�G���A����o����t���OFALSE
    /// </summary>
    /// <param name="other">�R���C�_�[�ɓ������Ă���</param>
    private void OnTriggerExit(Collider other)
    {
        if (!m_inSafeAreaFlag) return;

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<player>().m_inSafeAreaFlag = false;
            m_inSafeAreaFlag = false;
        }
    }
}

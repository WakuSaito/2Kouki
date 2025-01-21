using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �`���[�g���A���x�[�X�N���X
/// </summary>
public abstract class TutorialBase : MonoBehaviour
{
    //�}�l�[�W���N���X���������Ă���
    protected TutorialManager m_tutorialManager;

    //�ʒu���擾���邽�߂ɏ���
    private GameObject m_playerObj;

    //�I�u�W�F�N�g���̎擾
    private void Awake()
    {
        //�}�l�[�W���N���X�擾
        m_tutorialManager = gameObject.GetComponent<TutorialManager>();

        m_playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// �v���C���[�̈ʒu�擾
    /// </summary>
    /// <returns>�v���C���[�̈ʒu</returns>
    protected Vector3 PlayerPos()
    {
        return m_playerObj.transform.position;
    }

    /// <summary>
    /// <para>�����ݒ�</para>
    /// �}�l�[�W���[�N���X��Start�ŌĂяo�����
    /// </summary>
    public abstract void SetUpPhase();

    /// <summary>
    /// <para>�X�V����</para>
    /// �}�l�[�W���[�N���X��Update�ŌĂяo�����
    /// </summary>
    public abstract void UpdatePhase();

    /// <summary>
    /// <para>�I������</para>
    /// �`���[�g���A���̃t�F�[�Y�ڍs���ɌĂяo�����
    /// </summary>
    public abstract void EndPhase();

}

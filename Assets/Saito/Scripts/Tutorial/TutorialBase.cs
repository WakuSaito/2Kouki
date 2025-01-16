using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TutorialBase : MonoBehaviour
{
    //�}�l�[�W���N���X���������Ă���
    protected TutorialManager m_tutorialManager;

    //�ʒu���擾���邽�߂ɏ���
    private GameObject m_playerObj;

    private void Awake()
    {
        //�}�l�[�W���N���X�擾
        m_tutorialManager = gameObject.GetComponent<TutorialManager>();

        m_playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    //�v���C���[�̈ʒu�擾
    protected Vector3 PlayerPos()
    {
        return m_playerObj.transform.position;
    }


    //�����ݒ�
    public abstract void SetUpPhase();

    //����
    public abstract void UpdatePhase();

    //�I������
    public abstract void EndPhase();

}

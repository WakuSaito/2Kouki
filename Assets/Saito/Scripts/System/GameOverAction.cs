using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAction : MonoBehaviour
{
    private GameObject m_playerObj;
    private GameObject m_dogObj;

    //UI
    [SerializeField] private FadeImage m_fadeOutUI;

    //���X�^�[�g���W
    [SerializeField] private Vector3 m_restartPlayerPos;
    [SerializeField] private Vector3 m_restartDogPos;

    private bool m_isGameOver;

    private void Awake()
    {
        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        m_dogObj = GameObject.FindGameObjectWithTag("Dog");
    }

    private void Update()
    {
        if(m_isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
    }

    /// <summary>
    /// <para>���X�^�[�g</para>
    /// �v���C���[�ƌ������Z�b�g����
    /// </summary>
    public void Restart()
    {
        if (m_isGameOver == false) return;
        
        m_isGameOver = false;
        m_fadeOutUI.FadeOut();

        if (m_playerObj != null)
        {
            m_playerObj.transform.position = m_restartPlayerPos;
            //�Ƃ肠������
            m_playerObj.GetComponent<player>().TakeRest(1.0f, 0.0f);
        }
        
        if(m_dogObj != null)
        {
            m_dogObj.transform.position = m_restartDogPos;
        }
    }

    /// <summary>
    /// <para>�Q�[���I�[�o�[���s</para>
    /// �Q�[���I�[�o�[�����J�n
    /// </summary>
    public void OnGameOver()
    {
        if (m_isGameOver) return;

        m_isGameOver = true;

        //ui�̕\��
        m_fadeOutUI.FadeIn();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearManager : MonoBehaviour
{
    //�N���A�ڕW���W
    [SerializeField] private Vector3 m_targetPos;

    [SerializeField] private TimeController m_timeController;
    [SerializeField]private SceneChanger m_sceneChanger;
    [SerializeField] SoundManager m_soundManager;

    //�t�F�[�h�A�E�g�p
    [SerializeField] private GameObject m_fadeUI;
    //�����ڗp�w���R�v�^�[�I�u�W�F�N�g
    [SerializeField] private GameObject m_helicoptorObj;

    private GameObject m_playerObj;


    //�N���A�\�t���O
    private bool m_canClear = false;

    private void Awake()
    {
        m_playerObj = GameObject.FindGameObjectWithTag("Player");

        m_sceneChanger = GetComponent<SceneChanger>();
    }

    private void Update()
    {
        if(m_canClear)
        {
            //�v���C���[�ƖڕW���W�̋��������ȉ��Ȃ�
            float distance = Vector3.Distance(m_targetPos, m_playerObj.transform.position);
            if (distance < 2.0f)
            {
                m_fadeUI.SetActive(true);
                m_fadeUI.GetComponent<FadeImage>().StartFade();
                OnClear();
                m_canClear = false;
            }
        }
        else
        {
            // ���݂̓����擾
            int dayCount = m_timeController.GetDayCount();

            //3���ڂȂ�����𖞂���
            if (dayCount >= 3)
            {
                CanClearSetUp();
            }

        }
    }

    /// <summary>
    /// �N���A�\�ɂȂ������̐ݒ�
    /// </summary>
    private void CanClearSetUp()
    {
        m_canClear = true;

        //�w����\��
        if (m_helicoptorObj != null)
            m_helicoptorObj.SetActive(true);
    }

    /// <summary>
    /// �N���A
    /// </summary>
    private void OnClear()
    {
        m_soundManager.Play2DSE(m_soundManager.escapeMap);//se

        //�����ۑ�
        StaticVariables.liveingDayCount = m_timeController.GetDayCount();

        //�x�点�ăV�[���ړ�
        StartCoroutine(SceneChange(m_soundManager.escapeMap.length));
    }

    private IEnumerator SceneChange(float _delay)
    {
        Debug.Log("�N���A");
        yield return new WaitForSeconds(_delay);

        m_sceneChanger.LoadResultScene();
    }


}

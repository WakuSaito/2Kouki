using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>�X�^�e�B�b�N�ϐ��N���X</para>
/// �V�[���ԂŎ����z���������ϐ��p
/// </summary>
public class StaticVariables
{
    /// <summary>
    /// �����c��������
    /// </summary>
    public static int liveingDayCount = 1;
}

/// <summary>
/// <para>�V�[���ڍs�N���X</para>
/// �V�[���̐؂�ւ��ƃQ�[���I�����Ǘ�����
/// </summary>
public class SceneChanger : MonoBehaviour
{
    [SerializeField]//�T�E���h
    private SoundManager m_soundManager;

    /// <summary>
    /// <para>���C���V�[���Ɉڍs</para>
    /// �{�^���ŌĂяo�������V�[���؂�ւ� ���O
    /// ���[�h�V�[�����͂���ł���������
    /// </summary>
    public void LoadNextSceneAsync()
    {
        m_soundManager.Play2DSE(m_soundManager.pushButton);//SE�Đ�
        m_soundManager.ChangeBGM(null, 0.6f);//BGM�t�F�[�h�A�E�g
        //SE�����ꂫ������V�[���؂�ւ�
        StartCoroutine(LoadSceneAsync("conflict_saito", m_soundManager.pushButton.length));
    }

    /// <summary>
    /// ���U���g�V�[���Ɉڍs
    /// </summary>
    public void LoadResultScene()
    {
        StartCoroutine(LoadSceneAsync("ResultScene"));
    }
    /// <summary>
    /// �^�C�g���V�[���Ɉڍs
    /// </summary>
    public void LoadTitleScene()
    {
        StartCoroutine(LoadSceneAsync("TitleScene"));
    }
    /// <summary>
    /// �Q�[���I�[�o�[�V�[���Ɉڍs
    /// </summary>
    public void LoadGameOverScene()
    {
        StartCoroutine(LoadSceneAsync("GameOverScene", 2f));
    }

    /// <summary>
    /// <para>�V�[���؂�ւ��R���[�`��</para>
    /// ���[�h�����܂ő҂�
    /// </summary>
    /// <param name="_scene_name">�ڍs��̃V�[����</param>
    /// <param name="_delay">�x������</param>
    IEnumerator LoadSceneAsync(string _scene_name, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);

        //�J�[�\���L�[�\��
        Screen.lockCursor = false;

        AsyncOperation async_load = SceneManager.LoadSceneAsync(_scene_name);

        while (!async_load.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// �Q�[���I��
    /// </summary>
    public void EndGame()
    {

        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
        #else
            Application.Quit();//�Q�[���v���C�I��
        #endif
        
    }
}

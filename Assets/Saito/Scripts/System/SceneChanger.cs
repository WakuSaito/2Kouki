using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //�{�^���ŌĂяo�������V�[���؂�ւ�
    //���[�h�V�[�����͂���ł���������
    public void LoadNextSceneAsync()
    {
        StartCoroutine(LoadSceneAsync("TestScene"));
    }

    //�V�[���؂�ւ��R���[�`���i���[�h�����܂ő҂j
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    //�Q�[���I��
    public void EndGame()
    {

        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
        #else
            Application.Quit();//�Q�[���v���C�I��
        #endif
        
    }
}

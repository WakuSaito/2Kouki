using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]//sound
    private SoundManager soundManager;

    //�{�^���ŌĂяo�������V�[���؂�ւ�
    //���[�h�V�[�����͂���ł���������
    public void LoadNextSceneAsync()
    {
        soundManager.Play2DSE(soundManager.pushButton);
        StartCoroutine(LoadSceneAsync("MainGame", soundManager.pushButton.length));
    }

    public void LoadResultScene()
    {
        StartCoroutine(LoadSceneAsync("ResultScene"));
    }

    public void LoadTitleScene()
    {
        StartCoroutine(LoadSceneAsync("TitleScene"));
    }

    //�V�[���؂�ւ��R���[�`���i���[�h�����܂ő҂j
    IEnumerator LoadSceneAsync(string _sceneName, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticVariables
{
    //�����c��������
    public static int liveingDayCount = 1;

}

public class SceneChanger : MonoBehaviour
{
    [SerializeField]//sound
    private SoundManager mSoundManager;

    //�{�^���ŌĂяo�������V�[���؂�ւ�
    //���[�h�V�[�����͂���ł���������
    public void LoadNextSceneAsync()
    {
        mSoundManager.Play2DSE(mSoundManager.pushButton);
        mSoundManager.ChangeBGM(null, 0.6f);//BGM�t�F�[�h�A�E�g
        StartCoroutine(LoadSceneAsync("MainGame", mSoundManager.pushButton.length));
    }

    public void LoadResultScene()
    {
        StartCoroutine(LoadSceneAsync("ResultScene"));
    }

    public void LoadTitleScene()
    {
        StartCoroutine(LoadSceneAsync("TitleScene"));
    }

    public void LoadGameOverScene()
    {
        StartCoroutine(LoadSceneAsync("GameOverScene", 2f));
    }

    //�V�[���؂�ւ��R���[�`���i���[�h�����܂ő҂j
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

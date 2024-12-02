using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]//sound
    private SoundManager soundManager;

    //ボタンで呼び出したいシーン切り替え
    //ロードシーンをはさんでもいいかも
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

    //シーン切り替えコルーチン（ロード完了まで待つ）
    IEnumerator LoadSceneAsync(string _sceneName, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    //ゲーム終了
    public void EndGame()
    {

        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
        #else
            Application.Quit();//ゲームプレイ終了
        #endif
        
    }
}

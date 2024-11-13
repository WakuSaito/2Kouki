using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //ボタンで呼び出したいシーン切り替え
    //ロードシーンをはさんでもいいかも
    public void LoadNextSceneAsync()
    {
        StartCoroutine(LoadSceneAsync("TestScene"));
    }

    //シーン切り替えコルーチン（ロード完了まで待つ）
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

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

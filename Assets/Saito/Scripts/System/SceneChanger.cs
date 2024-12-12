using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticVariables
{
    //生き残った日数
    public static int liveingDayCount = 1;

}

public class SceneChanger : MonoBehaviour
{
    [SerializeField]//sound
    private SoundManager soundManager;

    //ボタンで呼び出したいシーン切り替え
    //ロードシーンをはさんでもいいかも
    public void LoadNextSceneAsync()
    {
        soundManager.Play2DSE(soundManager.pushButton);
        soundManager.ChangeBGM(null, 0.6f);//BGMフェードアウト
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

    public void LoadGameOverScene()
    {
        StartCoroutine(LoadSceneAsync("GameOverScene", 2f));
    }

    //シーン切り替えコルーチン（ロード完了まで待つ）
    IEnumerator LoadSceneAsync(string _sceneName, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);

        //カーソルキー表示
        Screen.lockCursor = false;

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

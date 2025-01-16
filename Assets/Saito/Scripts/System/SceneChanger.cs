using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// スタティック変数クラス
/// シーン間で持ち越ししたい変数用
/// </summary>
public class StaticVariables
{
    /// <summary>
    /// 生き残った日数
    /// </summary>
    public static int liveingDayCount = 1;
}

/// <summary>
/// シーン移行クラス
/// シーンの切り替えとゲーム終了を管理する
/// </summary>
public class SceneChanger : MonoBehaviour
{
    [SerializeField]//サウンド
    private SoundManager m_soundManager;

    /// <summary>
    /// メインシーンに移行
    /// ボタンで呼び出したいシーン切り替え 名前
    /// ロードシーンをはさんでもいいかも
    /// </summary>
    public void LoadNextSceneAsync()
    {
        m_soundManager.Play2DSE(m_soundManager.pushButton);//SE再生
        m_soundManager.ChangeBGM(null, 0.6f);//BGMフェードアウト
        //SEが流れきったらシーン切り替え
        StartCoroutine(LoadSceneAsync("MainGame", m_soundManager.pushButton.length));
    }

    /// <summary>
    /// リザルトシーンに移行
    /// </summary>
    public void LoadResultScene()
    {
        StartCoroutine(LoadSceneAsync("ResultScene"));
    }
    /// <summary>
    /// タイトルシーンに移行
    /// </summary>
    public void LoadTitleScene()
    {
        StartCoroutine(LoadSceneAsync("TitleScene"));
    }
    /// <summary>
    /// ゲームオーバーシーンに移行
    /// </summary>
    public void LoadGameOverScene()
    {
        StartCoroutine(LoadSceneAsync("GameOverScene", 2f));
    }

    /// <summary>
    /// シーン切り替えコルーチン
    /// ロード完了まで待つ
    /// </summary>
    /// <param name="_scene_name">移行先のシーン名</param>
    /// <param name="_delay">遅延時間</param>
    IEnumerator LoadSceneAsync(string _scene_name, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);

        //カーソルキー表示
        Screen.lockCursor = false;

        AsyncOperation async_load = SceneManager.LoadSceneAsync(_scene_name);

        while (!async_load.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void EndGame()
    {

        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
        #else
            Application.Quit();//ゲームプレイ終了
        #endif
        
    }
}

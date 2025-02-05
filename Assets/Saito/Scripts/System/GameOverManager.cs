using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームオーバー処理クラス
/// ゲームオーバー時のUI、リスタートの処理を管理
/// </summary>
public class GameOverManager : MonoBehaviour
{
    private GameObject m_playerObj;
    private GameObject m_dogObj;

    //オブジェクト停止クラス
    [SerializeField] private StopObjectAction m_stopObjectAction;

    //UI
    [SerializeField] private FadeImage m_fadeOutUI;
    [SerializeField] private GameOverUI m_gameOverUI;

    //リスタート座標
    [SerializeField] private Vector3 m_restartPlayerPos;
    [SerializeField] private Vector3 m_restartDogPos;

    private bool m_isGameOver;

    [SerializeField] private bool m_debugOnGameover = false;

    private void Awake()
    {
        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        m_dogObj = GameObject.FindGameObjectWithTag("Dog");
    }

    private void Update()
    {
        //デバッグ用
        if (m_debugOnGameover)
        {
            m_debugOnGameover = false;
            OnGameOver();
        }

        //Rでリスタート
        if (m_isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
    }

    /// <summary>
    /// <para>ゲームオーバー実行</para>
    /// ゲームオーバー処理開始
    /// </summary>
    public void OnGameOver()
    {
        if (m_isGameOver) return;
        m_isGameOver = true;

        Debug.Log("ゲームオーバー");

        //オブジェクトの停止
        m_stopObjectAction.ChangeStopState(true);

        //uiアニメーション
        float anim_sec =　m_fadeOutUI.FadeIn();
        //上のアニメーションが終わるころに始める
        m_gameOverUI.OnActive(anim_sec);
    }
    /// <summary>
    /// <para>リスタート</para>
    /// プレイヤーと犬をリセットする
    /// </summary>
    public void Restart()
    {
        if (m_isGameOver == false) return;
        m_isGameOver = false;

        Debug.Log("リスタート");

        //オブジェクトの停止解除
        m_stopObjectAction.ChangeStopState(false);

        m_fadeOutUI.FadeOut();
        m_gameOverUI.OffActive();

        if (m_playerObj != null)
        {
            m_playerObj.transform.position = m_restartPlayerPos;

            player player_script = m_playerObj.GetComponent<player>();
            //とりあえず回復
            if(player_script != null)
                m_playerObj.GetComponent<player>().TakeRest(1.0f, 0.0f);
        }
        
        if(m_dogObj != null)
        {
            m_dogObj.transform.position = m_restartDogPos;
        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// チュートリアルベースクラス
/// </summary>
public abstract class TutorialBase : MonoBehaviour
{
    //マネージャクラスを所持しておく
    protected TutorialManager m_tutorialManager;

    //位置を取得するために所持
    private GameObject m_playerObj;

    //オブジェクト等の取得
    private void Awake()
    {
        //マネージャクラス取得
        m_tutorialManager = gameObject.GetComponent<TutorialManager>();

        m_playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// プレイヤーの位置取得
    /// </summary>
    /// <returns>プレイヤーの位置</returns>
    protected Vector3 PlayerPos()
    {
        return m_playerObj.transform.position;
    }

    /// <summary>
    /// <para>初期設定</para>
    /// マネージャークラスのStartで呼び出される
    /// </summary>
    public abstract void SetUpPhase();

    /// <summary>
    /// <para>更新処理</para>
    /// マネージャークラスのUpdateで呼び出される
    /// </summary>
    public abstract void UpdatePhase();

    /// <summary>
    /// <para>終了処理</para>
    /// チュートリアルのフェーズ移行時に呼び出される
    /// </summary>
    public abstract void EndPhase();

}

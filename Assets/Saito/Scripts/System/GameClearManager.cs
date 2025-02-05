using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearManager : MonoBehaviour
{
    //クリア目標座標
    [SerializeField] private Vector3 m_targetPos;

    [SerializeField] private TimeController m_timeController;
    [SerializeField]private SceneChanger m_sceneChanger;
    [SerializeField] SoundManager m_soundManager;
    //オブジェクト停止クラス
    [SerializeField] private StopObjectAction m_stopObjectAction;

    //フェードアウト用
    [SerializeField] private GameObject m_fadeUI;
    //見た目用ヘリコプターオブジェクト
    [SerializeField] private GameObject m_helicoptorObj;

    private GameObject m_playerObj;


    //クリア可能フラグ
    private bool m_canClear = false;

    private bool m_isClear = false;

    private void Awake()
    {
        m_playerObj = GameObject.FindGameObjectWithTag("Player");

        m_sceneChanger = GetComponent<SceneChanger>();

        //ヘリを非表示
        if (m_helicoptorObj != null)
            m_helicoptorObj.SetActive(false);
    }

    private void Update()
    {
        if(m_canClear)
        {
            //プレイヤーと目標座標の距離が一定以下なら
            float distance = Vector3.Distance(m_targetPos, m_playerObj.transform.position);
            if (distance < 2.0f)
            {
                m_fadeUI.SetActive(true);
                m_fadeUI.GetComponent<FadeImage>().StartFade();
                OnClear();
            }
        }
        else
        {
            // 現在の日数取得
            int dayCount = m_timeController.GetDayCount();

            //3日目なら条件を満たす
            if (dayCount >= 3)
            {
                CanClearSetUp();
            }

        }
    }

    /// <summary>
    /// クリア可能になった時の設定
    /// </summary>
    private void CanClearSetUp()
    {
        m_canClear = true;

        //ヘリを表示
        if (m_helicoptorObj != null)
            m_helicoptorObj.SetActive(true);
    }

    /// <summary>
    /// クリア
    /// </summary>
    private void OnClear()
    {
        if (m_isClear) return;

        m_isClear = true;

        m_stopObjectAction.ChangeStopState(true);//時間停止
        m_soundManager.Play2DSE(m_soundManager.escapeMap);//se

        //日数保存
        StaticVariables.liveingDayCount = m_timeController.GetDayCount();

        //遅らせてシーン移動
        StartCoroutine(SceneChange(m_soundManager.escapeMap.length));
    }

    private IEnumerator SceneChange(float _delay)
    {
        Debug.Log("クリア");
        yield return new WaitForSeconds(_delay);

        m_sceneChanger.LoadResultScene();
    }


}

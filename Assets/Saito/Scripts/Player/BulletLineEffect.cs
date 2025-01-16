using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾道エフェクトクラス
/// エフェクトにアタッチする　次第にフェードアウトする
/// </summary>
public class BulletLineEffect : MonoBehaviour
{
    //フェードアウトする速度
    [SerializeField] float m_fadeOutSpeed = 1.0f;

    //現在のカラーのアルファ値
    private float m_currentAlpha;
    //元の色
    private Color m_originColor;

    //初期の色情報取得
    private void Awake()
    {
        //元のカラー情報保存
        m_originColor = gameObject.GetComponent<Renderer>().material.color;
        //カラーのアルファ値取得
        m_currentAlpha = m_originColor.a;
    }

    // フェードアウトさせる
    void Update()
    {
        m_currentAlpha -= m_fadeOutSpeed * Time.deltaTime;

        //アルファ値が0以下になるなら削除
        if(m_currentAlpha <= 0)
        {
            Destroy(gameObject);
        }

        //アルファ値変更
        gameObject.GetComponent<Renderer>().material.color = 
            new Color(m_originColor.r, m_originColor.g, m_originColor.b, m_currentAlpha);
    }
}

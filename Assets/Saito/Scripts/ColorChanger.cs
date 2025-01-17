using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>色変更クラス</para>
/// 変更したいオブジェクトにアタッチ 複数パーツに分かれているオブジェクトに対応
/// meshの2番目のマテリアルのα値を変更する
/// </summary>
public class ColorChanger : MonoBehaviour
{
    //現在の色のアルファ値
    private float m_currentAlpha;

    [SerializeField]//Meshがアタッチされたオブジェクト
    GameObject[] m_meshObjs;

    //最初のカラーのアルファ値保存
    private void Awake()
    {
        if (m_meshObjs.Length == 0) return;

        //カラーのアルファ値取得
        m_currentAlpha = m_meshObjs[0].GetComponent<Renderer>().materials[1].color.a;
    }

    /// <summary>
    /// 色のアルファ値変更
    /// マテリアルのカラーの透明度を変更する
    /// </summary>
    /// <param name="_alpha">アルファ値(透明度)</param>
    public void ChangeColorAlpha(float _alpha)
    {
        if (m_meshObjs.Length == 0) return;

        //色が変わらない場合処理を行わないようにする
        if (m_currentAlpha == _alpha) return;
        m_currentAlpha = _alpha;

        //色変更
        foreach(var mesh in m_meshObjs)
        {
            Color current_color = mesh.GetComponent<Renderer>().materials[1].color;
            mesh.GetComponent<Renderer>().materials[1].color = new Color(current_color.r, current_color.g, current_color.b, _alpha);
        }
    }
}

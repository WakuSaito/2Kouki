using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    //現在の色のアルファ値
    private float currentAlpha;

    [SerializeField]//Meshがアタッチされたオブジェクト
    GameObject meshObj;

    private void Awake()
    {
        //カラーのアルファ値取得
        currentAlpha = meshObj.GetComponent<Renderer>().materials[1].color.a;
    }

    //色のアルファ値変更
    public void ChangeColorAlpha(float _alpha)
    {
        //色が変わらない場合処理を行わないようにする
        if (currentAlpha == _alpha) return;
        currentAlpha = _alpha;

        Color currentColor = meshObj.GetComponent<Renderer>().materials[1].color;
        meshObj.GetComponent<Renderer>().materials[1].color = new Color(currentColor.r, currentColor.g, currentColor.b, _alpha);
    }
}

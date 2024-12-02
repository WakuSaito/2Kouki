using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    //現在の色のアルファ値
    private float currentAlpha;

    [SerializeField]//Meshがアタッチされたオブジェクト
    GameObject[] meshObjs;

    private void Awake()
    {
        //カラーのアルファ値取得
        currentAlpha = meshObjs[0].GetComponent<Renderer>().materials[1].color.a;
    }

    //色のアルファ値変更
    public void ChangeColorAlpha(float _alpha)
    {
        //色が変わらない場合処理を行わないようにする
        if (currentAlpha == _alpha) return;
        currentAlpha = _alpha;

        //色変更
        foreach(var mesh in meshObjs )
        {
            Color currentColor = mesh.GetComponent<Renderer>().materials[1].color;
            mesh.GetComponent<Renderer>().materials[1].color = new Color(currentColor.r, currentColor.g, currentColor.b, _alpha);
        }
    }
}

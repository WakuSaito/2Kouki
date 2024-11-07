using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineEffect : MonoBehaviour
{
    //フェードアウトする速度
    [SerializeField] float fadeOutSpeed = 1.0f;

    //現在のカラーのアルファ値
    private float currentAlpha;

    //元の色
    private Color originColor;

    private void Awake()
    {
        //元のカラー情報保存
        originColor = gameObject.GetComponent<Renderer>().material.color;

        //カラーのアルファ値取得
        currentAlpha =originColor.a;
    }

    // Update is called once per frame
    void Update()
    {
        currentAlpha -= fadeOutSpeed * Time.deltaTime;

        //アルファ値が0以下になるなら削除
        if(currentAlpha <= 0)
        {
            Destroy(gameObject);
        }

        //アルファ値変更
        gameObject.GetComponent<Renderer>().material.color = 
            new Color(originColor.r, originColor.g, originColor.b, currentAlpha);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineEffect : MonoBehaviour
{
    //フェードアウトする速度
    [SerializeField] float mFadeOutSpeed = 1.0f;

    //現在のカラーのアルファ値
    private float mCurrentAlpha;

    //元の色
    private Color mOriginColor;

    private void Awake()
    {
        //元のカラー情報保存
        mOriginColor = gameObject.GetComponent<Renderer>().material.color;

        //カラーのアルファ値取得
        mCurrentAlpha = mOriginColor.a;
    }

    // Update is called once per frame
    void Update()
    {
        mCurrentAlpha -= mFadeOutSpeed * Time.deltaTime;

        //アルファ値が0以下になるなら削除
        if(mCurrentAlpha <= 0)
        {
            Destroy(gameObject);
        }

        //アルファ値変更
        gameObject.GetComponent<Renderer>().material.color = 
            new Color(mOriginColor.r, mOriginColor.g, mOriginColor.b, mCurrentAlpha);
    }
}

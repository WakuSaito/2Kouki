using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    private Transform canvas;
    private Camera cameraObj;

    //画面に表示するマーカーUI
    [SerializeField] GameObject markUIPrefab;
    //距離表示用のテキスト
    GameObject distanceTextUI;

    [SerializeField] float destroySec = 3.0f;

    //生成したオブジェクト保存用
    private GameObject markUI;

    //削除フラグ
    private bool onDelete = false;

    private float currentAlpha; 

    // Start is called before the first frame update
    void Start()
    {
        //一定時間後削除
        StartCoroutine(DerayDestroy());

        canvas = GameObject.Find("Canvas").transform;
        cameraObj = Camera.main;
        
        //UI生成
        markUI = Instantiate(markUIPrefab, canvas);
        //テキスト取得
        distanceTextUI = markUI.transform.GetChild(0).gameObject;

        //アルファ値取得
        currentAlpha = markUI.GetComponent<Image>().color.a;

        //このオブジェクトの位置をCanvas用に変換し、UIの位置を変更
        Vector2 screenPosition = GetScreenPosition(transform.position);
        Vector2 localPosition = GetCanvasLocalPosition(screenPosition);
        markUI.transform.localPosition = localPosition;
    }

    //Update is called once per frame
    void Update()
    {
        //カメラまでのベクトル
        Vector3 cameraNormal = Vector3.Normalize(transform.position - cameraObj.transform.position);
        //カメラの視点方向とカメラまでのベクトルの内積
        float dot = Vector3.Dot(cameraNormal, cameraObj.transform.forward);

        //内積が一定以上（カメラに映る範囲）
        if (dot > 0.6f)
        {
            //UI表示
            markUI.SetActive(true);
            //このオブジェクトの位置とカメラ位置からキャンバス座標を求める
            Vector2 screenPosition = GetScreenPosition(transform.position);
            Vector2 localPosition = GetCanvasLocalPosition(screenPosition);
            markUI.transform.localPosition = localPosition;//移動
        }
        else
        {
            //無効化
            markUI.SetActive(false);
        }

        //テキスト変更
        if(distanceTextUI != null)
        {
            //距離をintで表示
            string distanceText =  ((int)GetCameraDistance()).ToString() + "m";
            distanceTextUI.GetComponent<Text>().text = distanceText;
        }

        //フェードアウト
        if(onDelete)
        {
            //アルファ値減らす
            currentAlpha -= 1.0f * Time.deltaTime;

            //0以下で削除
            if(currentAlpha <= 0)
            {
                //UIとこのオブジェクトを削除
                Destroy(markUI);
                Destroy(gameObject);
            }

            //カラー変更
            Color color = markUI.GetComponent<Image>().color;
            markUI.GetComponent<Image>().color = new Color(color.r,color.g,color.b,currentAlpha);
        }
    }

    //一定時間後削除
    private IEnumerator DerayDestroy()
    {
        yield return new WaitForSeconds(destroySec);

        onDelete = true;//削除フラグ
    }

    private float GetCameraDistance()
    {
        return Vector3.Distance(transform.position, cameraObj.transform.position);
    }

    //スクリーン座標をキャンバス座標に変換
    private Vector2 GetCanvasLocalPosition(Vector2 screenPosition)
    {
        return canvas.transform.InverseTransformPoint(screenPosition);
    }
    //ワールド座標をスクリーン座標（カメラ基準の座標）に変換
    private Vector2 GetScreenPosition(Vector3 worldPosition)
    {
        return RectTransformUtility.WorldToScreenPoint(cameraObj, worldPosition);
    }

}
    

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    private Transform m_canvas;
    private Camera m_cameraObj;

    //画面に表示するマーカーUI
    [SerializeField] 
    GameObject m_markUIPrefab;
    //距離表示用のテキスト
    GameObject m_distanceTextUI;

    //生成されて削除されるまでの時間（０以下で削除されない）
    [SerializeField] 
    float m_destroySec = 3.0f;
    //フェードアウトの速度
    [SerializeField]
    float m_fadeOutSpeed = 1.0f;

    //生成したオブジェクト保存用
    private GameObject m_markUI;

    //削除フラグ
    private bool m_onDelete = false;

    private float m_currentAlpha; 

    // Start is called before the first frame update
    void Start()
    {
        //削除までの時間が0以下なら削除しない
        if(m_destroySec > 0)
            //一定時間後削除
            StartCoroutine(DerayDestroy());

        m_canvas = GameObject.Find("Canvas").transform;
        m_cameraObj = Camera.main;

        //UI生成
        m_markUI = Instantiate(m_markUIPrefab, m_canvas);
        //テキスト取得
        m_distanceTextUI = m_markUI.transform.GetChild(0).gameObject;

        //アルファ値取得
        m_currentAlpha = m_markUI.GetComponent<Image>().color.a;

        //このオブジェクトの位置をCanvas用に変換し、UIの位置を変更
        Vector2 screen_position = GetScreenPosition(transform.position);
        Vector2 local_position = GetCanvasLocalPosition(screen_position);
        m_markUI.transform.localPosition = local_position;
    }

    //Update is called once per frame
    void Update()
    {
        //カメラまでのベクトル
        Vector3 camera_normal = Vector3.Normalize(transform.position - m_cameraObj.transform.position);
        //カメラの視点方向とカメラまでのベクトルの内積
        float dot = Vector3.Dot(camera_normal, m_cameraObj.transform.forward);

        //内積が一定以上（カメラに映る範囲）
        if (dot > 0.6f)
        {
            //UI表示
            m_markUI.SetActive(true);
            //このオブジェクトの位置とカメラ位置からキャンバス座標を求める
            Vector2 screen_position = GetScreenPosition(transform.position);
            Vector2 local_position = GetCanvasLocalPosition(screen_position);
            m_markUI.transform.localPosition = local_position;//移動
        }
        else
        {
            //無効化
            m_markUI.SetActive(false);
        }

        //テキスト変更
        if(m_distanceTextUI != null)
        {
            //距離をintで表示
            string distance_text =  ((int)GetCameraDistance()).ToString() + "m";
            m_distanceTextUI.GetComponent<Text>().text = distance_text;
        }

        //フェードアウト
        if(m_onDelete)
        {
            //アルファ値減らす
            m_currentAlpha -= m_fadeOutSpeed * Time.deltaTime;

            //0以下で削除
            if(m_currentAlpha <= 0)
            {
                //UIとこのオブジェクトを削除
                Destroy(m_markUI);
                Destroy(gameObject);
            }

            //カラー変更
            Color color = m_markUI.GetComponent<Image>().color;
            m_markUI.GetComponent<Image>().color = new Color(color.r,color.g,color.b, m_currentAlpha);
            Color text_color = m_distanceTextUI.GetComponent<Text>().color;
            m_distanceTextUI.GetComponent<Text>().color = new Color(text_color.r, text_color.g, text_color.b, m_currentAlpha);
        }
    }

    //一定時間後削除
    private IEnumerator DerayDestroy()
    {
        yield return new WaitForSeconds(m_destroySec);

        m_onDelete = true;//削除フラグ
    }
    //強制削除
    public void StartDelete()
    {
        m_onDelete = true;
    }

    private float GetCameraDistance()
    {
        return Vector3.Distance(transform.position, m_cameraObj.transform.position);
    }

    //スクリーン座標をキャンバス座標に変換
    private Vector2 GetCanvasLocalPosition(Vector2 _screen_pos)
    {
        return m_canvas.transform.InverseTransformPoint(_screen_pos);
    }
    //ワールド座標をスクリーン座標（カメラ基準の座標）に変換
    private Vector2 GetScreenPosition(Vector3 _world_pos)
    {
        return RectTransformUtility.WorldToScreenPoint(m_cameraObj, _world_pos);
    }

}
    

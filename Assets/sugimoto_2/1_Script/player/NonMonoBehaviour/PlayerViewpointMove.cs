using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerViewpointMove
{
    /// <summary> Y軸の制限 </summary>
    const int ROT_Y_MAX = 60;

    /*[SerializeField] インスペクターから設定*/
    /// <summary> X方向に回転させたいオブジェクト </summary>
    [SerializeField] GameObject m_rotXObj;
    /// <summary> Y方向に回転させたいオブジェクト </summary>
    [SerializeField] GameObject m_rotYObj;
    /// <summary> カメラ感度 </summary>
    [SerializeField] float m_cameraSensitivity;


    /*プライベート　private*/
    /// <summary> 回転量：X </summary>
    float m_rotX = 0.0f;
    /// <summary> 回転量：Y </summary>
    float m_rotY = 0.0f;


    /// <summary>
    /// 視点移動
    /// </summary>
    public void ViewpointMove()
    {
        //マウスの動きから移動量（回転量）を計算
        m_rotX += Input.GetAxis("Mouse X") * m_cameraSensitivity * Time.deltaTime;
        m_rotY += Input.GetAxis("Mouse Y") * m_cameraSensitivity * Time.deltaTime;

        //Y軸は移動制限をする
        m_rotY = Mathf.Clamp(m_rotY, -ROT_Y_MAX, ROT_Y_MAX);

        m_rotXObj.transform.localRotation = Quaternion.AngleAxis(m_rotX, Vector3.up);
        m_rotYObj.transform.localRotation = Quaternion.AngleAxis(m_rotY, Vector3.left);//*を外した
    }
}

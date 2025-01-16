using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//現在の停止案
//インタフェースを実装し、止めたいスクリプトで実装する
//singltonクラスを実装し、止めたいスクリプト側で状態を監視する 今のところこれ系がよさそう　ただsingletonじゃなくていいかも
//停止させたいクラス名を羅列し、enabled=falseにする（現状の形だとバグが出やすい）

public interface IStopObject
{
    /// <summary>
    /// 一時停止
    /// </summary>
    public void Pause();

    /// <summary>
    /// 再開
    /// </summary>
    public void Resume();
}


public class StopObjectAction : MonoBehaviour
{
    [SerializeField]//インベントリ
    private Inventory m_inventory;

    //現在の停止状態
    private bool m_currentStopState = false;

    private void Update()
    {
        bool new_stop_bool = m_currentStopState;

        if(m_inventory != null)
        {
            //インベントリを開いているとき
            if(m_inventory.item_inventory_flag == true)
            {
                new_stop_bool = true;
            }
            else
            {
                new_stop_bool = false;
            }
        }
        //デバッグ用
        else if (Input.GetKeyDown(KeyCode.P))
        {
            new_stop_bool = !m_currentStopState;
        }

        //変わらなければ終了
        if (new_stop_bool == m_currentStopState) return;

        //IStopObjectを全取得
        var stop_inter_faces = InterfaceUtils.FindObjectOfInterfaces<IStopObject>();

        //全IStopObjectの停止状態変更する
        foreach(var stopI in stop_inter_faces)
        {
            if (stopI == null) continue;

            if (new_stop_bool)
                stopI.Pause();//一時停止
            else
                stopI.Resume();//再開
        }
        Debug.Log("停止状態:" + new_stop_bool);
        m_currentStopState = new_stop_bool;
    }    
}

/// <summary>
/// Interface便利クラス
/// </summary>
public class InterfaceUtils
{
    /// <summary>
    /// 特定のインタフェースがアタッチされたオブジェクトを見つける
    /// </summary>
    /// <typeparam name="T"> 探索するインタフェース </typeparam>
    /// <returns> 取得したクラス配列 </returns>
    public static T[] FindObjectOfInterfaces<T>() where T : class
    {
        List<T> find_list = new List<T>();

        // オブジェクトを探索し、リストに格納
        foreach (var component in GameObject.FindObjectsOfType<Component>())
        {
            var obj = component as T;

            if (obj == null) continue;

            find_list.Add(obj);
        }

        T[] find_obj_array = new T[find_list.Count];
        int count = 0;

        // 取得したオブジェクトを指定したインタフェース型配列に格納
        foreach (T obj in find_list)
        {
            find_obj_array[count] = obj;
            count++;
        }
        return find_obj_array;
    }
}
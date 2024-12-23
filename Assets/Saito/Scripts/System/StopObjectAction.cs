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
    private Inventory inventory;

    //現在の停止状態
    private bool currentStopState = false;

    private void Update()
    {
        bool newStopBool = currentStopState;

        if(inventory != null)
        {
            //インベントリを開いているとき
            if(inventory.item_inventory_flag == true)
            {
                newStopBool = true;
            }
            else
            {
                newStopBool = false;
            }
        }
        //デバッグ用
        else if (Input.GetKeyDown(KeyCode.P))
        {
            newStopBool = !currentStopState;
        }

        //変わらなければ終了
        if (newStopBool == currentStopState) return;

        //IStopObjectを全取得
        var stopInterfaces = InterfaceUtils.FindObjectOfInterfaces<IStopObject>();

        //全IStopObjectの停止状態変更する
        foreach(var stopI in stopInterfaces)
        {
            if (stopI == null) continue;

            if (newStopBool)
                stopI.Pause();//一時停止
            else
                stopI.Resume();//再開
        }
        Debug.Log("停止状態:" + newStopBool);
        currentStopState = newStopBool;
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
        List<T> findList = new List<T>();

        // オブジェクトを探索し、リストに格納
        foreach (var component in GameObject.FindObjectsOfType<Component>())
        {
            var obj = component as T;

            if (obj == null) continue;

            findList.Add(obj);
        }

        T[] findObjArray = new T[findList.Count];
        int count = 0;

        // 取得したオブジェクトを指定したインタフェース型配列に格納
        foreach (T obj in findList)
        {
            findObjArray[count] = obj;
            count++;
        }
        return findObjArray;
    }
}
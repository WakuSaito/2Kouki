using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//現在の停止案
//インタフェースを実装し、止めたいスクリプトで実装する
//singltonクラスを実装し、止めたいスクリプト側で状態を監視する
//停止させたいクラス名を羅列し、enabled=falseにする（現状の形だとバグが出やすい）

public class StopObjectAction : MonoBehaviour
{
    public bool IsStopAction { get; set; } = false;



}

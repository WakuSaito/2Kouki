using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 犬ベースクラス
/// マネージャークラスで管理するスクリプトで継承する
/// </summary>
public abstract class DogBase : MonoBehaviour
{
    /// <summary>
    /// 初期設定
    /// 初期設定用の共通処理
    /// </summary>
    public abstract void SetUpDog();
}

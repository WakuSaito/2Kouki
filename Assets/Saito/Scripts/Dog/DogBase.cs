using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>犬ベースクラス</para>
/// マネージャークラスで管理するスクリプトで継承する
/// </summary>
public abstract class DogBase : MonoBehaviour
{
    /// <summary>
    /// <para>初期設定</para>
    /// 初期設定用の共通処理
    /// </summary>
    public abstract void SetUpDog();
}

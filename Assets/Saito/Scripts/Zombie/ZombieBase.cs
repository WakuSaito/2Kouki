using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゾンビ用の抽象クラス
/// </summary>
public abstract class ZombieBase : MonoBehaviour
{
    /// <summary>
    /// 初期設定用の共通処理
    /// </summary>
    public abstract void SetUpZombie();
}

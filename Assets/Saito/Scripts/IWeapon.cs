using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器インターフェース
/// 武器（手に持てるもの）を持った時、しまったときの処理実装用
/// </summary>
public interface IWeapon
{
    /// <summary>
    /// 仕舞う
    /// 武器が有効化されたとき
    /// </summary>
    public void PutAway();

    /// <summary>
    /// 取り出す
    /// 武器が収納されるとき
    /// </summary>
    public void PutOut();

}

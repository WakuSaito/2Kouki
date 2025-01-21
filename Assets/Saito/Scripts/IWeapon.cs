using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>武器インターフェース</para>
/// 武器（手に持てるもの）を持った時、しまったときの処理実装用
/// </summary>
public interface IWeapon
{
    /// <summary>
    /// <para>仕舞う</para>
    /// 武器が有効化されたとき
    /// </summary>
    public void PutAway();

    /// <summary>
    /// <para>取り出す</para>
    /// 武器が収納されるとき
    /// </summary>
    public void PutOut();

}

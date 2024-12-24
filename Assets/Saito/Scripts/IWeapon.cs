using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//武器用インターフェース
public interface IWeapon
{
    /// <summary>
    /// 仕舞う
    /// </summary>
    public void PutAway();

    /// <summary>
    /// 取り出す
    /// </summary>
    public void PutOut();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
後ほどクラスを分割したい
・移動クラス
・アニメーションクラス
・その他アクションごとに分割
*/

/// <summary>
/// 犬のマネージャークラス
/// </summary>
public class DogManager : MonoBehaviour
{
    //攻撃対象オブジェクト
    private GameObject attackTargetObj;

    //攻撃対象に突進中
    private bool isChargeTarget = false;
    //行動停止
    private bool isStopAction = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isChargeTarget)//突進
        {
            //攻撃対象が存在しないならreturn
            //攻撃対象が途中でDestroyされた場合の挙動注意
            if (attackTargetObj == null)
            {
                isChargeTarget = false;//突進中断
                return;
            }

            //attackTargetObjの位置に向かって移動する

            //攻撃対象に限りなく近づいたら
            //噛みつく
        }
        else//通常時の移動
        {
            //if(プレイヤーとの距離が離れている)
            //プレイヤーの方に移動する
            //障害物の判定に注意

            //else if(近くなら)
            //とりあえずは何もしない
            //プレイヤーの近くでウロウロさせたい　範囲内のランダム位置を目標として移動みたいな
        }
    }

    /// <summary>
    /// 攻撃指示を受けたとき
    /// </summary>
    public void OrderAttack(GameObject _obj)
    {
        isChargeTarget = true;

        attackTargetObj = _obj;//攻撃対象を取得
    }

    /// <summary>
    /// ゾンビに噛みつく
    /// </summary>
    private void BiteZombie(GameObject _zombieObj)
    {
        //attackTargetObjからZombieManagerを取得し、FreezePositionを呼び出し

        isStopAction = true;
    }
}

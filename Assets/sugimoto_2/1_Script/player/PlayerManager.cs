using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //クラス作成
    [SerializeField] GaugeTest m_foodGage;
    [SerializeField] GaugeTest m_hpGage;
    [SerializeField] PlayerAnimation m_anim;
    [SerializeField] PlayerMove m_move;
    [SerializeField] PlayerViewpointMove m_viewpointMove;
    [SerializeField] PlayerAttack m_attack;
    [SerializeField] PlayerPickUpItem m_pickUp;

    [SerializeField] GameObject m_inventoryManagerObj;
    InventoryManager m_inventoryManager;

    // Start is called before the first frame update
    void Start()
    {
        //ゲージの初期設定
        m_foodGage.SetGauge();
        m_hpGage.SetGauge();
        //アニメーター設定
        m_anim.SetAnim();
        //移動設定
        m_move.SetMove();
        //攻撃設定
        m_attack.SetAttack();
        //アイテム取得設定
        m_pickUp.SetPickUp();

        m_inventoryManager = m_inventoryManagerObj.GetComponent<InventoryManager>();

        //カーソルキー非表示
        Screen.lockCursor = true;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲージ処理
        {
            //食料ゲージを定期的に減らす
            m_foodGage.SubGaugeFixed(1.0f);

            //食料ゲージがなくなればHPを減らす
            if (m_foodGage.NonGauge())
            {
                m_hpGage.SubGaugeFixed(1.0f);
            }
        }

        //視点移動
        {
            m_viewpointMove.ViewpointMove();
        }

        //移動処理
        {
            //移動ベクトル設定
            m_move.MoveForwardVec(Input.GetKey(KeyCode.W));
            m_move.MoveLeftVec(Input.GetKey(KeyCode.A));
            m_move.MoveBackVec(Input.GetKey(KeyCode.S));
            m_move.MoveRightVec(Input.GetKey(KeyCode.D));

            //走っていない場合：走らせるかどうかを設定
            if (!m_move.RunFlag())
            {
                m_move.SetUpRun(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.LeftShift));
                m_move.SetUpRun(Input.GetKeyDown(KeyCode.W));
            }

            //座標更新
            m_move.AddVelocityVec();
        }

        //攻撃処理
        {
            //ナイフ
            m_attack.AttackKnife        (Input.GetMouseButtonDown(0));
            //銃
            m_attack.GunReload          (Input.GetKeyDown(KeyCode.R));  //リロード
            m_attack.AttackGunSingle    (Input.GetMouseButtonDown(0));  //単発
            m_attack.AttackGunRapidFire (Input.GetMouseButton(0));      //連射
            //犬
            m_attack.AttackDog          (Input.GetMouseButtonDown(0));  //攻撃
            m_attack.SearchSkillDog     (Input.GetMouseButtonDown(1));  //探知
        }

        //アイテム取得
        {
            GameObject all_get_item = m_pickUp.PickUpItem(Input.GetMouseButtonDown(1));
            if (all_get_item != null) Destroy(all_get_item);
        }


    }
}

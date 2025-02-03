using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IStopObject
{
    [SerializeField] GaugeTest m_foodGage;
    [SerializeField] GaugeTest m_hpGage;
    [SerializeField] PlayerAnimation m_anim;
    [SerializeField] PlayerMove m_move;
    [SerializeField] PlayerViewpointMove m_viewpointMove;
    [SerializeField] PlayerAttack m_playerAttack;

    public void Pause()
    {
        throw new System.NotImplementedException();
    }

    public void Resume()
    {
        throw new System.NotImplementedException();
    }

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
        m_playerAttack.SetAttack();
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
            m_move.SetMoveVec(Input.GetKey(KeyCode.W), KeyCode.W);
            m_move.SetMoveVec(Input.GetKey(KeyCode.A), KeyCode.A);
            m_move.SetMoveVec(Input.GetKey(KeyCode.S), KeyCode.S);
            m_move.SetMoveVec(Input.GetKey(KeyCode.D), KeyCode.D);

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
            m_playerAttack.AttackKnife(Input.GetMouseButtonDown(0));
            //銃
            m_playerAttack.GunReload(Input.GetKeyDown(KeyCode.R));
            m_playerAttack.AttackGunSingle(Input.GetMouseButtonDown(0));
            m_playerAttack.AttackGunRapidFire(Input.GetMouseButton(0));
        }
    }
}

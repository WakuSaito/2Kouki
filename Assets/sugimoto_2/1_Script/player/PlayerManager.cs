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
        //�Q�[�W�̏����ݒ�
        m_foodGage.SetGauge();
        m_hpGage.SetGauge();
        //�A�j���[�^�[�ݒ�
        m_anim.SetAnim();
        //�ړ��ݒ�
        m_move.SetMove();
        //�U���ݒ�
        m_playerAttack.SetAttack();
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[�W����
        {
            //�H���Q�[�W�����I�Ɍ��炷
            m_foodGage.SubGaugeFixed(1.0f);

            //�H���Q�[�W���Ȃ��Ȃ��HP�����炷
            if (m_foodGage.NonGauge())
            {
                m_hpGage.SubGaugeFixed(1.0f);
            }
        }

        //���_�ړ�
        {
            m_viewpointMove.ViewpointMove();
        }

        //�ړ�����
        {
            //�ړ��x�N�g���ݒ�
            m_move.SetMoveVec(Input.GetKey(KeyCode.W), KeyCode.W);
            m_move.SetMoveVec(Input.GetKey(KeyCode.A), KeyCode.A);
            m_move.SetMoveVec(Input.GetKey(KeyCode.S), KeyCode.S);
            m_move.SetMoveVec(Input.GetKey(KeyCode.D), KeyCode.D);

            //�����Ă��Ȃ��ꍇ�F���点�邩�ǂ�����ݒ�
            if (!m_move.RunFlag())
            {            
                m_move.SetUpRun(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.LeftShift));
                m_move.SetUpRun(Input.GetKeyDown(KeyCode.W));
            }

            //���W�X�V
            m_move.AddVelocityVec();
        }

        //�U������
        {
            //�i�C�t
            m_playerAttack.AttackKnife(Input.GetMouseButtonDown(0));
            //�e
            m_playerAttack.GunReload(Input.GetKeyDown(KeyCode.R));
            m_playerAttack.AttackGunSingle(Input.GetMouseButtonDown(0));
            m_playerAttack.AttackGunRapidFire(Input.GetMouseButton(0));
        }
    }
}

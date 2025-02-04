using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //�N���X�쐬
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
        //�Q�[�W�̏����ݒ�
        m_foodGage.SetGauge();
        m_hpGage.SetGauge();
        //�A�j���[�^�[�ݒ�
        m_anim.SetAnim();
        //�ړ��ݒ�
        m_move.SetMove();
        //�U���ݒ�
        m_attack.SetAttack();
        //�A�C�e���擾�ݒ�
        m_pickUp.SetPickUp();

        m_inventoryManager = m_inventoryManagerObj.GetComponent<InventoryManager>();

        //�J�[�\���L�[��\��
        Screen.lockCursor = true;
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
            m_move.MoveForwardVec(Input.GetKey(KeyCode.W));
            m_move.MoveLeftVec(Input.GetKey(KeyCode.A));
            m_move.MoveBackVec(Input.GetKey(KeyCode.S));
            m_move.MoveRightVec(Input.GetKey(KeyCode.D));

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
            m_attack.AttackKnife        (Input.GetMouseButtonDown(0));
            //�e
            m_attack.GunReload          (Input.GetKeyDown(KeyCode.R));  //�����[�h
            m_attack.AttackGunSingle    (Input.GetMouseButtonDown(0));  //�P��
            m_attack.AttackGunRapidFire (Input.GetMouseButton(0));      //�A��
            //��
            m_attack.AttackDog          (Input.GetMouseButtonDown(0));  //�U��
            m_attack.SearchSkillDog     (Input.GetMouseButtonDown(1));  //�T�m
        }

        //�A�C�e���擾
        {
            GameObject all_get_item = m_pickUp.PickUpItem(Input.GetMouseButtonDown(1));
            if (all_get_item != null) Destroy(all_get_item);
        }


    }
}

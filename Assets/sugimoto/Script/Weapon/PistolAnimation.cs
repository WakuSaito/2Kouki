using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolAnimation : MonoBehaviour
{
    //�s�X�g�̈ʒu
    public Transform ConstPos_Pistol;
    public Transform ConstPos_Hand_L;
    public Transform ReloadPos_Pistol;

    //IK�̈ʒuL
    public Transform ReloadMiddlePos_Hand_L;
    public Transform ReloadStartPos_Hand_L;

    //�}�K�W���̃g�����X�t�H�[��
    public Transform ConstPos_Magazine;
    public Transform MagazinePos;
    public Transform MagazineParent;

    //�������I�u�W�F�N�g�̏����l�ۑ�
    Transform pistol_obj_start_pos;
    Transform hand_L_obj_start_pos;
    Transform magazine_obj_start_pos;

    //IK�̃g�����X�t�H�[��
    public Transform PistolHandPos_R;
    public Transform PistolHandPos_L;

    //�t���O
    bool ReloadStart_Flag = false;  //reload�J�n
    bool ReloadMiddle_Flag = false; //reload��
    bool ReloadEnd_Flag = false;    //reload�I������
    bool Return_Pos_Flag = false;   //��ʒu�ɖ߂�


    float Timer = 0.0f;
    [SerializeField] float speed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReloadAnimation()
    {
        if (Input.GetKeyDown(KeyCode.R) && !ReloadStart_Flag && !ReloadMiddle_Flag && !ReloadEnd_Flag && !Return_Pos_Flag)  
        {
            //�����[�h�J�n
            ReloadStart_Flag = true;
            //�����l�ۑ�
            pistol_obj_start_pos = transform;
            hand_L_obj_start_pos = PistolHandPos_L;
            //�E����s�X�g���̎q�ɂ���i�����A���̂��߁j
            PistolHandPos_R.parent = transform;
        }

        //�����[�h�J�n
        if (ReloadStart_Flag)
        {
            Timer += Time.deltaTime;

            //�ʒu�X�V�i�����葁������j
            transform.position = Vector3.Lerp(pistol_obj_start_pos.position, ReloadPos_Pistol.position, Timer * speed * 2);
            transform.localRotation = Quaternion.Lerp(pistol_obj_start_pos.localRotation, ReloadPos_Pistol.localRotation, Timer * speed * 2);

            //����IK�X�V�i�s�X�g�����x������j
            PistolHandPos_L.position = Vector3.Lerp(hand_L_obj_start_pos.position, ReloadStartPos_Hand_L.position, Timer * speed * 0.4f);

            if (transform.position == ReloadPos_Pistol.position)
            {
                ReloadStart_Flag = false;
                ReloadMiddle_Flag = true;
                Timer = 0.0f;

                //�}�K�W��������̎q�ɂ���
                MagazinePos.parent = PistolHandPos_L;
            }
        }

        //�����[�h��
        if(ReloadMiddle_Flag)
        {
            Timer += Time.deltaTime;

            //�ʒu�Œ�
            transform.position = ReloadPos_Pistol.position;
            transform.localRotation = ReloadPos_Pistol.localRotation;

            //����IK�X�V
            PistolHandPos_L.position = Vector3.Lerp(ReloadStartPos_Hand_L.position, ReloadMiddlePos_Hand_L.position, Timer * speed);

            if (PistolHandPos_L.position == ReloadMiddlePos_Hand_L.position)
            {
                ReloadMiddle_Flag = false;
                ReloadEnd_Flag = true;
                Timer = 0.0f;

                magazine_obj_start_pos = MagazinePos;
            }
        }

        //�����[�h�I������
        if(ReloadEnd_Flag)
        {
            Timer += Time.deltaTime;

            //�ʒu�Œ�
            transform.position = ReloadPos_Pistol.position;
            transform.localRotation = ReloadPos_Pistol.localRotation;

            //����IK�X�V
            PistolHandPos_L.position = Vector3.Lerp(ReloadMiddlePos_Hand_L.position, ReloadStartPos_Hand_L.position, Timer * speed * 1.5f);
            //sMagazinePos.position = Vector3.Lerp(magazine_obj_start_pos.position, ConstPos_Magazine.position, Timer * speed * 1.5f);

            if (PistolHandPos_L.position == ReloadStartPos_Hand_L.position)
            {
                ReloadEnd_Flag = false;
                Return_Pos_Flag = true;
                Timer = 0.0f;

                //�}�K�W�����s�X�g���̎q(pistol�̒��ɂ���weapon)�ɂ���
                MagazinePos.parent = MagazineParent;
                MagazinePos.position = MagazineParent.position;
            }
        }

        //��ʒu�ɖ߂�
        if(Return_Pos_Flag)
        {
            Timer += Time.deltaTime;

            //�ʒu�X�V
            transform.position = Vector3.Lerp(ReloadPos_Pistol.position, ConstPos_Pistol.position, Timer * speed);
            transform.localRotation = Quaternion.Lerp(ReloadPos_Pistol.localRotation, ConstPos_Pistol.localRotation, Timer * speed);

            //����IK�X�V
            PistolHandPos_L.position = Vector3.Lerp(ReloadStartPos_Hand_L.position, ConstPos_Hand_L.position, Timer * speed);

            if (transform.position == ConstPos_Pistol.position)
            {
                Return_Pos_Flag = false;
                Timer = 0.0f;
            }
        }
    }
}

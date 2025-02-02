using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knifeAttackAnimetion : MonoBehaviour
{
    //��ʒu
    public Transform ConstPos;

    //�U���ʒu
    public Transform AttackStart_Pos;
    public Transform AttackEnd_Pos;
    Transform target_obj_start_pos;

    //�c���G�t�F�N�g
    [SerializeField] GameObject trailEffectObj;

    //�o�ߎ���
    float Timer = 0.0f;

    //animation���x
    [SerializeField] float speed = 3.0f;

    //�t���O
    bool Attack_Start_Flag = false;
    bool Attack_Flag = false;       //�U����
    bool Return_Pos_Flag = false;   //��ʒu�ɖ߂�

    private void Start()
    {
        if (trailEffectObj != null)//�c���I�t
            trailEffectObj.SetActive(false);
    }


    public void AttackAnimation(bool _phsh)
    {
        if (_phsh && !Attack_Flag && !Return_Pos_Flag)
        {
            Attack_Start_Flag = true;
            transform.localRotation = AttackStart_Pos.localRotation;
            target_obj_start_pos = transform;
        }

        if(Attack_Start_Flag)
        {
            Timer += Time.deltaTime;

            //�ʒu�X�V
            transform.position = Vector3.Lerp(target_obj_start_pos.position, AttackStart_Pos.position, Timer * speed);
            transform.localRotation = Quaternion.Lerp(target_obj_start_pos.localRotation, AttackStart_Pos.localRotation, Timer * speed);

            if (transform.position == AttackStart_Pos.position)
            {
                Attack_Start_Flag = false;
                GetComponent<KnifeManager>().StartAttack();
                Attack_Flag = true;
                Timer = 0.0f;
                if (trailEffectObj != null)//�c���I��
                    trailEffectObj.SetActive(true);
            }
        }

        if (Attack_Flag)
        {
            Timer += Time.deltaTime;
            transform.position = Vector3.Lerp(AttackStart_Pos.position, AttackEnd_Pos.position, Timer * speed);
            transform.localRotation = Quaternion.Lerp(AttackStart_Pos.localRotation, AttackEnd_Pos.localRotation, Timer * speed);

            if (transform.position == AttackEnd_Pos.position)
            {
                Attack_Flag = false;
                Return_Pos_Flag = true;
                Timer = 0.0f;
                if (trailEffectObj != null)//�c���I�t
                    trailEffectObj.SetActive(false);
            }
        }

        if (Return_Pos_Flag)
        {
            Timer += Time.deltaTime;
            transform.position = Vector3.Lerp(AttackEnd_Pos.position, ConstPos.position, Timer * speed);
            transform.localRotation = Quaternion.Lerp(AttackEnd_Pos.localRotation, ConstPos.localRotation, Timer * speed);

            if (transform.position == ConstPos.position)
            {
                Return_Pos_Flag = false;
                Timer = 0.0f;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knifeAttackAnimetion : MonoBehaviour
{
    //定位置
    public Transform ConstPos;

    //攻撃位置
    public Transform AttackStart_Pos;
    public Transform AttackEnd_Pos;
    Transform target_obj_start_pos;

    //経過時間
    float Timer = 0.0f;

    //animation速度
    [SerializeField] float speed = 3.0f;

    //フラグ
    bool Attack_Start_Flag = false;
    bool Attack_Flag = false;       //攻撃中
    bool Return_Pos_Flag = false;   //定位置に戻す

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AttackAnimation(GameObject _player)
    {
        if (Input.GetMouseButtonDown(0) && !Attack_Flag && !Return_Pos_Flag)
        {
            Attack_Start_Flag = true;
            transform.localRotation = AttackStart_Pos.localRotation;
            target_obj_start_pos = transform;
            GetComponent<Knife>().Attack(_player);
        }

        if(Attack_Start_Flag)
        {
            Timer += Time.deltaTime;
            transform.position = Vector3.Lerp(target_obj_start_pos.position, AttackStart_Pos.position, Timer * speed);
            transform.localRotation = Quaternion.Lerp(target_obj_start_pos.localRotation, AttackStart_Pos.localRotation, Timer * speed);

            if (transform.position == AttackStart_Pos.position)
            {
                Attack_Start_Flag = false;
                Attack_Flag = true;
                Timer = 0.0f;
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

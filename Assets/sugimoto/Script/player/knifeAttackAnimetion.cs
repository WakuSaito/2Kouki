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

    //�o�ߎ���
    float Timer = 0.0f;

    //animation���x
    [SerializeField] float speed = 3.0f;

    //�t���O
    bool Attack_Flag = false;       //�U����
    bool Return_Pos_Flag = false;   //��ʒu�ɖ߂�

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
            Attack_Flag = true;
            transform.localRotation = AttackStart_Pos.localRotation;
            GetComponent<Knife>().Attack(_player);
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
            transform.position = Vector3.Lerp(AttackEnd_Pos.position, ConstPos.position, Timer * 2);
            transform.localRotation = Quaternion.Lerp(AttackEnd_Pos.localRotation, ConstPos.localRotation, Timer * 2);

            if (transform.position == ConstPos.position)
            {
                Return_Pos_Flag = false;
                Timer = 0.0f;
            }
        }
    }
}

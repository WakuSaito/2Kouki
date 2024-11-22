using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolAnimation : MonoBehaviour
{
    //ピストの位置
    public Transform ConstPos_Pistol;
    public Transform ConstPos_Hand_L;
    public Transform ReloadPos_Pistol;

    //IKの位置L
    public Transform ReloadMiddlePos_Hand_L;
    public Transform ReloadStartPos_Hand_L;

    //マガジンのトランスフォーム
    public Transform ConstPos_Magazine;
    public Transform MagazinePos;
    public Transform MagazineParent;

    //動かすオブジェクトの初期値保存
    Transform pistol_obj_start_pos;
    Transform hand_L_obj_start_pos;
    Transform magazine_obj_start_pos;

    //IKのトランスフォーム
    public Transform PistolHandPos_R;
    public Transform PistolHandPos_L;

    //フラグ
    bool ReloadStart_Flag = false;  //reload開始
    bool ReloadMiddle_Flag = false; //reload中
    bool ReloadEnd_Flag = false;    //reload終了処理
    bool Return_Pos_Flag = false;   //定位置に戻す


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
            //リロード開始
            ReloadStart_Flag = true;
            //初期値保存
            pistol_obj_start_pos = transform;
            hand_L_obj_start_pos = PistolHandPos_L;
            //右手をピストルの子にする（動き連動のため）
            PistolHandPos_R.parent = transform;
        }

        //リロード開始
        if (ReloadStart_Flag)
        {
            Timer += Time.deltaTime;

            //位置更新（左手より早くする）
            transform.position = Vector3.Lerp(pistol_obj_start_pos.position, ReloadPos_Pistol.position, Timer * speed * 2);
            transform.localRotation = Quaternion.Lerp(pistol_obj_start_pos.localRotation, ReloadPos_Pistol.localRotation, Timer * speed * 2);

            //左手IK更新（ピストルより遅くする）
            PistolHandPos_L.position = Vector3.Lerp(hand_L_obj_start_pos.position, ReloadStartPos_Hand_L.position, Timer * speed * 0.4f);

            if (transform.position == ReloadPos_Pistol.position)
            {
                ReloadStart_Flag = false;
                ReloadMiddle_Flag = true;
                Timer = 0.0f;

                //マガジンを左手の子にする
                MagazinePos.parent = PistolHandPos_L;
            }
        }

        //リロード中
        if(ReloadMiddle_Flag)
        {
            Timer += Time.deltaTime;

            //位置固定
            transform.position = ReloadPos_Pistol.position;
            transform.localRotation = ReloadPos_Pistol.localRotation;

            //左手IK更新
            PistolHandPos_L.position = Vector3.Lerp(ReloadStartPos_Hand_L.position, ReloadMiddlePos_Hand_L.position, Timer * speed);

            if (PistolHandPos_L.position == ReloadMiddlePos_Hand_L.position)
            {
                ReloadMiddle_Flag = false;
                ReloadEnd_Flag = true;
                Timer = 0.0f;

                magazine_obj_start_pos = MagazinePos;
            }
        }

        //リロード終了処理
        if(ReloadEnd_Flag)
        {
            Timer += Time.deltaTime;

            //位置固定
            transform.position = ReloadPos_Pistol.position;
            transform.localRotation = ReloadPos_Pistol.localRotation;

            //左手IK更新
            PistolHandPos_L.position = Vector3.Lerp(ReloadMiddlePos_Hand_L.position, ReloadStartPos_Hand_L.position, Timer * speed * 1.5f);
            //sMagazinePos.position = Vector3.Lerp(magazine_obj_start_pos.position, ConstPos_Magazine.position, Timer * speed * 1.5f);

            if (PistolHandPos_L.position == ReloadStartPos_Hand_L.position)
            {
                ReloadEnd_Flag = false;
                Return_Pos_Flag = true;
                Timer = 0.0f;

                //マガジンをピストルの子(pistolの中にあるweapon)にする
                MagazinePos.parent = MagazineParent;
                MagazinePos.position = MagazineParent.position;
            }
        }

        //定位置に戻す
        if(Return_Pos_Flag)
        {
            Timer += Time.deltaTime;

            //位置更新
            transform.position = Vector3.Lerp(ReloadPos_Pistol.position, ConstPos_Pistol.position, Timer * speed);
            transform.localRotation = Quaternion.Lerp(ReloadPos_Pistol.localRotation, ConstPos_Pistol.localRotation, Timer * speed);

            //左手IK更新
            PistolHandPos_L.position = Vector3.Lerp(ReloadStartPos_Hand_L.position, ConstPos_Hand_L.position, Timer * speed);

            if (transform.position == ConstPos_Pistol.position)
            {
                Return_Pos_Flag = false;
                Timer = 0.0f;
            }
        }
    }
}

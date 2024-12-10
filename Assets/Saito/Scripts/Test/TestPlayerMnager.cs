using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestPlayerMnager : MonoBehaviour
{

    private CharacterController characterController;  // CharacterController型の変数
    private Vector3 moveVelocity;  // キャラクターコントローラーを動かすためのVector3型の変数
    private Transform verRot;  //縦の視点移動の変数(カメラに合わせる)
    private Transform horRot;  //横の視点移動の変数(プレイヤーに合わせる)
    [SerializeField] private float walkSpeed = 3.0f;  //移動速度
    [SerializeField] private float dashSpeed = 5.0f;  //移動速度
    [SerializeField] private float jumpPower;  //ジャンプ力

    [SerializeField] private float cameraSpeed = 100;

    [SerializeField] private Transform muzzleTransform;//銃口位置
    [SerializeField] private GameObject bulletLine;//弾道
    [SerializeField] private float bulletDistance = 20.0f;//弾の飛距離
    [SerializeField] private float bulletSpread = 0.03f;//弾ブレ

    [SerializeField] private GameObject cameraObj;

    [SerializeField] private bool activeMouse = false;//マウスの視点移動の有効化

    [SerializeField] private GameObject knifeObj;
    [SerializeField] private GameObject usePistol;

    //弾無限
    [SerializeField] private bool isInfinityBullet;

    private SearchViewArea searchViewArea;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        searchViewArea = GetComponent<SearchViewArea>();

        verRot = cameraObj.transform;
        horRot = transform;
    }

    void Update()
    {
        //常に視点が動かないようにした
        if (Input.GetMouseButtonDown(0))
        {
            activeMouse = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.LeftAlt)) 
        {
            activeMouse = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if(activeMouse)
        {
            // マウスによる視点操作
            float X_Rotation = Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime;
            float Y_Rotation = Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime;
            horRot.transform.Rotate(new Vector3(0, X_Rotation * 2, 0), Space.Self);
            verRot.transform.Rotate(-Y_Rotation * 2, 0, 0, Space.Self);
        }

        Vector3 vec = Vector3.zero;
        float moveSpeed;

        if(Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = dashSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        //Wキーがおされたら
        if (Input.GetKey(KeyCode.W))
        {
            vec += transform.forward;
        }
        //Sキーがおされたら
        if (Input.GetKey(KeyCode.S))
        {
            vec -= transform.forward;
        }
        //Aキーがおされたら
        if (Input.GetKey(KeyCode.A))
        {
            vec -= transform.right;
        }
        //Dキーがおされたら
        if (Input.GetKey(KeyCode.D))
        {
            vec += transform.right;
        }

        //銃
        if (Input.GetMouseButtonDown(0))
        {
            usePistol.GetComponent<GunManager>().PullTriggerDown();
        }
        else if (Input.GetMouseButton(0))
        {
            usePistol.GetComponent<GunManager>().PullTrigger();
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            //アニメーション起動
            usePistol.GetComponent<Animator>().enabled = true;

            //リロード処理
            usePistol.GetComponent<GunManager>().Reload();
        }
        //if(isInfinityBullet)
        //{
        //    usePistol.GetComponent<GunManager>().Reload();
        //}

        characterController.Move(vec.normalized * moveSpeed * Time.deltaTime);


        // 接地しているとき
        if (characterController.isGrounded)
        {
            // ジャンプ
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveVelocity.y = jumpPower;
            }
        }
        // 空中にいる時
        else
        {
            // 重力をかける
            moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

        // キャラクターを動かす
        characterController.Move(moveVelocity * Time.deltaTime);

        //if(Input.GetMouseButtonDown(1) && knifeObj != null)
        //{
        //    knifeObj.GetComponent<KnifeManager>().StartAttack();
        //}

        //休息する
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Vector3 rayVec = cameraObj.transform.forward * 10.0f;

            if (Physics.Raycast(cameraObj.transform.position, rayVec, out hit))
            {
                if(hit.transform.gameObject.tag == "RestPoint")
                {
                    hit.transform.gameObject.GetComponent<ObjRespawn>().RestPlayer();
                }
            }
        }

        //searchViewArea.GetObjUpdate("Zombie", 20f, 2f);
        //searchViewArea.GetObjUpdate("item", 5f);
    }

    

    //銃の処理Update（弾数や連射速度は考慮していない）
}


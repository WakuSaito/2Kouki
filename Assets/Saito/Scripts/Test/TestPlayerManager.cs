using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestPlayerManager : MonoBehaviour
{

    private CharacterController characterController;  // CharacterController型の変数
    private Vector3 moveVelocity;  // キャラクターコントローラーを動かすためのVector3型の変数
    private Transform verRot;  //縦の視点移動の変数(カメラに合わせる)
    private Transform horRot;  //横の視点移動の変数(プレイヤーに合わせる)
    [SerializeField] private float walkSpeed = 3.0f;  //移動速度
    [SerializeField] private float dashSpeed = 5.0f;  //移動速度
    [SerializeField] private float jumpPower;  //ジャンプ力

    [SerializeField] private float cameraSpeed = 100;
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private bool activeMouse = false;//マウスの視点移動の有効化

    [SerializeField] private TestWeaponSlot testWeaponSlot;

    [SerializeField] private DogManager dogManager;

    private GameObject handWeapon;

    private SearchViewArea searchViewArea;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        searchViewArea = GetComponent<SearchViewArea>();

        verRot = cameraObj.transform;
        horRot = transform;    
    }

    private void Start()
    {
        if(testWeaponSlot != null)
            handWeapon = testWeaponSlot.GetSelectWeapon();
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

        //移動
        {
            Vector3 vec = Vector3.zero;
            float moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
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
        }


        //スロット選択
        if (testWeaponSlot != null)
        {
            int scrollVec = (int)Input.mouseScrollDelta.y * -1;
            if (scrollVec != 0)
            {
               testWeaponSlot.SelectSlot(scrollVec);
            }
        }

       
        handWeapon = testWeaponSlot.GetSelectWeapon();
        if (handWeapon != null)
        {
            ItemSetting itemSetting = handWeapon.GetComponent<ItemSetting>();

            if(itemSetting != null)

            switch (itemSetting.iteminfo.id)
            {
                case ITEM_ID.PISTOL:
                case ITEM_ID.ASSAULT:
                case ITEM_ID.SHOTGUN:
                    //銃
                    if (Input.GetMouseButtonDown(0))
                    {
                        handWeapon.GetComponent<GunManager>().PullTriggerDown();
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        handWeapon.GetComponent<GunManager>().PullTrigger();
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        //アニメーション起動
                        handWeapon.GetComponent<Animator>().enabled = true;

                        //リロード処理
                        handWeapon.GetComponent<GunManager>().Reload();
                    }
                    break;
                case ITEM_ID.KNIFE:

                    //if (Input.GetMouseButtonDown(0))
                    //{
                    handWeapon.GetComponent<knifeAttackAnimetion>().AttackAnimation(cameraObj);
                    //}
                    break;
                case ITEM_ID.DOG_DIRECTION:

                    //攻撃するオブジェクト取得
                    GameObject attack_obj = searchViewArea.GetObjUpdate("Zombie", 20f, 0.5f);

                    //攻撃
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (attack_obj != null)
                        {
                            dogManager.OrderAttack(attack_obj.GetComponentInParent<ZombieManager>().gameObject);
                        }
                    }
                    //仮で探知
                    else if (Input.GetMouseButtonDown(1))
                    {
                        dogManager.GetComponent<DogManager>().OrderDetection();
                    }
                    break;
            }

        }

        //視点方向のアイテムを探す
        GameObject item = searchViewArea.GetObjUpdate("item", 1f, 0.03f);

        if (Input.GetMouseButtonDown(1))
        {
            PickUpItem(item);
        }


    }

    void PickUpItem(GameObject _item)  //アイテムを拾う
    {
        if (_item == null) return;

        Destroy(_item);
    }



}

//休息する
//if (Input.GetMouseButtonDown(1))
//{
//    RaycastHit hit;
//    Vector3 rayVec = cameraObj.transform.forward * 10.0f;

//    if (Physics.Raycast(cameraObj.transform.position, rayVec, out hit))
//    {
//        if(hit.transform.gameObject.tag == "RestPoint")
//        {
//            hit.transform.gameObject.GetComponent<ObjRespawn>().RestPlayer();
//        }
//    }
//}
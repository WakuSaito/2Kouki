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

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        verRot = cameraObj.transform;
        horRot = transform;
    }

    void Update()
    { 
        //常に視点が動かないようにした
        if(Input.GetMouseButtonDown(0))
        {
            activeMouse = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            //アニメーション起動
            usePistol.GetComponent<Animator>().enabled = true;

            //リロード処理
            usePistol.GetComponent<GunManager>().Reload();
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


        LockOnUpdate();
    }

    //見ている方向のゾンビをマークするUpdate アイテム用に変更 操作方法も表示したい
    public void LockOnUpdate()
    {
        //全対象タグのオブジェクト weaponも追加したい
        GameObject[] itemObjects =  GameObject.FindGameObjectsWithTag("item");
        GameObject[] weaponObjects = GameObject.FindGameObjectsWithTag("weapon");

        List<GameObject> targetObjects = new List<GameObject>();//うまくいっていない
        targetObjects.CopyTo(itemObjects, 0);
        targetObjects.CopyTo(weaponObjects, itemObjects.Length);

        //対象の条件　後でクラス変数化
        float activeAngle = 20.0f;   //対象となる範囲
        float activeDistance = 20.0f;//対象となる距離

        Vector3 playerPos = transform.position;
        Vector3 cameraPos = cameraObj.transform.position;
        Vector3 eyeDir = cameraObj.transform.forward;//視点方向ベクトル

        List<GameObject> targetItems = new List<GameObject>();

        //距離が一定以下のオブジェクトのみに絞る
        foreach (var item in targetObjects)
        {
            //全てのオブジェクトの色を通常に戻す 処理が重いかも
            item.GetComponent<ColorChanger>().ChangeColorAlpha(0.0f);

            Debug.Log("アイテム発見");
            //距離を調べる
            Vector3 itemPos = item.transform.position;
            
            if (Vector3.Distance(playerPos, itemPos) > activeDistance) continue;

            targetItems.Add(item);//リストに追加
        }

        if(targetItems.Count != 0)
        {
            //オブジェクトの中心位置調整用
            Vector3 itemCenterAd = Vector3.up * 0.0f;

            //対象のオブジェクトの中から視点から角度が一番近いオブジェクトを取得
            GameObject nearestEnemy =
                targetItems.OrderBy(p => 
                Vector3.Angle(((p.transform.position + itemCenterAd) - cameraPos).normalized, eyeDir)).First();

            //Debug.Log("角度:" + Vector3.Angle(((nearestEnemy.transform.position + zombieCenterAd) - cameraPos).normalized, eyeDir));

            //取得したオブジェクトまでと視点の角度が一定以下なら
            if(Vector3.Angle(((nearestEnemy.transform.position + itemCenterAd) - cameraPos).normalized, eyeDir) <= activeAngle)
            {
                //対象の色を変更
                nearestEnemy.GetComponent<ColorChanger>().ChangeColorAlpha(0.25f);

                //操作説明用の表示を出したい
                
                return;
            }

        }
        return;
    }

    //銃の処理Update（弾数や連射速度は考慮していない）
}


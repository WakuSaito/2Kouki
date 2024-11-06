using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestPlayerMnager : MonoBehaviour
{

    private CharacterController characterController;  // CharacterController型の変数
    private Vector3 moveVelocity;  // キャラクターコントローラーを動かすためのVector3型の変数
    [SerializeField] public Transform verRot;  //縦の視点移動の変数(カメラに合わせる)
    [SerializeField] public Transform horRot;  //横の視点移動の変数(プレイヤーに合わせる)
    [SerializeField] public float moveSpeed;  //移動速度
    [SerializeField] public float jumpPower;  //ジャンプ力

    [SerializeField] private float cameraSpeed = 100;

    [SerializeField] private GameObject circle;

    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private GameObject bulletLine;

    [SerializeField] private float bulletSpread = 0.03f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // マウスによる視点操作
        float X_Rotation = Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime;
        float Y_Rotation = Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime;
        //horRot.transform.localRotation = Quaternion.AngleAxis(X_Rotation, Vector3.up);
        //verRot.transform.localRotation = Quaternion.AngleAxis(Y_Rotation, Vector3.left);
        horRot.transform.Rotate(new Vector3(0, X_Rotation * 2, 0), Space.Self);
        verRot.transform.Rotate(-Y_Rotation * 2, 0, 0, Space.Self);

        //Wキーがおされたら
        if (Input.GetKey(KeyCode.W))
        {
            characterController.Move(this.gameObject.transform.forward * moveSpeed * Time.deltaTime);
        }
        //Sキーがおされたら
        if (Input.GetKey(KeyCode.S))
        {
            characterController.Move(this.gameObject.transform.forward * -1f * moveSpeed * Time.deltaTime);
        }
        //Aキーがおされたら
        if (Input.GetKey(KeyCode.A))
        {
            characterController.Move(this.gameObject.transform.right * -1 * moveSpeed * Time.deltaTime);
        }
        //Dキーがおされたら
        if (Input.GetKey(KeyCode.D))
        {
            characterController.Move(this.gameObject.transform.right * moveSpeed * Time.deltaTime);
        }

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

        GunUpdate();

        LockOnUpdate();
    }

    public void LockOnUpdate()
    {
        //全ゾンビオブジェクト
        GameObject[] zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");

        //対象の条件　後でクラス変数化
        float activeAngle = 20.0f;
        float activeDistance = 20.0f;

        Vector3 playerPos = transform.position;
        Vector3 eyeDir = transform.forward;//視点方向ベクトル

        List<GameObject> targetZombies = new List<GameObject>();

        //距離が一定以下のゾンビを全て取得
        foreach (var zombie in zombieObjects)
        {
            Debug.Log("ゾンビ発見");
            //距離を調べる
            Vector3 zombiePos = zombie.transform.position;
            zombiePos.y += 2.0f;
            if (Vector3.Distance(playerPos, zombiePos) > activeDistance) continue;

            targetZombies.Add(zombie);//リストに追加
        }

        if(targetZombies.Count != 0)
        {
            //対象のゾンビの中から視点から角度が一番近いオブジェクトを取得
            GameObject nearestEnemy =
                targetZombies.OrderBy(p => Vector3.Angle((p.transform.position- playerPos).normalized, eyeDir)).First();

            //取得したオブジェクトまでと視点の角度が一定以下なら
            if(Vector3.Angle((nearestEnemy.transform.position - playerPos).normalized, eyeDir) <= activeAngle)
            {
                //とりあえずマーク用のオブジェクトを移動させる
                circle.SetActive(true);
                circle.transform.position = nearestEnemy.transform.position;
                
                return;
            }

        }

        circle.SetActive(false);
        return;
    }


    private void GunUpdate()
    {
        //入力チェック
        if (!Input.GetMouseButtonDown(0)) return;

        //ばらつきをランダムに決める
        float x = Random.Range(-bulletSpread, bulletSpread);
        float y = Random.Range(-bulletSpread, bulletSpread);

        //視点ベクトルにばらつきを加算
        Vector3 gunVec = verRot.forward + new Vector3(x, y, 0);


        //弾道用のLineRendererを取得
        LineRenderer lineRend = Instantiate(bulletLine,
            Vector3.zero,
            Quaternion.identity).GetComponent<LineRenderer>();

        //点の数
        lineRend.positionCount = 2;
        //始点の座標指定
        lineRend.SetPosition(0, muzzleTransform.position);

        RaycastHit hit;
        //レイが当たったとき
        if (Physics.Raycast(verRot.position, gunVec, out hit, 20.0f))
        {
            //当たった場所を線の終点にする
            lineRend.SetPosition(1, hit.point);

            Debug.Log("タグ:"+hit.collider.gameObject.tag);
            //当たったcollider部分のタグが体なら
            if(hit.collider.gameObject.tag == "Body")
            {
                hit.transform.gameObject.GetComponent<ZombieManager>()
                    .DamageBody(hit.point);
            }
        }
        //レイが何にも当たらなかったとき
        else
        {
            //弾丸のベクトルの終点を線の終点にする
            lineRend.SetPosition(1, verRot.position+(gunVec * 20.0f));
        }


        //デバッグのRay
        Debug.DrawRay(verRot.position, gunVec * 20.0f, Color.red, 0.6f, true);
    }
}


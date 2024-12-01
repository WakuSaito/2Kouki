using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestPlayerMnager : MonoBehaviour
{

    private CharacterController characterController;  // CharacterController�^�̕ϐ�
    private Vector3 moveVelocity;  // �L�����N�^�[�R���g���[���[�𓮂������߂�Vector3�^�̕ϐ�
    private Transform verRot;  //�c�̎��_�ړ��̕ϐ�(�J�����ɍ��킹��)
    private Transform horRot;  //���̎��_�ړ��̕ϐ�(�v���C���[�ɍ��킹��)
    [SerializeField] private float walkSpeed = 3.0f;  //�ړ����x
    [SerializeField] private float dashSpeed = 5.0f;  //�ړ����x
    [SerializeField] private float jumpPower;  //�W�����v��

    [SerializeField] private float cameraSpeed = 100;

    [SerializeField] private Transform muzzleTransform;//�e���ʒu
    [SerializeField] private GameObject bulletLine;//�e��
    [SerializeField] private float bulletDistance = 20.0f;//�e�̔򋗗�
    [SerializeField] private float bulletSpread = 0.03f;//�e�u��

    [SerializeField] private GameObject cameraObj;

    [SerializeField] private bool activeMouse = false;//�}�E�X�̎��_�ړ��̗L����

    [SerializeField] private GameObject knifeObj;

    [SerializeField] private GameObject usePistol;

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
        //��Ɏ��_�������Ȃ��悤�ɂ���
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
            // �}�E�X�ɂ�鎋�_����
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

        //W�L�[�������ꂽ��
        if (Input.GetKey(KeyCode.W))
        {
            vec += transform.forward;
        }
        //S�L�[�������ꂽ��
        if (Input.GetKey(KeyCode.S))
        {
            vec -= transform.forward;
        }
        //A�L�[�������ꂽ��
        if (Input.GetKey(KeyCode.A))
        {
            vec -= transform.right;
        }
        //D�L�[�������ꂽ��
        if (Input.GetKey(KeyCode.D))
        {
            vec += transform.right;
        }

        //�e
        if (Input.GetMouseButtonDown(0))
        {
            usePistol.GetComponent<GunManager>().PullTriggerDown();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //�A�j���[�V�����N��
            usePistol.GetComponent<Animator>().enabled = true;

            //�����[�h����
            usePistol.GetComponent<GunManager>().Reload();
        }

        characterController.Move(vec.normalized * moveSpeed * Time.deltaTime);


        // �ڒn���Ă���Ƃ�
        if (characterController.isGrounded)
        {
            // �W�����v
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveVelocity.y = jumpPower;
            }
        }
        // �󒆂ɂ��鎞
        else
        {
            // �d�͂�������
            moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

        // �L�����N�^�[�𓮂���
        characterController.Move(moveVelocity * Time.deltaTime);

        //if(Input.GetMouseButtonDown(1) && knifeObj != null)
        //{
        //    knifeObj.GetComponent<KnifeManager>().StartAttack();
        //}

        //�x������
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

        searchViewArea.GetObjUpdate("Zombie", 20f, 2f);
        searchViewArea.GetObjUpdate("item", 5f);
    }

    

    //�e�̏���Update�i�e����A�ˑ��x�͍l�����Ă��Ȃ��j
}


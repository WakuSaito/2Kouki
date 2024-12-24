using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestPlayerManager : MonoBehaviour
{

    private CharacterController characterController;  // CharacterController�^�̕ϐ�
    private Vector3 moveVelocity;  // �L�����N�^�[�R���g���[���[�𓮂������߂�Vector3�^�̕ϐ�
    private Transform verRot;  //�c�̎��_�ړ��̕ϐ�(�J�����ɍ��킹��)
    private Transform horRot;  //���̎��_�ړ��̕ϐ�(�v���C���[�ɍ��킹��)
    [SerializeField] private float walkSpeed = 3.0f;  //�ړ����x
    [SerializeField] private float dashSpeed = 5.0f;  //�ړ����x
    [SerializeField] private float jumpPower;  //�W�����v��

    [SerializeField] private float cameraSpeed = 100;
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private bool activeMouse = false;//�}�E�X�̎��_�ړ��̗L����

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

        //�ړ�
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
        }


        //�X���b�g�I��
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
                    //�e
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
                        //�A�j���[�V�����N��
                        handWeapon.GetComponent<Animator>().enabled = true;

                        //�����[�h����
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

                    //�U������I�u�W�F�N�g�擾
                    GameObject attack_obj = searchViewArea.GetObjUpdate("Zombie", 20f, 0.5f);

                    //�U��
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (attack_obj != null)
                        {
                            dogManager.OrderAttack(attack_obj.GetComponentInParent<ZombieManager>().gameObject);
                        }
                    }
                    //���ŒT�m
                    else if (Input.GetMouseButtonDown(1))
                    {
                        dogManager.GetComponent<DogManager>().OrderDetection();
                    }
                    break;
            }

        }

        //���_�����̃A�C�e����T��
        GameObject item = searchViewArea.GetObjUpdate("item", 1f, 0.03f);

        if (Input.GetMouseButtonDown(1))
        {
            PickUpItem(item);
        }


    }

    void PickUpItem(GameObject _item)  //�A�C�e�����E��
    {
        if (_item == null) return;

        Destroy(_item);
    }



}

//�x������
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
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

    [SerializeField]private bool activeMouse = false;//�}�E�X�̎��_�ړ��̗L����

    [SerializeField] private GameObject knifeObj;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        verRot = cameraObj.transform;
        horRot = transform;
    }

    void Update()
    { 
        //��Ɏ��_�������Ȃ��悤�ɂ���
        if(Input.GetMouseButtonDown(0))
        {
            activeMouse = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            activeMouse = false;
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

        if(Input.GetMouseButtonDown(1) && knifeObj != null)
        {
            knifeObj.GetComponent<KnifeManager>().StartAttack();
        }

        LockOnUpdate();
    }

    //���Ă�������̃]���r���}�[�N����Update �A�C�e���p�ɕύX
    public void LockOnUpdate()
    {
        //�S�Ώۃ^�O�̃I�u�W�F�N�g
        GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("item");

        //�Ώۂ̏����@��ŃN���X�ϐ���
        float activeAngle = 20.0f;   //�ΏۂƂȂ�͈�
        float activeDistance = 20.0f;//�ΏۂƂȂ鋗��

        Vector3 playerPos = transform.position;
        Vector3 cameraPos = cameraObj.transform.position;
        Vector3 eyeDir = cameraObj.transform.forward;//���_�����x�N�g��

        List<GameObject> targetItems = new List<GameObject>();

        //���������ȉ��̃I�u�W�F�N�g�݂̂ɍi��
        foreach (var item in itemObjects)
        {
            //�S�ẴI�u�W�F�N�g�̐F��ʏ�ɖ߂� �������d������
            item.GetComponent<ColorChanger>().ChangeColorAlpha(0.0f);

            Debug.Log("�A�C�e������");
            //�����𒲂ׂ�
            Vector3 itemPos = item.transform.position;
            
            if (Vector3.Distance(playerPos, itemPos) > activeDistance) continue;

            targetItems.Add(item);//���X�g�ɒǉ�
        }

        if(targetItems.Count != 0)
        {
            //�I�u�W�F�N�g�̒��S�ʒu�����p
            Vector3 itemCenterAd = Vector3.up * 0.0f;

            //�Ώۂ̃I�u�W�F�N�g�̒����王�_����p�x����ԋ߂��I�u�W�F�N�g���擾
            GameObject nearestEnemy =
                targetItems.OrderBy(p => 
                Vector3.Angle(((p.transform.position + itemCenterAd) - cameraPos).normalized, eyeDir)).First();

            //Debug.Log("�p�x:" + Vector3.Angle(((nearestEnemy.transform.position + zombieCenterAd) - cameraPos).normalized, eyeDir));

            //�擾�����I�u�W�F�N�g�܂łƎ��_�̊p�x�����ȉ��Ȃ�
            if(Vector3.Angle(((nearestEnemy.transform.position + itemCenterAd) - cameraPos).normalized, eyeDir) <= activeAngle)
            {
                //�Ώۂ̐F��ύX
                nearestEnemy.GetComponent<ColorChanger>().ChangeColorAlpha(0.25f);

                //��������p�̕\�����o������
                
                return;
            }

        }
        return;
    }

    //�e�̏���Update�i�e����A�ˑ��x�͍l�����Ă��Ȃ��j
}


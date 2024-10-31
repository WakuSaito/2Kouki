using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMnager : MonoBehaviour
{

    private CharacterController characterController;  // CharacterController�^�̕ϐ�
    private Vector3 moveVelocity;  // �L�����N�^�[�R���g���[���[�𓮂������߂�Vector3�^�̕ϐ�
    [SerializeField] public Transform verRot;  //�c�̎��_�ړ��̕ϐ�(�J�����ɍ��킹��)
    [SerializeField] public Transform horRot;  //���̎��_�ړ��̕ϐ�(�v���C���[�ɍ��킹��)
    [SerializeField] public float moveSpeed;  //�ړ����x
    [SerializeField] public float jumpPower;  //�W�����v��

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // �}�E�X�ɂ�鎋�_����
        float X_Rotation = Input.GetAxis("Mouse X");
        float Y_Rotation = Input.GetAxis("Mouse Y");
        horRot.transform.Rotate(new Vector3(0, X_Rotation * 2, 0));
        verRot.transform.Rotate(-Y_Rotation * 2, 0, 0);

        //W�L�[�������ꂽ��
        if (Input.GetKey(KeyCode.W))
        {
            characterController.Move(this.gameObject.transform.forward * moveSpeed * Time.deltaTime);
        }
        //S�L�[�������ꂽ��
        if (Input.GetKey(KeyCode.S))
        {
            characterController.Move(this.gameObject.transform.forward * -1f * moveSpeed * Time.deltaTime);
        }
        //A�L�[�������ꂽ��
        if (Input.GetKey(KeyCode.A))
        {
            characterController.Move(this.gameObject.transform.right * -1 * moveSpeed * Time.deltaTime);
        }
        //D�L�[�������ꂽ��
        if (Input.GetKey(KeyCode.D))
        {
            characterController.Move(this.gameObject.transform.right * moveSpeed * Time.deltaTime);
        }

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
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestPlayerMnager : MonoBehaviour
{

    private CharacterController characterController;  // CharacterController�^�̕ϐ�
    private Vector3 moveVelocity;  // �L�����N�^�[�R���g���[���[�𓮂������߂�Vector3�^�̕ϐ�
    [SerializeField] public Transform verRot;  //�c�̎��_�ړ��̕ϐ�(�J�����ɍ��킹��)
    [SerializeField] public Transform horRot;  //���̎��_�ړ��̕ϐ�(�v���C���[�ɍ��킹��)
    [SerializeField] public float moveSpeed;  //�ړ����x
    [SerializeField] public float jumpPower;  //�W�����v��

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
        // �}�E�X�ɂ�鎋�_����
        float X_Rotation = Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime;
        float Y_Rotation = Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime;
        //horRot.transform.localRotation = Quaternion.AngleAxis(X_Rotation, Vector3.up);
        //verRot.transform.localRotation = Quaternion.AngleAxis(Y_Rotation, Vector3.left);
        horRot.transform.Rotate(new Vector3(0, X_Rotation * 2, 0), Space.Self);
        verRot.transform.Rotate(-Y_Rotation * 2, 0, 0, Space.Self);

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

        GunUpdate();

        LockOnUpdate();
    }

    public void LockOnUpdate()
    {
        //�S�]���r�I�u�W�F�N�g
        GameObject[] zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");

        //�Ώۂ̏����@��ŃN���X�ϐ���
        float activeAngle = 20.0f;
        float activeDistance = 20.0f;

        Vector3 playerPos = transform.position;
        Vector3 eyeDir = transform.forward;//���_�����x�N�g��

        List<GameObject> targetZombies = new List<GameObject>();

        //���������ȉ��̃]���r��S�Ď擾
        foreach (var zombie in zombieObjects)
        {
            Debug.Log("�]���r����");
            //�����𒲂ׂ�
            Vector3 zombiePos = zombie.transform.position;
            zombiePos.y += 2.0f;
            if (Vector3.Distance(playerPos, zombiePos) > activeDistance) continue;

            targetZombies.Add(zombie);//���X�g�ɒǉ�
        }

        if(targetZombies.Count != 0)
        {
            //�Ώۂ̃]���r�̒����王�_����p�x����ԋ߂��I�u�W�F�N�g���擾
            GameObject nearestEnemy =
                targetZombies.OrderBy(p => Vector3.Angle((p.transform.position- playerPos).normalized, eyeDir)).First();

            //�擾�����I�u�W�F�N�g�܂łƎ��_�̊p�x�����ȉ��Ȃ�
            if(Vector3.Angle((nearestEnemy.transform.position - playerPos).normalized, eyeDir) <= activeAngle)
            {
                //�Ƃ肠�����}�[�N�p�̃I�u�W�F�N�g���ړ�������
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
        //���̓`�F�b�N
        if (!Input.GetMouseButtonDown(0)) return;

        //�΂���������_���Ɍ��߂�
        float x = Random.Range(-bulletSpread, bulletSpread);
        float y = Random.Range(-bulletSpread, bulletSpread);

        //���_�x�N�g���ɂ΂�������Z
        Vector3 gunVec = verRot.forward + new Vector3(x, y, 0);


        //�e���p��LineRenderer���擾
        LineRenderer lineRend = Instantiate(bulletLine,
            Vector3.zero,
            Quaternion.identity).GetComponent<LineRenderer>();

        //�_�̐�
        lineRend.positionCount = 2;
        //�n�_�̍��W�w��
        lineRend.SetPosition(0, muzzleTransform.position);

        RaycastHit hit;
        //���C�����������Ƃ�
        if (Physics.Raycast(verRot.position, gunVec, out hit, 20.0f))
        {
            //���������ꏊ����̏I�_�ɂ���
            lineRend.SetPosition(1, hit.point);

            Debug.Log("�^�O:"+hit.collider.gameObject.tag);
            //��������collider�����̃^�O���̂Ȃ�
            if(hit.collider.gameObject.tag == "Body")
            {
                hit.transform.gameObject.GetComponent<ZombieManager>()
                    .DamageBody(hit.point);
            }
        }
        //���C�����ɂ�������Ȃ������Ƃ�
        else
        {
            //�e�ۂ̃x�N�g���̏I�_����̏I�_�ɂ���
            lineRend.SetPosition(1, verRot.position+(gunVec * 20.0f));
        }


        //�f�o�b�O��Ray
        Debug.DrawRay(verRot.position, gunVec * 20.0f, Color.red, 0.6f, true);
    }
}


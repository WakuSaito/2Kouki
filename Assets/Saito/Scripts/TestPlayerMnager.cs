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

        GunUpdate();

        LockOnUpdate();
    }

    //���Ă�������̃]���r���}�[�N����Update
    public void LockOnUpdate()
    {
        //�S�]���r�I�u�W�F�N�g
        GameObject[] zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");

        //�Ώۂ̏����@��ŃN���X�ϐ���
        float activeAngle = 20.0f;
        float activeDistance = 20.0f;

        Vector3 playerPos = transform.position;
        Vector3 cameraPos = cameraObj.transform.position;
        Vector3 eyeDir = cameraObj.transform.forward;//���_�����x�N�g��

        List<GameObject> targetZombies = new List<GameObject>();

        //���������ȉ��̃]���r��S�Ď擾
        foreach (var zombie in zombieObjects)
        {
            //�S�Ẵ]���r�̐F��ʏ�ɖ߂� �������d������
            zombie.GetComponent<ZombieManager>().ChangeColorAlpha(0.0f);

            Debug.Log("�]���r����");
            //�����𒲂ׂ�
            Vector3 zombiePos = zombie.transform.position;
            //zombiePos.y += 2.0f;
            if (Vector3.Distance(playerPos, zombiePos) > activeDistance) continue;

            targetZombies.Add(zombie);//���X�g�ɒǉ�
        }

        if(targetZombies.Count != 0)
        {
            //�]���r�̒��S�ʒu�����p
            Vector3 zombieCenterAd = Vector3.up * 2.0f;

            //�Ώۂ̃]���r�̒����王�_����p�x����ԋ߂��I�u�W�F�N�g���擾
            GameObject nearestEnemy =
                targetZombies.OrderBy(p => 
                Vector3.Angle(((p.transform.position + zombieCenterAd) - cameraPos).normalized, eyeDir)).First();

            Debug.Log("�p�x:" + Vector3.Angle(((nearestEnemy.transform.position + zombieCenterAd) - cameraPos).normalized, eyeDir));

            //�擾�����I�u�W�F�N�g�܂łƎ��_�̊p�x�����ȉ��Ȃ�
            if(Vector3.Angle(((nearestEnemy.transform.position + zombieCenterAd) - cameraPos).normalized, eyeDir) <= activeAngle)
            {
                //�Ώۂ̐F��ύX
                nearestEnemy.GetComponent<ZombieManager>().ChangeColorAlpha(0.25f);

                //nearestEnemy�����̍U���Ώ�
                
                return;
            }

        }
        return;
    }

    //�e�̏���Update�i�e����A�ˑ��x�͍l�����Ă��Ȃ��j
    private void GunUpdate()
    {
        //���̓`�F�b�N
        if (!Input.GetMouseButtonDown(0)) return;

        //�΂���������_���Ɍ��߂�
        float x = Random.Range(-bulletSpread, bulletSpread);
        float y = Random.Range(-bulletSpread, bulletSpread);

        //���_�x�N�g���ɂ΂�������Z
        Vector3 gunVec = verRot.forward + new Vector3(x, y, 0);


        //�e���p��LineRenderer���擾�i�����ڗp�j
        LineRenderer lineRend = Instantiate(bulletLine,
            Vector3.zero,
            Quaternion.identity).GetComponent<LineRenderer>();

        //�_�̐�
        lineRend.positionCount = 2;
        //�n�_�̍��W�w��
        lineRend.SetPosition(0, muzzleTransform.position);

        RaycastHit hit;
        //���C�����������Ƃ�
        if (Physics.Raycast(verRot.position, gunVec, out hit, bulletDistance))
        {
            //���������ꏊ����̏I�_�ɂ���
            lineRend.SetPosition(1, hit.point);

            Debug.Log("�^�O:"+hit.collider.gameObject.tag);
            //��������collider�����̃^�O���̂Ȃ�
            if(hit.collider.gameObject.tag == "Body")
            {
                hit.transform.gameObject.GetComponent<ZombieManager>()
                    .DamageBody(hit.point);//���[�V������؂�ւ��邽�߁A�����������W��n���Ă���
            }
            else if (hit.collider.gameObject.tag == "Head")
            {
                hit.transform.gameObject.GetComponent<ZombieManager>()
                    .DamageHead();
            }
        }
        //���C�����ɂ�������Ȃ������Ƃ�
        else
        {
            //�e�ۂ̃x�N�g���̏I�_����̏I�_�ɂ���
            lineRend.SetPosition(1, verRot.position+(gunVec * bulletDistance));
        }

    }
}


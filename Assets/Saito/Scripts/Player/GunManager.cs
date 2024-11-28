using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private Transform muzzleTransform;//�e���ʒu
    [SerializeField] private GameObject bulletLine;//�e��

    [SerializeField] private int magazineSize = 10;//�e�̗e��
    [SerializeField] private int oneShotBulletAmount = 1; //�ꔭ�Ŕ��˂�����
    [SerializeField] private float bulletSpread = 0.03f;  //�e�u��
    [SerializeField] private float bulletDistance = 20.0f;//�e�̔򋗗�
    [SerializeField] private float rapidSpeed = 1.0f;//�A�ˑ��x
    [SerializeField] private bool isCanRapid = false;//�A�ˉ\��

    [SerializeField] private int bulletDamage = 5;  //�e���G�ɗ^����_���[�W

    //�v���C���[�����������ɑ��
    public GameObject hand_player_obj = null;
    bool set_player_flag = false;

    private int currentMagazineAmount;//���݂̃}�K�W���̒e��

    bool isCooldown = false;//�N�[���^�C������

    GameObject cameraObj;
    Inventory inventory;
    Animator anim;

    private void Awake()
    {
        currentMagazineAmount = magazineSize;

        cameraObj = Camera.main.gameObject;
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        if (hand_player_obj != null && !set_player_flag)
        {
            inventory = hand_player_obj.GetComponent<Inventory>();
            set_player_flag = true;
        }
    }

    public int GetCurrentMagazine()
    {
        return currentMagazineAmount;
    }

    public void Reload()
    {

        //�����[�h����
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isCooldown) return;

            //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
            if (currentMagazineAmount < magazineSize)
            {
                for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
                {
                    //�C���x���g���ɒe�ۂ����邩
                    if (inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
                    {
                        anim.SetBool("Reload", true);  //reload
                        isCooldown = true;
                        Invoke("ReroadFin", 2.8f);
                    }
                }
            }
        }
    }

    void ReroadFin()
    {
        anim.SetBool("Reload", false);  //reload
        isCooldown = false;
        //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
        if (currentMagazineAmount < magazineSize)
        {
            for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
            {
                //�C���x���g���ɒe�ۂ����邩
                if (inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
                {
                    //�s�X�g���ɓ���e�ې��𒲂ׂ�
                    int reload_num = magazineSize - currentMagazineAmount;
                    //reload�ł���ő吔��ۑ�
                    int max_reload = reload_num;

                    //animation

                    for (int cnt = 0; cnt < max_reload; cnt++)
                    {
                        if (inventory.item_num[i] == 0)
                        {
                            //�C���x���g���ɂ������e�ۂ̎c�肪0�ɂȂ�����id������������
                            inventory.item_type_id[i] = -1;
                            break;
                        }
                        else
                        {
                            inventory.item_num[i]--;
                            currentMagazineAmount++;
                            reload_num--;
                        }
                        //�C���x���g���̒��g�����炷
                        inventory.ReduceInventory(i);
                    }
                }
            }
        }

    }

    public void StopReload()
    {
        if(IsInvoking("ReroadFin"))
        {
            isCooldown = false;
            anim.SetBool("Reload", false);  //reload
            CancelInvoke("ReroadFin");
        }
    }

    /// <summary>
    /// �e�̔��˃{�^�����������u�ԂɌĂяo��
    /// </summary>
    public void PullTriggerDown()
    {
        if (currentMagazineAmount <= 0) return;
        if (isCooldown) return;

        //�������ː����J��Ԃ�
        for (int i = 0; i < oneShotBulletAmount; i++)
        {
            CreateBullet();
        }

        //�o����Δ�����t������

        //�N�[���^�C��
        StartCoroutine(CooldownCoroutine(rapidSpeed));

        currentMagazineAmount--;
    }

    /// <summary>
    /// �e�̔��˃{�^���������Ă������Ăяo��
    /// </summary>
    public void PullTrigger()
    {
        if (currentMagazineAmount <= 0) return;
        if (isCooldown) return;
        if (!isCanRapid) return;

        //�������ː����J��Ԃ�
        for (int i = 0; i < oneShotBulletAmount; i++)
        {
            CreateBullet();
        }

        //�N�[���^�C��
        StartCoroutine(CooldownCoroutine(rapidSpeed));

        currentMagazineAmount--;
    }

    //�e����
    private void CreateBullet()
    {
        //�΂���������_���Ɍ��߂�
        float x = Random.Range(-bulletSpread, bulletSpread);
        float y = Random.Range(-bulletSpread, bulletSpread);

        //���_�x�N�g���ɂ΂�������Z
        Vector3 gunVec = cameraObj.transform.forward + new Vector3(x, y, 0);


        //�e���p��LineRenderer���擾�i�����ڗp�j
        LineRenderer lineRend = Instantiate(
            bulletLine,
            Vector3.zero,
            Quaternion.identity
            ).GetComponent<LineRenderer>();

        //�_�̐�
        lineRend.positionCount = 2;
        //�n�_�̍��W�w��
        lineRend.SetPosition(0, muzzleTransform.position);


        //�r���[�|�[�g���W�̃��C���΂�
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit = new RaycastHit();


        if (Physics.Raycast(cameraObj.transform.position, gunVec, out hit))
        {
            //���������ꏊ����̏I�_�ɂ���
            lineRend.SetPosition(1, hit.point);

            //�A�C�e���܂ł̋����𒲂ׂ�
            float distance = Vector3.Distance(hit.transform.position, transform.position);

            //�������͈͓��Ȃ�
            if (distance <= 30.0f)
            {
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);

                GameObject hit_obj = hit.collider.gameObject;
                Debug.Log(hit_obj);
                if (hit_obj.tag == "Body")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageBody(hit.point, bulletDamage);
                }
                if (hit_obj.tag == "Head")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageHead(bulletDamage);
                }
            }
        }
        //���C�����ɂ�������Ȃ������Ƃ�
        else
        {
            //�e�ۂ̃x�N�g���̏I�_����̏I�_�ɂ���
            lineRend.SetPosition(1, cameraObj.transform.position + (gunVec * bulletDistance));
        }
    }
    
    //�N�[���^�C���p�R���[�`��
    private IEnumerator CooldownCoroutine(float _sec)
    {
        isCooldown = true;

        //�A�ˑ��x���҂�
        yield return new WaitForSeconds(_sec);

        isCooldown = false;
    }

}

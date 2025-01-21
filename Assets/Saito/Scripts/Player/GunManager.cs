using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// <para>�e�}�l�[�W���[�N���X</para>
/// <para>�C���X�y�N�^�[�ŕϐ���ݒ肷�邱�Ƃŗl�X�ȏe��ɑΉ��o����</para>
/// ��Ɏ��A�C�e���Ȃ̂�IWeapon���p���@�C���x���g���֘A�̎d�l���ύX����Ă���ꍇ�K�v�����Ȃ邩��
/// </summary>
public class GunManager : MonoBehaviour, IWeapon
{
    [SerializeField] private string m_soundType;//�T�E���h�̎�ށi�����j
    [SerializeField] private Transform m_muzzleTransform; //�e���ʒu

    [SerializeField] private GameObject m_bulletLine;         //�e��
    [SerializeField] private GameObject m_muzzleFlashPrefab;  //�}�Y���t���b�V���p�G�t�F�N�g
    [SerializeField] private GameObject m_bulletHitPrefab;    //���e�G�t�F�N�g

    [SerializeField] private int m_magazineSize = 10;//�e�̗e��
    [SerializeField] private int m_oneShotBulletAmount = 1; //�ꔭ�Ŕ��˂�����
    [SerializeField] private float m_bulletSpread = 0.03f;  //�e�u��
    [SerializeField] private float m_bulletDistance = 20.0f;//�e�̔򋗗�
    [SerializeField] private float m_rapidSpeed = 1.0f; //�A�ˑ��x
    [SerializeField] private bool m_isCanRapid = false; //�A�ˉ\��
    [SerializeField] private float m_reloadSpeed = 2.8f;//�����[�h���x

    [SerializeField] private int m_bulletDamage = 5;  //�e���G�ɗ^����_���[�W

    //�v���C���[�����������ɑ�� �C���x���g���̎Q�Ɨp�H
    public GameObject m_handPlayerObj = null;
    bool m_setPlayerFlag = false;

    private int m_currentMagazineAmount;//���݂̃}�K�W���̒e��

    protected bool m_isShotCooldown = false;//���C�N�[���^�C������
    protected bool m_isReload = false;  //�����[�h����

    GameObject m_cameraObj;
    //protected Inventory m_inventory;
    //protected ItemInventory m_itemInventory;
    protected InventoryItem m_inventoryItem;
    //�T�E���h�Đ��p
    private GunSound m_gunSound;
    protected Animator m_animator;


    //�����ݒ�@�R���|�[�l���g�擾
    private void Awake()
    {
        m_currentMagazineAmount = m_magazineSize;

        m_cameraObj = Camera.main.gameObject;
        m_animator = GetComponent<Animator>();
        m_gunSound = GetComponent<GunSound>();
    }

    private void Update()
    {
        if (m_handPlayerObj != null && !m_setPlayerFlag)
        {
            //m_inventory = m_handPlayerObj.GetComponent<Inventory>();
            m_inventoryItem = m_handPlayerObj.GetComponent<InventoryItem>();
            m_setPlayerFlag = true;
        }
    }

    public int GetMagazineSize()
    {
        return m_magazineSize;
    }

    public int GetCurrentMagazine()
    {
        return m_currentMagazineAmount;
    }

    public virtual void Reload()
    {
        if (m_isShotCooldown) return;
        if (m_isReload) return;
        //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
        if (m_currentMagazineAmount >= m_magazineSize) return;

        if (m_inventoryItem == null)
        {
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            Invoke(nameof(ReroadFin), m_reloadSpeed);
            return;
        }

        if (m_inventoryItem.CheckBullet())
        {
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            Invoke(nameof(ReroadFin), m_reloadSpeed);
            return;
        }

        /*���Ԃ񂢂�Ȃ�*/
        //for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        //{
        //    //�C���x���g���ɒe�ۂ����邩
        //    if (inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
        //    {
        //        anim.SetBool("Reload", true);  //reload
        //        isReload = true;
        //        Invoke(nameof(ReroadFin), reloadSpeed);
        //    }
        //}

    }

    void ReroadFin()
    {
        m_animator.SetBool("Reload", false);  //reload
        m_isReload = false;

        //�s�X�g���ɓ���e�ې��𒲂ׂ�
        int empty_amount = HowManyCanLoaded();

        AddBullet(empty_amount);
    }

    public virtual void StopReload()
    {
        if (IsInvoking(nameof(ReroadFin)))
        {
            m_isReload = false;
            m_animator.SetBool("Reload", false);  //reload
            CancelInvoke(nameof(ReroadFin));
        }
    }

    public void AddBullet(int _amount)
    {
        int add_amount = _amount;
        //�ő�͒����Ȃ��悤��
        if (add_amount > m_magazineSize - m_currentMagazineAmount)
            add_amount = m_magazineSize - m_currentMagazineAmount;

        if (m_inventoryItem == null) {
            m_currentMagazineAmount += add_amount;
            return;
        }

        m_currentMagazineAmount += m_inventoryItem.UseBullet(_amount);
    }

    //�㉽���e�������邩
    public int HowManyCanLoaded()
    {
        //�}�K�W���ɓ���e���v�Z
        int can_loaded_amount = m_magazineSize - m_currentMagazineAmount;

        if (m_inventoryItem == null) return can_loaded_amount;

        /*���Ԃ񂢂�Ȃ�*/
        //for (int i = 0; i < m_inventoryItem.m_slotSize; i++)
        //{
        //    //�C���x���g���ɒe�ۂ����邩
        //    if (m_inventoryItem.item_type_id[i] != (int)ID.ITEM_ID.BULLET) continue;

        //    for (int cnt = 0; cnt < can_loaded_amount; cnt++)
        //    {
        //        //�C���x���g���ɒe������ꍇ
        //        if (m_inventoryItem.item_num[i] != 0)
        //            can_loaded_amount--; 
        //        //�e�������ꍇ
        //        else          
        //            break;
        //    }
        //}

        return can_loaded_amount;
    }

    /// <summary>
    /// �e�̔��˃{�^�����������u�ԂɌĂяo��
    /// </summary>
    public void PullTriggerDown()
    {
        if (m_isShotCooldown) return;
        if (m_currentMagazineAmount <= 0)
        {
            m_gunSound.PlayBlankShot();//���ˎ��s��
            return;
        }
        if (m_isReload)
        {
            //�A�j���[�V�����J�ڂ��m�肳���邽��1�t���[���x�����s
            StartCoroutine(DelayFrameCoroutine(
                1,
                () => StopReload()
            ));

        }

        Shot();
    }

    /// <summary>
    /// �e�̔��˃{�^���������Ă������Ăяo��
    /// </summary>
    public void PullTrigger()
    {
        if (m_isShotCooldown) return;
        if (!m_isCanRapid) return;
        if (m_currentMagazineAmount <= 0)
        {
            return;
        }

        Shot();
    }

    private void Shot()
    {
        //�������ː����J��Ԃ�
        for (int i = 0; i < m_oneShotBulletAmount; i++)
        {
            CreateBullet();
        }

        m_gunSound.PlayShot(m_soundType);//���ˉ�

        m_animator.SetTrigger("Shot");//�A�j���[�V����

        //�N�[���^�C��
        StartCoroutine(CooldownCoroutine(m_rapidSpeed));

        m_currentMagazineAmount--;
    }


    //�e����
    private void CreateBullet()
    {
        //�΂���������_���Ɍ��߂�
        float x = UnityEngine.Random.Range(-m_bulletSpread, m_bulletSpread);
        float y = UnityEngine.Random.Range(-m_bulletSpread, m_bulletSpread);

        //���_�x�N�g���ɂ΂�������Z
        Vector3 gun_vec = m_cameraObj.transform.forward + new Vector3(x, y, 0);

        //���������I�u�W�F�N�g���
        RaycastHit hit = new RaycastHit();

        //���e�n�_
        Vector3 bullet_hit_pos;

        //�Ȃɂ��ɓ�������
        if (Physics.Raycast(m_cameraObj.transform.position, gun_vec, out hit))
        {
            bullet_hit_pos = hit.point;

            //�A�C�e���܂ł̋����𒲂ׂ�
            float distance = Vector3.Distance(hit.transform.position, transform.position);

            //�������͈͓��Ȃ�
            if (distance <= 30.0f)
            {
                GameObject hit_obj = hit.collider.gameObject;
                Debug.Log(hit_obj);
                if (hit_obj.tag == "Body")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageBody(hit.point, m_bulletDamage);
                }
                if (hit_obj.tag == "Head")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageHead(hit.point, m_bulletDamage);
                }
            }
        }
        //���C�����ɂ�������Ȃ������Ƃ�
        else
        {
            //�e�ۂ̃x�N�g���̏I�_����̏I�_�ɂ���
            bullet_hit_pos = m_cameraObj.transform.position + (gun_vec * m_bulletDistance);
        }

        //�A�j���[�V������ɃG�t�F�N�g���o�������̂�1�t���[���x�点��   
        StartCoroutine(DelayFrameCoroutine(
            1, 
            () => CreateBulletEffect(bullet_hit_pos)
        ));
    }

    //�Ώۂ̃A�N�V���������t���[����x�����s������
    private IEnumerator DelayFrameCoroutine(int _frame, Action _action)
    {
        for (int i = 0; i < _frame; i++)
        {
            yield return null;
        }

        //�A�N�V�������s
        _action();
    }

    private void CreateBulletEffect(Vector3 _hit_pos)
    {
        //�}�Y���t���b�V��
        if (m_muzzleFlashPrefab != null)
        {
            Instantiate(m_muzzleFlashPrefab,
                m_muzzleTransform.position,
                Quaternion.identity,
                transform
                ).transform.localRotation = Quaternion.identity;
        }

        //�e��
        if (m_bulletLine != null)
        {
            //�e���p��LineRenderer���擾�i�����ڗp�j
            LineRenderer line_rend = Instantiate(
                m_bulletLine,
                Vector3.zero,
                Quaternion.identity
                ).GetComponent<LineRenderer>();

            //�_�̐�
            line_rend.positionCount = 2;
            //�n�_�̍��W�w��
            line_rend.SetPosition(0, m_muzzleTransform.position);

            //���������ꏊ����̏I�_�ɂ���
            line_rend.SetPosition(1, _hit_pos);
        }

        //���e
        if(m_bulletHitPrefab != null)
        {
            //���e�n�_�ɐ���
            GameObject hit_effect = 
                Instantiate(m_bulletHitPrefab,
                _hit_pos,
                Quaternion.identity
                );

            //���˕����Ɍ�����
            hit_effect.transform.rotation = 
                Quaternion.LookRotation(m_muzzleTransform.position - hit_effect.transform.position);
        }
    }
    
    //�N�[���^�C���p�R���[�`��
    private IEnumerator CooldownCoroutine(float _sec)
    {
        m_isShotCooldown = true;

        //�A�ˑ��x���҂�
        yield return new WaitForSeconds(_sec);

        m_isShotCooldown = false;
    }

    //�d����
    public void PutAway()
    {
        gameObject.SetActive(false);
    }
    //���o��
    public void PutOut()
    {
        gameObject.SetActive(true);

        m_isReload = false;
        m_isShotCooldown = false;
    }
}

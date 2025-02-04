using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// <para>�e�}�l�[�W���[�N���X</para>
/// <para>�C���X�y�N�^�[�ŕϐ���ݒ肷�邱�Ƃŗl�X�ȏe��ɑΉ��o����</para>
/// ��Ɏ��A�C�e���Ȃ̂�IWeapon���p���@�C���x���g���֘A�̎d�l���ύX����Ă���ꍇ�K�v�����Ȃ邩��
/// </summary>
public class GunManager : MonoBehaviour, IWeapon, IStopObject
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
    bool m_isActive = true;//��Ɏ����Ă����Ԃ�

    GameObject m_cameraObj;
    protected InventoryItem m_inventoryItem;
    //�T�E���h�Đ��p
    private GunSound m_gunSound;
    protected Animator m_animator;

    IEnumerator m_reloadCoroutine;


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
            m_inventoryItem = m_handPlayerObj.GetComponent<InventoryItem>();
            m_setPlayerFlag = true;
        }
    }

    /// <summary>
    /// �}�K�W���̍ő�e�ʎ擾
    /// </summary>
    /// <returns>�}�K�W���̍ő�e��</returns>
    public int GetMagazineSize()
    {
        return m_magazineSize;
    }

    /// <summary>
    /// ���݂̎c�e���擾
    /// </summary>
    /// <returns>���݂̎c�e��</returns>
    public int GetCurrentMagazine()
    {
        return m_currentMagazineAmount;
    }

    /// <summary>
    /// <para>�����[�h</para>
    /// �����[�h���J�n����
    /// </summary>
    public virtual void Reload()
    {
        if (m_isShotCooldown) return;
        if (m_isReload) return;
        //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
        if (m_currentMagazineAmount >= m_magazineSize) return;

        if (m_inventoryItem == null)
        {
            Debug.Log("�����[�h�J�n�@���݂̎c�e��:" + m_currentMagazineAmount);
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            m_reloadCoroutine = ReloadCoroutine();
            StartCoroutine(m_reloadCoroutine);
            return;
        }  
        else if (m_inventoryItem.CheckBullet())//�C���x���g���ɒe�ۂ�����΃����[�h�J�n
        {
            Debug.Log("�����[�h�J�n�@���݂̎c�e��:" + m_currentMagazineAmount);
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            m_reloadCoroutine = ReloadCoroutine();
            StartCoroutine(m_reloadCoroutine);
            return;
        }
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(m_reloadSpeed);

        ReloadFin();
        m_reloadCoroutine = null;
    }

    /// <summary>
    /// <para>�����[�h����</para>
    /// �����[�h���I������Ƃ�
    /// </summary>
    void ReloadFin()
    {
        m_animator.SetBool("Reload", false);  //reload
        m_isReload = false;

        //�s�X�g���ɓ���e�ې��𒲂ׂ�
        int empty_amount = HowManyCanLoaded();

        Debug.Log("�}�K�W���̋�:" + empty_amount);

        AddBullet(empty_amount);

        Debug.Log("�����[�h�I���@���݂̎c�e��:" + m_currentMagazineAmount);
    }

    /// <summary>
    /// <para>�����[�h��~</para>
    /// �����[�h���L�����Z������
    /// </summary>
    public virtual void StopReload()
    {    
        if (m_reloadCoroutine != null)
        {
            Debug.Log("�����[�h�L�����Z��");
            m_isReload = false;
            m_animator.SetBool("Reload", false);  //reload
            //�R���[�`�����S��~
            StopCoroutine(m_reloadCoroutine);
            m_reloadCoroutine = null;
        }
    }

    /// <summary>
    /// <para>�e�ǉ�</para>
    /// �w��̐������}�K�W���ɒe���[����
    /// </summary>
    /// <param name="_amount">��[����e��</param>
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

    /// <summary>
    /// �e���������邩�v�Z
    /// </summary>
    /// <returns>����e��</returns>
    public int HowManyCanLoaded()
    {
        //�}�K�W���ɓ���e���v�Z
        return m_magazineSize - m_currentMagazineAmount; ;
    }

    /// <summary>
    /// <para>�e�̃g���K�[�������ꂽ�u��</para>
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
    /// <para>�e�̃g���K�[��������Ă���</para>
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

    /// <summary>
    /// <para>���C</para>
    /// ���C���Ăяo����A�w��̉񐔒e�𐶐�
    /// </summary>
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


    /// <summary>
    /// <para>�e����</para>
    /// �e�����������
    /// </summary>
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

    /// <summary>
    /// <para>�Ώۂ̃A�N�V���������t���[����x�����s������</para>
    /// �A�j���[�V��������p
    /// </summary>
    /// <param name="_frame">�x���t���[��</param>
    /// <param name="_action">�x������s���鏈��</param>
    private IEnumerator DelayFrameCoroutine(int _frame, Action _action)
    {
        for (int i = 0; i < _frame; i++)
        {
            yield return null;
        }

        //�A�N�V�������s
        _action();
    }

    /// <summary>
    /// <para>�e�G�t�F�N�g����</para>
    /// ���C���ɃG�t�F�N�g�𐶐�����
    /// </summary>
    /// <param name="_hit_pos">���e�n�_</param>
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
    
    /// <summary>
    /// �N�[���^�C���R���[�`��
    /// ���C�N�[���^�C���Ǘ��p
    /// </summary>
    /// <param name="_sec">�N�[���^�C���b��</param>
    private IEnumerator CooldownCoroutine(float _sec)
    {
        m_isShotCooldown = true;

        //�A�ˑ��x���҂�
        yield return new WaitForSeconds(_sec);

        m_isShotCooldown = false;
    }

    /// <summary>
    /// <para>�d����</para>
    /// ����؂�ւ����ɌĂяo��
    /// </summary>
    public void PutAway()
    {
        if (m_isActive == false) return;
        m_isActive = false;
        gameObject.SetActive(false);
        StopReload();//�����[�h���L�����Z��
        Debug.Log("�d����");
    }
    /// <summary>
    /// <para>���o��</para>>
    /// ����؂�ւ����ɌĂяo��
    /// </summary>
    public void PutOut()
    {
        if (m_isActive == true) return;
        m_isActive = true;

        gameObject.SetActive(true);
        Debug.Log("���o��");

        m_isReload = false;
        m_isShotCooldown = false;
    }

    /// <summary>
    /// �A�C�e���𗎂Ƃ��Ƃ��̐ݒ�
    /// �I�u�W�F�N�g�\���A�R���C�_�[ON�A�Œ�����A�X�P�[���E��������
    /// </summary>
    public void DropItemSetting()
    {
        gameObject.SetActive(true);//�\��
        GetComponent<ItemSetting>().drop_flag = true;//�A�C�e��drop
        GetComponent<ItemSetting>().m_getFlag = false;//�l�����Ă��Ȃ�
        GetComponent<BoxCollider>().enabled = true;//�R���C�_�[ON
        GetComponent<Rigidbody>().isKinematic = false;//�Œ����
        GetComponent<Animator>().enabled = false;//�A�j���[�V����OFF
        tag = "item";//�^�O�ύX

        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);//�X�P�[�������Ƃ̑傫����
        transform.Rotate(new Vector3(0, 90, 45));//�����𒲐�
    }

    /// <summary>
    /// �A�C�e���擾�������̐ݒ�
    /// �R���C�_�[OFF�A�Œ肷��
    /// </summary>
    public void GetItemSetting()
    {
        //�R���|�[�l���g�ݒ�
        gameObject.SetActive(false);
        GetComponent<ItemSetting>().drop_flag = false;//�A�C�e��drop
        GetComponent<ItemSetting>().m_getFlag = true;//�A�C�e���l���ς�
        GetComponent<BoxCollider>().enabled = false;//�R���C�_�[OFF
        GetComponent<Rigidbody>().isKinematic = true;//�Œ�
        GetComponent<Animator>().enabled = true;//�A�j���[�V����ON
        tag = "weapon";//�^�O�ύX

        //�ʒu�ݒ�
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //�X�P�[���ύX
    }

    /// <summary>
    /// �ꎞ��~
    /// </summary>
    public void Pause()
    {
        m_animator.speed = 0;
        if (m_reloadCoroutine != null)
            StopCoroutine(m_reloadCoroutine);
    }

    /// <summary>
    /// �ĊJ
    /// </summary>
    public void Resume()
    {
        m_animator.speed = 1;
        if (m_reloadCoroutine != null)
            StartCoroutine(m_reloadCoroutine);
    }
}

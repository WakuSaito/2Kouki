using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunManager : MonoBehaviour, IWeapon
{
    [SerializeField] private string mSoundType;//�T�E���h�̎�ށi�����j
    [SerializeField] private Transform mMuzzleTransform; //�e���ʒu

    [SerializeField] private GameObject mBulletLine;         //�e��
    [SerializeField] private GameObject mMuzzleFlashPrefab;  //�}�Y���t���b�V���p�G�t�F�N�g
    [SerializeField] private GameObject mBulletHitPrefab;    //���e�G�t�F�N�g

    [SerializeField] private int mMagazineSize = 10;//�e�̗e��
    [SerializeField] private int mOneShotBulletAmount = 1; //�ꔭ�Ŕ��˂�����
    [SerializeField] private float mBulletSpread = 0.03f;  //�e�u��
    [SerializeField] private float mBulletDistance = 20.0f;//�e�̔򋗗�
    [SerializeField] private float mRapidSpeed = 1.0f; //�A�ˑ��x
    [SerializeField] private bool mIsCanRapid = false; //�A�ˉ\��
    [SerializeField] private float mReloadSpeed = 2.8f;//�����[�h���x

    [SerializeField] private int mBulletDamage = 5;  //�e���G�ɗ^����_���[�W

    //�v���C���[�����������ɑ��
    public GameObject hand_player_obj = null;
    bool mSetPlayerFlag = false;

    private int mCurrentMagazineAmount;//���݂̃}�K�W���̒e��

    protected bool mIsShotCooldown = false;//���C�N�[���^�C������
    protected bool mIsReload = false;  //�����[�h����

    GameObject mCameraObj;
    protected Inventory mInventory;
    protected ItemInventory mIteminventory;
    //�T�E���h�Đ��p
    private GunSound mGunSound;
    protected Animator mAnimator;


    private void Awake()
    {
        mCurrentMagazineAmount = mMagazineSize;

        mCameraObj = Camera.main.gameObject;
        mAnimator = GetComponent<Animator>();

        mGunSound = GetComponent<GunSound>();
    }

    private void Update()
    {
        if (hand_player_obj != null && !mSetPlayerFlag)
        {
            mInventory = hand_player_obj.GetComponent<Inventory>();
            mIteminventory = hand_player_obj.GetComponent<player>().ItemInventory;
            mSetPlayerFlag = true;
        }
    }

    public int GetMagazineSize()
    {
        return mMagazineSize;
    }

    public int GetCurrentMagazine()
    {
        return mCurrentMagazineAmount;
    }

    public virtual void Reload()
    {
        if (mIsShotCooldown) return;
        if (mIsReload) return;
        //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
        if (mCurrentMagazineAmount >= mMagazineSize) return;

        if (mInventory == null)
        {
            mAnimator.SetBool("Reload", true);  //reload
            mIsReload = true;
            Invoke(nameof(ReroadFin), mReloadSpeed);
            return;
        }

        if (mIteminventory.CheckInBullet())
        {
            mAnimator.SetBool("Reload", true);  //reload
            mIsReload = true;
            Invoke(nameof(ReroadFin), mReloadSpeed);
            return;
        }

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
        mAnimator.SetBool("Reload", false);  //reload
        mIsReload = false;

        //�s�X�g���ɓ���e�ې��𒲂ׂ�
        int empty_amount = HowManyCanLoaded();

        AddBullet(empty_amount);
    }

    public virtual void StopReload()
    {
        if (IsInvoking(nameof(ReroadFin)))
        {
            mIsReload = false;
            mAnimator.SetBool("Reload", false);  //reload
            CancelInvoke(nameof(ReroadFin));
        }
    }

    public void AddBullet(int _amount)
    {
        int add_amount = _amount;
        //�ő�͒����Ȃ��悤��
        if (add_amount > mMagazineSize - mCurrentMagazineAmount)
            add_amount = mMagazineSize - mCurrentMagazineAmount;

        if (mInventory == null) {
            mCurrentMagazineAmount += add_amount;
            return;
        }

        mCurrentMagazineAmount += mIteminventory.SubBullet(_amount);

        //for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        //{
        //    //�C���x���g���ɒe�ۂ����邩
        //    if (inventory.item_type_id[i] != (int)ID.ITEM_ID.BULLET) continue;

        //    for (int cnt = 0; cnt < addAmount; cnt++)
        //    {
        //        if (inventory.item_num[i] == 0)
        //        {
        //            //�C���x���g���ɂ������e�ۂ̎c�肪0�ɂȂ�����id������������
        //            inventory.item_type_id[i] = -1;
        //            break;
        //        }
        //        else
        //        {
        //            //���̊֐��ɂ܂Ƃ߂ė~����
        //            inventory.item_num[i]--;
        //            //�C���x���g���̒��g�����炷 �������点��悤�ɂ��Ăق���
        //            inventory.ReduceInventory(i);

        //            currentMagazineAmount++;
        //            addAmount--;
        //        }
        //    }
        //}
    }

    //�㉽���e�������邩
    public int HowManyCanLoaded()
    {
        //�}�K�W���ɓ���e���v�Z
        int can_loaded_amount = mMagazineSize - mCurrentMagazineAmount;

        if (mInventory == null) return can_loaded_amount;

        for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        {
            //�C���x���g���ɒe�ۂ����邩
            if (mInventory.item_type_id[i] != (int)ID.ITEM_ID.BULLET) continue;

            for (int cnt = 0; cnt < can_loaded_amount; cnt++)
            {
                //�C���x���g���ɒe������ꍇ
                if (mInventory.item_num[i] != 0)
                    can_loaded_amount--; 
                //�e�������ꍇ
                else          
                    break;
            }
        }

        return can_loaded_amount;
    }

    /// <summary>
    /// �e�̔��˃{�^�����������u�ԂɌĂяo��
    /// </summary>
    public void PullTriggerDown()
    {
        if (mIsShotCooldown) return;
        if (mCurrentMagazineAmount <= 0)
        {
            mGunSound.PlayBlankShot();//���ˎ��s��
            return;
        }
        if (mIsReload)
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
        if (mIsShotCooldown) return;
        if (!mIsCanRapid) return;
        if (mCurrentMagazineAmount <= 0)
        {
            return;
        }

        Shot();
    }

    private void Shot()
    {
        //�������ː����J��Ԃ�
        for (int i = 0; i < mOneShotBulletAmount; i++)
        {
            CreateBullet();
        }

        mGunSound.PlayShot(mSoundType);//���ˉ�

        mAnimator.SetTrigger("Shot");//�A�j���[�V����

        //�N�[���^�C��
        StartCoroutine(CooldownCoroutine(mRapidSpeed));

        mCurrentMagazineAmount--;
    }


    //�e����
    private void CreateBullet()
    {
        //�΂���������_���Ɍ��߂�
        float x = UnityEngine.Random.Range(-mBulletSpread, mBulletSpread);
        float y = UnityEngine.Random.Range(-mBulletSpread, mBulletSpread);

        //���_�x�N�g���ɂ΂�������Z
        Vector3 gun_vec = mCameraObj.transform.forward + new Vector3(x, y, 0);

        //���������I�u�W�F�N�g���
        RaycastHit hit = new RaycastHit();

        //���e�n�_
        Vector3 bullet_hit_pos;

        //�Ȃɂ��ɓ�������
        if (Physics.Raycast(mCameraObj.transform.position, gun_vec, out hit))
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
                    hit_obj.GetComponentInParent<ZombieManager>().DamageBody(hit.point, mBulletDamage);
                }
                if (hit_obj.tag == "Head")
                {
                    hit_obj.GetComponentInParent<ZombieManager>().DamageHead(hit.point, mBulletDamage);
                }
            }
        }
        //���C�����ɂ�������Ȃ������Ƃ�
        else
        {
            //�e�ۂ̃x�N�g���̏I�_����̏I�_�ɂ���
            bullet_hit_pos = mCameraObj.transform.position + (gun_vec * mBulletDistance);
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
        if (mMuzzleFlashPrefab != null)
        {
            Instantiate(mMuzzleFlashPrefab,
                mMuzzleTransform.position,
                Quaternion.identity,
                transform
                ).transform.localRotation = Quaternion.identity;
        }

        //�e��
        if (mBulletLine != null)
        {
            //�e���p��LineRenderer���擾�i�����ڗp�j
            LineRenderer line_rend = Instantiate(
                mBulletLine,
                Vector3.zero,
                Quaternion.identity
                ).GetComponent<LineRenderer>();

            //�_�̐�
            line_rend.positionCount = 2;
            //�n�_�̍��W�w��
            line_rend.SetPosition(0, mMuzzleTransform.position);

            //���������ꏊ����̏I�_�ɂ���
            line_rend.SetPosition(1, _hit_pos);
        }

        //���e
        if(mBulletHitPrefab != null)
        {
            //���e�n�_�ɐ���
            GameObject hit_effect = 
                Instantiate(mBulletHitPrefab,
                _hit_pos,
                Quaternion.identity
                );

            //���˕����Ɍ�����
            hit_effect.transform.rotation = 
                Quaternion.LookRotation(mMuzzleTransform.position - hit_effect.transform.position);
        }
    }
    
    //�N�[���^�C���p�R���[�`��
    private IEnumerator CooldownCoroutine(float _sec)
    {
        mIsShotCooldown = true;

        //�A�ˑ��x���҂�
        yield return new WaitForSeconds(_sec);

        mIsShotCooldown = false;
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

        mIsReload = false;
        mIsShotCooldown = false;
    }
}

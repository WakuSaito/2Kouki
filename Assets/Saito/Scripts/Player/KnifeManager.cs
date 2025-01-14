using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    // �����蔻�菈���ς݂��L�^  
    private Dictionary<int, bool> mHitMasters { get; } = new Dictionary<int, bool>();

    [SerializeField]//�����蔻��
    Collider mCollider;

    //�R���[�`���L�����Z���p
    Coroutine mAttackCoroutine;

    //�^����_���[�W
    [SerializeField] 
    private int mAttackDamage = 2;

    private void Start()
    {
        mCollider = gameObject.GetComponent<Collider>();
        mCollider.enabled = false;
    }


    /// <summary>
    /// �U���J�n
    /// </summary>
    public void StartAttack()
    {
        Debug.Log("�i�C�t�U���J�n");

        if (mAttackCoroutine != null)
            AttackCancel();//�Đ����̃R���[�`��������΃L�����Z��

        mAttackCoroutine = StartCoroutine(attack());//�R���[�`���J�n
    }

    /// <summary>
    /// �U���L�����Z���p
    /// </summary>
    public void AttackCancel()
    {
        //�Ƃ肠�����R���C�_�[�𖳌����ɂ���
        mCollider.enabled = false;

        if (mAttackCoroutine == null) return;

        //�R���[�`����~
        StopCoroutine(mAttackCoroutine);

        mAttackCoroutine = null;
    }

    IEnumerator attack()
    {
        mHitMasters.Clear(); // ���Z�b�g
        mCollider.enabled = true;

        yield return new WaitForSeconds(1.3f);

        mCollider.enabled = false;
        mAttackCoroutine = null;
    }

    void OnTriggerEnter(Collider other)
    {
        string hit_tag = other.tag;
        //�Ώۂ̃^�O�ȊO�͐ڐG���Ȃ�
        if (hit_tag != "Body" && hit_tag != "Head") return;

        // �ǉ�
        // �U���Ώە��ʂȂ�HitZone���擾�ł���
        var hit_zone = other.GetComponent<ZombieHitZone>();
        if (hit_zone == null) return;

        // �U���Ώە��ʂ̐e�̃C���X�^���XID�ŏd�������U���𔻒�
        int master_id = hit_zone.Master.GetInstanceID();
        if (mHitMasters.ContainsKey(master_id)) return;
        mHitMasters[master_id] = true;

        Vector3 hit_pos = other.ClosestPointOnBounds(transform.position);

        Debug.Log("Hit!");
        // �_���[�W�v�Z�Ƃ����̂ւ�Ŏ����ł��܂�
        hit_zone.Master.TakeDamage(hit_tag, mAttackDamage, hit_pos);

    }
}

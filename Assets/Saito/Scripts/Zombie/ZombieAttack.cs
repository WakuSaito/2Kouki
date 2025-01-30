using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�]���r�̍U���N���X</para>
/// �U������ɃA�^�b�`����
/// </summary>
public class ZombieAttack : ZombieBase
{
    // �����蔻�菈���ς݂��L�^  
    private Dictionary<int, bool> m_hitMasters { get; } = new Dictionary<int, bool>();

    [SerializeField]//�����蔻��
    Collider m_col;

    //�R���[�`���L�����Z���p
    IEnumerator m_attackCoroutine;

    [SerializeField]//��������
    private float m_setUpSec = 0.3f;
    [SerializeField]//�d������
    private float m_recoverySec = 1.0f;

    //�U�����t���O
    public bool m_isAttack { get; private set; }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    public override void SetUpZombie()
    {
        m_col = gameObject.GetComponent<Collider>();
        m_col.enabled = false;
    }

    /// <summary>
    /// �U���J�n
    /// </summary>
    public void StartAttack()
    {
        Debug.Log("�]���r�̍U��");
        m_isAttack = true;

        m_attackCoroutine = Attack();//�R���[�`���J�n
        StartCoroutine(m_attackCoroutine);
    }

    /// <summary>
    /// �U���L�����Z���p
    /// </summary>
    public void AttackCancel()
    {
        //�Ƃ肠�����R���C�_�[�𖳌����ɂ���
        m_col.enabled = false;
        m_isAttack = false;

        if (m_attackCoroutine == null) return;

        //�R���[�`����~
        StopCoroutine(m_attackCoroutine);

        m_attackCoroutine = null;
    }
    //�ꎞ��~
    public void Pause()
    {
        if (m_attackCoroutine == null) return;

        m_col.enabled = false;

        //�R���[�`����~
        StopCoroutine(m_attackCoroutine);
    }
    //�ĊJ
    public void Resume()
    {
        if (m_isAttack == false) return;
        if (m_attackCoroutine == null) return;

        m_col.enabled = true;

        StartCoroutine(m_attackCoroutine);
    }
    /// <summary>
    /// <para>�U���R���[�`��</para>
    /// �U������̗L���A�������Ȃ�
    /// </summary>
    IEnumerator Attack()
    {
        m_hitMasters.Clear(); // ���Z�b�g
        m_col.enabled = false;
        yield return new WaitForSeconds(m_setUpSec);
        m_col.enabled = true;
        yield return new WaitForSeconds(m_recoverySec);
        m_col.enabled = false;
        yield return new WaitForSeconds(1);//�ǉ��A�j���[�V�����^�C�~���O�����p
        m_attackCoroutine = null;
        m_isAttack = false;
    }

    //�U������Ƀv���C���[�������������Ƀ_���[�W��^����
    void OnTriggerEnter(Collider other)
    {
        // �ǉ�
        // �U���Ώە��ʂȂ�HitZone���擾�ł���
        var hit_zone = other.GetComponent<HitZone>();
        if (hit_zone == null) return;

        // �U���Ώە��ʂ̐e�̃C���X�^���XID�ŏd�������U���𔻒�
        int master_id = hit_zone.Master.GetInstanceID();
        if (m_hitMasters.ContainsKey(master_id)) return;
        m_hitMasters[master_id] = true;

        Debug.Log("Hit!");
        // �_���[�W�v�Z�Ƃ����̂ւ�Ŏ����ł��܂�
        hit_zone.Master.TakeDamage();
        // �q�b�g�ӏ����v�Z���ăG�t�F�N�g��\������i�O�񂩂���ɕύX�Ȃ��j
        //Vector3 hitPos = other.ClosestPointOnBounds(col.bounds.center);
        //GameObject effectIstance = Instantiate(hitEffect, hitPos, Quaternion.identity);
        //Destroy(effectIstance, 1);
    }
}

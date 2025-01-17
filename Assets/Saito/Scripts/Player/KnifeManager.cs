using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    // �����蔻�菈���ς݂��L�^  
    private Dictionary<int, bool> m_hitMasters { get; } = new Dictionary<int, bool>();
    //�����蔻��
    [SerializeField] Collider m_collider;

    //�^����_���[�W
    [SerializeField] private int m_attackDamage = 2;

    //�R���[�`���L�����Z���p
    Coroutine m_attackCoroutine;

    private void Start()
    {
        m_collider = gameObject.GetComponent<Collider>();
        m_collider.enabled = false;
    }


    /// <summary>
    /// �U���J�n
    /// </summary>
    public void StartAttack()
    {
        Debug.Log("�i�C�t�U���J�n");

        if (m_attackCoroutine != null)
            AttackCancel();//�Đ����̃R���[�`��������΃L�����Z��

        m_attackCoroutine = StartCoroutine(attack());//�R���[�`���J�n
    }

    /// <summary>
    /// �U���L�����Z���p
    /// </summary>
    public void AttackCancel()
    {
        //�Ƃ肠�����R���C�_�[�𖳌����ɂ���
        m_collider.enabled = false;

        if (m_attackCoroutine == null) return;

        //�R���[�`����~
        StopCoroutine(m_attackCoroutine);

        m_attackCoroutine = null;
    }

    IEnumerator attack()
    {
        m_hitMasters.Clear(); // ���Z�b�g
        m_collider.enabled = true;

        yield return new WaitForSeconds(1.3f);

        m_collider.enabled = false;
        m_attackCoroutine = null;
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
        if (m_hitMasters.ContainsKey(master_id)) return;
        m_hitMasters[master_id] = true;

        Vector3 hit_pos = other.ClosestPointOnBounds(transform.position);

        Debug.Log("Hit!");
        // �_���[�W�v�Z�Ƃ����̂ւ�Ŏ����ł��܂�
        hit_zone.Master.TakeDamage(hit_tag, m_attackDamage, hit_pos);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    // �����蔻�菈���ς݂��L�^  
    private Dictionary<int, bool> hitMasters { get; } = new Dictionary<int, bool>();

    [SerializeField]//�����蔻��
    Collider col;

    //�R���[�`���L�����Z���p
    Coroutine attackCoroutine;

    //�^����_���[�W
    [SerializeField] 
    private int attackDamage = 2;

    private void Start()
    {
        col = gameObject.GetComponent<Collider>();
        col.enabled = false;
    }


    /// <summary>
    /// �U���J�n
    /// </summary>
    public void StartAttack()
    {
        Debug.Log("�i�C�t�U���J�n");

        if (attackCoroutine != null)
            AttackCancel();//�Đ����̃R���[�`��������΃L�����Z��

        attackCoroutine = StartCoroutine(attack());//�R���[�`���J�n
    }

    /// <summary>
    /// �U���L�����Z���p
    /// </summary>
    public void AttackCancel()
    {
        //�Ƃ肠�����R���C�_�[�𖳌����ɂ���
        col.enabled = false;

        if (attackCoroutine == null) return;

        //�R���[�`����~
        StopCoroutine(attackCoroutine);

        attackCoroutine = null;
    }

    IEnumerator attack()
    {
        hitMasters.Clear(); // ���Z�b�g
        col.enabled = true;

        yield return new WaitForSeconds(1.3f);

        col.enabled = false;
        attackCoroutine = null;
    }

    void OnTriggerEnter(Collider other)
    {
        string hitTag = other.tag;
        //�Ώۂ̃^�O�ȊO�͐ڐG���Ȃ�
        if (hitTag != "Body" && hitTag != "Head") return;

        // �ǉ�
        // �U���Ώە��ʂȂ�HitZone���擾�ł���
        var hitZone = other.GetComponent<ZombieHitZone>();
        if (hitZone == null) return;

        // �U���Ώە��ʂ̐e�̃C���X�^���XID�ŏd�������U���𔻒�
        int masterId = hitZone.Master.GetInstanceID();
        if (hitMasters.ContainsKey(masterId)) return;
        hitMasters[masterId] = true;

        //Vector3 hitPos = other.

        Debug.Log("Hit!");
        // �_���[�W�v�Z�Ƃ����̂ւ�Ŏ����ł��܂�
        hitZone.Master.TakeDamage(hitTag, attackDamage);

        // �q�b�g�ӏ����v�Z���ăG�t�F�N�g��\������i�O�񂩂���ɕύX�Ȃ��j
        //Vector3 hitPos = other.ClosestPointOnBounds(col.bounds.center);
        //GameObject effectIstance = Instantiate(hitEffect, hitPos, Quaternion.identity);
        //Destroy(effectIstance, 1);
    }
}

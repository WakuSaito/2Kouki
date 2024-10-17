using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    /// <summary>
    /// �����蔻�菈���ς݂��L�^
    /// </summary>
    private Dictionary<int, bool> hitMasters { get; } = new Dictionary<int, bool>();

    [SerializeField]//�����蔻��
    Collider col;

    [SerializeField]//�A�j���[�V�����p
    Animator ani;

    [SerializeField]
    bool on_attack = false;//�f�o�b�O�p

    private void Start()
    {
        col.enabled = false;
    }

    private void Update()
    {
        if(on_attack)
        {
            on_attack = false;
            StartAttack();
        }
    }

    //�U���J�n
    public void StartAttack()
    {
        StartCoroutine(attack());//�R���[�`���J�n
        ani.SetTrigger("Attack");
    }

    IEnumerator attack()
    {
        hitMasters.Clear(); // �ǉ�
        col.enabled = true;
        yield return new WaitForSeconds(2.0f);
        col.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // �ǉ�
        // �U���Ώە��ʂȂ�HitZone���擾�ł���
        var hitZone = other.GetComponent<HitZone>();
        if (hitZone == null) return;

        // �U���Ώە��ʂ̐e�̃C���X�^���XID�ŏd�������U���𔻒�
        int masterId = hitZone.Master.GetInstanceID();
        if (hitMasters.ContainsKey(masterId)) return;
        hitMasters[masterId] = true;

        Debug.Log("Hit!");
        // �_���[�W�v�Z�Ƃ����̂ւ�Ŏ����ł��܂��˂�
        hitZone.Master.TakeDamage();
        // �q�b�g�ӏ����v�Z���ăG�t�F�N�g��\������i�O�񂩂���ɕύX�Ȃ��j
        //Vector3 hitPos = other.ClosestPointOnBounds(col.bounds.center);
        //GameObject effectIstance = Instantiate(hitEffect, hitPos, Quaternion.identity);
        //Destroy(effectIstance, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �]���r�̍s��
/// �N���X���͕ύX�����ق�����������
/// </summary>
public class ZombieAction : ZombieBase
{
    [SerializeField]//�̂�����̗�
    private float knockBackPower = 10.0f;

    Rigidbody rb;

    public override void SetUpZombie()
    {
        //rigidbody�̎擾
        rb = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// �̂�����
    /// ��ň����ɂ̂�����������w�肵����
    /// </summary>
    public void KnockBack()
    {
        //�̂�����
        rb.AddForce(transform.forward * -1.0f * knockBackPower, ForceMode.Impulse);
    }
    /// <summary>
    /// ���S����
    /// </summary>
    public void Dead()
    {
        Debug.Log("�]���r�����S");
        Destroy(gameObject);
    }
}

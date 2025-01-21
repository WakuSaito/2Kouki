using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�]���r�̍s��</para>
/// �N���X���͕ύX�����ق�����������
/// </summary>
public class ZombieAction : ZombieBase
{
    [SerializeField]//�̂�����̗�
    private float m_knockBackPower = 10.0f;

    [SerializeField]//���S���ɐ�������I�u�W�F�N�g
    private GameObject m_dropItemPrefab;

    Rigidbody m_rigidbody;

    public override void SetUpZombie()
    {
        //rigidbody�̎擾
        m_rigidbody = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// �̂�����
    /// </summary>
    public void KnockBack(Vector3 _vec)
    {
        Vector3.Normalize(_vec);//���K��
        //�̂�����
        m_rigidbody.AddForce(_vec * m_knockBackPower, ForceMode.Impulse);
    }
    /// <summary>
    /// ���S����
    /// </summary>
    public void Dead()
    {
        Debug.Log("�]���r�����S");

        ItemDrop();//�A�C�e���h���b�v

        Destroy(gameObject);//�Ƃ肠������destroy
    }

    /// <summary>
    /// �I�u�W�F�N�g���h���b�v����
    /// </summary>
    private void ItemDrop()
    {
        //�h���b�v����A�C�e���̎w�肪�������return
        if (m_dropItemPrefab == null) return;

        //�Ƃ肠���������ʒu�Ƀh���b�v
        Vector3 drop_pos = transform.position;
        //�Ƃ肠�������܂�Ȃ��悤��

        //����
        Instantiate(
            m_dropItemPrefab,
            drop_pos, 
            Quaternion.identity
            );
    }
}

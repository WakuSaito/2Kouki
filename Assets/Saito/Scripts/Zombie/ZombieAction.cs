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

    [SerializeField]//���S���ɐ�������I�u�W�F�N�g
    private GameObject dropItemPrefab;

    Rigidbody rb;

    public override void SetUpZombie()
    {
        //rigidbody�̎擾
        rb = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// �̂�����
    /// </summary>
    public void KnockBack(Vector3 _vec)
    {
        Vector3.Normalize(_vec);//���K��
        //�̂�����
        rb.AddForce(_vec * knockBackPower, ForceMode.Impulse);
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
        if (dropItemPrefab == null) return;

        //�Ƃ肠���������ʒu�Ƀh���b�v
        Vector3 dropPos = transform.position;

        //����
        Instantiate(
            dropItemPrefab, 
            dropPos, 
            Quaternion.identity
            );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�V���b�g�K���}�l�[�W���[�N���X</para>
/// �e�}�l�[�W���[�N���X�����Ƀ����[�h�̕�����ύX���������ߌp�����Ď���
/// </summary>
public class ShotGunManager : GunManager
{
    //�e�����߂�Ԋu
    private const float BULLET_IN_INTERVAL = 0.6f;

    //�����[�h�L�����Z���t���O
    private bool onCancelReload = false;

    /// <summary>
    /// <para>�����[�h</para>
    /// �p�������㏑�����A����e������悤�ɂ���
    /// </summary>
    public override void Reload()
    {
        if (m_isShotCooldown) return;
        if (m_isReload) return;
        //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
        if (GetCurrentMagazine() >= GetMagazineSize()) return;

        onCancelReload = false;//�L�����Z����ԃ��Z�b�g

        //if (m_inventory == null)
        if (m_inventoryItem == null)
        {
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            Invoke(nameof(StartBulletIn), 0.22f);
            return;
        }

        if (m_inventoryItem.CheckBullet())
        {
            //�C���x���g���ɒe�ۂ����邩
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            Invoke(nameof(StartBulletIn), 0.22f);
        }

    }

    /// <summary>
    /// <para>�����[�h�L�����Z��</para>
    /// �A�j���[�V�����J�ڂƃt���O�Ǘ�
    /// </summary>
    public override void StopReload()
    {
        onCancelReload = true;
        m_isReload = false;
        m_animator.SetBool("Reload", false);
    }

    /// <summary>
    /// <para>�e���ߊJ�n</para>
    /// Invoke�Œx�����s���邽�߂̃N�b�V����
    /// </summary>
    public void StartBulletIn()
    {
        StartCoroutine(BulletIn());
    }

    /// <summary>
    /// <para>�e���߃R���[�`��</para>
    /// ���Ԋu���Ƃɒe�����߂�
    /// </summary>
    private IEnumerator BulletIn()
    {
        //�����[�h�\�Ȓe���擾
        int bulletInNum = HowManyCanLoaded();
        //�ő�A�܂��̓L�����Z�������܂ň���e������
        for (int i = 0; i < bulletInNum; i++)
        {
            m_animator.SetTrigger("BulletIn");//�A�j���[�V�����Đ�

            yield return new WaitForSeconds(BULLET_IN_INTERVAL);

            if (onCancelReload)//�L�����Z��
                yield break;

            AddBullet(1);
        }

        //�����[�h�I��
        m_animator.SetBool("Reload", false);
        m_isReload = false;
    }

}

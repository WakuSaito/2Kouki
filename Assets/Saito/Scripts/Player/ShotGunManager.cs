using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�V���b�g�K���}�l�[�W���[�N���X</para>
/// �e�}�l�[�W���[�N���X�����Ƀ����[�h�̕�����ύX���������ߌp�����Ď���
/// </summary>
public class ShotGunManager : GunManager
{
    //�e�����n�߂�܂ł̎���
    private const float START_RELOAD_DELAY = 0.22f;
    //�e�����߂�Ԋu
    private const float BULLET_IN_INTERVAL = 0.6f;

    //�����[�h�L�����Z���t���O
    private bool m_onCancelReload = false;

    //�ꎞ��~�p
    IEnumerator m_bulletInCoroutine;

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

        m_onCancelReload = false;//�L�����Z����ԃ��Z�b�g

        //if (m_inventory == null)
        if (m_inventoryItem == null)
        {
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            m_bulletInCoroutine = BulletIn();
            StartCoroutine(m_bulletInCoroutine);
            return;
        }
        else if (m_inventoryItem.CheckBullet())
        {
            //�C���x���g���ɒe�ۂ����邩
            m_animator.SetBool("Reload", true);  //reload
            m_isReload = true;
            m_bulletInCoroutine = BulletIn();
            StartCoroutine(m_bulletInCoroutine);
        }

    }


    /// <summary>
    /// <para>�����[�h�L�����Z��</para>
    /// �A�j���[�V�����J�ڂƃt���O�Ǘ�
    /// </summary>
    public override void StopReload()
    {
        m_onCancelReload = true;
        m_isReload = false;
        m_animator.SetBool("Reload", false);
        StopCoroutine(m_bulletInCoroutine);
        m_bulletInCoroutine = null;
    }

    /// <summary>
    /// <para>�e���߃R���[�`��</para>
    /// ���Ԋu���Ƃɒe�����߂�
    /// </summary>
    private IEnumerator BulletIn()
    {
        //�R���[�`�����ĊJ���Ă��ҋ@���ԏ�񂪏����Ȃ��悤�ɂ���
        for (float j = 0; j < START_RELOAD_DELAY; j += 0.1f)
            yield return new WaitForSeconds(0.1f);

        //�����[�h�\�Ȓe���擾
        int bulletInNum = HowManyCanLoaded();
        //�ő�A�܂��̓L�����Z�������܂ň���e������
        for (int i = 0; i < bulletInNum; i++)
        {
            //�e�������Ȃ�����
            if (m_inventoryItem.CheckBullet() == false)
            {
                break;
            }

            m_animator.SetTrigger("BulletIn");//�A�j���[�V�����Đ�

            //�R���[�`�����ĊJ���Ă��ҋ@���ԏ�񂪏����Ȃ��悤�ɂ���
            for (float j = 0; j < BULLET_IN_INTERVAL; j += 0.1f)
                yield return new WaitForSeconds(0.1f);

            if (m_onCancelReload)//�L�����Z��
            {
                m_bulletInCoroutine = null;
                yield break;
            }
            AddBullet(1);
        }

        //�����[�h�I��
        m_animator.SetBool("Reload", false);
        m_isReload = false;
        m_bulletInCoroutine = null;
    }

    public override void Pause()
    {
        base.Pause();

        if (m_bulletInCoroutine != null)
            StopCoroutine(m_bulletInCoroutine);
    }

    public override void Resume()
    {
        base.Resume();

        if (m_bulletInCoroutine != null)
            StartCoroutine(m_bulletInCoroutine);
    }

}

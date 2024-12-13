using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunManager : GunManager
{
    private const float BULLET_IN_INTERVAL = 0.6f;

    private bool onCancelReload = false;


    public override void Reload()
    {
        if (isShotCooldown) return;
        if (isReload) return;
        //�s�X�g���̒e�ۂ��ő吔����Ȃ����reload�\
        if (GetCurrentMagazine() >= GetMagazineSize()) return;

        onCancelReload = false;//�L�����Z����ԃ��Z�b�g

        if (inventory == null)
        {
            anim.SetBool("Reload", true);  //reload
            isReload = true;
            Invoke(nameof(StartBulletIn), 0.22f);
            return;
        }

        for (int i = 0; i < Inventory.INVENTORY_MAX; i++)
        {
            //�C���x���g���ɒe�ۂ����邩
            if (inventory.item_type_id[i] == (int)ID.ITEM_ID.BULLET)
            {
                anim.SetBool("Reload", true);  //reload
                isReload = true;
                Invoke(nameof(StartBulletIn), 0.22f);
            }
        }

    }

    //�����[�h�L�����Z��
    public override void StopReload()
    {
        onCancelReload = true;
        isReload = false;
        anim.SetBool("Reload", false);
    }

    public void StartBulletIn()
    {
        StartCoroutine(BulletIn());
    }

    private IEnumerator BulletIn()
    {
        //�����[�h�\�Ȓe���擾
        int bulletInNum = HowManyCanLoaded();

        for (int i = 0; i < bulletInNum; i++)
        {
            anim.SetTrigger("BulletIn");

            yield return new WaitForSeconds(BULLET_IN_INTERVAL);

            if (onCancelReload)//�L�����Z��
                yield break;

            AddBullet(1);
        }

        //�����[�h�I��
        anim.SetBool("Reload", false);
        isReload = false;
    }

}

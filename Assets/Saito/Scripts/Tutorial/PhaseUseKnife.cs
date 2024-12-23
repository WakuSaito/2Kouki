using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseUseKnife : TutorialBase
{
    [SerializeField]//����̍��W
    private GameObject targetZombieObj;

    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject plzChangeWeaponUI;
    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject plzUseKnifeUI;

    private Inventory inventory;

    private WeaponInventory weaponInventory;

    [SerializeField]
    private DogManager dogManager;

    [SerializeField]//�J
    private GameObject dogWhistle;

    public override void SetUpPhase()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        //weaponInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<weaponInventory>();

        tutorialManager.SetText("�i�C�t�Ń]���r��|����");
        tutorialManager.CreateMarker(targetZombieObj.transform.position);
    }

    public override void UpdatePhase()
    {
        if (inventory == null) return;

        //�C���x���g�����J���Ă���Ƃ��͎ז��ɂȂ�̂ŏ���
        if (inventory.item_inventory_flag == true)
        {
            plzChangeWeaponUI.SetActive(false);
            plzUseKnifeUI.SetActive(false);
        }
        //if �i�C�t�������Ă��Ȃ��Ȃ� �X���b�g�؂�ւ��𑣂�
        else if (inventory.hand_weapon != Inventory.WEAPON_ID.KNIFE)
        {
            plzChangeWeaponUI.SetActive(true);
            plzUseKnifeUI.SetActive(false);
        }
        //�����Ă���Ȃ� �U���𑣂�
        else
        {
            plzChangeWeaponUI.SetActive(false);
            plzUseKnifeUI.SetActive(true);

        }

        //�]���r��|������
        //�t�F�[�Y�ڍs
        if (targetZombieObj == null)
        {
            //�������ԂɂȂ�
            dogManager.OnStopAction(false);
            //�J����������
            inventory.weapon_hand_obj[3] = dogWhistle;
            //weaponInventory.weapon[3] = dogWhistle;

            tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();

        plzChangeWeaponUI.SetActive(false);
        plzUseKnifeUI.SetActive(false);
    }
}

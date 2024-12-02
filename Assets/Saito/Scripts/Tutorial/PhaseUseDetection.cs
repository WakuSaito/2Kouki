using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseUseDetection : TutorialBase
{
    [SerializeField]//�ڕW�̍��W
    private Vector3 TargetPos;

    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject plzChangeWeaponUI;
    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject plzUseDetectionUI;

    private Inventory inventory;

    [SerializeField]
    private DogManager dogManager;

    public override void SetUpPhase()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        tutorialManager.SetText("���Ɏw�����o��\n�Ƃ𒲂ׂ悤");
    }

    public override void UpdatePhase()
    {
        //if �J�������Ă��Ȃ��Ȃ�@��������悤����
        if (inventory.hand_weapon != Inventory.WEAPON_ID.DOG)
        {
            plzChangeWeaponUI.SetActive(true);
            plzUseDetectionUI.SetActive(false);
        }
        //�����Ă���Ȃ�@�g���悤����
        else
        {
            plzChangeWeaponUI.SetActive(false);
            plzUseDetectionUI.SetActive(true);
        }

        if (dogManager.UsedOrderDetection())
        {
            //���̃t�F�[�Y�ɐi�߂�
            tutorialManager.NextPhase();

        }
    }

    public override void EndPhase()
    {
        tutorialManager.HideText();
        tutorialManager.DeleteMarker();

        plzChangeWeaponUI.SetActive(false);
        plzUseDetectionUI.SetActive(false);
    }

}

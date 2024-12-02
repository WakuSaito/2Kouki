using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseExploreHouse : TutorialBase
{
    [SerializeField]//�ڕW�̍��W
    private Vector3 TargetPos;

    [SerializeField]//����؂�ւ��𑣂�UI
    private GameObject plzChangeWeaponUI;

    private Inventory inventory;

    public override void SetUpPhase()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        tutorialManager.SetText("���Ɏw�����o��\n�ӂ�𒲂ׂ悤");
    }

    public override void UpdatePhase()
    {
        //if �J�������Ă��Ȃ��Ȃ�@��������悤����
        if (inventory.hand_weapon != Inventory.WEAPON_ID.DOG)
        {
            plzChangeWeaponUI.SetActive(true);
        }
        //�����Ă���Ȃ�@�g���悤����
        else
        {
            plzChangeWeaponUI.SetActive(false);
        }

        //if(�w�����o������)
        //DogManager�̃N�[���_�E�����Ď�

        tutorialManager.SetText("�Ƃ̒���T�����悤");
        tutorialManager.CreateMarker(TargetPos);
        //�v���C���[�ƖڕW���W�̋��������ȉ��Ȃ�
        float distance = Vector3.Distance(TargetPos, PlayerPos());
        if (distance < 4.0f)
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
    }

}

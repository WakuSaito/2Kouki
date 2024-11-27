using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TutorialBase : MonoBehaviour
{
    //�}�l�[�W���N���X���������Ă���
    protected TutorialManager tutorialManager;

    //�ʒu���擾���邽�߂ɏ���
    private GameObject playerObj;

    private void Awake()
    {
        //�}�l�[�W���N���X�擾
        tutorialManager = gameObject.GetComponent<TutorialManager>();

        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    //�v���C���[�̈ʒu�擾
    protected Vector3 PlayerPos()
    {
        return playerObj.transform.position;
    }


    //�����ݒ�
    public abstract void SetUpPhase();

    //����
    public abstract void UpdatePhase();

    //�I������
    public abstract void EndPhase();

}

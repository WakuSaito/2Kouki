using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����ŃA�^�b�`�����X�N���v�g
[RequireComponent(typeof(PhaseGoHome))]
[RequireComponent(typeof(PhaseTakeWhistle))]
[RequireComponent(typeof(PhaseUseKnife))]
[RequireComponent(typeof(PhaseExploreHouse))]

public class TutorialManager : MonoBehaviour
{
    //���݂̐i�s�x�i�`���[�g���A���́j
    private int currentPhase = 0;

    [SerializeField]//�w���p�e�L�X�g
    private Text tutorialText;

    [SerializeField]//�ʒu�w���p�}�[�J�[
    private GameObject markerPrefab;

    private GameObject markerObj;

    //�`���[�g���A�����t�F�[�Y���Ƃɕ������X�N���v�g
    [SerializeField]
    private TutorialBase[] tutorialBases;

    private void Start()
    {
        //�J�n���̃X�N���v�g�Ăяo��
        tutorialBases[currentPhase].SetUpPhase();
    }

    private void Update()
    {
        if (currentPhase >= tutorialBases.Length) return;

        //���݂̃t�F�[�Y�̃X�N���v�g�����Ăяo��
        tutorialBases[currentPhase].UpdatePhase();

    }

    public void NextPhase()
    {
        Debug.Log("�`���[�g���A���̃t�F�[�Y�ڍs");
        //�I�������Ăяo��
        tutorialBases[currentPhase].EndPhase();

        currentPhase++;
        if (currentPhase >= tutorialBases.Length) return;

        //�J�n���̃X�N���v�g�Ăяo��
        tutorialBases[currentPhase].SetUpPhase();
    }

    /// <summary>
    /// �w�肵���e�L�X�g�̕\��
    /// </summary>
    public void SetText(string _str)
    {
        if (tutorialText == null) return;
        tutorialText.text = _str;

        //���i�����Ȃ��悤�ɂ�����
    }
    /// <summary>
    /// �e�L�X�g���\���ɂ���
    /// </summary>
    public void HideText()
    {
        if (tutorialText == null) return;
        tutorialText.text = "";//�Ƃ肠�����e�L�X�g�̒��g���㏑��
    }

    /// <summary>
    /// �w�肵�����W�Ƀ}�[�J�[�ݒu
    /// </summary>
    public void CreateMarker(Vector3 _pos)
    {
        if (markerPrefab == null) return;

        //���ɐݒu���Ă���}�[�J�[�폜
        DeleteMarker();

        markerObj = Instantiate(markerPrefab, _pos, Quaternion.identity);
    }
    /// <summary>
    /// �}�[�J�[�폜
    /// </summary>
    public void DeleteMarker()
    {
        if (markerObj == null) return;
        //�}�[�J�[�폜
        markerObj.GetComponent<Marker>().StartDelete();
        markerObj = null;
    }

    //�J���������̏ꏊ�ɃX���C�h�ړ��ł���悤�ɂ�����
}

/*
�Q�[���J�n�i�t�F�[�h�C���j

�������T������

���������邽�ߊO�̃]���r��|���i�i�C�t���g���j�i�ȒP�ɓ|����悤�ɂ������j

�������ԂɂȂ�
���Ɏw�����o�����J�����Ɏ���Ɍ�����

�J���擾������

�߂��̖��Ƃ����̒T�m�����p���T��

���ɍU���w�������A�����̃]���r�����j

���Ԋ�n���U�����悤�i�Q�[���̖ړI�����j
 */
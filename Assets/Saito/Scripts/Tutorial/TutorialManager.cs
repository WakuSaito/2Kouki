using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����ŃA�^�b�`�����X�N���v�g
//[RequireComponent(typeof(PhaseGoHome))]
//[RequireComponent(typeof(PhaseTakeItem))]
//[RequireComponent(typeof(PhaseUseKnife))]
//[RequireComponent(typeof(PhaseExploreHouse))]

/// <summary>
/// �`���[�g���A���Ǘ��N���X
/// TutorialBase���p�������N���X�����ԂɎ��s����
/// </summary>
public class TutorialManager : MonoBehaviour
{
    //���݂̐i�s�x�i�`���[�g���A���́j
    private int m_currentPhase = 0;

    //�w���p�e�L�X�g
    [SerializeField] private Text m_tutorialText;
    //�ʒu�w���p�}�[�J�[
    [SerializeField] private GameObject m_markerPrefab;

    private GameObject m_markerObj;

    //�`���[�g���A�����t�F�[�Y���Ƃɕ������X�N���v�g
    [SerializeField] private TutorialBase[] m_tutorialBases;

    private void Start()
    {
        //�J�n���̃X�N���v�g�Ăяo��
        m_tutorialBases[m_currentPhase].SetUpPhase();
    }

    private void Update()
    {
        if (m_currentPhase >= m_tutorialBases.Length) return;

        //���݂̃t�F�[�Y�̃X�N���v�g�����Ăяo��
        m_tutorialBases[m_currentPhase].UpdatePhase();
    }

    /// <summary>
    /// �t�F�[�Y�ڍs
    /// ���ɕ��񂾃`���[�g���A�����������ɐi�߂�
    /// </summary>
    public void NextPhase()
    {
        Debug.Log("�`���[�g���A���̃t�F�[�Y�ڍs");
        //�I�������Ăяo��
        m_tutorialBases[m_currentPhase].EndPhase();

        m_currentPhase++;
        if (m_currentPhase >= m_tutorialBases.Length) return;

        //�J�n���̃X�N���v�g�Ăяo��
        m_tutorialBases[m_currentPhase].SetUpPhase();
    }

    /// <summary>
    /// �e�L�X�g�̕\��
    /// �`���[�g���A���p�e�L�X�g��\������
    /// </summary>
    public void SetText(string _str)
    {
        if (m_tutorialText == null) return;
        m_tutorialText.text = _str;

        //���i�����Ȃ��悤�ɂ�����
    }
    /// <summary>
    /// �e�L�X�g�̔�\��
    /// </summary>
    public void HideText()
    {
        if (m_tutorialText == null) return;
        m_tutorialText.text = "";//�Ƃ肠�����e�L�X�g�̒��g���㏑��
    }

    /// <summary>
    /// �}�[�J�[�쐬
    /// �w�肵�����W�Ƀ}�[�J�[�ݒu
    /// </summary>
    public void CreateMarker(Vector3 _pos)
    {
        if (m_markerPrefab == null) return;

        //���ɐݒu���Ă���}�[�J�[�폜
        DeleteMarker();

        m_markerObj = Instantiate(m_markerPrefab, _pos, Quaternion.identity);
    }
    /// <summary>
    /// �}�[�J�[�폜
    /// </summary>
    public void DeleteMarker()
    {
        if (m_markerObj == null) return;
        //�}�[�J�[�폜
        m_markerObj.GetComponent<Marker>().StartDelete();
        m_markerObj = null;
    }

    //�J���������̏ꏊ�ɃX���C�h�ړ��ł���悤�ɂ�����
}

/*
�Q�[���J�n�i�t�F�[�h�C���j

���������邽�ߊO�̃]���r��|���i�i�C�t���g���j�i�ȒP�ɓ|����悤�ɂ������j

�߂��̖��Ƃ����̒T�m�����p���T��

�H�����Q�b�g

�C���x���g������H����H�ׂ�����

���ɍU���w�������A�����̃]���r�����j

���Ԋ�n���U�����悤�i�Q�[���̖ړI�����j
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>���U���gUI�N���X</para>
/// ���U���g�V�[����UI�\�� 
/// </summary>
public class ResultUI : MonoBehaviour
{
    //�\���e�L�X�g
    [SerializeField] private Text m_dayText;

    // Static�ϐ����Q�Ƃ��A����������ݒ�
    void Start()
    {
        m_dayText.text = "�����c���������@" + StaticVariables.liveingDayCount + "��";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�F�ύX�N���X</para>
/// �ύX�������I�u�W�F�N�g�ɃA�^�b�` �����p�[�c�ɕ�����Ă���I�u�W�F�N�g�ɑΉ�
/// mesh��2�Ԗڂ̃}�e���A���̃��l��ύX����
/// </summary>
public class ColorChanger : MonoBehaviour
{
    //���݂̐F�̃A���t�@�l
    private float m_currentAlpha;

    [SerializeField]//Mesh���A�^�b�`���ꂽ�I�u�W�F�N�g
    GameObject[] m_meshObjs;

    //�ŏ��̃J���[�̃A���t�@�l�ۑ�
    private void Awake()
    {
        if (m_meshObjs.Length == 0) return;

        //�J���[�̃A���t�@�l�擾
        m_currentAlpha = m_meshObjs[0].GetComponent<Renderer>().materials[1].color.a;
    }

    /// <summary>
    /// �F�̃A���t�@�l�ύX
    /// �}�e���A���̃J���[�̓����x��ύX����
    /// </summary>
    /// <param name="_alpha">�A���t�@�l(�����x)</param>
    public void ChangeColorAlpha(float _alpha)
    {
        if (m_meshObjs.Length == 0) return;

        //�F���ς��Ȃ��ꍇ�������s��Ȃ��悤�ɂ���
        if (m_currentAlpha == _alpha) return;
        m_currentAlpha = _alpha;

        //�F�ύX
        foreach(var mesh in m_meshObjs)
        {
            Color current_color = mesh.GetComponent<Renderer>().materials[1].color;
            mesh.GetComponent<Renderer>().materials[1].color = new Color(current_color.r, current_color.g, current_color.b, _alpha);
        }
    }
}

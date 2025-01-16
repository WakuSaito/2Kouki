using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e���G�t�F�N�g�N���X
/// �G�t�F�N�g�ɃA�^�b�`����@����Ƀt�F�[�h�A�E�g����
/// </summary>
public class BulletLineEffect : MonoBehaviour
{
    //�t�F�[�h�A�E�g���鑬�x
    [SerializeField] float m_fadeOutSpeed = 1.0f;

    //���݂̃J���[�̃A���t�@�l
    private float m_currentAlpha;
    //���̐F
    private Color m_originColor;

    //�����̐F���擾
    private void Awake()
    {
        //���̃J���[���ۑ�
        m_originColor = gameObject.GetComponent<Renderer>().material.color;
        //�J���[�̃A���t�@�l�擾
        m_currentAlpha = m_originColor.a;
    }

    // �t�F�[�h�A�E�g������
    void Update()
    {
        m_currentAlpha -= m_fadeOutSpeed * Time.deltaTime;

        //�A���t�@�l��0�ȉ��ɂȂ�Ȃ�폜
        if(m_currentAlpha <= 0)
        {
            Destroy(gameObject);
        }

        //�A���t�@�l�ύX
        gameObject.GetComponent<Renderer>().material.color = 
            new Color(m_originColor.r, m_originColor.g, m_originColor.b, m_currentAlpha);
    }
}

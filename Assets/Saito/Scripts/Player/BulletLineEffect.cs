using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineEffect : MonoBehaviour
{
    //�t�F�[�h�A�E�g���鑬�x
    [SerializeField] float m_fadeOutSpeed = 1.0f;

    //���݂̃J���[�̃A���t�@�l
    private float m_currentAlpha;

    //���̐F
    private Color m_originColor;

    private void Awake()
    {
        //���̃J���[���ۑ�
        m_originColor = gameObject.GetComponent<Renderer>().material.color;

        //�J���[�̃A���t�@�l�擾
        m_currentAlpha = m_originColor.a;
    }

    // Update is called once per frame
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

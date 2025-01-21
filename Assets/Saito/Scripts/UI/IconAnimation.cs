using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>�A�C�R���A�j���[�V�����N���X</para>
/// �A�C�R����_�ł������肷��
/// </summary>
public class IconAnimation : MonoBehaviour
{
    enum ANIM_TYPE
    {
        BLINK,//�_��
        UPDOWN,//�㉺
    }
    [SerializeField]
    ANIM_TYPE m_animationType = ANIM_TYPE.BLINK;

    [SerializeField]
    Sprite[] m_iconSprites;

    Image m_image;

    float m_count = 0;
    int m_currentSprite = 0;

    private void Awake()
    {
        m_image = gameObject.GetComponent<Image>();
    }

    // ��Ԃɂ���ăA�j���[�V���������s����
    void Update()
    {
        switch (m_animationType) 
        {
            case ANIM_TYPE.BLINK:

                m_count += Time.deltaTime;
                if (m_count > 0.5f)
                {
                    m_count = 0;
                    if (m_currentSprite == 0)
                        m_currentSprite = 1;
                    else
                        m_currentSprite = 0;

                    m_image.sprite = m_iconSprites[m_currentSprite];
                }

                break;

            case ANIM_TYPE.UPDOWN:

                break;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    private Transform m_canvas;
    private Camera m_cameraObj;

    //��ʂɕ\������}�[�J�[UI
    [SerializeField] 
    GameObject m_markUIPrefab;
    //�����\���p�̃e�L�X�g
    GameObject m_distanceTextUI;

    //��������č폜�����܂ł̎��ԁi�O�ȉ��ō폜����Ȃ��j
    [SerializeField] 
    float m_destroySec = 3.0f;
    //�t�F�[�h�A�E�g�̑��x
    [SerializeField]
    float m_fadeOutSpeed = 1.0f;

    //���������I�u�W�F�N�g�ۑ��p
    private GameObject m_markUI;

    //�폜�t���O
    private bool m_onDelete = false;

    private float m_currentAlpha; 

    // Start is called before the first frame update
    void Start()
    {
        //�폜�܂ł̎��Ԃ�0�ȉ��Ȃ�폜���Ȃ�
        if(m_destroySec > 0)
            //��莞�Ԍ�폜
            StartCoroutine(DerayDestroy());

        m_canvas = GameObject.Find("Canvas").transform;
        m_cameraObj = Camera.main;

        //UI����
        m_markUI = Instantiate(m_markUIPrefab, m_canvas);
        //�e�L�X�g�擾
        m_distanceTextUI = m_markUI.transform.GetChild(0).gameObject;

        //�A���t�@�l�擾
        m_currentAlpha = m_markUI.GetComponent<Image>().color.a;

        //���̃I�u�W�F�N�g�̈ʒu��Canvas�p�ɕϊ����AUI�̈ʒu��ύX
        Vector2 screen_position = GetScreenPosition(transform.position);
        Vector2 local_position = GetCanvasLocalPosition(screen_position);
        m_markUI.transform.localPosition = local_position;
    }

    //Update is called once per frame
    void Update()
    {
        //�J�����܂ł̃x�N�g��
        Vector3 camera_normal = Vector3.Normalize(transform.position - m_cameraObj.transform.position);
        //�J�����̎��_�����ƃJ�����܂ł̃x�N�g���̓���
        float dot = Vector3.Dot(camera_normal, m_cameraObj.transform.forward);

        //���ς����ȏ�i�J�����ɉf��͈́j
        if (dot > 0.6f)
        {
            //UI�\��
            m_markUI.SetActive(true);
            //���̃I�u�W�F�N�g�̈ʒu�ƃJ�����ʒu����L�����o�X���W�����߂�
            Vector2 screen_position = GetScreenPosition(transform.position);
            Vector2 local_position = GetCanvasLocalPosition(screen_position);
            m_markUI.transform.localPosition = local_position;//�ړ�
        }
        else
        {
            //������
            m_markUI.SetActive(false);
        }

        //�e�L�X�g�ύX
        if(m_distanceTextUI != null)
        {
            //������int�ŕ\��
            string distance_text =  ((int)GetCameraDistance()).ToString() + "m";
            m_distanceTextUI.GetComponent<Text>().text = distance_text;
        }

        //�t�F�[�h�A�E�g
        if(m_onDelete)
        {
            //�A���t�@�l���炷
            m_currentAlpha -= m_fadeOutSpeed * Time.deltaTime;

            //0�ȉ��ō폜
            if(m_currentAlpha <= 0)
            {
                //UI�Ƃ��̃I�u�W�F�N�g���폜
                Destroy(m_markUI);
                Destroy(gameObject);
            }

            //�J���[�ύX
            Color color = m_markUI.GetComponent<Image>().color;
            m_markUI.GetComponent<Image>().color = new Color(color.r,color.g,color.b, m_currentAlpha);
            Color text_color = m_distanceTextUI.GetComponent<Text>().color;
            m_distanceTextUI.GetComponent<Text>().color = new Color(text_color.r, text_color.g, text_color.b, m_currentAlpha);
        }
    }

    //��莞�Ԍ�폜
    private IEnumerator DerayDestroy()
    {
        yield return new WaitForSeconds(m_destroySec);

        m_onDelete = true;//�폜�t���O
    }
    //�����폜
    public void StartDelete()
    {
        m_onDelete = true;
    }

    private float GetCameraDistance()
    {
        return Vector3.Distance(transform.position, m_cameraObj.transform.position);
    }

    //�X�N���[�����W���L�����o�X���W�ɕϊ�
    private Vector2 GetCanvasLocalPosition(Vector2 _screen_pos)
    {
        return m_canvas.transform.InverseTransformPoint(_screen_pos);
    }
    //���[���h���W���X�N���[�����W�i�J������̍��W�j�ɕϊ�
    private Vector2 GetScreenPosition(Vector3 _world_pos)
    {
        return RectTransformUtility.WorldToScreenPoint(m_cameraObj, _world_pos);
    }

}
    

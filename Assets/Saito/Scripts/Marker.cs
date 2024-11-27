using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    private Transform canvas;
    private Camera cameraObj;

    //��ʂɕ\������}�[�J�[UI
    [SerializeField] 
    GameObject markUIPrefab;
    //�����\���p�̃e�L�X�g
    GameObject distanceTextUI;

    //��������č폜�����܂ł̎��ԁi�O�ȉ��ō폜����Ȃ��j
    [SerializeField] 
    float destroySec = 3.0f;

    //�t�F�[�h�A�E�g�̑��x
    [SerializeField]
    float fadeOutSpeed = 1.0f;

    //���������I�u�W�F�N�g�ۑ��p
    private GameObject markUI;

    //�폜�t���O
    private bool onDelete = false;

    private float currentAlpha; 

    // Start is called before the first frame update
    void Start()
    {
        //�폜�܂ł̎��Ԃ�0�ȉ��Ȃ�폜���Ȃ�
        if(destroySec > 0)
            //��莞�Ԍ�폜
            StartCoroutine(DerayDestroy());

        canvas = GameObject.Find("Canvas").transform;
        cameraObj = Camera.main;
        
        //UI����
        markUI = Instantiate(markUIPrefab, canvas);
        //�e�L�X�g�擾
        distanceTextUI = markUI.transform.GetChild(0).gameObject;

        //�A���t�@�l�擾
        currentAlpha = markUI.GetComponent<Image>().color.a;

        //���̃I�u�W�F�N�g�̈ʒu��Canvas�p�ɕϊ����AUI�̈ʒu��ύX
        Vector2 screenPosition = GetScreenPosition(transform.position);
        Vector2 localPosition = GetCanvasLocalPosition(screenPosition);
        markUI.transform.localPosition = localPosition;
    }

    //Update is called once per frame
    void Update()
    {
        //�J�����܂ł̃x�N�g��
        Vector3 cameraNormal = Vector3.Normalize(transform.position - cameraObj.transform.position);
        //�J�����̎��_�����ƃJ�����܂ł̃x�N�g���̓���
        float dot = Vector3.Dot(cameraNormal, cameraObj.transform.forward);

        //���ς����ȏ�i�J�����ɉf��͈́j
        if (dot > 0.6f)
        {
            //UI�\��
            markUI.SetActive(true);
            //���̃I�u�W�F�N�g�̈ʒu�ƃJ�����ʒu����L�����o�X���W�����߂�
            Vector2 screenPosition = GetScreenPosition(transform.position);
            Vector2 localPosition = GetCanvasLocalPosition(screenPosition);
            markUI.transform.localPosition = localPosition;//�ړ�
        }
        else
        {
            //������
            markUI.SetActive(false);
        }

        //�e�L�X�g�ύX
        if(distanceTextUI != null)
        {
            //������int�ŕ\��
            string distanceText =  ((int)GetCameraDistance()).ToString() + "m";
            distanceTextUI.GetComponent<Text>().text = distanceText;
        }

        //�t�F�[�h�A�E�g
        if(onDelete)
        {
            //�A���t�@�l���炷
            currentAlpha -= fadeOutSpeed * Time.deltaTime;

            //0�ȉ��ō폜
            if(currentAlpha <= 0)
            {
                //UI�Ƃ��̃I�u�W�F�N�g���폜
                Destroy(markUI);
                Destroy(gameObject);
            }

            //�J���[�ύX
            Color color = markUI.GetComponent<Image>().color;
            markUI.GetComponent<Image>().color = new Color(color.r,color.g,color.b,currentAlpha);
            Color textColor = distanceTextUI.GetComponent<Text>().color;
            distanceTextUI.GetComponent<Text>().color = new Color(textColor.r, textColor.g, textColor.b, currentAlpha);
        }
    }

    //��莞�Ԍ�폜
    private IEnumerator DerayDestroy()
    {
        yield return new WaitForSeconds(destroySec);

        onDelete = true;//�폜�t���O
    }
    //�����폜
    public void StartDelete()
    {
        onDelete = true;
    }

    private float GetCameraDistance()
    {
        return Vector3.Distance(transform.position, cameraObj.transform.position);
    }

    //�X�N���[�����W���L�����o�X���W�ɕϊ�
    private Vector2 GetCanvasLocalPosition(Vector2 screenPosition)
    {
        return canvas.transform.InverseTransformPoint(screenPosition);
    }
    //���[���h���W���X�N���[�����W�i�J������̍��W�j�ɕϊ�
    private Vector2 GetScreenPosition(Vector3 worldPosition)
    {
        return RectTransformUtility.WorldToScreenPoint(cameraObj, worldPosition);
    }

}
    

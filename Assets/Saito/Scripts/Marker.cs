using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    private Transform canvas;
    private Camera cameraObj;

    //��ʂɕ\������}�[�J�[UI
    [SerializeField] GameObject markUIPrefab;

    [SerializeField] float destroySec = 3.0f;

    //���������I�u�W�F�N�g�ۑ��p
    private GameObject markUI;

    // Start is called before the first frame update
    void Start()
    {
        //��莞�Ԍ�폜
        StartCoroutine(DerayDestroy());

        canvas = GameObject.Find("Canvas").transform;
        cameraObj = Camera.main;
        
        //UI����
        markUI = Instantiate(markUIPrefab, canvas);

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

    }

    //��莞�Ԍ�폜
    private IEnumerator DerayDestroy()
    {
        yield return new WaitForSeconds(destroySec);

        //UI�Ƃ��̃I�u�W�F�N�g���폜
        Destroy(markUI);
        Destroy(gameObject);
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
    

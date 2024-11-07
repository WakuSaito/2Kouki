using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineEffect : MonoBehaviour
{
    //�t�F�[�h�A�E�g���鑬�x
    [SerializeField] float fadeOutSpeed = 1.0f;

    //���݂̃J���[�̃A���t�@�l
    private float currentAlpha;

    //���̐F
    private Color originColor;

    private void Awake()
    {
        //���̃J���[���ۑ�
        originColor = gameObject.GetComponent<Renderer>().material.color;

        //�J���[�̃A���t�@�l�擾
        currentAlpha =originColor.a;
    }

    // Update is called once per frame
    void Update()
    {
        currentAlpha -= fadeOutSpeed * Time.deltaTime;

        //�A���t�@�l��0�ȉ��ɂȂ�Ȃ�폜
        if(currentAlpha <= 0)
        {
            Destroy(gameObject);
        }

        //�A���t�@�l�ύX
        gameObject.GetComponent<Renderer>().material.color = 
            new Color(originColor.r, originColor.g, originColor.b, currentAlpha);
    }
}

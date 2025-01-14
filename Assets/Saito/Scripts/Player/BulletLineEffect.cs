using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineEffect : MonoBehaviour
{
    //�t�F�[�h�A�E�g���鑬�x
    [SerializeField] float mFadeOutSpeed = 1.0f;

    //���݂̃J���[�̃A���t�@�l
    private float mCurrentAlpha;

    //���̐F
    private Color mOriginColor;

    private void Awake()
    {
        //���̃J���[���ۑ�
        mOriginColor = gameObject.GetComponent<Renderer>().material.color;

        //�J���[�̃A���t�@�l�擾
        mCurrentAlpha = mOriginColor.a;
    }

    // Update is called once per frame
    void Update()
    {
        mCurrentAlpha -= mFadeOutSpeed * Time.deltaTime;

        //�A���t�@�l��0�ȉ��ɂȂ�Ȃ�폜
        if(mCurrentAlpha <= 0)
        {
            Destroy(gameObject);
        }

        //�A���t�@�l�ύX
        gameObject.GetComponent<Renderer>().material.color = 
            new Color(mOriginColor.r, mOriginColor.g, mOriginColor.b, mCurrentAlpha);
    }
}

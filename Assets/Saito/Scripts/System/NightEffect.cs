using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightEffect : MonoBehaviour
{
    [SerializeField]
    TimeController timeController;

    [SerializeField]//�`�拗���̍ő�i���ԁj
    float dayCameraFarMax = 25.0f;
    [SerializeField]//�`�拗���̍ŏ��i��ԁj
    float nightCameraFarMin = 15.0f;

    [SerializeField, Header("���v���牽���ŕ`�拗�����ŏ��ɂ��邩")]
    int minOfMinutesAfterSunset = 60;

    [SerializeField, Header("���̏o�܂ŉ������_����`�拗�����L���Ă�����")]
    int maxOfMinutesBeforeSunrise = 60;

    private void Awake()
    {
        //�C���X�y�N�^�[�ł̎w�肪������Γ����I�u�W�F�N�g����擾
        if (timeController == null)
            timeController = gameObject.GetComponent<TimeController>();       
    }

    private void Update()
    {
        //����
        if(timeController.GetIsDaylight())
        {
            Camera.main.farClipPlane = dayCameraFarMax;
        }
        //���
        else
        {
            //���v���牽����
            int minutesAfterSunset = timeController.GetMinutesAfterSunset();
            //���̏o�܂ŉ�����
            int minutesBeforeSunrise = timeController.GetMinutesBeforeSunrise();

            //�Â��̊���
            float darknessPer;

            //�Â��Ȃ�n�߂�
            if (minutesAfterSunset < minOfMinutesAfterSunset)
            {
                darknessPer = (float)minutesAfterSunset / minOfMinutesAfterSunset;
            }
            //���邭�Ȃ�n�߂�
            else if(minutesBeforeSunrise < maxOfMinutesBeforeSunrise)
            {
                darknessPer = (float)minutesBeforeSunrise / minOfMinutesAfterSunset;
            }
            //�^�钆
            else
            {
                darknessPer = 1.0f;
            }

            //�`�拗����⊮�����
            Camera.main.farClipPlane = dayCameraFarMax * (1.0f - darknessPer) + nightCameraFarMin * darknessPer;
        }
    }
}

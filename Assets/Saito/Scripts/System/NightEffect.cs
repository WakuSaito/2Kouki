using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightEffect : MonoBehaviour
{
    [SerializeField]
    TimeController mTimeController;

    [SerializeField]//�`�拗���̍ő�i���ԁj
    float mDayCameraFarMax = 25.0f;
    [SerializeField]//�`�拗���̍ŏ��i��ԁj
    float mNightCameraFarMin = 15.0f;

    [SerializeField, Header("���v���牽���ŕ`�拗�����ŏ��ɂ��邩")]
    int mMinOfMinutesAfterSunset = 60;

    [SerializeField, Header("���̏o�܂ŉ������_����`�拗�����L���Ă�����")]
    int mMaxOfMinutesBeforeSunrise = 60;

    private void Awake()
    {
        //�C���X�y�N�^�[�ł̎w�肪������Γ����I�u�W�F�N�g����擾
        if (mTimeController == null)
            mTimeController = gameObject.GetComponent<TimeController>();       
    }

    private void Update()
    {
        //����
        if(mTimeController.GetIsDaylight())
        {
            Camera.main.farClipPlane = mDayCameraFarMax;
        }
        //���
        else
        {
            //���v���牽����
            int minutes_after_sunset = mTimeController.GetMinutesAfterSunset();
            //���̏o�܂ŉ�����
            int minutes_before_sunrise = mTimeController.GetMinutesBeforeSunrise();

            //�Â��̊���
            float darkness_per;

            //�Â��Ȃ�n�߂�
            if (minutes_after_sunset < mMinOfMinutesAfterSunset)
            {
                darkness_per = (float)minutes_after_sunset / mMinOfMinutesAfterSunset;
            }
            //���邭�Ȃ�n�߂�
            else if(minutes_before_sunrise < mMaxOfMinutesBeforeSunrise)
            {
                darkness_per = (float)minutes_before_sunrise / mMinOfMinutesAfterSunset;
            }
            //�^�钆
            else
            {
                darkness_per = 1.0f;
            }

            //�`�拗����⊮�����
            Camera.main.farClipPlane = mDayCameraFarMax * (1.0f - darkness_per) + mNightCameraFarMin * darkness_per;
        }
    }
}

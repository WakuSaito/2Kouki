using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���Ԍo�߃X�N���v�g�@�i����̏����Ȃǁj
public class TimeController : MonoBehaviour
{
    [SerializeField]//���z���I�u�W�F�N�g
    private GameObject directionalLightObj;

    [SerializeField]//���Ԃ̒���
    private float daylightLengthSec = 120;
    [SerializeField]//��̒���
    private float nightLengthSec = 90;

    //1���̍��v����
    private float cicleLengthSec;

    [SerializeField]//�J�n���̎��ԁi�����琔�������ԁj
    private float startTime = 30;
    //�o�ߎ��ԃJ�E���g�p
    private float timeCount = 0;

    [SerializeField]//���Ԃ̌o�߂��~�߂�
    private bool isStopPassageTime = false;

    //���ԃt���O
    private bool isDaylight;

    //�f�o�b�O�p���Ԑ؂�ւ�
    [SerializeField]
    private bool onDebugSunset = false;
    [SerializeField]
    private bool onDebugSunrise = false;

    private void Awake()
    {
        cicleLengthSec = daylightLengthSec + nightLengthSec;
    }

    // Update is called once per frame
    void Update()
    {
        //�f�o�b�O�p
        {
            if (onDebugSunrise)
            {
                ChangeSunrise();
                onDebugSunrise = false;
            }
            if (onDebugSunset)
            {
                ChangeSunset();
                onDebugSunset = false;
            }
        }

        if (isStopPassageTime) return;      

        //���Ԍv��
        timeCount += Time.deltaTime;

        //���l�𒴂��Ȃ��悤��
        if (timeCount >= cicleLengthSec)
        {
            timeCount -= cicleLengthSec;
        }

        //���z���̊p�x
        float sunRotate;
        //����
        if (timeCount < daylightLengthSec)
        {
            //���̌o�ߎ��Ԃ̊���
            float daylightTimePer = timeCount / daylightLengthSec;

            sunRotate = 180.0f * daylightTimePer;
            //�����ԕۑ�
            isDaylight = true;
        }
        //���
        else
        {
            //��̌o�ߎ��Ԃ̊���
            float nightTimePer = (timeCount - daylightLengthSec) / nightLengthSec;

            sunRotate = 180.0f * (1.0f - nightTimePer) + 360.0f * nightTimePer;
            //�����ԕۑ�
            isDaylight = false;
        }

        Debug.Log(timeCount);

        //���z���̊p�x�ύX
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(sunRotate, Vector3.right);
    }

    //���Ԃ���v�ɕύX
    public void ChangeSunset()
    {
        isDaylight = false;
        timeCount = daylightLengthSec;
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(180, Vector3.right);
    }
    //���Ԃ���̏o�ɕύX
    public void ChangeSunrise()
    {
        isDaylight = true;
        timeCount = 0;
        directionalLightObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);

    }

    /// <summary>
    /// ���Ԃ��ǂ������擾���� (��:true, ��:false)
    /// </summary>
    public bool GetIsDaylight()
    {
        return isDaylight;
    }
}

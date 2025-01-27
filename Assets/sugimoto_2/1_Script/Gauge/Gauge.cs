using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    //�Q�[�W
    [SerializeField] GameObject gauge_obj;
    [SerializeField] GameObject back_gauge_obj;

    //���l
    float gauge_num_max;//�ő吔�l
    float gauge_num_now;//���̐��l

    //1������
    float gauge_one_memory;

    //�����I�Ɍ��炷�p
    float food_reduce_timer = 0;
    float damage_reduce_timer = 0;


    //���ʊ֐�
    public float GaugeSetting(float _max)//�Q�[�W�̐ݒ�
    {
        //�ő吔�l�ݒ�
        gauge_num_max = _max;
        //���̐��l�ݒ�
        gauge_num_now = gauge_num_max;
        //�P�������������ݒ�
        gauge_one_memory = gauge_obj.GetComponent<RectTransform>().sizeDelta.x / gauge_num_max;

        return gauge_num_now;
    }

    public float Increase_Gauge(float _increase_value)    //�Q�[�W�𑝂₷����
    {
        //�Q�[�W�̑��₷�ʂ�ݒ�
        float _increase_gauge = gauge_one_memory * _increase_value;

        //���݂̃Q�[�W�̃T�C�Y�f�[�^�擾
        Vector2 _now_gauge_size = gauge_obj.GetComponent<RectTransform>().sizeDelta;

        //���݂̃Q�[�W�̕��T�C�Y����Q�[�W�̌��炷�ʂ�����
        _now_gauge_size.x += _increase_gauge;

        //�v�Z�����Q�[�W�̃T�C�Y�ɐݒ�
        gauge_obj.GetComponent<RectTransform>().sizeDelta = _now_gauge_size;

        //gauge���ő�l�𒴂����珉����
        if (gauge_obj.GetComponent<RectTransform>().sizeDelta.x > gauge_num_max * gauge_one_memory) 
        {
            _now_gauge_size.x = gauge_num_max * gauge_one_memory;
            gauge_num_now = gauge_num_max;
            gauge_obj.GetComponent<RectTransform>().sizeDelta = _now_gauge_size;
        }

        //���̐��l��ݒ�
        gauge_num_now += _increase_value;

        //���̃Q�[�W�̐��l��Ԃ�
        return gauge_num_now;
    }

    public float ReduceGauge(float _reduce_value)     //�Q�[�W�����炷����
    {
        //�Q�[�W���O���傫����Ύ��s
        if (gauge_obj.GetComponent<RectTransform>().sizeDelta.x > 0)
        {
            //�Q�[�W�̌��炷�ʂ�ݒ�
            float _reduce_gauge = gauge_one_memory * _reduce_value;

            //���݂̃Q�[�W�̃T�C�Y�f�[�^�擾
            Vector2 _now_gauge_size = gauge_obj.GetComponent<RectTransform>().sizeDelta;

            //���݂̃Q�[�W�̕��T�C�Y����Q�[�W�̌��炷�ʂ�����
            _now_gauge_size.x -= _reduce_gauge;

            //�v�Z�����Q�[�W�̃T�C�Y�ɐݒ�
            gauge_obj.GetComponent<RectTransform>().sizeDelta = _now_gauge_size;

            //���̐��l��ݒ�
            gauge_num_now -= _reduce_value;
        }
        if (gauge_obj.GetComponent<RectTransform>().sizeDelta.x <= 0)
        //�O�𒴂����ꍇ
        {
            //�����𒲐�
            gauge_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(0, gauge_obj.GetComponent<RectTransform>().sizeDelta.y);
            gauge_num_now = 0;
        }

        //���̃Q�[�W�̐��l��Ԃ�
        return gauge_num_now;
    }


    //�H���Q�[�W�p
    public float DurationReduce(float _timer,float _reduce_value)    //�����I�ɃQ�[�W�����炷
    {
        //�^�C�}�[����
        food_reduce_timer += Time.deltaTime;

        //���Ԃ�������Q�[�W�����炷
        if (food_reduce_timer >= _timer) 
        {
            food_reduce_timer = 0.0f;
            return ReduceGauge(_reduce_value);
        }

        return gauge_num_now;
    }

    //�����_���[�W
    public float DurationDamage(float _timer, float _reduce_value, GameObject _chack_gage_obj,GameObject _reduce_gage_obj)   //�����_���[�W
    {
        if (_chack_gage_obj.GetComponent<Gauge>().gauge_num_now <= 0) 
        {
            //�^�C�}�[����
            damage_reduce_timer += Time.deltaTime;

            //���Ԃ�������Q�[�W�����炷
            if (damage_reduce_timer >= _timer)
            {
                damage_reduce_timer = 0.0f;
                return _reduce_gage_obj.GetComponent<Gauge>().ReduceGauge(_reduce_value);            
            }
        }
        else
        {
            damage_reduce_timer = 0.0f;
        }

        return gauge_num_now;
    }

    /// <summary>
    /// ���݂̐��l�擾
    /// </summary>
    public float GetCurrentAmount()
    {
        return gauge_num_now;
    }
}

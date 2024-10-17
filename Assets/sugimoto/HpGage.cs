using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpGage : MonoBehaviour
{
    // �̗̓Q�[�W�i�\�ʂ̏�Ɍ����镔���j
    [SerializeField]  GameObject gauge;
    // �P�\�Q�[�W�i�̗͂��������Ƃ���u�����镔���j
    [SerializeField]  GameObject graceGauge;

    //�ő�̗�
    float hp;

    // HP1������̕�
    float hp_memory;
    // �̗̓Q�[�W���������㗠�Q�[�W������܂ł̑ҋ@����
    float waitingTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponent<player>().hp;
        Debug.Log(hp);
        // �X�v���C�g�̕����ő�HP�Ŋ�����HP1������̕����h_HP1�h�ɓ���Ă���
        hp_memory = gauge.GetComponent<RectTransform>().sizeDelta.x / hp;
    }

    public void HpDamageGage(float _damege)
    {
        float damage = hp_memory * _damege;

        // �̗̓Q�[�W�̕��ƍ�����Vector2�Ŏ��o��(Width,Height)
        Vector2 nowsafes = gauge.GetComponent<RectTransform>().sizeDelta;
        // �̗̓Q�[�W�̕�����_���[�W���̕�������
        nowsafes.x -= damage;
        // �̗̓Q�[�W�Ɍv�Z�ς݂�Vector2��ݒ肷��
        gauge.GetComponent<RectTransform>().sizeDelta = nowsafes;
    }
}

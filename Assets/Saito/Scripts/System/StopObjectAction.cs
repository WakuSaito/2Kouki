using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���݂̒�~��
//�C���^�t�F�[�X���������A�~�߂����X�N���v�g�Ŏ�������
//singlton�N���X���������A�~�߂����X�N���v�g���ŏ�Ԃ��Ď����� ���̂Ƃ��낱��n���悳�����@����singleton����Ȃ��Ă�������
//��~���������N���X���𗅗񂵁Aenabled=false�ɂ���i����̌`���ƃo�O���o�₷���j

public class StopObjectAction : MonoBehaviour
{
    public bool IsStopAction { get; set; } = false;
}

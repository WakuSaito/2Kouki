using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����C���^�[�t�F�[�X
/// ����i��Ɏ��Ă���́j�����������A���܂����Ƃ��̏��������p
/// </summary>
public interface IWeapon
{
    /// <summary>
    /// �d����
    /// ���킪�L�������ꂽ�Ƃ�
    /// </summary>
    public void PutAway();

    /// <summary>
    /// ���o��
    /// ���킪���[�����Ƃ�
    /// </summary>
    public void PutOut();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>����C���^�[�t�F�[�X</para>
/// ����i��Ɏ��Ă���́j�����������A���܂����Ƃ��̏��������p
/// </summary>
public interface IWeapon
{
    /// <summary>
    /// <para>�d����</para>
    /// ���킪�L�������ꂽ�Ƃ�
    /// </summary>
    public void PutAway();

    /// <summary>
    /// <para>���o��</para>
    /// ���킪���[�����Ƃ�
    /// </summary>
    public void PutOut();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// <para>��~�I�u�W�F�N�g�C���^�[�t�F�[�X</para>
/// �|�[�Y���ɒ�~����I�u�W�F�N�g���p������C���^�[�t�F�[�X
/// </summary>
public interface IStopObject
{
    /// <summary>
    /// <para>�ꎞ��~</para>
    /// �|�[�Y�J�n���ɌĂяo�����
    /// </summary>
    public void Pause();

    /// <summary>
    /// <para>�ĊJ</para>
    /// �|�[�Y�������ɌĂяo���ꃋ
    /// </summary>
    public void Resume();
}

/// <summary>
/// <para>�I�u�W�F�N�g��~���s�N���X</para>
/// �C���x���g�����Ď����AIStopObject���p������I�u�W�F�N�g�̈ꎞ��~�A�ĊJ���s��
/// </summary>
public class StopObjectAction : MonoBehaviour
{
    [SerializeField]//�C���x���g��
    private Inventory m_inventory;

    //���݂̒�~���
    private bool m_currentStopState = false;


    //�C���x���g���̊J���Ď��A�I�u�W�F�N�g�̒�~��Ԃ�ύX����
    private void Update()
    {
        bool new_stop_bool = m_currentStopState;

        if(m_inventory != null)
        {
            //�C���x���g�����J���Ă���Ƃ�
            if(m_inventory.item_inventory_flag == true)
            {
                new_stop_bool = true;
            }
            else
            {
                new_stop_bool = false;
            }
        }
        //�f�o�b�O�p
        else if (Input.GetKeyDown(KeyCode.P))
        {
            new_stop_bool = !m_currentStopState;
        }

        //�ς��Ȃ���ΏI��
        if (new_stop_bool == m_currentStopState) return;

        //IStopObject��S�擾
        var stop_inter_faces = InterfaceUtils.FindObjectOfInterfaces<IStopObject>();

        //�SIStopObject�̒�~��ԕύX����
        foreach(var stopI in stop_inter_faces)
        {
            if (stopI == null) continue;

            if (new_stop_bool)
                stopI.Pause();//�ꎞ��~
            else
                stopI.Resume();//�ĊJ
        }
        Debug.Log("��~���:" + new_stop_bool);
        m_currentStopState = new_stop_bool;
    }    
}

/// <summary>
/// Interface�p�⏕�N���X
/// </summary>
public class InterfaceUtils
{
    /// <summary>
    /// <para>����̃C���^�t�F�[�X���A�^�b�`���ꂽ�I�u�W�F�N�g��������</para>
    /// </summary>
    /// <typeparam name="T"> �T������C���^�t�F�[�X </typeparam>
    /// <returns> �擾�����N���X�z�� </returns>
    public static T[] FindObjectOfInterfaces<T>() where T : class
    {
        List<T> find_list = new List<T>();//�T�������C���^�[�t�F�[�X�i�[�p

        // �I�u�W�F�N�g��T�����A���X�g�Ɋi�[
        foreach (var component in GameObject.FindObjectsOfType<Component>())
        {
            var obj = component as T;

            if (obj == null) continue;

            find_list.Add(obj);
        }

        //���X�g��z��ɃR�s�[����
        T[] find_obj_array = new T[find_list.Count];//�߂�l�p�z��
        int count = 0;
        // �擾�����I�u�W�F�N�g���w�肵���C���^�t�F�[�X�^�z��Ɋi�[
        foreach (T obj in find_list)
        {
            find_obj_array[count] = obj;
            count++;
        }

        return find_obj_array;
    }
}
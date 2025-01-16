using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���݂̒�~��
//�C���^�t�F�[�X���������A�~�߂����X�N���v�g�Ŏ�������
//singlton�N���X���������A�~�߂����X�N���v�g���ŏ�Ԃ��Ď����� ���̂Ƃ��낱��n���悳�����@����singleton����Ȃ��Ă�������
//��~���������N���X���𗅗񂵁Aenabled=false�ɂ���i����̌`���ƃo�O���o�₷���j

public interface IStopObject
{
    /// <summary>
    /// �ꎞ��~
    /// </summary>
    public void Pause();

    /// <summary>
    /// �ĊJ
    /// </summary>
    public void Resume();
}


public class StopObjectAction : MonoBehaviour
{
    [SerializeField]//�C���x���g��
    private Inventory m_inventory;

    //���݂̒�~���
    private bool m_currentStopState = false;

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
/// Interface�֗��N���X
/// </summary>
public class InterfaceUtils
{
    /// <summary>
    /// ����̃C���^�t�F�[�X���A�^�b�`���ꂽ�I�u�W�F�N�g��������
    /// </summary>
    /// <typeparam name="T"> �T������C���^�t�F�[�X </typeparam>
    /// <returns> �擾�����N���X�z�� </returns>
    public static T[] FindObjectOfInterfaces<T>() where T : class
    {
        List<T> find_list = new List<T>();

        // �I�u�W�F�N�g��T�����A���X�g�Ɋi�[
        foreach (var component in GameObject.FindObjectsOfType<Component>())
        {
            var obj = component as T;

            if (obj == null) continue;

            find_list.Add(obj);
        }

        T[] find_obj_array = new T[find_list.Count];
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
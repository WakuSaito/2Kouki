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
    private Inventory inventory;

    //���݂̒�~���
    private bool currentStopState = false;

    private void Update()
    {
        //if (inventory == null) return;

        ////�ύX��̏�Ԍ���
        //bool onStop = inventory.item_inventory_flag;
        if (!Input.GetKeyDown(KeyCode.P)) return;
        bool onStop = !currentStopState;
        //�ς��Ȃ���ΏI��
        if (onStop == currentStopState) return;

        //IStopObject��S�擾
        var stopInterfaces = InterfaceUtils.FindObjectOfInterfaces<IStopObject>();

        //�SIStopObject�̒�~��ԕύX����
        foreach(var stopI in stopInterfaces)
        {
            if (stopI == null) continue;

            if (onStop)
                stopI.Pause();//�ꎞ��~
            else
                stopI.Resume();//�ĊJ
        }
        Debug.Log("��~���:" + onStop);
        currentStopState = onStop;
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
        List<T> findList = new List<T>();

        // �I�u�W�F�N�g��T�����A���X�g�Ɋi�[
        foreach (var component in GameObject.FindObjectsOfType<Component>())
        {
            var obj = component as T;

            if (obj == null) continue;

            findList.Add(obj);
        }

        T[] findObjArray = new T[findList.Count];
        int count = 0;

        // �擾�����I�u�W�F�N�g���w�肵���C���^�t�F�[�X�^�z��Ɋi�[
        foreach (T obj in findList)
        {
            findObjArray[count] = obj;
            count++;
        }
        return findObjArray;
    }
}
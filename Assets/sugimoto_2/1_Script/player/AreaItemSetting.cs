using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaItemSetting : MonoBehaviour
{   
    //�A�C�e���I�u�W�F�N�g��ۑ�
    List<GameObject> m_itemObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(GetItemObj), 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSphereAreaItems(transform.position, 15.0f);
    }

    public void GetItemObj()
    {
        //�A�C�e���^�O�����I�u�W�F�N�g�擾
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("item");

        //���W�b�g�{�f�B�[�������Ă���I�u�W�F�N�g�̂ݕۑ�
        foreach (var obj in tmp)
        {
            if (!obj.GetComponent<Rigidbody>()) continue;
            if (obj.GetComponent<ItemSetting>().m_tutorialFlag) continue;

            m_itemObj.Add(obj);
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// �G���A���ɓ����Ă���I�u�W�F�N�g�𒲂ׂ�
    /// </summary>
    /// <param name="_center">���ׂ钆�S</param>
    /// <param name="_radius">���ׂ锼�a</param>
    void CheckSphereAreaItems(Vector3 _center, float _radius)
    {
        //���ׂẴA�C�e���̃I�u�W�F�N�g�𒲂ׂ�
        foreach (var obj in m_itemObj)
        {
            if (obj == null) continue;

            //�͈͓��̃I�u�W�F�N�g�͕\���A�͈͊O�͔�\���ɂ���
            if (Vector3.Distance(_center, obj.transform.position) <= _radius)
            {
                if (!obj.GetComponent<ItemSetting>().m_getFlag)
                {
                    obj.SetActive(true);
                }
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// �͈͊O�̃X�|�i�[���擾
    /// </summary>
    /// <returns>�͈͊O�̃X�|�i�[���X�g</returns>
    public List<Transform> OutSideItemSpawner()
    {
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("ItemSpawnPos");
        List<Transform> spawner = new List<Transform>();
        Debug.Log(tmp.Length);

        foreach (var obj in tmp)
        {
            if (obj == null) continue;

            //�͈͊O�̃X�|�i�[���擾
            if (Vector3.Distance(transform.position, obj.transform.position) > 15.0f)
            {
                spawner.Add(obj.transform);
            }
        }

        return spawner;
    }

    /// <summary>
    /// �͈͊O�̃A�C�e���S�폜
    /// </summary>
    public void DeleteOutsideAreaItems()
    {
        //���ׂẴA�C�e���̃I�u�W�F�N�g�𒲂ׂ�
        foreach (var obj in m_itemObj)
        {
            if (obj == null) continue;

            //�͈͊O�I�u�W�F�N�g�폜
            if (Vector3.Distance(transform.position, obj.transform.position) > 15.0f)
            {
                Destroy(obj.gameObject);
            }
        }

        m_itemObj.Clear();
    }
}

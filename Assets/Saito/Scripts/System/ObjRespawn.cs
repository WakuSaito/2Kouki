using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRespawn : MonoBehaviour
{
    //�X�|�i�[�I�u�W�F�N�g
    GameObject[] m_spawners;

    player m_playerScript;

    private void Awake()
    {
        //�A�N�e�B�u��Ԃ̕ύX��Ɏ擾����ƌ�����Ȃ��̂�
        m_spawners = GameObject.FindGameObjectsWithTag("Spawner");

        //�v���C���[�X�N���v�g�擾
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {
        //�f�o�b�O�p
        if (Input.GetKey(KeyCode.B)&&
            Input.GetKeyDown(KeyCode.R))
        {
            
            RestPlayer();
        }
    }

    public void Respawn()
    {
        //�S�X�|�i�[�L����
        foreach(var obj in m_spawners)
        {
            obj.SetActive(true);
        }

        //�S�A�C�e���ݒu�X�N���v�g�Ăяo��
        GameObject[] item_setter = GameObject.FindGameObjectsWithTag("ItemSetter");
        foreach(var obj in item_setter)
        {
            obj.GetComponent<SetItem>().SetItemPos();
        }
    }

    //�v���C���[���x��������
    public void RestPlayer()
    {
        Respawn();

        Debug.Log("�x��");

        if (m_playerScript == null) return;

        //�x��
        m_playerScript.TakeRest(0.7f, 0.3f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRespawn : MonoBehaviour
{
    //�X�|�i�[�I�u�W�F�N�g
    GameObject[] spawners;

    player playerScript;

    private void Awake()
    {
        //�A�N�e�B�u��Ԃ̕ύX��Ɏ擾����ƌ�����Ȃ��̂�
        spawners = GameObject.FindGameObjectsWithTag("Spawner");

        //�v���C���[�X�N���v�g�擾
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {
        //�f�o�b�O�p
        if (Input.GetKey(KeyCode.B)&&
            Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
            RestPlayer();
        }
    }

    public void Respawn()
    {
        //�S�X�|�i�[�L����
        foreach(var obj in spawners)
        {
            obj.SetActive(true);
        }

        //�S�A�C�e���ݒu�X�N���v�g�Ăяo��
        GameObject[] itemSetter = GameObject.FindGameObjectsWithTag("ItemSetter");
        foreach(var obj in itemSetter)
        {
            obj.GetComponent<SetItem>().SetItemPos();
        }
    }

    //�v���C���[���x��������
    public void RestPlayer()
    {
        if (playerScript == null) return;

        //�x��
        playerScript.TakeRest(0.7f, 0.3f);
    }
}

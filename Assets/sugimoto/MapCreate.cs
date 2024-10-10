using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    //�P�^�C���̑傫��
    const int MapTipSize = 100;

    //�}�b�v�S�̂̃`�b�v��
    const int MAP_X = 9;
    const int MAP_Y = 9;
    //��ʂ̃}�b�v�`�b�v��
    const int MAPTIP_X = 5;
    const int MAPTIP_Y = 5;
    //�`��J�n�ʒu
    const int STARAT_X = 2;
    const int STARAT_Y = 2;

    player Player;

    int move_disX = 0;//�ړ���
    int move_disY = 0;//�ړ���

    int Array_X = 0;//�z��̈ʒu
    int Array_Y = 0;//�z��̈ʒu

    //�ړ�����L�����N�^
    [SerializeField] GameObject player_obj;

    //�}�b�v�`�b�v�I�u�W�F�N�g�z��
    [SerializeField] GameObject[] map_tips;

    int create_tip_index;

    [SerializeField] int start_index;

    //��x�ɐ�������`�b�v��
    [SerializeField] int create_tip_num;

    [SerializeField]
    int[,] map = new int[MAP_Y, MAP_X]
    {
        {1,0,1,0,1,1,1,0,1},
        {1,0,1,0,1,1,1,0,1},
        {1,0,1,0,1,1,1,0,1},
        {1,0,1,0,1,1,1,0,1},
        {1,0,1,0,1,1,1,0,1},
        {1,0,1,0,1,1,1,0,1},
        {1,0,1,0,1,1,1,0,1},
        {1,0,1,0,1,1,1,0,1},
        {1,0,1,0,1,1,1,0,1},
    };

    ////int�^��ϐ�StageTipSize�Ő錾���܂��B�����̐��l�͎��������������I�u�W�F�N�g�̒[����[�܂ł̍��W�̑傫��
    //const int StageTipSize = 100;
    ////int�^��ϐ�currentTipIndex�Ő錾���܂��B
    //int currentTipIndex;
    ////�^�[�Q�b�g�L�����N�^�[�̎w�肪�o����l�ɂ����
    //public Transform character;
    ////�X�e�[�W�`�b�v�̔z��
    //public GameObject[] stageTips;
    ////�����������鎞�Ɏg���ϐ�startTipIndex
    //public int startTipIndex;
    ////�X�e�[�W�����̐�ǂ݌�
    //public int preInstantiate;
    ////������X�e�[�W�`�b�v�̕ێ����X�g
    //public List<GameObject> generatedStageList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Player = player_obj.GetComponent<player>();

        //�����}�b�v����
        for (int y = 0; y < MAPTIP_Y; y++)
        {
            for (int x = 0; x < MAPTIP_X; x++)
            {
                int MapChip_num = map[y + STARAT_Y, x + STARAT_X];
                Instantiate(map_tips[MapChip_num], new Vector3((x - STARAT_X) * MapTipSize, 0.0f, (y - STARAT_Y) * MapTipSize), Quaternion.Euler(90.0f, 0.0f, 0.0f)) ;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ////�v���C���[�̈ʒu���猻�݂̃X�e�[�W�`�b�v�̃C���f�b�N�X���v�Z
        //int player_index_X = ((int)player_obj.transform.position.x / MapTipSize);
        //int player_index_Y = ((int)player_obj.transform.position.y / MapTipSize);

        ////��
        //if ((int)Player.moving_distance_X < MapTipSize)
        //{

        //}

        ////���̃X�e�[�W�`�b�v�ɓ�������X�e�[�W�̍X�V�������s���܂��B
        //if (charaPositionIndex + preInstantiate > currentTipIndex)
        //{
        //    UpdateStage(charaPositionIndex + (PLAYER_STARAT_X));
        //}
    }

    //�w��̃C���f�b�N�X�܂ł̃X�e�[�W�`�b�v�𐶐����āA�Ǘ����ɂ���
    //void UpdateStage(int toTipIndex)
    //{
    //    if (toTipIndex <= currentTipIndex) return;
    //    //�w��̃X�e�[�W�`�b�v�܂Ő��������
    //    for (int i = currentTipIndex + 1; i <= toTipIndex; i++)
    //    {
    //        GameObject stageObject = GenerateStage(i);
    //        generatedStageList.Add(stageObject);
    //    }
    //    while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();
    //    currentTipIndex = toTipIndex;

    //}

    ////�w��̃C���f�b�N�X�ʒu��stage�I�u�W�F�N�g�������_���ɐ���
    //GameObject GenerateStage(int tipIndex)
    //{
    //    int nextStageTip = Random.Range(0, stageTips.Length);

    //    GameObject stageObject = (GameObject)Instantiate(
    //        stageTips[nextStageTip],
    //        new Vector3(tipIndex * StageTipSize, 0, 0), //�����x�������ɖ�����������̂ł��̏����������Ă���
    //        Quaternion.Euler(90.0f,0.0f,0.0f)) as GameObject;
    //    return stageObject;
    //}

    ////��ԌÂ��X�e�[�W���폜���܂�
    //void DestroyOldestStage()
    //{
    //    GameObject oldStage = generatedStageList[0];
    //    generatedStageList.RemoveAt(0);
    //    Destroy(oldStage);
    //}
}

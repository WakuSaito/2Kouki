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

    //�}�b�v�̒��S
    const int MAP_CENTER_X = MAP_X / 2;
    const int MAP_CENTER_Y = MAP_Y / 2;

    //��ʂ̃}�b�v�`�b�v��
    const int MAPTIP_X = 5;
    const int MAPTIP_Y = 5;
    //�`��J�n�ʒu
    const int STARAT_X = 2;
    const int STARAT_Y = 2;

    //�`�拗��
    const int DRAWING_DISTANCE = 2;

    player Player;
    Vector3 player_pos;

    //�v���C���[�̏����ʒu�i�z��j
    int player_array_x = MAP_CENTER_X;
    int player_array_y = MAP_CENTER_Y;

    int move_disX = 0;//�ړ���
    int move_disY = 0;//�ړ���

    public int Array_X = MAP_CENTER_X;//�z��̈ʒu
    public int Array_Y = MAP_CENTER_Y;//�z��̈ʒu

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
        {0,1,2,3,4,5,6,7,8},
        {0,1,2,3,4,5,6,7,8},
        {0,1,2,3,4,5,6,7,8},
        {0,1,2,3,4,5,6,7,8},
        {0,1,2,3,4,5,6,7,8},
        {0,1,2,3,4,5,6,7,8},
        {0,1,2,3,4,5,6,7,8},
        {0,1,2,3,4,5,6,7,8},
        {0,1,2,3,4,5,6,7,8},
    };

    [SerializeField] GameObject[,] map_obj = new GameObject[MAP_Y, MAP_X];

    int min_x = STARAT_X;
    int max_x = STARAT_X;

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
        player_pos = player_obj.transform.position;

        //�����}�b�v����
        for (int y = 0; y < MAPTIP_Y; y++)
        {
            for (int x = 0; x < MAPTIP_X; x++)
            {
                int MapChip_num = map[y + STARAT_Y, x + STARAT_X];
                map_obj[y + STARAT_Y, x + STARAT_X] = Instantiate(map_tips[MapChip_num], new Vector3((x - STARAT_X) * MapTipSize, 0.0f, (y - STARAT_Y) * MapTipSize), Quaternion.Euler(90.0f, 0.0f, 0.0f)) ;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�̎��͂Q�}�b�v�͕\��������

        //Y��Z���������Ă邯��Z�ł�(2D���Q�l�ɂ��Ȃ��������̂ō�����܂�����łȂ����܁B�B�B)

        //x����mapTipSize���ړ�������V���ȃ}�b�v�𐶐�
        if (Mathf.Abs((int)Player.moving_distance_X) == MapTipSize)
        {

            //�v���C���[�̈ʒu����`��ʒu�����߂�
            int draw_X = 0, draw_Y = 0, draw_start_X = 0, draw_start_Y = 0;

            //���S���^�C���̐^�񒆂Ȃ̂Ń}�b�v�`�b�v�T�C�Y�̔����̐����������Ē���
            //x������
            if (player_obj.transform.position.x > 0)
            {
                Player.moving_distance_X -= MapTipSize;//�������i0�ŏ���������Ə����_�ȉ���������Ă���̂Łj
                draw_start_X = (((int)player_obj.transform.position.x + (MapTipSize / 2)) / MapTipSize) + STARAT_X;//�v���C���[������Ă���}�b�v�`�b�v�ʒu+�ŏ��̃}�b�v�`�b�v�\���ʒu
            }
            else if (player_obj.transform.position.x < 0)
            {
                Player.moving_distance_X += MapTipSize;
                draw_start_X = (((int)player_obj.transform.position.x - (MapTipSize / 2)) / MapTipSize) + STARAT_X;//�v���C���[������Ă���}�b�v�`�b�v�ʒu+�ŏ��̃}�b�v�`�b�v�\���ʒu
            }

            //z������
            if (player_obj.transform.position.z > 0)
            {
                draw_start_Y = (((int)player_obj.transform.position.z + (MapTipSize / 2)) / MapTipSize) - DRAWING_DISTANCE;
            }
            else if(player_obj.transform.position.z<0)
            {
                draw_start_Y = (((int)player_obj.transform.position.z - (MapTipSize / 2)) / MapTipSize) - DRAWING_DISTANCE;
            }

            //Debug.Log(draw_start_Y);

            //���݂̃v���C���[�̈ʒu���A1�t���[���O�̈ʒu���傫�����x���̕����֐i��ł���
            if (player_obj.transform.position.x > player_pos.x)
            {
                player_array_x++;//�v���C���[���P�}�X�i��           
                Array_X = player_array_x + DRAWING_DISTANCE; //�`�悵�����}�X�̔z��ԍ�
                Array_Y = draw_start_Y + MAP_CENTER_Y;

                //�}�b�v����
                for (int i = 0; i < MAPTIP_Y; Array_Y++, i++)
                {
                    draw_X = draw_start_X;
                    draw_Y = draw_start_Y + i;

                    //�\���͈͂𐧌�(�z��̐������\��)
                    if (Array_X < MAP_X && Array_X >= 0 && Array_Y < MAP_Y && Array_Y >= 0)
                    {
                        int MapChip_num = map[Array_Y, Array_X];

                        //���Ƃł��łɐ����ς݂̏ꍇ�̏�����������

                        map_obj[Array_Y, Array_X] = Instantiate(map_tips[MapChip_num], new Vector3(draw_X * MapTipSize, 0.0f, draw_Y * MapTipSize), Quaternion.Euler(90.0f, 0.0f, 0.0f));

                        //�`��͈͊O�̃}�b�v���\��
                        if (map_obj[Array_Y, Array_X - MAPTIP_X] != null)
                        {
                            map_obj[Array_Y, Array_X - MAPTIP_X].SetActive(false);
                        }

                    }

                }

                for(int y=0;y<MAP_Y;y++)
                {
                    for(int x=0;x<MAP_X;x++)
                    {
                        Debug.Log(map_obj[y, x] + "y=" + y + "x=" + x);
                    }
                }

            }
        }

        if (Mathf.Abs((int)Player.moving_distance_Z) == MapTipSize)
        {
            Player.moving_distance_Z -= MapTipSize;
        }
            ////���̃X�e�[�W�`�b�v�ɓ�������X�e�[�W�̍X�V�������s���܂��B
            //if (charaPositionIndex + preInstantiate > currentTipIndex)
            //{
            //    UpdateStage(charaPositionIndex + (PLAYER_STARAT_X));
            //}

            player_pos = player_obj.transform.position;
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

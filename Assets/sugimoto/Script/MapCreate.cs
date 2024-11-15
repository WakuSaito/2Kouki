using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    //�P�^�C���̑傫��
    public int MapTipSize = 50;

    //�}�b�v�S�̂̃`�b�v��
    const int MAP_X = 9;
    const int MAP_Y = 9;

    //�}�b�v�̒��S
    public int MAP_CENTER_X = MAP_X / 2;
    public int MAP_CENTER_Y = MAP_Y / 2;

    //�}�b�v
    [SerializeField]
    int[,] map = new int[MAP_Y, MAP_X]
    {
        {1,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,2,0,0,0},
        {0,0,0,0,0,0,2,0,0},
        {0,0,2,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,2,0},
        {0,0,0,0,0,0,0,0,0},
        {0,0,0,0,2,0,0,2,0},
        {0,2,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,2},
    };


    //�ړ�����L�����N�^
    [SerializeField] GameObject player_obj;

    //�}�b�v�`�b�v�I�u�W�F�N�g�z��
    [SerializeField] GameObject[] map_tips;

    //�������ꂽ�}�b�v�̃I�u�W�F�N�g
    [SerializeField] GameObject[,] map_obj = new GameObject[MAP_Y, MAP_X];

    //�}�b�v���܂Ƃ߂�e�I�u�W�F�N�g
    [SerializeField] GameObject map_parent;

    // Start is called before the first frame update
    void Start()
    {
        //�����}�b�v����
        for (int y = 0; y < MAP_Y; y++)
        {
            for (int x = 0; x < MAP_X; x++)
            {
                int MapChip_num = map[y, x];
                map_obj[y, x] = Instantiate(map_tips[MapChip_num], new Vector3((x - MAP_CENTER_X) * MapTipSize, 0.0f, (y - MAP_CENTER_Y) * MapTipSize), Quaternion.Euler(0.0f, 0.0f, 0.0f), map_parent.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

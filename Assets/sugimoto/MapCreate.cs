using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    //１タイルの大きさ
    public int MapTipSize = 100;

    //マップ全体のチップ数
    const int MAP_X = 9;
    const int MAP_Y = 9;

    //マップの中心
    public int MAP_CENTER_X = MAP_X / 2;
    public int MAP_CENTER_Y = MAP_Y / 2;

    //マップ
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


    //移動するキャラクタ
    [SerializeField] GameObject player_obj;

    //マップチップオブジェクト配列
    [SerializeField] GameObject[] map_tips;

    //生成されたマップのオブジェクト
    [SerializeField] GameObject[,] map_obj = new GameObject[MAP_Y, MAP_X];

    //マップをまとめる親オブジェクト
    [SerializeField] GameObject map_parent;

    // Start is called before the first frame update
    void Start()
    {
        //初期マップ生成
        for (int y = 0; y < MAP_Y; y++)
        {
            for (int x = 0; x < MAP_X; x++)
            {
                int MapChip_num = map[y, x];
                map_obj[y, x] = Instantiate(map_tips[MapChip_num], new Vector3((x - MAP_CENTER_X) * MapTipSize, 0.0f, (y - MAP_CENTER_Y) * MapTipSize), Quaternion.Euler(90.0f, 0.0f, 0.0f), map_parent.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

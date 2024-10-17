using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    //１タイルの大きさ
    const int MapTipSize = 100;

    //マップ全体のチップ数
    const int MAP_X = 9;
    const int MAP_Y = 9;

    //マップの中心
    const int MAP_CENTER_X = MAP_X / 2;
    const int MAP_CENTER_Y = MAP_Y / 2;

    //画面のマップチップ数
    const int MAPTIP_X = 5;
    const int MAPTIP_Y = 5;
    //描画開始位置
    const int STARAT_X = 2;
    const int STARAT_Y = 2;

    //描画距離
    const int DRAWING_DISTANCE = 2;

    player Player;
    Vector3 player_pos;

    //プレイヤーの初期位置（配列）
    int player_array_x = MAP_CENTER_X;
    int player_array_y = MAP_CENTER_Y;

    int move_disX = 0;//移動量
    int move_disY = 0;//移動量

    public int Array_X = MAP_CENTER_X;//配列の位置
    public int Array_Y = MAP_CENTER_Y;//配列の位置

    //移動するキャラクタ
    [SerializeField] GameObject player_obj;

    //マップチップオブジェクト配列
    [SerializeField] GameObject[] map_tips;

    int create_tip_index;

    [SerializeField] int start_index;

    //一度に生成するチップ数
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

    ////int型を変数StageTipSizeで宣言します。ここの数値は自動生成したいオブジェクトの端から端までの座標の大きさ
    //const int StageTipSize = 100;
    ////int型を変数currentTipIndexで宣言します。
    //int currentTipIndex;
    ////ターゲットキャラクターの指定が出来る様にするよ
    //public Transform character;
    ////ステージチップの配列
    //public GameObject[] stageTips;
    ////自動生成する時に使う変数startTipIndex
    //public int startTipIndex;
    ////ステージ生成の先読み個数
    //public int preInstantiate;
    ////作ったステージチップの保持リスト
    //public List<GameObject> generatedStageList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Player = player_obj.GetComponent<player>();
        player_pos = player_obj.transform.position;

        //初期マップ生成
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
        //プレイヤーの周囲２マップは表示させる

        //YとZが混合してるけどZです(2Dを参考にしながらやったので混ざりました後でなおしま。。。)

        //x軸がmapTipSize分移動したら新たなマップを生成
        if (Mathf.Abs((int)Player.moving_distance_X) == MapTipSize)
        {

            //プレイヤーの位置から描画位置を求める
            int draw_X = 0, draw_Y = 0, draw_start_X = 0, draw_start_Y = 0;

            //中心がタイルの真ん中なのでマップチップサイズの半分の数を加減して調整
            //x軸制御
            if (player_obj.transform.position.x > 0)
            {
                Player.moving_distance_X -= MapTipSize;//初期化（0で初期化すると小数点以下がくるってくるので）
                draw_start_X = (((int)player_obj.transform.position.x + (MapTipSize / 2)) / MapTipSize) + STARAT_X;//プレイヤーが乗っているマップチップ位置+最初のマップチップ表示位置
            }
            else if (player_obj.transform.position.x < 0)
            {
                Player.moving_distance_X += MapTipSize;
                draw_start_X = (((int)player_obj.transform.position.x - (MapTipSize / 2)) / MapTipSize) + STARAT_X;//プレイヤーが乗っているマップチップ位置+最初のマップチップ表示位置
            }

            //z軸制御
            if (player_obj.transform.position.z > 0)
            {
                draw_start_Y = (((int)player_obj.transform.position.z + (MapTipSize / 2)) / MapTipSize) - DRAWING_DISTANCE;
            }
            else if(player_obj.transform.position.z<0)
            {
                draw_start_Y = (((int)player_obj.transform.position.z - (MapTipSize / 2)) / MapTipSize) - DRAWING_DISTANCE;
            }

            //Debug.Log(draw_start_Y);

            //現在のプレイヤーの位置が、1フレーム前の位置より大きければx軸の方向へ進んでいる
            if (player_obj.transform.position.x > player_pos.x)
            {
                player_array_x++;//プレイヤーが１マス進む           
                Array_X = player_array_x + DRAWING_DISTANCE; //描画したいマスの配列番号
                Array_Y = draw_start_Y + MAP_CENTER_Y;

                //マップ生成
                for (int i = 0; i < MAPTIP_Y; Array_Y++, i++)
                {
                    draw_X = draw_start_X;
                    draw_Y = draw_start_Y + i;

                    //表示範囲を制御(配列の数だけ表示)
                    if (Array_X < MAP_X && Array_X >= 0 && Array_Y < MAP_Y && Array_Y >= 0)
                    {
                        int MapChip_num = map[Array_Y, Array_X];

                        //あとですでに生成済みの場合の処理をつけ足す

                        map_obj[Array_Y, Array_X] = Instantiate(map_tips[MapChip_num], new Vector3(draw_X * MapTipSize, 0.0f, draw_Y * MapTipSize), Quaternion.Euler(90.0f, 0.0f, 0.0f));

                        //描画範囲外のマップを非表示
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
            ////次のステージチップに入ったらステージの更新処理を行います。
            //if (charaPositionIndex + preInstantiate > currentTipIndex)
            //{
            //    UpdateStage(charaPositionIndex + (PLAYER_STARAT_X));
            //}

            player_pos = player_obj.transform.position;
    }

    //指定のインデックスまでのステージチップを生成して、管理下におく
    //void UpdateStage(int toTipIndex)
    //{
    //    if (toTipIndex <= currentTipIndex) return;
    //    //指定のステージチップまで生成するよ
    //    for (int i = currentTipIndex + 1; i <= toTipIndex; i++)
    //    {
    //        GameObject stageObject = GenerateStage(i);
    //        generatedStageList.Add(stageObject);
    //    }
    //    while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();
    //    currentTipIndex = toTipIndex;

    //}

    ////指定のインデックス位置にstageオブジェクトをランダムに生成
    //GameObject GenerateStage(int tipIndex)
    //{
    //    int nextStageTip = Random.Range(0, stageTips.Length);

    //    GameObject stageObject = (GameObject)Instantiate(
    //        stageTips[nextStageTip],
    //        new Vector3(tipIndex * StageTipSize, 0, 0), //今回はx軸方向に無限生成するのでこの書き方をしている
    //        Quaternion.Euler(90.0f,0.0f,0.0f)) as GameObject;
    //    return stageObject;
    //}

    ////一番古いステージを削除します
    //void DestroyOldestStage()
    //{
    //    GameObject oldStage = generatedStageList[0];
    //    generatedStageList.RemoveAt(0);
    //    Destroy(oldStage);
    //}
}

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
    //画面のマップチップ数
    const int MAPTIP_X = 5;
    const int MAPTIP_Y = 5;
    //描画開始位置
    const int STARAT_X = 2;
    const int STARAT_Y = 2;

    player Player;

    int move_disX = 0;//移動量
    int move_disY = 0;//移動量

    int Array_X = 0;//配列の位置
    int Array_Y = 0;//配列の位置

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

        //初期マップ生成
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
        ////プレイヤーの位置から現在のステージチップのインデックスを計算
        //int player_index_X = ((int)player_obj.transform.position.x / MapTipSize);
        //int player_index_Y = ((int)player_obj.transform.position.y / MapTipSize);

        ////左
        //if ((int)Player.moving_distance_X < MapTipSize)
        //{

        //}

        ////次のステージチップに入ったらステージの更新処理を行います。
        //if (charaPositionIndex + preInstantiate > currentTipIndex)
        //{
        //    UpdateStage(charaPositionIndex + (PLAYER_STARAT_X));
        //}
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int colums = 10, rows = 10;
    private List<Vector3> gridPosition = new List<Vector3>();
    private bool[,] wallExists;

    public GameObject floorTiles;
    public GameObject wallTiles;
    public GameObject enemyTiles;
    public GameObject playerTiles;

    public GameObject Stairs;

    //各タイルの下限と上限数
    public int wallMinimum = 5, wallMaximum = 9, enemyMinimum = 0, enemyMaximum = 0;

    private void Awake()
    {
        wallExists = new bool[colums, rows];
    }
    void Start()
    {
        SetupStage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //外枠と床（土台）の生成
    void BoardSetup()
    {
        for (int x = 0; x < colums; ++x) 
        {
            for (int y = 0; y < rows; ++y) 
            {
                GameObject toInstantiate;

                toInstantiate = floorTiles;

                Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    //タイルを置けるスペースの情報確保
    void initialiseList()
    {
        gridPosition.Clear();

        for (int x = 1; x < colums - 1; ++x) 
        {
            for (int y = 1; y < rows - 1; ++y) 
            {
                gridPosition.Add(new Vector3(x, y, 0));
            }
        }
    }

    //ポジションのランダム値を返す
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPosition.Count);

        Vector3 randomPosition = gridPosition[randomIndex];

        gridPosition.RemoveAt(randomIndex);

        return randomPosition;
    }

    //引き値で受け取ったタイルを、ランダムで配置する
    void LayoutobjectRandom(GameObject tilesArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; ++i)
        {
            Vector3 randomPosition = RandomPosition();

            GameObject tileChoice = tilesArray;

            Instantiate(tileChoice, randomPosition, Quaternion.identity);

            if (tileChoice == wallTiles)
            {
                wallExists[(int)randomPosition.y, (int)randomPosition.x] = true;
            }
        }
    }

    //プレイヤーをセットする
    void PlayerSetting()
    {
        Instantiate(playerTiles, new Vector3(0, 0, 0), Quaternion.identity);
    }

    //最初のもろもろのセッティング
    public void SetupStage()
    {
        BoardSetup();
        initialiseList();
        LayoutobjectRandom(wallTiles, wallMinimum, wallMaximum);
        LayoutobjectRandom(enemyTiles, enemyMinimum, enemyMaximum);
        PlayerSetting();

        Instantiate(Stairs, new Vector3(colums - 1, rows - 1, 0), Quaternion.identity);
    }

    //プレイヤーが進めるか
    public bool PlayerCheckPosition(PlayerMove player)
    {
        Vector2 pos = Rounding.Round(player.gameObject.transform.position);

        if (!StageOutCheck((int)pos.x, (int)pos.y))
        {
            return false;
        }

        if (WallExists((int)pos.x, (int)pos.y)) 
        {
            return false;
        }

        return true;
    }

    //進んだ先に壁があるか
    bool WallExists(int x, int y)
    {
        return wallExists[y, x];
    }

    //枠からはみ出ていないか
    bool StageOutCheck(int x, int y)
    {
        return (x >= 0 && x < colums && y >= 0 && y < rows);
    }

    
}

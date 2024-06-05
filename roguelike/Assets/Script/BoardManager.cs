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

    //�e�^�C���̉����Ə����
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

    //�O�g�Ə��i�y��j�̐���
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

    //�^�C����u����X�y�[�X�̏��m��
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

    //�|�W�V�����̃����_���l��Ԃ�
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPosition.Count);

        Vector3 randomPosition = gridPosition[randomIndex];

        gridPosition.RemoveAt(randomIndex);

        return randomPosition;
    }

    //�����l�Ŏ󂯎�����^�C�����A�����_���Ŕz�u����
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

    //�v���C���[���Z�b�g����
    void PlayerSetting()
    {
        Instantiate(playerTiles, new Vector3(0, 0, 0), Quaternion.identity);
    }

    //�ŏ��̂������̃Z�b�e�B���O
    public void SetupStage()
    {
        BoardSetup();
        initialiseList();
        LayoutobjectRandom(wallTiles, wallMinimum, wallMaximum);
        LayoutobjectRandom(enemyTiles, enemyMinimum, enemyMaximum);
        PlayerSetting();

        Instantiate(Stairs, new Vector3(colums - 1, rows - 1, 0), Quaternion.identity);
    }

    //�v���C���[���i�߂邩
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

    //�i�񂾐�ɕǂ����邩
    bool WallExists(int x, int y)
    {
        return wallExists[y, x];
    }

    //�g����͂ݏo�Ă��Ȃ���
    bool StageOutCheck(int x, int y)
    {
        return (x >= 0 && x < colums && y >= 0 && y < rows);
    }

    
}

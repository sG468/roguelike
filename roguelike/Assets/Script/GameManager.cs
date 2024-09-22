using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject InGameUI;
    public GameObject gameOverUI;
    public GameObject MenuUI;

    //ターン変数
    public bool playerTurn;

    public Slider playerHp;
    public Slider EnemyHp;

    private Transform stairPosition;

    //クラス変数
    PlayerMove playerMove;
    EnemyMove enemyMove;
    BoardManager boardManager;

    bool playerExist = false;
    bool enemyExist = false;
    bool IsGameOver = false;
    bool IsMenu = false;

    //入力受付タイマー
    float nextKeyUpDownTime, nextKeyLeftRightTime;

    //入力インターバル(カチャカチャ押されても一定間隔処理を置くための変数)
    [SerializeField]
    private float nextKeyUpDownInterval, nextKeyLeftRightInterval;


    // Start is called before the first frame update
    void Start()
    {
        boardManager = GameObject.FindObjectOfType<BoardManager>();
        InGameUI.SetActive(true);
        gameOverUI.SetActive(false);
        MenuUI.SetActive(false);

        //タイム変数の初期化
        nextKeyLeftRightTime = Time.time + nextKeyLeftRightInterval;
        nextKeyUpDownTime = Time.time + nextKeyUpDownInterval;

        playerTurn = true;



    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameOver) //GameOverの時は処理を停止
        {
            if (!IsMenu) //Menuが開いていないとき
            {
                //最初にMove(Player,Enemy)関連のクラスを持ったオブジェクトを保持する
                if (!playerExist)
                {
                    SetUp();
                }

                //HPのUI更新
                showHp();

                //HPが0になったかどうか
                if ((playerMove.GetHp() == 0) && (playerExist)) //Playerが死んだとき
                {
                    GameOver();
                }
                else if ((enemyMove.GetHp() == 0) && (enemyExist)) //Enemyが死んだとき
                {
                    enemyExist = false;
                    Destroy(enemyMove.gameObject);
                }

                if (playerTurn) //プレイヤーのターン(プレイヤー移動処理)
                {
                    PlayerInput();
                }
                else //敵のターン(敵移動処理)
                {
                    if (enemyExist)
                    {
                        EnemyMove();
                    }
                    else
                    {
                        //Enemyが倒された後はスルー
                        playerTurn = true;
                    }
                }
            }       
        }
    }

    void SetUp()
    {
        playerMove = GameObject.FindObjectOfType<PlayerMove>();
        enemyMove = GameObject.FindObjectOfType<EnemyMove>();
        playerExist = true;
        enemyExist = true;

        playerHp.maxValue = playerMove.GetHp();
        EnemyHp.maxValue = enemyMove.GetHp();

        int currentPlayerHP = PlayerPrefs.GetInt("PlayerHP", playerMove.GetHp());

        playerMove.SetHp(currentPlayerHP);

    }

    //プレイヤー操作
    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTime)
            || Input.GetKeyDown(KeyCode.D)) //右移動
        {
            PlayerInputDepth(KeyCode.D, ref nextKeyLeftRightTime, nextKeyLeftRightInterval);
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTime) //左移動
            || Input.GetKeyDown(KeyCode.A))
        {
            PlayerInputDepth(KeyCode.A, ref nextKeyLeftRightTime, nextKeyLeftRightInterval);
        }
        else if (Input.GetKey(KeyCode.W) && (Time.time > nextKeyUpDownTime) //上移動
            || Input.GetKeyDown(KeyCode.W))
        {
            PlayerInputDepth(KeyCode.W, ref nextKeyUpDownTime, nextKeyUpDownInterval);
        }
        else if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyUpDownTime) //下移動
            || Input.GetKeyDown(KeyCode.S))
        {
            PlayerInputDepth(KeyCode.S, ref nextKeyUpDownTime, nextKeyUpDownInterval);
        }
    }

    //キー分岐先の処理
    void PlayerInputDepth(KeyCode keyCode, ref float nextKeyAnyTime, float nextKeyAnyInterval)
    {
        //キーを検知して、それぞれの移動処理
        switch (keyCode)
        {
            case KeyCode.W: //上移動
                playerMove.MoveUp();
                nextKeyAnyTime = Time.time + nextKeyAnyInterval;
                break;
            case KeyCode.A: //左移動
                playerMove.MoveLeft();
                nextKeyAnyTime = Time.time + nextKeyAnyInterval;
                break;
            case KeyCode.S: //下移動
                playerMove.MoveDown();
                nextKeyAnyTime = Time.time + nextKeyAnyInterval;
                break;
            case KeyCode.D: //右移動
                playerMove.MoveRight();
                nextKeyAnyTime = Time.time + nextKeyAnyInterval;
                break;
        }

        //進んだ先が枠外か壁の中でないかチェック
        if (!boardManager.PlayerCheckPosition(playerMove.gameObject.transform.position))
        {
            switch (keyCode)
            {
                case KeyCode.W: //上移動
                    playerMove.MoveDown();
                    break;
                case KeyCode.A: //左移動
                    playerMove.MoveRight();
                    break;
                case KeyCode.S: //下移動
                    playerMove.MoveUp();
                    break;
                case KeyCode.D: //右移動
                    playerMove.MoveLeft();
                    break;
            }
        }
        //敵が進んだ方向にいたら止まる
        if (enemyExist)
        {
            if (boardManager.HitEnemy(playerMove.transform.position, enemyMove.transform.position))
            {
                switch (keyCode)
                {
                    case KeyCode.W: //上移動
                        playerMove.MoveDown();
                        break;
                    case KeyCode.A: //左移動
                        playerMove.MoveRight();
                        break;
                    case KeyCode.S: //下移動
                        playerMove.MoveUp();
                        break;
                    case KeyCode.D: //右移動
                        playerMove.MoveLeft();
                        break;
                }

                enemyMove.DamageEnemy();
            }
        }

        if (IsPotion())
        {
            playerMove.RecoverHP();
        }

        playerTurn = false;

        IsStair();
    }

    //enemyの移動処理
    void EnemyMove()
    {
        //現在の位置から上下左右に移動したときの情報をListに格納
        enemyMove.AddPosition();

        //決まるまでループ
        while (true)
        {
            if (enemyMove.nextEnemyPosition.Count == 0)
            {
                break;
            }

            int random = Random.Range(0, enemyMove.nextEnemyPosition.Count);
            if (boardManager.PlayerCheckPosition(enemyMove.nextEnemyPosition[random])) //敵の現在地の更新
            {
                Vector3 nextPosition = enemyMove.nextEnemyPosition[random];
                if (boardManager.HitEnemy(nextPosition, playerMove.transform.position))
                {
                    playerMove.DamagePlayer();
                    break;
                }
                enemyMove.transform.position = nextPosition;
                break;
            }
            else //行けない方向の情報はListから消去する
            {
                enemyMove.RemovePosition(random);
                continue;
            }
        }

        //Listの情報はもう必要ないため、空にする
        enemyMove.ClearList();
        playerTurn = true;
    }

    public bool IsPotion()
    {
        for (int i = 0; i < boardManager.potionPositions.Count; ++i)
        {
            if (boardManager.potionPositions[i].transform.position == playerMove.transform.position)
            {
                Destroy(boardManager.potionPositions[i]);
                boardManager.potionPositions.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public void ReLoad()
    {
        SceneManager.LoadScene("roguelike");
    }

    //画面のHPUIの更新
    void showHp()
    {
        playerHp.value = (float)playerMove.GetHp();
        EnemyHp.value = (float)enemyMove.GetHp();
    }

    void GameOver()
    {
        //Destroy(playerMove.gameObject);
        playerMove.gameObject.SetActive(false);
        InGameUI.SetActive(false);
        gameOverUI.SetActive(true);

        IsGameOver = true;

    }

    //GameOver画面のRetryボタンを押したときの処理
    public void OnRetryClick()
    {

        playerMove.HullHp();
        enemyMove.HullHp();

        playerMove.gameObject.SetActive(true);
        playerMove.transform.position = new Vector3(0, 0, 0);

        IsGameOver = false;
        gameOverUI.SetActive(false);
        InGameUI.SetActive(true);

    }

    //階段に着いた時の処理
    void IsStair()
    {
        if (playerMove.transform.position == boardManager.stairPosition)
        {
            PlayerPrefs.SetInt("PlayerHP", playerMove.GetHp());
            SceneManager.LoadScene("roguelike");
        }
    }

    //インゲーム中に、Menuボタンが押されたときの処理
    public void OnClickMenu()
    {
        playerMove.gameObject.SetActive(false);
        InGameUI.SetActive(false);
        MenuUI.SetActive(true);
        IsMenu = true;
    }

    //Menu画面でBackボタンが押されたときの処理
    public void OnClickBack()
    {
        playerMove.gameObject.SetActive(true);
        MenuUI.SetActive(false);
        InGameUI.SetActive(true);
        IsMenu = false;
    }

    //Menu画面のRetryボタンの処理
    public void OnClickRetry()
    {
        playerMove.HullHp();
        enemyMove.HullHp();

        PlayerPrefs.SetInt("PlayerHP", playerMove.GetHp());

        playerMove.gameObject.SetActive(true);
        playerMove.transform.position = new Vector3(0, 0, 0);

        MenuUI.SetActive(false);
        InGameUI.SetActive(true);
        IsMenu = false;
    }

    //Menu画面のTitleボタンの処理
    public void OnClickTitle()
    {
        //MenuUI.SetActive(false);
        //IsMenu = false;
        SceneManager.LoadScene("Title");
    }
}

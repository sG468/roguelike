using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerMove playerMove;
    BoardManager boardManager;

    bool playerExist = false;

    //入力受付タイマー
    float nextKeyUpDownTime, nextKeyLeftRightTime;

    //入力インターバル(カチャカチャ押されても一定間隔処理を置くための変数)
    [SerializeField]
    private float nextKeyUpDownInterval, nextKeyLeftRightInterval;

    // Start is called before the first frame update
    void Start()
    {
        boardManager = GameObject.FindObjectOfType<BoardManager>();

        //タイム変数の初期化
        nextKeyLeftRightTime = Time.time + nextKeyLeftRightInterval;
        nextKeyUpDownTime = Time.time + nextKeyUpDownInterval;
    }

    // Update is called once per frame
    void Update()
    {
        //最初にPlayerMoveクラスを持ったオブジェクトを保持する
        if (!playerExist)
        {
            playerMove = GameObject.FindObjectOfType<PlayerMove>();
            playerExist = true;
        }

        PlayerInput();
    }

    //プレイヤー操作
    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTime)
            ||Input.GetKeyDown(KeyCode.D)) //右移動)
        {
            PlayerInputDepth(KeyCode.D, ref nextKeyLeftRightTime, nextKeyLeftRightInterval);
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTime) //左移動
            || Input.GetKeyDown(KeyCode.A))
        {
            PlayerInputDepth(KeyCode.A, ref nextKeyLeftRightTime, nextKeyLeftRightInterval);
        }
        else if(Input.GetKey(KeyCode.W) && (Time.time > nextKeyUpDownTime) //上移動
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
        if (!boardManager.PlayerCheckPosition(playerMove))
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
    }
}

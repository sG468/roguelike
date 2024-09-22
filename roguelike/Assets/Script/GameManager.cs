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

    //�^�[���ϐ�
    public bool playerTurn;

    public Slider playerHp;
    public Slider EnemyHp;

    private Transform stairPosition;

    //�N���X�ϐ�
    PlayerMove playerMove;
    EnemyMove enemyMove;
    BoardManager boardManager;

    bool playerExist = false;
    bool enemyExist = false;
    bool IsGameOver = false;
    bool IsMenu = false;

    //���͎�t�^�C�}�[
    float nextKeyUpDownTime, nextKeyLeftRightTime;

    //���̓C���^�[�o��(�J�`���J�`��������Ă����Ԋu������u�����߂̕ϐ�)
    [SerializeField]
    private float nextKeyUpDownInterval, nextKeyLeftRightInterval;


    // Start is called before the first frame update
    void Start()
    {
        boardManager = GameObject.FindObjectOfType<BoardManager>();
        InGameUI.SetActive(true);
        gameOverUI.SetActive(false);
        MenuUI.SetActive(false);

        //�^�C���ϐ��̏�����
        nextKeyLeftRightTime = Time.time + nextKeyLeftRightInterval;
        nextKeyUpDownTime = Time.time + nextKeyUpDownInterval;

        playerTurn = true;



    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameOver) //GameOver�̎��͏������~
        {
            if (!IsMenu) //Menu���J���Ă��Ȃ��Ƃ�
            {
                //�ŏ���Move(Player,Enemy)�֘A�̃N���X���������I�u�W�F�N�g��ێ�����
                if (!playerExist)
                {
                    SetUp();
                }

                //HP��UI�X�V
                showHp();

                //HP��0�ɂȂ������ǂ���
                if ((playerMove.GetHp() == 0) && (playerExist)) //Player�����񂾂Ƃ�
                {
                    GameOver();
                }
                else if ((enemyMove.GetHp() == 0) && (enemyExist)) //Enemy�����񂾂Ƃ�
                {
                    enemyExist = false;
                    Destroy(enemyMove.gameObject);
                }

                if (playerTurn) //�v���C���[�̃^�[��(�v���C���[�ړ�����)
                {
                    PlayerInput();
                }
                else //�G�̃^�[��(�G�ړ�����)
                {
                    if (enemyExist)
                    {
                        EnemyMove();
                    }
                    else
                    {
                        //Enemy���|���ꂽ��̓X���[
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

    //�v���C���[����
    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTime)
            || Input.GetKeyDown(KeyCode.D)) //�E�ړ�
        {
            PlayerInputDepth(KeyCode.D, ref nextKeyLeftRightTime, nextKeyLeftRightInterval);
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTime) //���ړ�
            || Input.GetKeyDown(KeyCode.A))
        {
            PlayerInputDepth(KeyCode.A, ref nextKeyLeftRightTime, nextKeyLeftRightInterval);
        }
        else if (Input.GetKey(KeyCode.W) && (Time.time > nextKeyUpDownTime) //��ړ�
            || Input.GetKeyDown(KeyCode.W))
        {
            PlayerInputDepth(KeyCode.W, ref nextKeyUpDownTime, nextKeyUpDownInterval);
        }
        else if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyUpDownTime) //���ړ�
            || Input.GetKeyDown(KeyCode.S))
        {
            PlayerInputDepth(KeyCode.S, ref nextKeyUpDownTime, nextKeyUpDownInterval);
        }
    }

    //�L�[�����̏���
    void PlayerInputDepth(KeyCode keyCode, ref float nextKeyAnyTime, float nextKeyAnyInterval)
    {
        //�L�[�����m���āA���ꂼ��̈ړ�����
        switch (keyCode)
        {
            case KeyCode.W: //��ړ�
                playerMove.MoveUp();
                nextKeyAnyTime = Time.time + nextKeyAnyInterval;
                break;
            case KeyCode.A: //���ړ�
                playerMove.MoveLeft();
                nextKeyAnyTime = Time.time + nextKeyAnyInterval;
                break;
            case KeyCode.S: //���ړ�
                playerMove.MoveDown();
                nextKeyAnyTime = Time.time + nextKeyAnyInterval;
                break;
            case KeyCode.D: //�E�ړ�
                playerMove.MoveRight();
                nextKeyAnyTime = Time.time + nextKeyAnyInterval;
                break;
        }

        //�i�񂾐悪�g�O���ǂ̒��łȂ����`�F�b�N
        if (!boardManager.PlayerCheckPosition(playerMove.gameObject.transform.position))
        {
            switch (keyCode)
            {
                case KeyCode.W: //��ړ�
                    playerMove.MoveDown();
                    break;
                case KeyCode.A: //���ړ�
                    playerMove.MoveRight();
                    break;
                case KeyCode.S: //���ړ�
                    playerMove.MoveUp();
                    break;
                case KeyCode.D: //�E�ړ�
                    playerMove.MoveLeft();
                    break;
            }
        }
        //�G���i�񂾕����ɂ�����~�܂�
        if (enemyExist)
        {
            if (boardManager.HitEnemy(playerMove.transform.position, enemyMove.transform.position))
            {
                switch (keyCode)
                {
                    case KeyCode.W: //��ړ�
                        playerMove.MoveDown();
                        break;
                    case KeyCode.A: //���ړ�
                        playerMove.MoveRight();
                        break;
                    case KeyCode.S: //���ړ�
                        playerMove.MoveUp();
                        break;
                    case KeyCode.D: //�E�ړ�
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

    //enemy�̈ړ�����
    void EnemyMove()
    {
        //���݂̈ʒu����㉺���E�Ɉړ������Ƃ��̏���List�Ɋi�[
        enemyMove.AddPosition();

        //���܂�܂Ń��[�v
        while (true)
        {
            if (enemyMove.nextEnemyPosition.Count == 0)
            {
                break;
            }

            int random = Random.Range(0, enemyMove.nextEnemyPosition.Count);
            if (boardManager.PlayerCheckPosition(enemyMove.nextEnemyPosition[random])) //�G�̌��ݒn�̍X�V
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
            else //�s���Ȃ������̏���List�����������
            {
                enemyMove.RemovePosition(random);
                continue;
            }
        }

        //List�̏��͂����K�v�Ȃ����߁A��ɂ���
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

    //��ʂ�HPUI�̍X�V
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

    //GameOver��ʂ�Retry�{�^�����������Ƃ��̏���
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

    //�K�i�ɒ��������̏���
    void IsStair()
    {
        if (playerMove.transform.position == boardManager.stairPosition)
        {
            PlayerPrefs.SetInt("PlayerHP", playerMove.GetHp());
            SceneManager.LoadScene("roguelike");
        }
    }

    //�C���Q�[�����ɁAMenu�{�^���������ꂽ�Ƃ��̏���
    public void OnClickMenu()
    {
        playerMove.gameObject.SetActive(false);
        InGameUI.SetActive(false);
        MenuUI.SetActive(true);
        IsMenu = true;
    }

    //Menu��ʂ�Back�{�^���������ꂽ�Ƃ��̏���
    public void OnClickBack()
    {
        playerMove.gameObject.SetActive(true);
        MenuUI.SetActive(false);
        InGameUI.SetActive(true);
        IsMenu = false;
    }

    //Menu��ʂ�Retry�{�^���̏���
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

    //Menu��ʂ�Title�{�^���̏���
    public void OnClickTitle()
    {
        //MenuUI.SetActive(false);
        //IsMenu = false;
        SceneManager.LoadScene("Title");
    }
}

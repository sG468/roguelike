using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerMove playerMove;
    BoardManager boardManager;

    bool playerExist = false;

    //���͎�t�^�C�}�[
    float nextKeyUpDownTime, nextKeyLeftRightTime;

    //���̓C���^�[�o��(�J�`���J�`��������Ă����Ԋu������u�����߂̕ϐ�)
    [SerializeField]
    private float nextKeyUpDownInterval, nextKeyLeftRightInterval;

    // Start is called before the first frame update
    void Start()
    {
        boardManager = GameObject.FindObjectOfType<BoardManager>();

        //�^�C���ϐ��̏�����
        nextKeyLeftRightTime = Time.time + nextKeyLeftRightInterval;
        nextKeyUpDownTime = Time.time + nextKeyUpDownInterval;
    }

    // Update is called once per frame
    void Update()
    {
        //�ŏ���PlayerMove�N���X���������I�u�W�F�N�g��ێ�����
        if (!playerExist)
        {
            playerMove = GameObject.FindObjectOfType<PlayerMove>();
            playerExist = true;
        }

        PlayerInput();
    }

    //�v���C���[����
    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTime)
            ||Input.GetKeyDown(KeyCode.D)) //�E�ړ�)
        {
            PlayerInputDepth(KeyCode.D, ref nextKeyLeftRightTime, nextKeyLeftRightInterval);
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTime) //���ړ�
            || Input.GetKeyDown(KeyCode.A))
        {
            PlayerInputDepth(KeyCode.A, ref nextKeyLeftRightTime, nextKeyLeftRightInterval);
        }
        else if(Input.GetKey(KeyCode.W) && (Time.time > nextKeyUpDownTime) //��ړ�
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
        if (!boardManager.PlayerCheckPosition(playerMove))
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
    }
}

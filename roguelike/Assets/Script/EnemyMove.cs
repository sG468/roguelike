using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private int maxHp = 3;
    private int hp = 3;

    //�G�̌��ݒn����A�l�����ɐi�񂾂Ƃ���Position�����i�[����
    public List<Vector3> nextEnemyPosition = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    //�ړ��֐����ĂԊ֐�
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

    //������ɐi�񂾂Ƃ��̈ʒu��Ԃ�
    public Vector3 UpPosition()
    {
        return transform.position + new Vector3(0, 1, 0);
    }

    //�������ɐi�񂾂Ƃ��̈ʒu��Ԃ�
    public Vector3 DownPosition()
    {
        return transform.position + new Vector3(0, -1, 0);
    }

    //�������ɐi�񂾂Ƃ��̈ʒu��Ԃ�
    public Vector3 LeftPosition()
    {
        return transform.position + new Vector3(-1, 0, 0);
    }

    //�E�����ɐi�񂾂Ƃ��̈ʒu��Ԃ�
    public Vector3 RightPosition()
    {
        return transform.position + new Vector3(1, 0, 0);
    }

    //�l�����A���ꂼ��ɐi�񂾂Ƃ��̏���ǉ�
    public void AddPosition()
    {
        nextEnemyPosition.Add(UpPosition());
        nextEnemyPosition.Add(DownPosition());
        nextEnemyPosition.Add(RightPosition());
        nextEnemyPosition.Add(LeftPosition());
    }

    //�v��Ȃ������̏����폜
    public void RemovePosition(int index)
    {
        nextEnemyPosition.RemoveAt(index);
    }

    //�v�f�̏�����
    public void ClearList()
    {
        nextEnemyPosition.Clear();
    }

    public int GetHp()
    {
        return hp;
    }

    public void SetHp(int saveHp)
    {
        hp = saveHp;
    }

    public void RecoverHP()
    {
        hp++;
    }

    public void DamageEnemy()
    {
        hp--;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void HullHp()
    {
        hp = maxHp;
    }
}

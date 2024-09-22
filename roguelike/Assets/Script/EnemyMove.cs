using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private int maxHp = 3;
    private int hp = 3;

    //敵の現在地から、四方向に進んだときのPosition情報を格納する
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

    //移動関数を呼ぶ関数
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

    //上方向に進んだときの位置を返す
    public Vector3 UpPosition()
    {
        return transform.position + new Vector3(0, 1, 0);
    }

    //下方向に進んだときの位置を返す
    public Vector3 DownPosition()
    {
        return transform.position + new Vector3(0, -1, 0);
    }

    //左方向に進んだときの位置を返す
    public Vector3 LeftPosition()
    {
        return transform.position + new Vector3(-1, 0, 0);
    }

    //右方向に進んだときの位置を返す
    public Vector3 RightPosition()
    {
        return transform.position + new Vector3(1, 0, 0);
    }

    //四方向、それぞれに進んだときの情報を追加
    public void AddPosition()
    {
        nextEnemyPosition.Add(UpPosition());
        nextEnemyPosition.Add(DownPosition());
        nextEnemyPosition.Add(RightPosition());
        nextEnemyPosition.Add(LeftPosition());
    }

    //要らない方向の情報を削除
    public void RemovePosition(int index)
    {
        nextEnemyPosition.RemoveAt(index);
    }

    //要素の初期化
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

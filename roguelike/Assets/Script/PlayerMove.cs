using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public int maxHp = 6;
    private int hp = 6;
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

    //à⁄ìÆä÷êîÇåƒÇ‘ä÷êî
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

    public int GetHp()
    {
        return hp;
    }

    public void SetHp(int saveHp)
    {
        hp = saveHp;
    }

    public void DamagePlayer()
    {
        hp--;
    }

    public void RecoverHP()
    {
        if (hp < maxHp)
        {
            hp++;
        }
        else
        {

        }
    }

    public void HullHp()
    {
        hp = maxHp;
    }
}

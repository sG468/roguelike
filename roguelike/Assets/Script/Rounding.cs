using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rounding : MonoBehaviour
{
    //�v���C���[���W�̐�����
    public static Vector2 Round(Vector2 i)
    {
        return new Vector2(Mathf.Round(i.x), Mathf.Round(i.y));
    }

    public static Vector3 Round(Vector3 i)
    {
        return new Vector3(Mathf.Round(i.x), Mathf.Round(i.y));
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float Speed;
    public Transform Player;
    public float Zpos;
    void Update()
    {
        if(Player != null)
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Zpos);
    }
}

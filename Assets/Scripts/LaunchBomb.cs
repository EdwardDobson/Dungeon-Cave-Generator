using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBomb : MonoBehaviour
{
   public GameObject Bomb;
    public Transform BombParent;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            DropBomb();
        }
    }
    void DropBomb()
    {
        GameObject bombClone = Instantiate(Bomb,transform.position,Quaternion.identity, BombParent);
    }
}

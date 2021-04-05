using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBomb : MonoBehaviour
{
    public GameObject Bomb;
    GameManager m_manager;
    private void Start()
    {
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (m_manager.CanPerformAction)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropBomb();
            }
        }
    }
    void DropBomb()
    {
        GameObject bombClone = Instantiate(Bomb, transform.position, Quaternion.identity);
    }
}

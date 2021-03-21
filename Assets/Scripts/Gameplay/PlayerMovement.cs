using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D m_rb2d;
    public float Speed;
    public Tilemap Map;
    float h;
    float v;
    bool m_playerPlaced;

    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        if (Map == null)
        {
            Map = GameObject.Find("SaveHolder").transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
        }

        BorderDetection();
        Vector2 moveVector = new Vector2(h, v);
        m_rb2d.velocity = moveVector * Speed;
    }
    public bool GetPlayerPlaced()
    {
        return m_playerPlaced;
    }
    //Handes border detection and movement
    void BorderDetection()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (h != 0 || v != 0)
            TileChecker();
        if (transform.position.x - 0.5f < 0)
        {
            if (Input.GetKey(KeyCode.A))
            {
                h = 0;
            }
        }
        if (transform.position.x + 0.5f > Map.cellBounds.xMax)
        {
            if (Input.GetKey(KeyCode.D))
            {
                h = 0;
            }
        }
        if (transform.position.y + 0.5f > Map.cellBounds.yMax)
        {
            if (Input.GetKey(KeyCode.W))
            {
                v = 0;
            }
        }
        if (transform.position.y - 0.5f < 0)
        {
            if (Input.GetKey(KeyCode.S))
            {
                v = 0;
            }
        }
    }
    void TileChecker()
    {
        Vector3Int pos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        Speed = TileManager.GetTileDictionaryFloor()[pos].CustomTile.Speed;
    }
}

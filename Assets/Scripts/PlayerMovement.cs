using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
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
        if (DungeonUtility.GetFloorPositions().Count > 0 && !m_playerPlaced)
        {
            transform.position = DungeonUtility.GetFloorPositions()[Random.Range(0, DungeonUtility.GetFloorPositions().Count)];
            m_playerPlaced = true;
        }

        BorderDetection();
        Vector2 moveVector = new Vector2(h, v);
        m_rb2d.velocity = moveVector * Speed;
        TileChecker();
    }
    void BorderDetection()
    {
        if (transform.position.x - 0.5f > 0)
        {
            if (Input.GetKey(KeyCode.A))
            {
                h = Input.GetAxisRaw("Horizontal");
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                h = 0;
            }
        }
        if (transform.position.x - 0.5f < 0)
        {
            if (Input.GetKey(KeyCode.A))
            {
                h = 0;
            }
            if (Input.GetKey(KeyCode.D))
            {
                h = Input.GetAxisRaw("Horizontal");
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                h = 0;
            }
        }
        if (transform.position.x + 0.5f < Map.cellBounds.xMax)
        {
            if (Input.GetKey(KeyCode.D))
            {
                h = Input.GetAxisRaw("Horizontal");
            }
            if (Input.GetKeyUp(KeyCode.D))
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
            if (Input.GetKey(KeyCode.A))
            {
                h = Input.GetAxisRaw("Horizontal");
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                h = 0;
            }

        }
        if (transform.position.y + 0.5f < Map.cellBounds.yMax)
        {
            if (Input.GetKey(KeyCode.W))
            {
                v = Input.GetAxisRaw("Vertical");
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                v = 0;
            }
        }
        if (transform.position.y + 0.5f > Map.cellBounds.yMax)
        {
            if (Input.GetKey(KeyCode.W))
            {
                v = 0;
            }
            if (Input.GetKey(KeyCode.S))
            {
                v = Input.GetAxisRaw("Vertical");
            }
            if (Input.GetKeyUp(KeyCode.S))
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
            if (Input.GetKey(KeyCode.W))
            {
                v = Input.GetAxisRaw("Vertical");
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                v = 0;
            }
        }
        if (transform.position.y - 0.5f > 0)
        {
            if (Input.GetKey(KeyCode.S))
            {
                v = Input.GetAxisRaw("Vertical");
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                v = 0;
            }
        }
    }
    void TileChecker()
    {
        Vector3Int pos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        if (Map.GetTile(pos) != null)
        {
            if (Map.GetTile(pos).name.Contains("Floor"))
            {
                CustomTile copy = Instantiate(TileManager.GetTileDictionary()[pos].CustomTile);
                copy.Pos = pos;
                Speed = copy.Speed;
            }
            if (Map.GetTile(pos).name.Contains("Path"))
            {
                Speed = 5;
            }
        }
    }
}
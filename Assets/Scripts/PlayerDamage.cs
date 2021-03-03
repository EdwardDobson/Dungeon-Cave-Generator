using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField]
    Tilemap m_map;
    public float Health;
    [SerializeField]
    List<CustomTile> m_damageTiles = new List<CustomTile>();
    Vector3Int m_pos;
    void Start()
    {

    }
    void Update()
    {
        TileDetection();
        for (int i = 0; i < m_damageTiles.Count; ++i)
        {
            if (m_damageTiles[i].Pos == m_pos)
            {
                m_damageTiles[i].CurrentAttackCoolDown -= Time.deltaTime;
                if (m_damageTiles[i].CurrentAttackCoolDown <= 0)
                {
                    Health -= m_damageTiles[i].Damage;
                    m_damageTiles[i].CurrentAttackCoolDown = m_damageTiles[i].MaxAttackCoolDown;
                    GetComponent<PlayerMovement>().Speed = m_damageTiles[i].Speed;
                }
            }
            else
            {
                m_damageTiles[i].CurrentAttackCoolDown = m_damageTiles[i].MaxAttackCoolDown;
                m_damageTiles.RemoveAt(i);
                GetComponent<PlayerMovement>().Speed = 5;
            }
        }
    }
    void TileDetection()
    {
        m_pos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

        if (m_map.GetTile(m_pos) != null)
        {
            if (m_map.GetTile(m_pos).name.Contains("Floor"))
            {
                if(TileManager.GetTileDictionaryFloor()[m_pos].CustomTile.MaxAttackCoolDown > 0)
                {
                    if (m_damageTiles.All(d => d.Pos != m_pos))
                    {
                        CustomTile copy = Instantiate(TileManager.GetTileDictionaryFloor()[m_pos].CustomTile);
                        copy.Pos = m_pos;
                        m_damageTiles.Add(copy);
                    }
                }
       
            }
        }
    }

}

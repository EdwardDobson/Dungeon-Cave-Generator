using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField]
    Tilemap m_map;
    float m_currentHealth;
    public float MaxHealth;
    [SerializeField]
    List<CustomTile> m_damageTiles = new List<CustomTile>();
    Vector3Int m_pos;
    public GameObject GameOverScreen;
    public Slider HealthBar;
    GameManager m_manager;
    private void Start()
    {
        m_currentHealth = MaxHealth;
        HealthBar.maxValue = m_currentHealth;
        HealthBar.value = m_currentHealth;
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (m_manager.Creative)
        {
            m_currentHealth = MaxHealth;
            HealthBar.gameObject.SetActive(false);
        }
        else
        {
            HealthBar.gameObject.SetActive(true);
        }
        HealthBar.value = m_currentHealth;
        TileDetection();
        for (int i = 0; i < m_damageTiles.Count; ++i)
        {
            if (m_damageTiles[i].Pos == m_pos)
            {
                m_damageTiles[i].CurrentAttackCoolDown -= Time.deltaTime;
                if (m_damageTiles[i].CurrentAttackCoolDown <= 0)
                {
                    m_currentHealth -= m_damageTiles[i].Damage;
                    m_damageTiles[i].CurrentAttackCoolDown = m_damageTiles[i].MaxAttackCoolDown;
                }
            }
            else
            {
                m_damageTiles[i].CurrentAttackCoolDown = m_damageTiles[i].MaxAttackCoolDown;
                m_damageTiles.RemoveAt(i);
            }
        }
        if (m_currentHealth <= 0)
        {
            GameOverScreen.SetActive(true);
        }
    }
    void TileDetection()
    {
        m_pos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        if(TileManager.GetTileDictionaryFloor().ContainsKey(m_pos))
        {
            if (TileManager.GetTileDictionaryFloor()[m_pos].CustomTile.MaxAttackCoolDown > 0)
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
    public void DecreaseCurrentHealth(float _damage)
    {
        m_currentHealth -= _damage;
    }

}

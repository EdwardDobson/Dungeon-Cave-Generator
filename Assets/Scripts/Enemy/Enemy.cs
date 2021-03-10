using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool ShouldMove;
    public bool ShouldShoot;
    public float MoveSpeed;
    public int MaxHealth;
    int m_currentHealth;
    GameObject m_player;
    bool m_enemyPlaced;
    public bool FoundPlayer;
    bool m_canSeePlayer;
    public GameObject Projectile;
    public float MaxRateOfFire;
    public float FireRange;
    float m_currentFireTimer;
    public float ProjectileSpeed;
    public int ProjectileDamage;
    bool m_isShooting;
    void Start()
    {
        m_currentHealth = MaxHealth;
        m_player = GameObject.Find("Player");
    }
    void Update()
    {
            if (FloorGen.GetFloorPositions().Count > 0 && !m_enemyPlaced)
            {
                Vector3Int position = FloorGen.GetFloorPositions()[Random.Range(0, FloorGen.GetFloorPositions().Count)];
                Vector3 positionReadjusted = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
                transform.position = positionReadjusted;
                m_enemyPlaced = true;
            }
        if (ShouldMove && m_canSeePlayer)
            Movement();
        if(FoundPlayer)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -(transform.position - m_player.transform.position), LayerMask.GetMask("Level"));
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.tag.Contains("Player"))
                {
                    m_canSeePlayer = true;
                }
                else
                {
                    m_canSeePlayer = false;
                }
            }
        }
        Fire();
        if (m_currentHealth <= 0)
        {
            transform.parent.GetComponent<EnemySpawner>().Enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    void Movement()
    {
        float step = MoveSpeed * Time.deltaTime;
        if(ShouldShoot)
        {
            if(Vector2.Distance(transform.position,m_player.transform.position) > FireRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_player.transform.position, step);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, m_player.transform.position) > 1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_player.transform.position, step);
            }
        }
    }
    void Fire()
    {
        if (m_canSeePlayer && ShouldShoot &&  !m_isShooting)
        {
            m_currentFireTimer -= Time.deltaTime;
            if (m_currentFireTimer <= 0)
            {
                m_isShooting = true;
                SpawnBullet();
            }
        }
 
    }
    void SpawnBullet()
    {
        GameObject clone = Instantiate(Projectile, transform.position, Quaternion.identity);
        Vector2 dir = (transform.position - m_player.transform.position).normalized;
        clone.GetComponent<Projectile>().SetValues(-dir, ProjectileSpeed, ProjectileDamage, ProjectileSide.Enemy);
        m_currentFireTimer = MaxRateOfFire;
        m_isShooting = false;
    }
    public void DecreaseCurrentHealth(int _amount)
    {
        m_currentHealth -= _amount;
    }
    public float SetCurrentFireTimer()
    {
        return m_currentFireTimer = MaxRateOfFire;
    }
}

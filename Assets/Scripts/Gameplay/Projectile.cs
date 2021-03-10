using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ProjectileSide
{
    None,
    Player,
    Enemy
}
public class Projectile : MonoBehaviour
{
    Rigidbody2D m_rb2d;
    Vector2 m_direction;
    float m_speed;
    int m_damage;
    ProjectileSide m_side;
    void Update()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_rb2d.velocity = m_direction * m_speed;
    }
    public void SetValues(Vector2 _direction, float _speed, int _damage,ProjectileSide _side)
    {
        m_direction = _direction;
        m_speed = _speed;
        m_damage = _damage;
        m_side = _side;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Player") && m_side == ProjectileSide.Enemy)
        {
            collision.gameObject.GetComponent<PlayerDamage>().DecreaseCurrentHealth(m_damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag.Contains("Enemy") && m_side == ProjectileSide.Player)
        {
            collision.gameObject.GetComponent<Enemy>().DecreaseCurrentHealth(m_damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag.Contains("Wall"))
        {
            Destroy(gameObject);
        }
    }
    public float GetDamage()
    {
        return m_damage;
    }
}

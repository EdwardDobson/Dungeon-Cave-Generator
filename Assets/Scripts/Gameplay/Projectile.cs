using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D m_rb2d;
    Vector2 m_direction;
    float m_speed;
    float m_damage;
    void Update()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_rb2d.velocity = m_direction * m_speed;
    }
    public void SetValues(Vector2 _direction, float _speed, float _damage)
    {
        m_direction = _direction;
        m_speed = _speed;
        m_damage = _damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Player"))
        {
            collision.gameObject.GetComponent<PlayerDamage>().Health -= m_damage;
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

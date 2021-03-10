using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public float RateOfFire;
    float m_timer;
    public int Damage;
    public float ProjectileSpeed;
    public bool CanAttack;
    void Update()
    {
        if(CanAttack)
        {
        
            if (Input.GetMouseButton(0))
            {
                Fire();
                m_timer -= Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_timer = 0;
            }
        }
    }
    void Fire()
    {
        if(m_timer <=0)
        {
            Vector2 dir = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
            dir.Normalize();
            GameObject clone = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
            clone.GetComponent<Projectile>().SetValues(-dir, ProjectileSpeed, Damage,ProjectileSide.Player);
            m_timer = RateOfFire;
        }
    }
}

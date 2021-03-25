using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int AmountToSpawn;
    public GameObject EnemyPrefab;
    public List<GameObject> Enemies;
    bool m_buildEnemies =true;
    void Start()
    {

    }
    private void Update()
    {
        if (m_buildEnemies)
        {
            AmountToSpawn = FloorGen.GetFloorPositions().Count / 100;
            for (int i = 0; i < AmountToSpawn; ++i)
            {
                GameObject clone = Instantiate(EnemyPrefab);
                clone.transform.SetParent(transform);
                Enemies.Add(clone);
            }
            m_buildEnemies = false;
        }
    }
}

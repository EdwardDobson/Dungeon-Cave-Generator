using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int AmountToSpawn;
    public GameObject EnemyPrefab;
    public List<GameObject> Enemies;
    void Start()
    {
        for(int i = 0; i < AmountToSpawn; ++i)
        {
            GameObject clone = Instantiate(EnemyPrefab);
            clone.transform.SetParent(transform);
            Enemies.Add(clone);
        }
    }
}

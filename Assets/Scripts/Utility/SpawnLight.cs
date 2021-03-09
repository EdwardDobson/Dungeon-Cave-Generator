using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLight : MonoBehaviour
{
    public GameObject LightPrefab;
    public int LightAmount;
    bool m_lightsPlaced;
    void Start()
    {
     
    }
    private void Update()
    {
        if(!m_lightsPlaced)
        {
            for (int i = 0; i < LightAmount; ++i)
            {
                Vector3Int position = FloorGen.GetFloorPositions()[Random.Range(0, FloorGen.GetFloorPositions().Count)];
                Vector3 positionReadjusted = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
                Instantiate(LightPrefab, positionReadjusted, Quaternion.identity);
                FloorGen.GetFloorPositions().Remove(position);
            }
            m_lightsPlaced = true;
        }
    }
}

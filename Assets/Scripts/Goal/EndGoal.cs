using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoal : MonoBehaviour
{
    [SerializeField]
    bool m_endPointPlaced;
    PlayerMovement m_playerMovement;
    void Start()
    {
        m_playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    void Update()
    {
        if (FloorGen.GetFloorPositions().Count > 0 && !m_endPointPlaced && m_playerMovement.GetPlayerPlaced())
        {
          
            Vector3Int position = FloorGen.GetFloorPositions()[Random.Range(0, FloorGen.GetFloorPositions().Count)];
            Vector3 positionReadjusted = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
            transform.position = positionReadjusted;
            m_endPointPlaced = true;
        }
    }
    public bool GetEndPointSet()
    {
        return m_endPointPlaced;
    }
}
